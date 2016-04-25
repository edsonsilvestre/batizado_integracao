using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoTeste.Domain.Entities
{
    public class ProductList
    {
        public string Id { get; set; }
        public string Site_Id { get; set; }
        public List<Results> Results { get; set; }
    }
}
