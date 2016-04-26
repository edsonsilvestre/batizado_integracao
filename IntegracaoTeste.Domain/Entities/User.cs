using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoTeste.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string nickname { get; set; }
        public string registration_date { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string country_id { get; set; }
        public string email { get; set; }
        public string code { get; set; }
        public string refreshToken { get; set; }
        public string accessToken { get; set; }
    }
}
