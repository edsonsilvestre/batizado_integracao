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
    public class QuestionAnswerController : AuthenticatedBaseController
    {
        private readonly IQuestionAnswerApp _questionAnswerApp;
        private readonly IUserApp _userApp;

        static long clientId = Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]);
        static string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
        
        Meli m = new Meli(clientId, clientSecret);

        public QuestionAnswerController()
        {
            _questionAnswerApp = NinjectWebCommon.Kernel.Get<IQuestionAnswerApp>();
            _userApp = NinjectWebCommon.Kernel.Get<IUserApp>();
        }

        public ActionResult QuestionAnswer()
        {
            ////AutorizarTeste(SessionProfile.Meli);

            using (SqlConnection openCon = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=IntegracaoBatizado;Data Source=ED-NOTE"))
            {
                string query = "DELETE from tblNotification";

                using (SqlCommand comm = new SqlCommand(query))
                {
                    comm.Connection = openCon;

                    openCon.Open();

                    comm.ExecuteNonQuery();

                    openCon.Close();
                }
            }

            var questionAnswers = _questionAnswerApp.GetQuestionAnswerBySellerId(SessionProfile.Meli, SessionProfile.Usuario.Id);

            Mapper.CreateMap<QuestionAnswer, QuestionAnswerModel>();
            Mapper.AssertConfigurationIsValid();

            var model = Mapper.Map<QuestionAnswerModel>(questionAnswers);

            User user = new User();

            foreach(Question question in model.Questions)
            {
                user = _userApp.GetUserById(SessionProfile.Meli, question.from.Id);
                question.from.Name = user.nickname;
            }
            
            return View("QuestionAnswer", model);
        }

        public ActionResult Answer(string id, string descricao)
        {
            ////AutorizarTeste(SessionProfile.Meli);

            Return retorno = _questionAnswerApp.Answer(SessionProfile.Meli, id, descricao);

            if (retorno.Message.Split(';').Count() > 1)
            {
                if (retorno.Message.Split(';')[0] != "OK")
                    TempData["error"] = "Ocorreu o seguinte erro ao anunciar: " + retorno.Message.Split(';')[1];
            }

            return RedirectToAction("QuestionAnswer");
        }
    }
}
