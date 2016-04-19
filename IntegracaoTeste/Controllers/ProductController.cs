using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MercadoLibre.SDK;
using RestSharp;
using IntegracaoTeste.Models;
using AutoMapper;
using System.Web.Routing;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace IntegracaoBatizado.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            //MLA600190449
            return View();
        }

        public ActionResult Filtrar(string Id)
        {
            ProductModel model = new ProductModel();

            model = GetProduct(Id);

            return View("Index", model);
        }

        public ProductModel GetProduct(string productId)
        {
            string URLProduct = "https://api.mercadolibre.com/items/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLProduct);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(productId).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            ProductModel m = new ProductModel();

            if (response.IsSuccessStatusCode)
            {
                m = JsonConvert.DeserializeObject<ProductModel>(result);
            }
            else
            {
                m = null;
            }

            return m;
        }

        public void SetProduct()
        {
        }
    }
}
