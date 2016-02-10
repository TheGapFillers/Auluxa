using System;
using Moq;
using Auluxa.WebApp.Models;
using System.Data.Entity;
using Auluxa.WebApp.Repositories.Contexts;
using Auluxa.WebApp.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class ZonesRepositoryTests
	{
		public IQueryable<Zone> MockedZonesList { get; set; }
		public IQueryable<Appliance> MockedAppliancesList { get; set; }

		public Mock<DbSet<Zone>> MockedZonesDbSet { get; set; }
		public Mock<DbSet<Appliance>> MockedAppliancesDbSet { get; set; }

		public Mock<IApplicationDbContext> MockedContext { get; set; }
		public EfApplicationRepository MockedRepository { get; set; }

		[SetUp]
		public void SetUp()
		{
			var appl1 = new Appliance
			{
				Id = 1,
				Name = "Appliance1",
				Model = new ApplianceModel(),
			};
			var appl2 = new Appliance
			{
				Id = 2,
				Name = "Appliance2",
				Model = new ApplianceModel(),
			};
			var appl3 = new Appliance
			{
				Id = 3,
				Name = "Appliance3",
				Model = new ApplianceModel(),
			};

			MockedAppliancesList = new List<Appliance>
			{
				appl1, appl2, appl3
			}.AsQueryable();

			MockedZonesList = new List<Zone>
			{
				new Zone { Id = 1, Name = "Bed Room", Appliances = new List<Appliance> { appl1, appl2 } },
				new Zone { Id = 2, Name = "Kitchen", Appliances = new List<Appliance> { appl3 } },
			}.AsQueryable();

			MockedZonesDbSet = MockedZonesList.GetQueryableMockDbSet();
			MockedAppliancesDbSet = MockedAppliancesList.GetQueryableMockDbSet();

			MockedContext = new Mock<IApplicationDbContext>();
			MockedContext.Setup(c => c.Zones).Returns(MockedZonesDbSet.Object);
			MockedContext.Setup(c => c.Appliances).Returns(MockedAppliancesDbSet.Object);

			MockedRepository = new EfApplicationRepository { Context = MockedContext.Object };
		}

		[Test]
		public async Task GetZonesAsyncTest()
		{
			IEnumerable<Zone> zones = await MockedRepository.GetZonesAsync();	//todo understand failure when including appliance
			Assert.IsNotNull(zones);
			Assert.AreEqual(2, zones.Count());

			var bedRoom = zones.FirstOrDefault(z => z.Name == "Bed Room");
			Assert.IsNotNull(bedRoom);
			Assert.AreEqual(2, bedRoom.Appliances.Count());

			var kitchen = zones.FirstOrDefault(z => z.Name == "Kitchen");
			Assert.IsNotNull(kitchen);
			Assert.AreEqual(1, kitchen.Appliances.Count());
		}

		[Test]
		public async Task CreateZoneAsyncTest()
		{
			Zone newZone = new Zone { Name = "Living Room" };

			await MockedRepository.CreateZoneAsync(newZone);

			MockedContext.Verify(c => c.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task UpdateZoneAsyncTest()
		{
			Zone zoneToUpdate = new Zone { Id =  1, Name = "Renovated Bed Room" };

			await MockedRepository.UpdateZoneAsync(zoneToUpdate);

			MockedContext.Verify(c => c.SaveChangesAsync(), Times.Once);
		}

		[Test]
		public async Task UpdateNonExistingZoneAsyncTest()
		{
			Zone zoneToUpdate = new Zone { Id = 4, Name = "Area51" };

			await MockedRepository.UpdateZoneAsync(zoneToUpdate);

			MockedContext.Verify(c => c.SaveChangesAsync(), Times.Never);
		}
	}
}
