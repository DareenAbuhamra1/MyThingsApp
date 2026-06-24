namespace MyThings.Infrastructure.Extensions;

using MyThings.Core.Entities;

public static class RowValidityExtension
{
    public static IQueryable<Partner> IsValidRow(this IQueryable<Partner> query)
    {
        return query.
            Where(x => x.AvailabilityId == 1 && x.IsDeleted != false);
    }
}