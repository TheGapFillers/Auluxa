using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Auluxa.WebApp.IntegrationTests.Helpers
{
	public class HttpHelpers
	{
		public static HttpRequestMessage CreateRequest(Uri uri, string mthv, HttpMethod method)
		{
			var request = new HttpRequestMessage();

			request.RequestUri = uri;
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
			request.Method = method;

			return request;
		}

		public static HttpRequestMessage CreateRequest<T>(Uri uri, string mthv, HttpMethod method, T content, MediaTypeFormatter formatter) where T : class
		{
			HttpRequestMessage request = CreateRequest(uri, mthv, method);
			request.Content = new ObjectContent<T>(content, formatter);

			return request;
		}

		public static T[] GetEntities<T>(Uri uri, HttpMessageHandler serverHandler)
		{
			using (HttpClient client = new HttpClient(serverHandler))
			using (HttpResponseMessage httpResponseMessage = client.GetAsync(uri).Result)
			{
				Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

				T[] entities = JsonConvert.DeserializeObject<T[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);

				return entities;
			}
		}
	}
}
