using System;
using System.Web;
using System.Web.UI;

namespace ModernisationChallenge
{
    public partial class SiteMaster : MasterPage
    {
        private string Token { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;

            var tokenCookie = Request.Cookies["token"];

            if (tokenCookie != null && Guid.TryParse(tokenCookie.Value, out var _))
            {
                Token = tokenCookie.Value;

                Page.ViewStateUserKey = Token;
            }
            else
            {
                Token = Guid.NewGuid().ToString("N");

                Page.ViewStateUserKey = Token;

                var responseCookie = new HttpCookie("token")
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection,
                    Value = Token
                };

                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += Page_PreLoad;
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["token"] = Page.ViewStateUserKey;
            }
            else if ((string)ViewState["token"] != Token)
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }
}
