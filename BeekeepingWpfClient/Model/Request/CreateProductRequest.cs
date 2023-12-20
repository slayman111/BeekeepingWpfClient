using Newtonsoft.Json;

namespace BeekeepingWpfClient.Model.Request;

public record CreateProductRequest(
    [property:JsonProperty("name")] string Name,
    [property:JsonProperty("price")] decimal Price,
    [property:JsonProperty("productTypeId")] int ProductTypeId);
