using Moq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using Auluxa.WebApp.Kranium.Models;
using Auluxa.WebApp.Kranium.Repositories;
using Auluxa.WebApp.Tests.Repositories;
using NUnit.Framework;

namespace Auluxa.WebApp.Tests
{
	[TestFixture]
	public class KraniumTests
	{
		[Test]
		public async Task GetKraniumAsyncTest()
		{
			var kraniumsMockedList = new List<KraniumEntity>
			{
				new KraniumEntity { Name = "TheKranium", IPAddress = "192.168.0.50" },
				//new Kranium { Name = "TheBackupKranium", IPAddress = "192.168.0.51" },
			}.AsQueryable();

			var mockSet = new Mock<DbSet<KraniumEntity>>();
			mockSet.As<IDbAsyncEnumerable<KraniumEntity>>()
				.Setup(m => m.GetAsyncEnumerator())
				.Returns(new TestDbAsyncEnumerator<KraniumEntity>(kraniumsMockedList.GetEnumerator()));

			mockSet.As<IQueryable<KraniumEntity>>()
				.Setup(m => m.Provider)
				.Returns(new TestDbAsyncQueryProvider<KraniumEntity>(kraniumsMockedList.Provider));

			mockSet.As<IQueryable<KraniumEntity>>().Setup(m => m.Expression).Returns(kraniumsMockedList.Expression);
			mockSet.As<IQueryable<KraniumEntity>>().Setup(m => m.ElementType).Returns(kraniumsMockedList.ElementType);
			mockSet.As<IQueryable<KraniumEntity>>().Setup(m => m.GetEnumerator()).Returns(kraniumsMockedList.GetEnumerator());

			var mockContext = new Mock<IKraniumDbContext>();
			mockContext.Setup(c => c.Kranium).Returns(mockSet.Object);

			var repository = new EfKraniumRepository { Context = mockContext.Object };
			var kranium = await repository.GetKraniumAsync();

			Assert.IsNotNull(kranium);
			Assert.AreEqual("TheKranium", kranium.Name);
			Assert.AreEqual("192.168.0.50", kranium.IPAddress);
		}
	}
}
