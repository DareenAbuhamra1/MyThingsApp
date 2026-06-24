using MyThings.Core.Entities;

namespace MyThings.Infrastructure.Extensions;

public static class PartnerQueryExtensions
{
    public static IQueryable<Partner> InDomain(this IQueryable<Partner> query, int domainId)
    {
        return query
            .Where(p => p.PartnerDomains.Any(d => d.DomainId == domainId));
    }
    public static IQueryable<Partner> SearchText(this IQueryable<Partner> query, string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return query;

        return query.Where(p =>
            p.Name.Contains(term) ||
            p.DescriptionEn.Contains(term) ||
            p.DescriptionAr.Contains(term));
    }
    public static IQueryable<Partner> SearchInCategories(this IQueryable<Partner> query, string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return query;

        return query.Where(p =>
            p.PartnerCategories.Any(pc =>
                pc.Category != null &&
                pc.Category.Name.Contains(term)));
    }
    public static IQueryable<Partner> SearchInProducts(this IQueryable<Partner> query, string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return query;

        return query.Where(p => 
            p.Products.Any(pp =>
                pp.AvailabilityId == 1 &&
                pp.Name.Contains(term)));
    }
    public static IQueryable<Partner> FullSearch(this IQueryable<Partner> query, string term)
    {
        return query
            .SearchText(term)
            .SearchInCategories(term)
            .SearchInProducts(term);
    }
}