using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkIT_Backend.Model;

public class Role
{
    public Role()
    {
        Users = new HashSet<User>();
    }

    public long RoleId { get; set; }
    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; }
}