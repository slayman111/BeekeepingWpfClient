namespace BeekeepingWpfClient.Model.Request;

public record RegisterRequest(string FullName, string PhoneNumber, string Email, string Login, string Password);
