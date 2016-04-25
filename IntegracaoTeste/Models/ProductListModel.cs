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
using System.ComponentModel.DataAnnotations;

namespace IntegracaoTeste.Models
{
    public class ProductListModel
    {
        public string Id { get; set; }
        public string Site_Id { get; set; }
        public List<Results> Results { get; set; }
    }
}
