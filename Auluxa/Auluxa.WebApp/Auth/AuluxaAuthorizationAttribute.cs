using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Auluxa.WebApp.Tools;

namespace Auluxa.WebApp.Auth
{
    /// <summary>
    /// AuthorizationAttribute which supports claims as well as users and roles
    /// </summary>
    public class AuluxaAuthorizationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The list of claims restricting the entrance
        /// </summary>
        private static List<Claim> _claims = new List<Claim>();

        /// <summary>
        /// Format: {type:value,type:value,...}
        /// </summary>
        public string Claims
        {
            get
            {
                return string.Join(",", _claims.Select(c =>
                    $"{c.Type}:{c.Value}"
                ));
            }
            set
            {
                _claims = value.SplitAndTrim(",")
                    .Select(v => new Claim
                    (
                        v.SplitAndTrim(":").FirstOrDefault(),
                        v.SplitAndTrim(":").LastOrDefault()
                    )).ToList();
            }
        }

        /// <summary>
        /// Enhance authorization to also restrict by claims
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // Check user validity
            bool validUser = base.IsAuthorized(actionContext);
            if (!validUser)
                return false;

            // Get claims from user
            ClaimsIdentity identity = actionContext.ControllerContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (identity?.Claims == null)
                return false;

            // Check that the user has all claims to access the controller/action 
            bool validClaims = _claims.All(c => identity.Claims.Select(a =>
                $"{a.Type}:{a.Value}")
                .Contains($"{c.Type}:{c.Value}"));

            return validClaims;
        }
    }
}