using Hanamilegal.Web.Auth.Models;

namespace Hanamilegal.Web.AccountsApi.Configuration;

public record struct InitialUser(string Email, string Password, UserRole Role);
