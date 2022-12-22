using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Services.Identity.DbContexts;
using Restaurant.Services.Identity.Models;
using System.Security.Claims;

namespace Restaurant.Services.Identity.Pages.Account.Register
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public IndexModel(SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext db, 
            IIdentityServerInteractionService interaction, 
            IEventService events, 
            IAuthenticationSchemeProvider schemeProvider, 
            IIdentityProviderStore identityProviderStore)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _interaction = interaction;
            _events = events;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
        }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                { 
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = true,
                    Name = Input.Name
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.User);

                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, Input.Name),
                        new Claim(JwtClaimTypes.Email, Input.Email),
                        new Claim(JwtClaimTypes.Role, SD.User)
                    });

                    var loginresult = await _signInManager.PasswordSignInAsync(
                        Input.Email, Input.Password, false, lockoutOnFailure: true);
                    
                    if (loginresult.Succeeded)
                    {
                        if (Url.IsLocalUrl(Input.ReturnUrl))
                        {
                            return Redirect(Input.ReturnUrl);
                        }
                        else if (string.IsNullOrEmpty(Input.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        else
                        {
                            throw new Exception("Invalid return URL");
                        }
                    }
                }
            }
            return Page();
        }
    }
}
