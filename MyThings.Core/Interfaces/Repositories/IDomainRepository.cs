using MyThings.Core.Entities;

namespace MyThings.Core.Interfaces;

public interface IDomainRepository : IReadOnlyRepository<Domain>, IGenericRepository<Domain>
{
    
}