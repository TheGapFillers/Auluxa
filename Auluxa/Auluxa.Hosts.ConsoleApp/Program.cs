using System;
using Auluxa.WebApi;
using Microsoft.Owin.Hosting;

namespace Auluxa.Hosts.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        static void Main()
        {
            string baseAddress = "http://*:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Started.");
                Console.ReadLine();
            }  
        }
    }
}
