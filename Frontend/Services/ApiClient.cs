using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using _225DAPM32.Models;

namespace _225DAPM32.Services
{
    public class ApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient CreateClient(bool withToken = true)
        {
            var client = _httpClientFactory.CreateClient("API");
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

            if (withToken && !string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public async Task<T> GetResultAsync<T>(string url, bool withToken = true)
        {
            var response = await CreateClient(withToken).GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
            return apiResponse == null ? default : apiResponse.Results;
        }

        public async Task<(bool Success, T Result, string? Message)> PostResultAsync<T>(string url, object body, bool withToken = true)
        {
            var json = JsonSerializer.Serialize(body);
            var response = await CreateClient(withToken).PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = string.IsNullOrWhiteSpace(content)
                ? null
                : JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);

            return (response.IsSuccessStatusCode, apiResponse == null ? default : apiResponse.Results, apiResponse?.Message);
        }

        public async Task<(bool Success, T Result, string? Message)> PutResultAsync<T>(string url, object body, bool withToken = true)
        {
            var json = JsonSerializer.Serialize(body);
            var response = await CreateClient(withToken).PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = string.IsNullOrWhiteSpace(content)
                ? null
                : JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);

            return (response.IsSuccessStatusCode, apiResponse == null ? default : apiResponse.Results, apiResponse?.Message);
        }

        public async Task<bool> DeleteAsync(string url, bool withToken = true)
        {
            var response = await CreateClient(withToken).DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
