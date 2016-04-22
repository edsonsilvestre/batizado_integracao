using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoTeste.Domain.Entities
{
    public class Question
    {
        public DateTime date_created { get; set; }
        public string item_id { get; set; }
        public string seller_id { get; set; }
        public string status { get; set; }
        public string text { get; set; }
        public string id { get; set; }
        public string deleted_from_listing { get; set; }
        public string hold { get; set; }
        public string answer { get; set; }
    }
}
