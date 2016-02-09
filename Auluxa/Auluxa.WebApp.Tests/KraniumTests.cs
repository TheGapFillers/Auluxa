using System;
using Moq;
using Auluxa.WebApp.Models;
using System.Data.Entity;
using Auluxa.WebApp.Repositories.Contexts;
using Auluxa.WebApp.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using NUnit.Framework;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class KraniumTests
	{
		[Test]
		public async Task GetKraniumAsyncTest()
		{
			var kraniumsMockedList = new List<Kranium>
			{
				new Kranium { Name = "TheKranium", IPAddress = "192.168.0.50" },
				//new Kranium { Name = "TheBackupKranium", IPAddress = "192.168.0.51" },
			}.AsQueryable();

			var mockSet = new Mock<DbSet<Kranium>>();
			mockSet.As<IDbAsyncEnumerable<Kranium>>()
				.Setup(m => m.GetAsyncEnumerator())
				.Returns(new TestDbAsyncEnumerator<Kranium>(kraniumsMockedList.GetEnumerator()));

			mockSet.As<IQueryable<Kranium>>()
				.Setup(m => m.Provider)
				.Returns(new TestDbAsyncQueryProvider<Kranium>(kraniumsMockedList.Provider));

			mockSet.As<IQueryable<Kranium>>().Setup(m => m.Expression).Returns(kraniumsMockedList.Expression);
			mockSet.As<IQueryable<Kranium>>().Setup(m => m.ElementType).Returns(kraniumsMockedList.ElementType);
			mockSet.As<IQueryable<Kranium>>().Setup(m => m.GetEnumerator()).Returns(kraniumsMockedList.GetEnumerator());

			var mockContext = new Mock<IApplicationDbContext>();
			mockContext.Setup(c => c.Kranium).Returns(mockSet.Object);

			var repository = new EfApplicationRepository { Context = mockContext.Object };
			var kranium = await repository.GetKraniumAsync();

			Assert.IsNotNull(kranium);
			Assert.AreEqual("TheKranium", kranium.Name);
			Assert.AreEqual("192.168.0.50", kranium.IPAddress);
		}
	}
}
