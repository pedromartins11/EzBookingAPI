using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EzBooking.Repository;
using System;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

[AttributeUsage(AttributeTargets.Method)]
public class AdminAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        if (user != null && user.HasClaim(c => c.Type == ClaimTypes.UserData && c.Value == "3"))
        {
            base.OnActionExecuting(context);
            return; // Acesso autorizado
        }
        else
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "Acesso não autorizado"
            };
        }

        
    }
}

public class AdvertiserAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        var userDataClaim = user?.FindFirst(c => c.Type == ClaimTypes.UserData);

        if (userDataClaim != null)
        {
            
            if (int.TryParse(userDataClaim.Value, out int userType) && userType >= 2)
            {
                base.OnActionExecuting(context);
                return; // Acesso autorizado
            }
        }

        context.Result = new ContentResult
        {
            StatusCode = 401,
            Content = "Acesso não autorizado"
        };
    }
}


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


        var userRepo = context.HttpContext.RequestServices.GetService(typeof(UserRepo)) as UserRepo;

        if (userRepo.TokenIsRevoked(token))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "Acesso não autorizado: Token revogado"
            };
        }

        if (!userRepo.IsTokenValid(token))
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

