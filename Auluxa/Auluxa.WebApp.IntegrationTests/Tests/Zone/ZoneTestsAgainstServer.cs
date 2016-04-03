using System;
using Auluxa.WebApp.IntegrationTests.Hosts;
using NUnit.Framework;

namespace Auluxa.WebApp.IntegrationTests
{
    [TestFixture]
    public class ZoneTestsAgainstServer : ZoneTestsImpl
    {
        public ZoneTestsAgainstServer(): base(new AspNetApiServer(ApiHost.URI))
        {
        }
    }
}