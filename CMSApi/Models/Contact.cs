using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CMSApi.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public int? UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Company { get; set; }

    public string? JobTitle { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Notes { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
