﻿using Microsoft.AspNetCore.Identity;

namespace Worker.Model;

public class Users: IdentityUser
{
    public string? Cpf { get; set; }
    public int YearsOld { get; set; }
    public string Role { get; set; }
    public virtual List<Cart> CartTickets { get; set; }
    public decimal TotalPrice { get; set; }
}
