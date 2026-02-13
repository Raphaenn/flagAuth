using App.IRepository;
using Domain.Entities;
using Infra.Mappers;

namespace Infra.Repository;

public class IssueRepository : IIssueRepository
{
    private readonly InfraDbContext _infraDbContext;

    public IssueRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    }

    public async Task<Issues> CreateIssue(Issues issues)
    {
        var request = IssuesMapper.ToEntity(issues);
        await _infraDbContext.IssueDb.AddAsync(request);
        await _infraDbContext.SaveChangesAsync();
        return issues;
    }
}