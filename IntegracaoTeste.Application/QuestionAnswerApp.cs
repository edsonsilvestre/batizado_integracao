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
using System.IO;

namespace IntegracaoTeste.Application
{
    public class QuestionAnswerApp : IQuestionAnswerApp
    {
        public Entity.QuestionAnswer GetQuestionAnswerBySellerId(Meli m, string Id)
        {
            var ps = new List<Parameter>();

            var p = new Parameter();
            p.Name = "access_token";
            p.Value = m.AccessToken;
            ps.Add(p);

            p = new Parameter();
            p.Name = "seller_id";
            p.Value = Id;
            ps.Add(p);

            //https://api.mercadolibre.com/
            IRestResponse response = m.Get("/questions/search", ps);

            Entity.QuestionAnswer questionsAnswers = new Entity.QuestionAnswer();

            questionsAnswers = JsonConvert.DeserializeObject<Entity.QuestionAnswer>(response.Content);

            return questionsAnswers;
        }

        public Entity.Question GetQuestionById(Meli m, string Id)
        {
            var ps = new List<Parameter>();

            var p = new Parameter();
            p.Name = "access_token";
            p.Value = m.AccessToken;
            ps.Add(p);

            //https://api.mercadolibre.com/
            IRestResponse response = m.Get("/questions/" + Id, ps);

            Entity.Question question = new Entity.Question();

            question = JsonConvert.DeserializeObject<Entity.Question>(response.Content);

            return question;
        }

        public Entity.Return Answer(Meli m, string Id, string Descricao)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.mercadolibre.com/answers?access_token=" + m.AccessToken);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"question_id\":\"" + Id + "\"," +
                                    "\"text\":\"" + Descricao + "\"}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                string result;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                Entity.Question produto = JsonConvert.DeserializeObject<Entity.Question>(result);

                Entity.Return retorno = new Entity.Return();

                retorno.Message = "OK" + ";" + Id;

                return retorno;
            }
            catch (WebException exception)
            {
                string responseText;

                using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
                
                Entity.Return retorno = JsonConvert.DeserializeObject<Entity.Return>(responseText);

                retorno.Message = "NOK" + ";" + retorno.Message;

                return retorno;
            }
        }
    }
}
