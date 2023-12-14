using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BeekeepingWpfClient.Exception;
using BeekeepingWpfClient.Model.Request;
using BeekeepingWpfClient.Model.Response;
using Microsoft.Extensions.Configuration;

namespace BeekeepingWpfClient.Core;

public class HttpService
{
    private readonly string _basePath;

    public HttpService()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();


        _basePath = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
    {
        using var client = new HttpClient();

        var response = await client.PostAsJsonAsync(_basePath + "/login", loginRequest);

        if (response.StatusCode != HttpStatusCode.OK) throw new InvalidCredentialsException();

        return new AuthResponse(
            response.Headers.GetValues("Authorization").First(),
            response.Headers.GetValues("JWT-Refresh-Token").First());
    }
}
