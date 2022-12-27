using WorkIT_Backend.Model;

namespace WorkIT_Backend.Api;

public class OfferDto
{
    public OfferDto()
    {
        Responses = new List<ResponseDto>();
    }

    public long OfferId { get; set; }
    public string? OfferName { get; set; }
    public string? OfferDescription { get; set; }
    public double SalaryMin { get; set; }
    public double SalaryMax { get; set; }
    public UserDto? User { get; set; }
    public LocationDto? Location { get; set; }
    public CategoryDto? Category { get; set; }
    public ICollection<ResponseDto> Responses { get; set; }
}