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
    public class QuestionModel
    {
        public DateTime date_created { get; set; }
        public string item_id { get; set; }
        public string seller_id { get; set; }
        public string status { get; set; }
        public string text { get; set; }
        public string id { get; set; }
        public string deleted_from_listing { get; set; }
        public string hold { get; set; }
        public string answer { get; set; }
        public From from { get; set; }
    }

    public class From
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}