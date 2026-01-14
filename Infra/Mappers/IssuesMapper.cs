using Domain.Entities;
using Infra.Models;

namespace Infra.Mappers;

public static class IssuesMapper
{
    public static IssueDbModel ToEntity(Issues domain)
    {
        return new IssueDbModel
        {
            Id = domain.Id,
            UserId = domain.UserId,
            Content = domain.Content,
            CreatedAt = domain.CreatedAt
        };
    }

    public static Issues ToDomain(IssueDbModel entity)
    {
        return Issues.Rehydrate(entity.Id, entity.UserId, entity.Content, entity.CreatedAt);
    }
}