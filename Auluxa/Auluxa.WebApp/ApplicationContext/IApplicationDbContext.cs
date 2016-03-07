using System;
using System.Threading.Tasks;

namespace Auluxa.WebApp.ApplicationContext
{
	public interface IApplicationDbContext : IDisposable
	{
		Task<int> SaveChangesAsync();
	}
}
