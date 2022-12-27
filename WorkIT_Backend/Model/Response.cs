namespace WorkIT_Backend.Model;

public sealed class Response
{
    public long ResponseId { get; set; }
    public string? ResponseText { get; set; }
    public string? CurriculumVitae { get; set; }

    public long UserId { get; set; }
    public User? User { get; set; }
    public long OfferId { get; set; }
    public Offer? Offer { get; set; }
}