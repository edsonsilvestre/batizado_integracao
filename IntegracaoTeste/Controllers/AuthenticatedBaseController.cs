using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using IntegracaoTeste.Application.Contracts;
using IntegracaoTeste.App_Start;
using IntegracaoTeste.Models;
using MercadoLibre.SDK;
using RestSharp;
using IntegracaoTeste.Sessions;

namespace IntegracaoBatizado.Controllers
{
    public class AuthenticatedBaseController : Controller
    {
        private readonly IUserApp _userApp;

        static long clientId = Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]);
        static string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];

        Meli m = new Meli(clientId, clientSecret);

        public AuthenticatedBaseController()
        {
            _userApp = NinjectWebCommon.Kernel.Get<IUserApp>();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Request.QueryString["code"] != null)
                m.Authorize(Request.QueryString["code"], "http://localhost:3000");
            else
                m.Authorize(SessionProfile.Usuario.code, "http://localhost:3000");

            if (SessionProfile.Usuario == null || SessionProfile.Usuario.Id == null)
            {
                SessionProfile.Usuario = _userApp.GetMyUser(m);

                SessionProfile.Usuario.code = Request.QueryString["code"];
            }
        }

        public string GetAuthUrl(long clientId, string clientSecret)
        {
            Meli m = new Meli(clientId, clientSecret);

            return m.GetAuthUrl("http://localhost:3000");
        }
    }
}