using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RestSharp;
using IntegracaoTeste.API;
using AutoMapper;
using System.Web.Routing;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Ninject;
using Ninject.Web;
using System.Reflection;
using System.Threading;
using System.Net.Http.Formatting;
using IntegracaoTeste.API.Models;
using System.Data.SqlClient;
using System.Data;

namespace IntegracaoBatizado.API.Controllers
{
    public class NotificationController : ApiController
    {
        public NotificationController()
        {
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post(NotificationModel notification)
        {
            System.Net.Http.HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (SqlConnection openCon = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=IntegracaoBatizado;Data Source=ED-NOTE"))
                {
                    string query = "INSERT into tblNotification (user_id,resource,topic,received,sent) VALUES (@user_id,@resource,@topic,@received,@sent)";

                    using (SqlCommand comm = new SqlCommand(query))
                    {
                        comm.Connection = openCon;
                        comm.Parameters.Add("@user_id", SqlDbType.Int, 10).Value = notification.user_id;
                        comm.Parameters.Add("@resource", SqlDbType.VarChar, 50).Value = notification.resource;
                        comm.Parameters.Add("@topic", SqlDbType.VarChar, 50).Value = notification.topic;
                        comm.Parameters.Add("@received", SqlDbType.DateTime, 20).Value = notification.received;
                        comm.Parameters.Add("@sent", SqlDbType.DateTime, 20).Value = notification.sent;

                        openCon.Open();

                        comm.ExecuteNonQuery();

                        openCon.Close();
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            Request.CreateResponse<NotificationModel>(System.Net.HttpStatusCode.OK, notification);

            return response;
        }
    }
}
