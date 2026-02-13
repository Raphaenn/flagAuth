using App.IRepository;
using App.Issue.Commands;
using Domain.Entities;
using MediatR;

namespace App.Issue.CommandHandler;

public class CreateIssueCmdHandler : IRequestHandler<CreateIssueCommand, Issues>
{

    private readonly IIssueRepository _issueRepository;

    public CreateIssueCmdHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }
    
    public async Task<Issues> Handle(CreateIssueCommand request, CancellationToken ct)
    {
        Issues issue = Issues.CreateIssue(request.UserId, request.Content, DateTime.UtcNow);
        await _issueRepository.CreateIssue(issue);
        return issue;
    }
}