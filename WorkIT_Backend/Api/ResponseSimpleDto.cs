using WorkIT_Backend.Model;

namespace WorkIT_Backend.Api;

public class ResponseSimpleDto
{
    public long ResponseId { get; set; }
    public OfferSimpleDto? Offer { get; set; }
}