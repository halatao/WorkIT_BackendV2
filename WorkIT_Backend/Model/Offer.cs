namespace WorkIT_Backend.Model;

public sealed class Offer
{
    public long OfferId { get; set; }
    public string? OfferName { get; set; }
    public string? OfferDescription { get; set; }
    public double SalaryMin { get; set; }
    public double SalaryMax { get; set; }
    public DateTime Created { get; set; }

    public long UserId { get; set; }
    public long LocationId { get; set; }
    public User? User { get; set; }
    public long CategoryId { get; set; }
    public Location? Location { get; set; }
    public Category? Category { get; set; }
    public ICollection<Response> Responses { get; set; }

    public Offer()
    {
        Responses = new HashSet<Response>();
    }
}