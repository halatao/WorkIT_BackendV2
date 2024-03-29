﻿namespace WorkIT_Backend.Api;

public class Filter
{
    public List<long>? LocationIds { get; set; }
    public List<long>? CategoryIds { get; set; }
    public double SalaryMin { get; set; }
    public DateTime Created { get; set; }
    public string? Search { get; set; }
}