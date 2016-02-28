using Auluxa.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
