using Auluxa.WebApp.Models;
using System.Linq;

namespace Auluxa.WebApp.Tests.Repositories
{
	public class TestApplianceModelDbSet : TestDbSet<ApplianceModel>
	{
		public override ApplianceModel Find(params object[] keyValues)
		{
			return this.SingleOrDefault(am => am.Id == (int)keyValues.Single());
		}
	}
}
