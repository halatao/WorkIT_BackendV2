using WorkIT_Backend.Model;

namespace WorkIT_Backend.Api;

public class ResponseDto
{
    public long ResponseId { get; set; }
    public string? ResponseText { get; set; }
    public string? CurriculumVitae { get; set; }
    public UserDto? User { get; set; }
}