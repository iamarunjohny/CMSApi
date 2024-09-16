using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CMSApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Passwords { get; set; } = null!;

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [JsonIgnore]  // Prevent serialization of related Contacts
    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
