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
    public class ListingTypeApp : IListingTypeApp
    {
        public List<Entity.ListingType> GetListingTypesByCountry(Meli m, string country)
        {
            var p = new Parameter();
            p.Name = "access_token";
            p.Value = m.AccessToken;

            var ps = new List<Parameter>();
            ps.Add(p);

            //https://api.mercadolibre.com/
            IRestResponse response = m.Get("/sites/" + country + "/listing_types/", ps);

            List<Entity.ListingType> listingTypes = new List<Entity.ListingType>();

            listingTypes = JsonConvert.DeserializeObject<List<Entity.ListingType>>(response.Content);

            return listingTypes;
        }
    }
}
