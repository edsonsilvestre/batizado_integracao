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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace IntegracaoTeste.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Site_Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public Int64 Seller_Id { get; set; }
        public string Category_Id { get; set; }
        public string Official_Store_Id { get; set; }
        public double Price { get; set; }
        public List<PictureModel> Pictures { get; set; }
    }
}