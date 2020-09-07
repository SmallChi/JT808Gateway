using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace JT808.Gateway.Authorization
{
    class JT808AuthorizationDefault : IJT808Authorization
    {
        private IOptionsMonitor<JT808Configuration> optionsMonitor;
        public JT808AuthorizationDefault(IOptionsMonitor<JT808Configuration> optionsMonitor)
        {
            this.optionsMonitor = optionsMonitor;
        }
        public bool Authorization(HttpListenerContext context, out IPrincipal principal)
        {
            var uriSpan = context.Request.Url.AbsoluteUri.AsSpan();
            var uriParamStr = uriSpan.Slice(uriSpan.IndexOf('?')+1).ToString().ToLower();
            var uriParams = uriParamStr.Split('&');
            var tokenParam = uriParams.FirstOrDefault(m => m.Contains("token"));
            string tokenValue = string.Empty;
            if (!string.IsNullOrEmpty(tokenParam))
            {
                tokenValue = tokenParam.Split('=')[1];
            }
            else
            {
                tokenValue = context.Request.Headers.Get("token");
            }
            if (optionsMonitor.CurrentValue.WebApiToken == tokenValue)
            {
                principal = new ClaimsPrincipal(new GenericIdentity(tokenValue));
                return true;
            }
            else
            {
                principal = null;
                return false;
            }
        }
    }
}
