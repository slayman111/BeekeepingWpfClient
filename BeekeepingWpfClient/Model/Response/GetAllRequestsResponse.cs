using System;
using System.Collections.Generic;

namespace BeekeepingWpfClient.Model.Response;

public record GetAllRequestsResponse(
    int Id,
    GetAllRequestsResponse.UserDto User,
    GetAllRequestsResponse.ProductDto Product,
    short Amount,
    string Status,
    long DeliveryDate,
    long CreatedDate)
{
    public decimal TotalPrice => Amount * Product.Price;

    public string FormattedStatus => DeliveryStatuses[Status];

    public string FormattedDeliveryDate =>
        new DateTime(1970, 1, 1).AddMilliseconds(DeliveryDate).ToShortDateString();

    public string FormattedCreatedDate =>
        new DateTime(1970, 1, 1).AddMilliseconds(CreatedDate).ToShortDateString();

    public record UserDto(int Id, string FullName, string Email, string Phone, string Role);

    public record ProductDto(int Id, string Name, decimal Price, string ProductType, string Image);

    private static readonly Dictionary<string, string> DeliveryStatuses = new()
    {
        { "CREATED", "Создан" },
        { "DELIVERY", "Доставляется" },
        { "FINISHED", "Выдан" },
        { "CANCELED", "Отменен" }
    };
}
