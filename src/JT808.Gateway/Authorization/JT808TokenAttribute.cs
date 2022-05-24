using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc;
using JT808.Gateway.Abstractions.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace JT808.Gateway.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public class JT808TokenAttribute : Attribute, IAuthorizationFilter
    {
        const string token = "token";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IOptions<JT808Configuration> jT808Configuration = context.HttpContext.RequestServices.GetRequiredService<IOptions<JT808Configuration>>();
            StringValues tokenValue;
            if (context.HttpContext.Request.Query.TryGetValue(token, out tokenValue))
            {
                if(jT808Configuration.Value.WebApiToken != tokenValue)
                {
                    JT808ResultDto<string> resultDto = new JT808ResultDto<string>();
                    resultDto.Message = "auth error";
                    resultDto.Data = "auth error";
                    resultDto.Code = 401;
                    context.Result =new OkObjectResult(resultDto);
                }
            }
            else if (context.HttpContext.Request.Headers.TryGetValue(token, out tokenValue))
            {
                if (jT808Configuration.Value.WebApiToken != tokenValue)
                {
                    JT808ResultDto<string> resultDto = new JT808ResultDto<string>();
                    resultDto.Message = "auth error";
                    resultDto.Data = "auth error";
                    resultDto.Code = 401;
                    context.Result = new OkObjectResult(resultDto);
                }
            }
            else
            {
                JT808ResultDto<string> resultDto = new JT808ResultDto<string>();
                resultDto.Message = "auth error";
                resultDto.Data = "auth error";
                resultDto.Code = 401;
                context.Result = new OkObjectResult(resultDto);
            }
        }
    }
}
