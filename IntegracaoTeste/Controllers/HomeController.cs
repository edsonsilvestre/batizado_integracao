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
using System.Data.SqlClient;
using System.Data;

namespace IntegracaoTeste.Controllers
{
    public class HomeController : AuthenticatedBaseController
    {
        private readonly IUserApp _userApp;

        static long clientId = Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]);
        static string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];

        Meli m = new Meli(clientId, clientSecret);

        public HomeController()
        {
            _userApp = NinjectWebCommon.Kernel.Get<IUserApp>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Bagde()
        {
            int contador = 0;

            if(SessionProfile.Usuario.Id == null)
            {
                Autorizar(m);
                
                SessionProfile.Usuario = _userApp.GetMyUser(m);
            }

            string query = "SELECT * FROM tblNotification WHERE user_id = '" + SessionProfile.Usuario.Id + "'";
            string html_test = "";

            try
            {
                using (SqlConnection openCon = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=IntegracaoBatizado;Data Source=ED-NOTE"))
                {
                    using (SqlCommand comm = new SqlCommand(query))
                    {
                        comm.Connection = openCon;
                        openCon.Open();

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                contador++;
                                html_test += "<li><a href=\"http://localhost:3000/QuestionAnswer/QuestionAnswer\"><span class=\"label label-default\">Pergunta " + contador + "</span></a></li>";
                            }
                        }

                        openCon.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(new { Cont = contador, Html = html_test });
        }
    }
}