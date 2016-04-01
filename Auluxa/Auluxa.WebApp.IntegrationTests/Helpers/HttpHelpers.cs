using System;
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
	}
}
