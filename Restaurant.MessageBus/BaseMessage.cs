using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.MessageBus
{
    public class BaseMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime MessageCreatedTime { get; set; }
    }
}
