using CMSApi.Core.DTO;

namespace CMSApi.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Passwords { get; set; } = null!;
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
