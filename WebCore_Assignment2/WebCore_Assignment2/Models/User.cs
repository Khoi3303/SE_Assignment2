using System;
using System.Collections.Generic;

namespace WebCore_Assignment2.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public bool? Lock { get; set; }
}
