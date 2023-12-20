using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
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

    public async Task<List<GetAllProductsResponse>?> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync("/admin/product");

        return JsonConvert.DeserializeObject<List<GetAllProductsResponse>>(
            await response.Content.ReadAsStringAsync());
    }

    public async Task CreateProductAsync(CreateProductRequest createProductRequest, BitmapImage? image = null)
    {
        var requestContent = new MultipartFormDataContent();

        requestContent.Add(new StringContent(JsonConvert.SerializeObject(createProductRequest)), "product");

        if (image is not null)
        {
            requestContent.Headers.ContentType.MediaType = "multipart/form-data";
            requestContent.Add(new ByteArrayContent(ImageToByte(image)), "image",
                image.UriSource.ToString());
        }

        var response = await _httpClient.PostAsync("/admin/product", requestContent);

        if (response.StatusCode != HttpStatusCode.Created) throw new CantCreateProductException();
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/admin/product/{id}");

        if (response.StatusCode != HttpStatusCode.OK) throw new CantDeleteProductException();
    }

    public async Task<List<GetAllProductTypeResponse>?> GetProductTypesAsync()
    {
        var response = await _httpClient.GetAsync("/admin/product-type");

        return JsonConvert.DeserializeObject<List<GetAllProductTypeResponse>>(
            await response.Content.ReadAsStringAsync());
    }

    public void Dispose() => _httpClient.Dispose();

    private static byte[] ImageToByte(BitmapImage imageSource)
    {
        var encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(imageSource));

        using var ms = new MemoryStream();
        encoder.Save(ms);
        return ms.ToArray();
    }
}
