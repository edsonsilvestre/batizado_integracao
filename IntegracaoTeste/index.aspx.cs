using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MercadoLibre.SDK;

namespace IntegracaoTeste
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long clientId = Convert.ToInt64("0" + System.Configuration.ConfigurationManager.AppSettings["ClientID"]);
            string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];

            Meli m = new Meli(clientId, clientSecret);

            Response.Redirect(m.GetAuthUrl("http://localhost:3000"));
        }
    }
}