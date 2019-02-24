using System;
using System.Collections.Generic;
using System.Text;

namespace NetRabbitMQ.Sender
{
    public class EmailTemplate
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
