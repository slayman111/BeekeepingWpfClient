using Newtonsoft.Json;

namespace BeekeepingWpfClient.Model.Request;

public record CreateRequestRequest(
    [property: JsonProperty("productId")] int ProductId,
    [property: JsonProperty("amount")] short Amount,
    [property: JsonProperty("deliveryDate")] long DeliveryDate);
