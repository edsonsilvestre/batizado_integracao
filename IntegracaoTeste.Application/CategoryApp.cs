using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MercadoLibre.SDK;
using RestSharp;
using IntegracaoTeste.Application.Contracts;
using Entity = IntegracaoTeste.Domain.Entities;
using AutoMapper;
using System.Web.Routing;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;

namespace IntegracaoTeste.Application
{
    public class CategoryApp : ICategoryApp
    {
        public List<Entity.Category> GetCategoriesByCountry(Meli m, string country)
        {
            var p = new Parameter();
            p.Name = "access_token";
            p.Value = m.AccessToken;

            var ps = new List<Parameter>();
            ps.Add(p);

            //https://api.mercadolibre.com/
            IRestResponse response = m.Get("/sites/" + country + "/categories/", ps);

            List<Entity.Category> categorias = new List<Entity.Category>();

            categorias = JsonConvert.DeserializeObject<List<Entity.Category>>(response.Content);

            return categorias;
        }

        public List<Entity.ChildrenCategory> GetChildrenCategoryById(Meli m, string country, string Id)
        {
            var p = new Parameter();
            p.Name = "access_token";
            p.Value = m.AccessToken;

            var ps = new List<Parameter>();
            ps.Add(p);

            //https://api.mercadolibre.com/
            IRestResponse response = m.Get("/categories/" + Id, ps);

            Entity.Category categoria = new Entity.Category();

            categoria = JsonConvert.DeserializeObject<Entity.Category>(response.Content);

            return categoria.Children_Categories;
        }
    }
}
