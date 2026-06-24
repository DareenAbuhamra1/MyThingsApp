using Microsoft.EntityFrameworkCore;
using MyThings.Core.Entities;

namespace MyThings.Core.DTOs;

public static class OrderQueryExtensions
{
    public static IQueryable<Order> IncludeOrderLineDetails(this IQueryable<Order> query)
    {
        return query
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.OrderLineOptions);
    }
    public static IQueryable<Order> IncludePartnerLocation(this IQueryable<Order> query)
    {
        return query
            .Include(o => o.Partner)
                .ThenInclude(pl => pl.Location);
    }
    public static IQueryable<Order> IncludeCustomerLocation(this IQueryable<Order> query)
    {
        return query
            .Include(o => o.Customer)
            .Include(o => o.DeliveryLocation);
    }
    public static IQueryable<Order> IncludeAllDetails(this IQueryable<Order> query)
    {
        return query
            .Include(o => o.Customer)
            .Include(o => o.DeliveryLocation)
            .Include(o => o.Partner)
                .ThenInclude(pl => pl.Location)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.OrderLineOptions);
    }
}