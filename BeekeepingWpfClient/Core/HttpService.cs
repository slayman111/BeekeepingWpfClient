using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BeekeepingWpfClient.Exception;
using BeekeepingWpfClient.Model.Request;
using BeekeepingWpfClient.Model.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeekeepingWpfClient.Core;

public class HttpService : IDisposable
{
    private readonly HttpClient _httpClient;

    public HttpService(string? accessToken = null)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var basePath = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(basePath);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("/login", loginRequest);

        if (response.StatusCode != HttpStatusCode.OK) throw new InvalidCredentialsException();

        return new AuthResponse(
            response.Headers.GetValues("Authorization").First(),
            response.Headers.GetValues("JWT-Refresh-Token").First());
    }

    public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("/register", registerRequest);

        if (response.StatusCode != HttpStatusCode.OK) throw new BadRegistrationException();

        return true;
    }

    public async Task<List<GetAllRequestsResponse>?> GetRequestsAsync()
    {
        var response = await _httpClient.GetAsync("/admin/request");

        return JsonConvert.DeserializeObject<List<GetAllRequestsResponse>>(
            await response.Content.ReadAsStringAsync());
    }

    public void Dispose() => _httpClient.Dispose();
}
