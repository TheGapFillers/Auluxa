using Auluxa.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
