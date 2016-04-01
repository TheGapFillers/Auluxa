﻿using System;
using System.Net.Http;

namespace Auluxa.WebApp.IntegrationTests.Hosts
{
    public interface IApiServer
    {
        Uri BaseAddress { get; }
        ApiServerHost Kind { get; }
        HttpMessageHandler ServerHandler { get; }
        void Start();
        void Stop();
    }
}