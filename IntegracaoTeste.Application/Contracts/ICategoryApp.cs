using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoTeste.Domain.Entities;
using MercadoLibre.SDK;

namespace IntegracaoTeste.Application.Contracts
{
    public interface ICategoryApp
    {
        List<Category> GetCategoriesByCountry(Meli m, string country);
        List<ChildrenCategory> GetChildrenCategoryById(Meli m, string country, string Id);
    }
}
