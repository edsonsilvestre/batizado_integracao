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
using System.IO;
using System.Web.Http;
using System.Net;
using Newtonsoft.Json;

namespace IntegracaoTeste.Controllers
{
    public class AuthenticatedBaseController : Controller
    {
        private RestClient client = new RestClient(ApiUrl);

        static private string apiUrl = "https://api.mercadolibre.com";
        static private string sdkVersion = "MELI-NET-SDK-0.0.1";
        static public string ApiUrl
        {
            get
            {
                return apiUrl;
            }
            set
            {
                apiUrl = value;
            }
        }

        private readonly IUserApp _userApp;

        static long clientId = Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]);
        static string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];


        //Meli m = new Meli(clientId, clientSecret);

        public AuthenticatedBaseController()
        {
            _userApp = NinjectWebCommon.Kernel.Get<IUserApp>();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            AutorizarTeste(SessionProfile.Meli);

            if (SessionProfile.Usuario == null || SessionProfile.Usuario.Id == null)
            {
                SessionProfile.Usuario = _userApp.GetMyUser(SessionProfile.Meli);

                SessionProfile.Usuario.code = Request.QueryString["code"];
                SessionProfile.Usuario.refreshToken = SessionProfile.Meli.RefreshToken;
                SessionProfile.Usuario.accessToken = SessionProfile.Meli.AccessToken;
            }
        }

        public string GetAuthUrl(long clientId, string clientSecret)
        {
            Meli m = new Meli(clientId, clientSecret);

            return m.GetAuthUrl("http://localhost:3000");
        }
        //public void Autorizar(Meli m)
        //{

        //    try
        //    {
        //        if (Request.QueryString["code"] != null)
        //            m.Authorize(Request.QueryString["code"], "http://localhost:3000");
        //        else
        //            m.Authorize(SessionProfile.Usuario.code, "http://localhost:3000");
        //    }
        //    catch (AuthorizationException e)
        //    {
        //        RefreshToken(m);

        //        AutorizarTeste(m);
        //    }
        //}

        public void AutorizarTeste(Meli m)
        {
            var request = new RestRequest("/oauth/token?grant_type=authorization_code&client_id={client_id}&client_secret={client_secret}&code={code}&redirect_uri={redirect_uri}", Method.POST);

            request.AddParameter("client_id", m.ClientId, ParameterType.UrlSegment);
            request.AddParameter("client_secret", m.ClientSecret, ParameterType.UrlSegment);

            if (Request.QueryString["code"] != null)
                request.AddParameter("code", Request.QueryString["code"], ParameterType.UrlSegment); 
            else
                request.AddParameter("code", SessionProfile.Usuario.code, ParameterType.UrlSegment);

            request.AddParameter("redirect_uri", "http://localhost:3000", ParameterType.UrlSegment);

            request.AddHeader("Accept", "application/json");

            var response = ExecuteRequest(request);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                var token = JsonConvert.DeserializeAnonymousType(response.Content, new { refresh_token = "", access_token = "" });
                SessionProfile.Usuario.accessToken = token.access_token;
                SessionProfile.Usuario.refreshToken = token.refresh_token;
                SessionProfile.Usuario.code = token.refresh_token;

                m = new Meli(m.ClientId, m.ClientSecret, token.access_token, token.refresh_token);

                SessionProfile.Meli = m;
            }
            else
            {
                var token = JsonConvert.DeserializeAnonymousType(response.Content, new { error = "" });

                if(token.error == "invalid_grant")
                    RefreshToken(m);
                else
                    throw new AuthorizationException();
            }
        }
        public void RefreshToken(Meli m)
        {
            if (SessionProfile.Usuario != null && SessionProfile.Usuario.refreshToken != null && SessionProfile.Usuario.code != null && SessionProfile.Usuario.accessToken != null)
            {
                var request = new RestRequest("/oauth/token?grant_type=refresh_token&client_id={client_id}&client_secret={client_secret}&refresh_token={refresh_token}", Method.POST);
                request.AddParameter("client_id", m.ClientId, ParameterType.UrlSegment);
                request.AddParameter("client_secret", m.ClientSecret, ParameterType.UrlSegment);
                request.AddParameter("refresh_token", SessionProfile.Usuario.refreshToken, ParameterType.UrlSegment);

                request.AddHeader("Accept", "application/json");

                var response = ExecuteRequest(request);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var token = JsonConvert.DeserializeAnonymousType(response.Content, new { refresh_token = "", access_token = "" });
                    SessionProfile.Usuario.accessToken = token.access_token;
                    SessionProfile.Usuario.refreshToken = token.refresh_token;
                }
                else
                {
                    throw new AuthorizationException();
                }

                m = new Meli(m.ClientId, m.ClientSecret, SessionProfile.Usuario.accessToken, SessionProfile.Usuario.refreshToken);
            }

            SessionProfile.Meli = m;
            //AuthorizationFailure(m);
        }

        public IRestResponse ExecuteRequest(RestRequest request)
        {
            client.UserAgent = sdkVersion;
            return client.Execute(request);
        }
    }
}