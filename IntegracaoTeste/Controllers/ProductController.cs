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

namespace IntegracaoTeste.Controllers
{
    public class ProductController : AuthenticatedBaseController
    {
        private readonly IProductApp _productApp;
        private readonly IUserApp _userApp;
        private readonly ICategoryApp _categoryApp;
        private readonly ICurrencyApp _currencyApp;
        private readonly IListingTypeApp _listingTypeApp;
        private readonly IQuestionAnswerApp _questionAnswerApp;

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
            _questionAnswerApp = NinjectWebCommon.Kernel.Get<IQuestionAnswerApp>();
        }
        public ActionResult Main()
        {
            Autorizar(m);

            var product = _productApp.GetProductsByUserID(m, SessionProfile.Usuario.Id);

            Mapper.CreateMap<ProductList, ProductListModel>();
            Mapper.AssertConfigurationIsValid();

            var model = Mapper.Map<ProductListModel>(product);

            //CarregarViewBag();

            return View("Main", model);
        }

        public ActionResult Announce()
        {
            Autorizar(m);

            QuestionAnswer questionAnswers = new QuestionAnswer();

            questionAnswers = _questionAnswerApp.GetQuestionAnswerBySellerId(m, SessionProfile.Usuario.Id);

            CarregarViewBag();

            return View("Announce");
        }

        public ActionResult Search()
        {
            return View("Search");
        }

        public ActionResult Filtrar(string Id)
        {
            Autorizar(m);

            //MLA600190449
            var product = _productApp.GetProduct(Id);

            Mapper.CreateMap<Product, ProductModel>();
            Mapper.AssertConfigurationIsValid();

            var model = Mapper.Map<ProductModel>(product);
            
            return View("Search", model);
        }

        public ActionResult Anunciar(ProductModel model)
        {
            Autorizar(m);

            Mapper.CreateMap<ProductModel, Product>();
            Mapper.AssertConfigurationIsValid();

            var product = Mapper.Map<Product>(model);

            Return retorno = _productApp.Anunciar(m, product);

            if (retorno.Message.Split(';').Count() > 1)
            {
                if (retorno.Message.Split(';')[0] == "OK")
                    TempData["message"] = "Anuncio " + retorno.Message.Split(';')[1] + " criado com sucesso!";
                else
                    TempData["error"] = "Ocorreu o seguinte erro ao anunciar: " + retorno.Message.Split(';')[1];
            }

            return RedirectToAction("Announce");
        }

        public JsonResult LoadCategories(string Id)
        {
            List<ChildrenCategory> categorias = new List<ChildrenCategory>();

            Autorizar(m);

            categorias = _categoryApp.GetChildrenCategoryById(m, "MLA", Id);

            IEnumerable<SelectListItem> categoriasData = null;

            if (categorias != null && categorias.Count > 0)
            {
                categoriasData = categorias.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
            }

            return Json(categoriasData, JsonRequestBehavior.AllowGet);
        }

        public void CarregarViewBag()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Selecione uma condição", Value = "" });
            items.Add(new SelectListItem { Text = "Novo", Value = "new"});
            items.Add(new SelectListItem { Text = "Usado", Value = "used" });
            ViewBag.Conditions = items;

            Autorizar(m);

            List<Category> categorias = new List<Category>();
            categorias = _categoryApp.GetCategoriesByCountry(m, "MLA");

            Category blankCategory = new Category();
            blankCategory.Name = "Selecione uma categoria";
            blankCategory.Id = "";

            if (categorias == null)
                categorias = new List<Category>();

            categorias.Insert(0, blankCategory);
            ViewBag.Categories = categorias;

            List<Currency> currencies = new List<Currency>();
            currencies = _currencyApp.GetCurrencies(m);

            Currency blankCurrency = new Currency();
            blankCurrency.Description = "Selecione uma moeda";
            blankCurrency.Id = "";

            if (currencies == null)
                currencies = new List<Currency>();

            currencies.Insert(0, blankCurrency);
            ViewBag.Currencies = currencies;

            List<ListingType> listingTypes = new List<ListingType>();
            listingTypes = _listingTypeApp.GetListingTypesByCountry(m, "MLA");

            ListingType blankListingType = new ListingType();
            blankListingType.Name = "Selecione um tipo de anúncio";
            blankListingType.Id = "";

            if (listingTypes == null)
                listingTypes = new List<ListingType>();

            listingTypes.Insert(0, blankListingType);
            ViewBag.ListingTypes = listingTypes;
        }
    }
}
