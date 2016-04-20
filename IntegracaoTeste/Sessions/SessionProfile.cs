
using System.Collections.Generic;
using System.Web;
using IntegracaoTeste.Domain.Entities;

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
    }
}