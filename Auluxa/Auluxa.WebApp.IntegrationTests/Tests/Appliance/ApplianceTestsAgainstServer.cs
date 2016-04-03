using System;
using Auluxa.WebApp.IntegrationTests.Hosts;
using NUnit.Framework;

namespace Auluxa.WebApp.IntegrationTests
{
	[TestFixture]
	public class ApplianceTestsAgainstServer : ApplianceTestsImpl
	{
		public ApplianceTestsAgainstServer(): base(new AspNetApiServer(ApiHost.URI))
		{
		}
	}
}