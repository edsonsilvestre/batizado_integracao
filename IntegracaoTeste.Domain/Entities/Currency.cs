﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoTeste.Domain.Entities
{
    public class Currency
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public Int16 Decimal_Places { get; set; }
    }
}
