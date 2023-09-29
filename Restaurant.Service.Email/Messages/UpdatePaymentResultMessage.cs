using Restaurant.MessageBus;

namespace Restaurant.Service.Email.Messages
{
    public class UpdatePaymentResultMessage : BaseMessage
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
