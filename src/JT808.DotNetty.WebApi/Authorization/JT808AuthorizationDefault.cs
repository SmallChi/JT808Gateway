using DotNetty.Codecs.Http;
using DotNetty.Common.Utilities;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.Core.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace JT808.DotNetty.WebApi.Authorization
{
    class JT808AuthorizationDefault : IJT808WebApiAuthorization
    {
        private IOptionsMonitor<JT808Configuration> optionsMonitor;
        public JT808AuthorizationDefault(IOptionsMonitor<JT808Configuration> optionsMonitor)
        {
            this.optionsMonitor = optionsMonitor;
        }
        public bool Authorization(IFullHttpRequest request, out IPrincipal principal)
        {
            var uriSpan = request.Uri.AsSpan();
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
                if (request.Headers.TryGetAsString((AsciiString)"token", out tokenValue))
                {
                }
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
