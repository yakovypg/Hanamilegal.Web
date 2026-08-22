using System;

namespace Hanamilegal.Web.Auth.Models;

public record struct AccessToken(string Token, DateTime ExpireDateUtc);