using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Auluxa.WebApp.Tests
{
	public static class DbSetExtensions
	{
		public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(this IEnumerable<T> sourceList) where T : class
		{
			var queryable = sourceList.AsQueryable();
			var mockedDbSet = new Mock<DbSet<T>>();

			mockedDbSet.As<IDbAsyncEnumerable<T>>()
				.Setup(m => m.GetAsyncEnumerator())
				.Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));

			mockedDbSet.As<IQueryable<T>>()
				.Setup(m => m.Provider)
				.Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));

			mockedDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
			mockedDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
			mockedDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

			return mockedDbSet;
		}
	}
}
