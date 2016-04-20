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
using IntegracaoTeste.Application;
using IntegracaoTeste.Application.Contracts;
using IntegracaoTeste.Domain.Entities;
using IntegracaoTeste.App_Start;
using Ninject;
using System.Reflection;
using IntegracaoTeste.Sessions;
using System.Threading;

namespace IntegracaoBatizado.Controllers
{
    public class ProductController : AuthenticatedBaseController
    {
        private readonly IProductApp _productApp;
        private readonly IUserApp _userApp;
        private readonly ICategoryApp _categoryApp;
        private readonly ICurrencyApp _currencyApp;
        private readonly IListingTypeApp _listingTypeApp;

        static long clientId = Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]);
        static string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
        
        Meli m = new Meli(clientId, clientSecret);

        public ProductController()
        {
            _productApp = NinjectWebCommon.Kernel.Get<IProductApp>();
            _userApp = NinjectWebCommon.Kernel.Get<IUserApp>();
            _categoryApp = NinjectWebCommon.Kernel.Get<ICategoryApp>();
            _currencyApp = NinjectWebCommon.Kernel.Get<ICurrencyApp>();
            _listingTypeApp = NinjectWebCommon.Kernel.Get<IListingTypeApp>();
        }

        public ActionResult Announce()
        {
            CarregarViewBag();

            return View("Announce");
        }

        public ActionResult Search()
        {
            m.Authorize(Request.QueryString["code"], "http://localhost:3000");

            User user = new User();

            user = _userApp.GetMyUser(m);

            List<Product> produtos = new List<Product>();

            produtos = _productApp.GetProductsByUserID(m.AccessToken, m, user.Id);

            return View("Search");
        }

        public ActionResult Filtrar(string Id)
        {
            m.Authorize(Request.QueryString["code"], "http://localhost:3000");

            //MLA600190449
            var product = _productApp.GetProduct(Id);

            Mapper.CreateMap<Product, ProductModel>();
            Mapper.AssertConfigurationIsValid();

            var model = Mapper.Map<ProductModel>(product);
            
            return View("Search", model);
        }

        public ActionResult Anunciar(ProductModel model)
        {
            m.Authorize(SessionProfile.Usuario.code, "http://localhost:3000");

            Mapper.CreateMap<ProductModel, Product>();
            Mapper.AssertConfigurationIsValid();

            var product = Mapper.Map<Product>(model);

            product.Title = "Teste de Anuncio. Não clicar em comprar!";
            product.Subtitle = "Teste de Anuncio. Não clicar em comprar!";
            product.Price = 1000;
            product.Accepts_Mercadopago = true;
            product.Available_Quantity = 10;
            product.Currency_Id = "ARS";
            product.Category_Id = "MLA48904";
            product.Condition = "new";
            product.Listing_Type_Id = "gold";

            _productApp.Anunciar(m, product);

            return View("Search");
        }

        public JsonResult LoadCategories(string Id)
        {
            List<ChildrenCategory> categorias = new List<ChildrenCategory>();

            if (Request.QueryString["code"] != null)
                m.Authorize(Request.QueryString["code"], "http://localhost:3000");
            else
                m.Authorize(SessionProfile.Usuario.code, "http://localhost:3000");

            categorias = _categoryApp.GetChildrenCategoryById(m, "MLA", Id);

            var categoriasData = categorias.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            return Json(categoriasData, JsonRequestBehavior.AllowGet);
        }

        public void CarregarViewBag()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Novo", Value = "new"});
            items.Add(new SelectListItem { Text = "Usado", Value = "used" });
            ViewBag.Conditions = items;

            m.Authorize(Request.QueryString["code"], "http://localhost:3000");
            List<Category> categorias = new List<Category>();
            categorias = _categoryApp.GetCategoriesByCountry(m, "MLA");
            ViewBag.Categories = categorias;

            List<Currency> currencies = new List<Currency>();
            currencies = _currencyApp.GetCurrencies(m);
            ViewBag.Currencies = currencies;

            List<ListingType> listingTypes = new List<ListingType>();
            listingTypes = _listingTypeApp.GetListingTypesByCountry(m, "MLA");
            ViewBag.ListingTypes = listingTypes;
        }
    }
}
