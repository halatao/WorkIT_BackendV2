using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkIT_Backend.Model;

public class User
{
    public User()
    {
        Offers = new HashSet<Offer>();
        Responses = new HashSet<Response>();
    }

    public long UserId { get; set; }
    public string? UserName { get; set; }
    public string? PasswordHash { get; set; }

    public long RoleId { get; set; }
    public virtual Role Role { get; set; }
    public virtual ICollection<Offer> Offers { get; set; }
    public virtual ICollection<Response> Responses { get; set; }
}