using WorkIT_Backend.Model;

namespace WorkIT_Backend.Api;

public class ResponsePostDto
{
    public string? ResponseText { get; set; }
    public string? CurriculumVitae { get; set; }
    public long UserId { get; set; }
    public long OfferId { get; set; }
}