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
using IntegracaoTeste.Domain.Entities;
using MercadoLibre.SDK;

namespace IntegracaoTeste.Sessions
{
    public static class SessionProfile
    {
        public static User Usuario
        {
            get
            {
                if (HttpContext.Current.Session["User"] == null)
                    return new User();

                return (User)HttpContext.Current.Session["User"];
            }
            set
            {
                HttpContext.Current.Session["User"] = value;
            }
        }

        public static Meli Meli
        {
            get
            {
                if (HttpContext.Current.Session["Meli"] == null)
                    return new Meli(Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]), System.Configuration.ConfigurationManager.AppSettings["ClientSecret"]);

                return (Meli)HttpContext.Current.Session["Meli"];
            }
            set
            {
                HttpContext.Current.Session["Meli"] = value;
            }
        }
    }
}