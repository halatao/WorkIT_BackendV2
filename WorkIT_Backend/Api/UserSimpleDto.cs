using WorkIT_Backend.Model;

namespace WorkIT_Backend.Api;

public class UserSimpleDto
{
    public long UserId { get; set; }
    public string? UserName { get; set; }
    public RoleDto? Role { get; set; }
}