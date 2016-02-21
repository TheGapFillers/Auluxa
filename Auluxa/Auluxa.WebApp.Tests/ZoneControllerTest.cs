using Auluxa.WebApp.Controllers;
using Auluxa.WebApp.Models;
using Auluxa.WebApp.Repositories;
using Auluxa.WebApp.Tests.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class ZoneControllerTest
	{
		[Test]
		public async Task PostZoneTest()
		{
			var zoneController = new ZoneController(new EfApplicationRepository { Context = new TestDbContext()});
			Zone zone = GetTestZone();

			var result = await zoneController.Post(zone) as CreatedAtRouteNegotiatedContentResult<Zone>;

			Assert.IsNotNull(result);
			Assert.AreEqual(result.RouteName, "DefaultApi");
			Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
			Assert.AreEqual(result.Content.Name, zone.Name);
		}

		private Zone GetTestZone()
		{
			return new Zone()
			{
				Id = 1, Name = "Test Area", UserName = "Mr Tester",
				Appliances = new List<Appliance>
				{
					new Appliance { Id = 0, Name = "AC" }
				}
			};
		}
	}
}
