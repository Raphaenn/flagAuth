using Domain.Entities;
using MediatR;

namespace App.Issue.Commands;

public record struct CreateIssueCommand(Guid UserId, string Content) : IRequest<Issues>;