using WorkIT_Backend.Model;

namespace WorkIT_Backend.Api;

public class UserFullDto
{
    public UserFullDto()
    {
        Offers = new List<OfferSimpleDto>();
        Responses = new List<ResponseSimpleDto>();
    }

    public long UserId { get; set; }
    public string? UserName { get; set; }
    public RoleDto? Role { get; set; }
    public ICollection<OfferSimpleDto> Offers { get; set; }
    public ICollection<ResponseSimpleDto> Responses { get; set; }
}