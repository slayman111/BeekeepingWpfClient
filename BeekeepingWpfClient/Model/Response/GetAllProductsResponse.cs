namespace BeekeepingWpfClient.Model.Response;

public record GetAllProductsResponse(int Id, string Name, decimal Price, string ProductType, string? Image);
