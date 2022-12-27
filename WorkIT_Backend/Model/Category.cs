namespace WorkIT_Backend.Model;

public class Category
{
    public long CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public ICollection<Offer> Offers { get; set; }

    public Category()
    {
        Offers = new HashSet<Offer>();
    }
}