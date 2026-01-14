using Domain.Entities;

namespace App.IRepository;

public interface IIssueRepository
{
    Task<Issues> CreateIssue(Issues issues);
}