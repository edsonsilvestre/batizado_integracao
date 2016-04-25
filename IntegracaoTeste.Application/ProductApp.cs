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

        public Entity.ProductList GetProductsByUserID(Meli m, string userID)
        {
            var ps = new List<Parameter>();

            var p = new Parameter();
            p.Name = "access_token";
            p.Value = m.AccessToken;
            ps.Add(p);

            p = new Parameter();
            p.Name = "seller_id";
            p.Value = userID;
            ps.Add(p);
                        
            //https://api.mercadolibre.com/
            IRestResponse response = m.Get("/sites/MLA/search", ps);

            Entity.ProductList produtos = new Entity.ProductList();

            produtos = JsonConvert.DeserializeObject<Entity.ProductList>(response.Content);

            return produtos;
        }

        public Entity.Return Anunciar(Meli m, Entity.Product product)
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

                string result;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                Entity.Product produto = JsonConvert.DeserializeObject<Entity.Product>(result);

                Entity.Return retorno = new Entity.Return();

                retorno.Message = "OK" + ";" + produto.Id;

                return retorno;
            }
            catch (WebException exception)
            {
                string responseText;

                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
                
                Entity.Return retorno = JsonConvert.DeserializeObject<Entity.Return>(responseText);

                retorno.Message = "NOK" + ";" + retorno.Message;

                return retorno;
            }
        }
    }
}
