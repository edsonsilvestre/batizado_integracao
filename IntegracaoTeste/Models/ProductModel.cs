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
using IntegracaoTeste.Domain.Entities;
using AutoMapper;
using System.Web.Routing;
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
        public string Condition { get; set; }
        public Int32 Available_Quantity { get; set; }
        //public List<Picture> Pictures { get; set; }
        public string Category_Id { get; set; }
        public double Price { get; set; }
        public string Currency_Id { get; set; }
        public bool Accepts_Mercadopago { get; set; }
        public string Listing_Type_Id { get; set; }
    }
}