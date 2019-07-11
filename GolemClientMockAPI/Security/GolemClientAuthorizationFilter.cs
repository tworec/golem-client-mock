using GolemClientMockAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Security
{
    public class GolemClientAuthorizationFilter : Attribute, IActionFilter
    {
        public string DefaultNodeId { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // resolve the Bearer header here
            // and inject default if no header

            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var clientContext = new ClientContext();

                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

                token = token.Replace("Bearer ", "");

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var jwt = tokenHandler.ReadJwtToken(token);

                    clientContext.NodeId = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

                    if (clientContext.NodeId != null)
                    {
                        context.HttpContext.Items["ClientContext"] = clientContext;
                        return;
                    }
                }
                catch (Exception exc)
                {
                    // TODO Log the invalid token exception
                }

            }

            // if we are here - there was no proper authorization token in request
            {
                // context.Result = new StatusCodeResult(401); // short circuit to return status 401

                var clientContext = new ClientContext()
                {
                    NodeId = DefaultNodeId
                };

                context.HttpContext.Items["ClientContext"] = clientContext;
            }

        }
    }
}
