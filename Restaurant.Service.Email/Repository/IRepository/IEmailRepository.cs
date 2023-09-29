using Restaurant.Service.Email.Messages;

namespace Restaurant.Service.Email.Repository.IRepository
{
    public interface IEmailRepository
    {
        Task SendAndLogEmail(UpdatePaymentResultMessage message);
    }
}
