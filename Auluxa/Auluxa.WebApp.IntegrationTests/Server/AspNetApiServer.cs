using System;
using System.Net.Http;

namespace Auluxa.WebApp.IntegrationTests.Server
{
    public class AspNetApiServer : IApiServer
    {
        private readonly Uri _baseAddress;

        public AspNetApiServer(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public Uri BaseAddress
        {
            get { return _baseAddress; }
        }

        public ApiServerHost Kind
        {
            get{ return ApiServerHost.AspNet;}
        }

        public HttpMessageHandler ServerHandler { get{return new HttpClientHandler();} }

        public void Start()
        {
           Console.WriteLine("Nothing to start....");
        }

        public void Stop()
        {
            Console.WriteLine("Nothing to stop....");
        }
    }
}