using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DosingApp.Models.AgroAssist;
using DosingApp.Models.AgroAssist.Dto;
using System.Threading;

namespace DosingApp.Services
{
    public class ApiService
    {
        public string Email { get; set; }
        public string Password { get; set; }

        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private string _cachedToken;
        private DateTime _tokenExpirationTime = DateTime.MinValue;

        public ApiService(string baseAddress)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        private async Task<string> GetValidTokenAsync()
        {
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpirationTime.AddSeconds(-30))
            {
                return _cachedToken;
            }
            Console.WriteLine("Токен устарел или отсутствует. Запрашиваем новый...");

            var loginData = new LoginRequestDto { Email = Email, Password = Password };
            string jsonPayload = JsonSerializer.Serialize(loginData, _jsonOptions);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("account/sign_in", content);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var authResult = JsonSerializer.Deserialize<LoginDto>(jsonResponse, _jsonOptions);

            _cachedToken = authResult.AccessToken;
            _tokenExpirationTime = DateTime.UtcNow.AddSeconds(authResult.AccessTokenExpiresIn);

            return _cachedToken;
        }

        public async Task<bool> IsServerReachableAsync(TimeSpan? timeout = null, CancellationToken userCt = default)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(userCt))
            {
                cts.CancelAfter(timeout ?? TimeSpan.FromSeconds(5));
                try
                {
                    using (var response = await _httpClient.GetAsync("account/users/current", HttpCompletionOption.ResponseHeadersRead, cts.Token).ConfigureAwait(false))
                    {
                        return true;   // любой статус = связь есть
                    }
                }
                catch (OperationCanceledException) when (userCt.IsCancellationRequested)
                {
                    throw;             // отменил пользователь
                }
                catch (OperationCanceledException)
                {
                    return false;      // таймаут
                }
                catch (HttpRequestException)
                {
                    return false;      // сеть/DNS/TLS на уровне .NET
                }
                catch (Exception ex)
                {
                    // на всякий случай — логируем и считаем «нет связи»
                    System.Diagnostics.Debug.WriteLine($"[Reach] {ex.GetType().Name}: {ex.Message}");
                    return false;
                }
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                string token = await GetValidTokenAsync();
                request.Headers.Add("x-token", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(jsonString, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка GET ({endpoint}): {ex.Message}");
                throw;
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                string jsonPayload = JsonSerializer.Serialize(data, _jsonOptions);

                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
                };

                string token = await GetValidTokenAsync();
                request.Headers.Add("x-token", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(jsonResponse, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка POST ({endpoint}): {ex.Message}");
                throw;
            }
        }

        public async Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                string jsonPayload = JsonSerializer.Serialize(data, _jsonOptions);

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, endpoint)
                {
                    Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
                };

                string token = await GetValidTokenAsync();
                request.Headers.Add("x-token", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(jsonResponse, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка PATCH ({endpoint}): {ex.Message}");
                throw;
            }
        }
    }
}
