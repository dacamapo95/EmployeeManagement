namespace EmployeeManagement.API.Contracts.Requests.Auth;

public sealed record LoginRequest(string Email, string Password);
