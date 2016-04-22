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
    public class QuestionAnswerController : AuthenticatedBaseController
    {
        private readonly IQuestionAnswerApp _questionAnswerApp;

        static long clientId = Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]);
        static string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
        
        Meli m = new Meli(clientId, clientSecret);

        public QuestionAnswerController()
        {
            _questionAnswerApp = NinjectWebCommon.Kernel.Get<IQuestionAnswerApp>();
        }

        public ActionResult QuestionAnswer()
        {
            Autorizar(m);

            var questionAnswers = _questionAnswerApp.GetQuestionAnswerBySellerId(m, SessionProfile.Usuario.Id);

            Mapper.CreateMap<QuestionAnswer, QuestionAnswerModel>();
            Mapper.AssertConfigurationIsValid();

            var model = Mapper.Map<QuestionAnswerModel>(questionAnswers);

            return View("QuestionAnswer", model);
        }
    }
}
