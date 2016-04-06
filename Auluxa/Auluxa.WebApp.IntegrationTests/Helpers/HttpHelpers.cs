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

		public static T[] GetEntities<T>(Uri uri, HttpMessageHandler serverHandler, bool assertSuccees = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
		{
			using (HttpClient client = new HttpClient(serverHandler))
			using (HttpResponseMessage httpResponseMessage = client.GetAsync(uri).Result)
			{
				Assert.AreEqual(assertSuccees, httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(expectedStatusCode, httpResponseMessage.StatusCode);

				T[] retrievedEntities = JsonConvert.DeserializeObject<T[]>(httpResponseMessage.Content.ReadAsStringAsync().Result);

				return retrievedEntities;
			}
		}

		public static T PostEntities<T>(Uri uri, HttpMessageHandler serverHandler, T entityToPost, bool assertSuccees = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK) where T : class
		{
			HttpRequestMessage request = CreateRequest(uri, "application/json", HttpMethod.Post, entityToPost, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(serverHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.AreEqual(assertSuccees, httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(expectedStatusCode, httpResponseMessage.StatusCode);

				if (expectedStatusCode == HttpStatusCode.InternalServerError)
					return null;

				T createdEntity = JsonConvert.DeserializeObject<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				return createdEntity;
			}
		}

		public static T PatchEntities<T>(Uri uri, HttpMessageHandler serverHandler, T entityToPatch, bool assertSuccees = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK) where T : class
		{
			HttpRequestMessage request = CreateRequest(uri, "application/json", new HttpMethod("PATCH"), entityToPatch, new JsonMediaTypeFormatter());
			using (HttpClient client = new HttpClient(serverHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.AreEqual(assertSuccees, httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(expectedStatusCode, httpResponseMessage.StatusCode);

				if (expectedStatusCode == HttpStatusCode.InternalServerError)
					return null;

				T modifiedEntity = JsonConvert.DeserializeObject<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				return modifiedEntity;
			}
		}

		public static T DeleteEntity<T>(Uri uri, HttpMessageHandler serverHandler, bool assertSuccees = true, HttpStatusCode expectedStatusCode = HttpStatusCode.OK) where T : class
		{
			HttpRequestMessage request = HttpHelpers.CreateRequest(uri, "application/json", HttpMethod.Delete);
			using (HttpClient client = new HttpClient(serverHandler))
			using (HttpResponseMessage httpResponseMessage = client.SendAsync(request).Result)
			{
				Assert.AreEqual(assertSuccees, httpResponseMessage.IsSuccessStatusCode);
				Assert.AreEqual(expectedStatusCode, httpResponseMessage.StatusCode);

				if (expectedStatusCode == HttpStatusCode.InternalServerError)
					return null;

				T deletedEntity = JsonConvert.DeserializeObject<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
				return deletedEntity;
			}
		}

	}
}
