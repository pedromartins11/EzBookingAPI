using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AuthenticationEzBooking.Repository;

[AttributeUsage(AttributeTargets.Method)]

public class AuthAuthorizeAttribute : ActionFilterAttribute
{

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorizationHeader.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "Acesso não autorizado"
            };
        }


        var authRepo = context.HttpContext.RequestServices.GetService(typeof(AuthRepo)) as AuthRepo;

        if (authRepo.TokenIsRevoked(token))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "Acesso não autorizado: Token revogado"
            };
        }

        if (!authRepo.IsTokenValid(token))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "Acesso não autorizado: Token inválido"
            };
        }

        base.OnActionExecuting(context);
        return; // Acesso autorizado
    }
}

