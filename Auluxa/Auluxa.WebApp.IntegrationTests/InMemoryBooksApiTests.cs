using Auluxa.WebApp.IntegrationTests.Server;
using Auluxa.WebApp.IntegrationTests.Tests;
using NUnit.Framework;

namespace Auluxa.WebApp.IntegrationTests
{
    [TestFixture]
    public class InMemoryBooksApiTests : ZoneTest
    {
        public InMemoryBooksApiTests() : base(new InMemoryApiServer())
        {
        }
    }

    //[TestFixture]
    //public class InMemoryBookApiTests : BookApiTests
    //{
    //    public InMemoryBookApiTests(): base(new InMemoryApiServer())
    //    {
    //    }
    //}

    //[TestFixture]
    //public class InMemoryCreateBooksApiTests : CreateBooksApiTests
    //{
    //    public InMemoryCreateBooksApiTests(): base(new InMemoryApiServer())
    //    {
    //    }
    //}

    //[TestFixture]
    //public class InMemoryRemoveBooksApiTests : RemoveBooksApiTests
    //{
    //    public InMemoryRemoveBooksApiTests() : base(new InMemoryApiServer())
    //    {
    //    }
    //}
}