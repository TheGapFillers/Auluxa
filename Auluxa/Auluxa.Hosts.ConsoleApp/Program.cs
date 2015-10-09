using System;
using Auluxa.WebApi;
using Microsoft.Owin.Hosting;

namespace Auluxa.Hosts.ConsoleApp
{
	class Program
	{

		private const int PortNumber = 9000;
		/// <summary>
		/// 
		/// </summary>
		static void Main()
		{
			string baseAddress = $"http://localhost:{PortNumber}/" ;

			// Start OWIN host 
			using (WebApp.Start<Startup>(url: baseAddress))
			{
				Console.WriteLine($"Started. Listening on port {PortNumber}.");
				Console.ReadLine();
			}  
		}
	}
}
