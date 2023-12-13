using System;
using System.Collections.Generic;

namespace Airline_Project.Models;

public partial class TokenInfo
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime RefreshTokenExpiry { get; set; }
}
