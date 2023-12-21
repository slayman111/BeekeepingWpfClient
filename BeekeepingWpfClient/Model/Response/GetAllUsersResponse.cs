namespace BeekeepingWpfClient.Model.Response;

public record GetAllUsersResponse(
    int Id,
    string FullName,
    string PhoneNumber,
    string Email,
    string Login,
    string Role,
    string Authority);
