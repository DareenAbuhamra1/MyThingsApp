using MyThings.Core.Entities;

public class PartnerSearchResult
{
    public Partner Partner { get; set; } = null!;

    public int Order { get; set; }

    public double Distance { get; set; }
}