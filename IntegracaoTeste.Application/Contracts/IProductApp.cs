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
            List<Product> GetProductsByUserID(string access_token, Meli m, string userID);
            bool Anunciar(Meli m, Product product);
    }
}
