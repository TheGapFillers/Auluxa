using System;
using Auluxa.WebApp.IntegrationTests.Hosts;
using NUnit.Framework;

namespace Auluxa.WebApp.IntegrationTests
{
    public static class ApiHost
    {
        public static readonly Uri URI = new Uri("http://localhost:57776/");
    }

    [TestFixture]
    public class ZoneTestsAgainstServer : ZoneTestsImpl
    {
        public ZoneTestsAgainstServer(): base(new AspNetApiServer(ApiHost.URI))
        {
        }
    }

    //[TestFixture]
    //public class AgainstServerBookApiTests : BookApiTests
    //{
    //    public AgainstServerBookApiTests(): base(new AspNetApiServer(ApiHost.URI))
    //    {
    //    }
    //}

    //[TestFixture]
    //public class AgainstServerCreateBooksApiTests : CreateBooksApiTests
    //{
    //    public AgainstServerCreateBooksApiTests() : base(new AspNetApiServer(ApiHost.URI))
    //    {
    //    }
    //}

    //[TestFixture]
    //public class AgainstServerRemoveBooksApiTests : RemoveBooksApiTests
    //{
    //    public AgainstServerRemoveBooksApiTests()  : base(new AspNetApiServer(ApiHost.URI))
    //    {
    //    }
    //}
}