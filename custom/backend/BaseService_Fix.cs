using ColetorA41.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ColetorA41.Services
{
    public class BaseServiceFix
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        public HttpClient _httpClient;
        private readonly IConfiguration _config;

        public BaseServiceFix(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("coletor");
            _httpClient.BaseAddress = new Uri(_config["BASE_URL"] ?? string.Empty);
            _httpClient.DefaultRequestHeaders.Add("x-totvs-server-alias", _config["ALIAS_APPSERVER"]);
            _httpClient.DefaultRequestHeaders.Add("CompanyId", _config["EMPRESA_PADRAO"]);
            
            // CORREÇÃO: Timeout finito em vez de infinito
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        protected async Task<T?> GetAsync<T>(string endpoint, NameValueCollection parameters = null)
        {
            var stringParam = new StringBuilder();

            if (!IsInternetAvailable())
            {
                return default;
            }

            if (parameters != null)
            {
                string str = "?";
                for (int index = 0; index < parameters.Count; ++index)
                {
                    stringParam.Append(str + parameters.AllKeys[index] + "=" + parameters[index]);
                    str = "&";
                }
            }

            try
            {
                // CORREÇÃO: Adicionar timeout específico para esta requisição
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25));
                
                var response = await _httpClient.GetAsync(endpoint + stringParam.ToString(), cts.Token);
                
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new Exception($"Erro HTTP: {response.StatusCode} - {response.ReasonPhrase}");
                }
                
                var responseStream = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(responseStream))
                {
                    Debug.WriteLine("Resposta vazia da API");
                    return default;
                }
                
                var data = JsonConvert.DeserializeObject<T>(responseStream);
                return data;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Timeout na requisição HTTP");
                throw new Exception("Timeout: A requisição demorou muito para responder.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro na requisição HTTP: {ex.Message}");
                throw new Exception($"Erro na comunicação com o servidor: {ex.Message}");
            }
        }

        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string metodo, TRequest requestBody = default)
        {
            var request = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = new Uri(Path.Combine(_config["BASE_URL"] ?? string.Empty, metodo)) };
            
            if (requestBody != null)
            {
                var json = JsonConvert.SerializeObject(requestBody);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            
            try
            {
                // CORREÇÃO: Timeout mais curto para evitar travamento
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25));
                var response = await _httpClient.SendAsync(request, cts.Token).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new Exception($"Erro HTTP: {response.StatusCode} - {response.ReasonPhrase}");
                }

                var responseStream = await response.Content.ReadAsStreamAsync();
                { 
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();
                    
                    if (string.IsNullOrEmpty(text))
                    {
                        Debug.WriteLine("Resposta vazia da API");
                        return default;
                    }
                    
                    var data = JsonConvert.DeserializeObject<TResponse>(text);
                    return data;
                };
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Timeout na requisição POST");
                throw new Exception("Timeout: A requisição demorou muito para responder.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro na requisição POST: {ex.Message}");
                throw new Exception($"Erro na comunicação com o servidor: {ex.Message}");
            }
        }

        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private bool IsInternetAvailable()
        {
            NetworkAccess accessType = Connectivity.NetworkAccess;

            if (accessType != NetworkAccess.Internet)
            {
                if (Shell.Current != null)
                {
                    if (accessType == NetworkAccess.ConstrainedInternet)
                    {
                        Shell.Current.DisplayAlert("Erro!", "Acesso à internet limitado.", "OK");
                    }
                    else
                    {
                        Shell.Current.DisplayAlert("Erro!", "Sem acesso à internet.", "OK");
                    }
                }

                return false;
            }

            return true;
        }
    }
} 