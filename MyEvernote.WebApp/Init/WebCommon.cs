using MyEvernote.Common;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WepApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (HttpContext.Current.Session["login"] != null)
            {
                EvernoteUser user = HttpContext.Current.Session["login"] as EvernoteUser;

                return user.Username;
            }
            return "system";
        }
    }
}