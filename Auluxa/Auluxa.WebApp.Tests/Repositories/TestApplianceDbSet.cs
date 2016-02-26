using Auluxa.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auluxa.WebApp.Tests.Repositories
{
	public class TestApplianceDbSet : TestDbSet<Appliance>
	{
		public override Appliance Find(params object[] keyValues)
		{
			return this.SingleOrDefault(am => am.Id == (int)keyValues.Single());
		}
	}
}
