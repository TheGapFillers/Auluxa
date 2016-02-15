using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Auluxa.WebApp.ErrorHandling
{
    public class AuluxaExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Text error implementation of an IHttpActionResult
        /// </summary>
        private class TextPlainErrorResult : IHttpActionResult
        {
            /// <summary>
            /// Injected request that triggered that error
            /// </summary>
            public HttpRequestMessage Request { private get; set; }

            /// <summary>
            /// Content of the message
            /// </summary>
            public string Content { private get; set; }

            /// <summary>
            /// Sends back the error message as an Internal Server Error
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(Content),
                    RequestMessage = Request
                };

                return Task.FromResult(response);
            }
        }

        /// <summary>
        /// Format the result of the error context to be send back.
        /// </summary>
        /// <param name="context"></param>
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new TextPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = $"We apologize but an unexpected error occurred. Please try again later. {context.Exception.Message}"
            };
        }
    }
}