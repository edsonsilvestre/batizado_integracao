using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoTeste.Domain.Entities
{
    public class Return
    {
        public string Message { get; set; }
        public string Error { get; set; }
        public Int16 Status { get; set; }
    }
}
