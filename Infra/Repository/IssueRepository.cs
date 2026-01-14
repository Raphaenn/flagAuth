using Domain.Entities;
using Infra.Mappers;

namespace Infra.Repository;

public class IssueRepository
{
    private readonly InfraDbContext _infraDbContext;

    public IssueRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    }

    public async Task<Guid> CreateUser(Issues issue)
    {
        var request = IssuesMapper.ToEntity(issue);
        await _infraDbContext.IssueDb.AddAsync(request);
        return request.Id;
    }
}