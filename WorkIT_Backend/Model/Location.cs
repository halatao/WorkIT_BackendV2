namespace WorkIT_Backend.Model;

public class Location
{
    public long LocationId { get; set; }
    public string? LocationName { get; set; }

    public ICollection<Offer> Offers { get; set; }

    public Location()
    {
        Offers = new HashSet<Offer>();
    }
}