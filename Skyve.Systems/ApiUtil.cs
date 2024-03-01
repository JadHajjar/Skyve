using Extensions;

using Newtonsoft.Json;

using Skyve.Domain;
using Skyve.Domain.Systems;

using SkyveApi.Domain.Generic;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Systems;
public class ApiUtil(ILogger logger)
{
	private readonly ILogger _logger = logger;

	public async Task<T?> Get<T>(string url, params (string, object)[] queryParams)
	{
		return await Get<T>(url, new (string, string)[0], queryParams);
	}

	public async Task<T?> Get<T>(string url, (string, string)[] headers, params (string, object)[] queryParams)
	{
		return await Send<T>("GET", url, headers, queryParams);
	}

	public async Task<T?> Delete<T>(string url, params (string, object)[] queryParams)
	{
		return await Delete<T>(url, new (string, string)[0], queryParams);
	}

	public async Task<T?> Delete<T>(string url, (string, string)[] headers, params (string, object)[] queryParams)
	{
		return await Send<T>("DELETE", url, headers, queryParams);
	}

	private async Task<T?> Send<T>(string method, string baseUrl, (string, string)[] headers, params (string, object)[] queryParams)
	{
		var url = baseUrl;

		if (queryParams.Length > 0)
		{
			var query = queryParams.Select(x => $"{Uri.EscapeDataString(x.Item1)}={Uri.EscapeDataString(x.Item2.ToString())}");

			url += "?" + string.Join("&", query);
		}

		if (CrossIO.CurrentPlatform is not Platform.Windows)
		{
			var request = WebRequest.Create(url);

			request.Method = method;

			foreach (var item in headers)
			{
				request.Headers.Add(item.Item1, item.Item2);
			}

#if !STABLE
			_logger.Info($"[API] [GET] {baseUrl}");
#endif

			using var response = request.GetResponse();
			using var dataStream = response.GetResponseStream();
			using var reader = new StreamReader(dataStream);

			if (typeof(T) == typeof(byte[]))
			{
				using var memoryStream = new MemoryStream();
				reader.BaseStream.CopyTo(memoryStream);
				return (T)(object)memoryStream.ToArray();
			}

			var responseContent = reader.ReadToEnd();

			return JsonConvert.DeserializeObject<T>(responseContent);
		}

		using var httpClient = new HttpClient();

		foreach (var item in headers)
		{
			httpClient.DefaultRequestHeaders.Add(item.Item1, item.Item2);
		}

#if !STABLE
		_logger.Info($"[API] [GET] {baseUrl}");
#endif

		var httpResponse = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(method), new Uri(url)));

		if (httpResponse.IsSuccessStatusCode)
		{
			if (typeof(T) == typeof(byte[]))
			{
				return (T)(object)await httpResponse.Content.ReadAsByteArrayAsync();
			}

			var response = await httpResponse.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<T>(response);
		}

		_logger.Error($"[API] ({baseUrl}) failed: {httpResponse.ReasonPhrase}");

		return typeof(T) == typeof(ApiResponse)
			? (T)(object)new ApiResponse
			{
				Message = httpResponse.ReasonPhrase
			}
			: default;
	}

	public async Task<T?> Post<TBody, T>(string url, TBody body, params (string, object)[] queryParams)
	{
		return await Post<TBody, T>(url, body, new (string, string)[0], queryParams);
	}

	public async Task<T?> Post<TBody, T>(string baseUrl, TBody body, (string, string)[] headers, params (string, object)[] queryParams)
	{
		var url = baseUrl;
		var json = JsonConvert.SerializeObject(body);

		if (queryParams.Length > 0)
		{
			var query = queryParams.Select(x => $"{Uri.EscapeDataString(x.Item1)}={Uri.EscapeDataString(x.Item2.ToString())}");

			url += "?" + string.Join("&", query);
		}

		if (CrossIO.CurrentPlatform is not Platform.Windows)
		{
			var request = WebRequest.Create(url);

			request.Method = "POST";

			foreach (var item in headers)
			{
				request.Headers.Add(item.Item1, item.Item2);
			}

			var postDataBytes = Encoding.UTF8.GetBytes(json);

			request.ContentType = "application/json";
			request.ContentLength = postDataBytes.Length;

#if !STABLE
			_logger.Info($"[API] [POST] {baseUrl}");
#endif

			using (var requestStream = request.GetRequestStream())
			{
				requestStream.Write(postDataBytes, 0, postDataBytes.Length);
			}

			using var response = request.GetResponse();
			using var dataStream = response.GetResponseStream();
			using var reader = new StreamReader(dataStream);

			if (typeof(T) == typeof(byte[]))
			{
				using var memoryStream = new MemoryStream();
				reader.BaseStream.CopyTo(memoryStream);
				return (T)(object)memoryStream.ToArray();
			}

			var responseContent = reader.ReadToEnd();

			return JsonConvert.DeserializeObject<T>(responseContent);
		}

		using var httpClient = new HttpClient();

		foreach (var item in headers)
		{
			httpClient.DefaultRequestHeaders.Add(item.Item1, item.Item2);
		}

		var content = new StringContent(json, Encoding.UTF8, "application/json");
		var httpResponse = await httpClient.PostAsync(url, content);

#if !STABLE
		_logger.Info($"[API] [POST] {baseUrl}");
#endif

		if (httpResponse.IsSuccessStatusCode)
		{
			if (typeof(T) == typeof(byte[]))
			{
				return (T)(object)await httpResponse.Content.ReadAsByteArrayAsync();
			}

			var response = await httpResponse.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<T>(response);
		}

		_logger.Error($"[API] ({baseUrl}) failed: {httpResponse.ReasonPhrase}");

		return typeof(T) == typeof(ApiResponse)
			? (T)(object)new ApiResponse { Message = httpResponse.ReasonPhrase }
			: default;
	}
}
