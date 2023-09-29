using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Services.ShoppingCartAPI.DbContexts;
using Restaurant.Services.ShoppingCartAPI.Models;
using Restaurant.Services.ShoppingCartAPI.Models.Dto;
using Restaurant.Services.ShoppingCartAPI.Repository.IRepository;

namespace Restaurant.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public CartRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, string code)
        {
            var cart = await _db.CartHeaders.FirstOrDefaultAsync(i => i.UserId == userId);
            cart.CouponCode = code;
            _db.CartHeaders.Update(cart);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
        
            if (cartHeaderFromDb != null)
            {
                _db.CartDetails
                    .RemoveRange(_db.CartDetails.Where(i => i.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _db.CartHeaders.Remove(cartHeaderFromDb);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);
            var prodInDb = _db.Products.FirstOrDefault(i => i.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);
            if (prodInDb == null)
            {
                _db.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _db.SaveChangesAsync();
            }

            var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(i => i.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _db.SaveChangesAsync();
            }
            else
            {
                var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    i => i.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    i.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    _db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _db.CartHeaders
                .FirstOrDefaultAsync(x => x.UserId == userId)
            };

            cart.CartDetails = _db.CartDetails
                .Where(i => i.CartHeaderId == cart.CartHeader.CartHeaderId).Include(i => i.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var cart = await _db.CartHeaders.FirstOrDefaultAsync(i => i.UserId == userId);
            cart.CouponCode = "";
            _db.CartHeaders.Update(cart);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await _db.CartDetails
                    .FirstOrDefaultAsync(i => i.CartDetailsId == cartDetailsId);

                int totalCountOfItems = _db.CartDetails.Where(i => i.CartHeaderId == cartDetails.CartHeaderId).Count();

                _db.CartDetails.Remove(cartDetails);
                if (totalCountOfItems == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                        .FirstOrDefaultAsync(i => i.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
