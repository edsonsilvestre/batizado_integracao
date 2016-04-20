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
using System.IO;

namespace IntegracaoTeste.Application
{
    public class ProductApp : IProductApp
    {
        public Entity.Product GetProduct(string productId)
        {
            string URLProduct = "https://api.mercadolibre.com/items/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLProduct);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(productId.ToString()).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            Entity.Product m = new Entity.Product();

            if (response.IsSuccessStatusCode)
            {
                m = JsonConvert.DeserializeObject<Entity.Product>(result);
            }
            else
            {
                m = null;
            }

            return m;
        }

        public List<Entity.Product> GetProductsByUserID(string access_token, Meli m, string userID)
        {
            var p = new Parameter();
            p.Name = "access_token";
            p.Value = m.AccessToken;

            var ps = new List<Parameter>();
            ps.Add(p);

            //https://api.mercadolibre.com/
            IRestResponse response = m.Get("/sites/MLA/search?seller_id=" + userID, ps);

            List<Entity.Product> produtos = new List<Entity.Product>();

            //produtos = JsonConvert.DeserializeObject<List<Entity.Product>>(response.Content);

            return produtos;
        }

        public bool Anunciar(Meli m, Entity.Product product)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.mercadolibre.com/items?access_token=" + m.AccessToken);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json =   //"{\"site_id\":\"" + m.ClientId.ToString() + "\"," +
                                    "{\"title\":\"" + product.Title + "\"," +
                                    //"\"subtitle\":\"" + product.Subtitle + "\"," +
                                    //"\"seller_id\":\"" ++ "\"," +
                                    "\"category_id\":\"" + product.Category_Id + "\"," +
                                    "\"price\":\"" + product.Price + "\"," +
                                    "\"currency_id\":\"" + product.Currency_Id + "\"," +
                                    "\"available_quantity\":\"" + product.Available_Quantity + "\"," +
                                    "\"listing_type_id\":\"" + product.Listing_Type_Id + "\"," +
                                    "\"condition\":\"" + product.Condition + "\"," +
                                    "\"accepts_mercadopago\":\"" + product.Accepts_Mercadopago + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (WebException exception)
            {
                string responseText;

                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }

            return true;
        }
    }
}
