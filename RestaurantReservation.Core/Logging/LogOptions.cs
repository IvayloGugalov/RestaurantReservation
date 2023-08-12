namespace RestaurantReservation.Core.Logging;

public class LogOptions
{
    public string Level { get; set; }
    public ElasticOptions Elastic { get; set; }

    public FileOptions File { get; set; }
    public string LogTemplate { get; set; }
}

public class ElasticOptions
{
    public bool Enabled { get; set; }
    public string ElasticServiceUrl { get; set; }
    public string ElasticSearchIndex { get; set; }
}
