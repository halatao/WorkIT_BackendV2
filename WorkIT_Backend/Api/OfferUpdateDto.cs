﻿namespace WorkIT_Backend.Api;

public class OfferUpdateDto
{
    public long OfferId { get; set; }
    public string? OfferName { get; set; }
    public string? OfferDescription { get; set; }
    public long UserId { get; set; }
    public long CategoryId { get; set; }
    public long LocationId { get; set; }
    public double SalaryMin { get; set; }
    public double SalaryMax { get; set; }
}