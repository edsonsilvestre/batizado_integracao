using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoTeste.Domain.Entities;
using MercadoLibre.SDK;

namespace IntegracaoTeste.Application.Contracts
{
    public interface IProductApp
    {
            Product GetProduct(string productId);
            ProductList GetProductsByUserID(Meli m, string userID);
            Return Anunciar(Meli m, Product product);
    }
}
