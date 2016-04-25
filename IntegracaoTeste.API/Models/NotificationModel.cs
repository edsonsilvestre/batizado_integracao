using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoTeste.API.Models
{
    public class NotificationModel
    {
        public Int32 user_id { get; set; }
        public string resource { get; set; }
        public string topic { get; set; }
        public DateTime received { get; set; }
        public DateTime sent { get; set; }
    }
}
