using Auluxa.WebApp.Models;
using System.Linq;

namespace Auluxa.WebApp.Tests.Repositories
{
	public class TestZoneDbSet : TestDbSet<Zone>
	{
		public override Zone Find(params object[] keyValues)
		{
			return this.SingleOrDefault(z => z.Id == (int)keyValues.Single());
		}
	}
}
