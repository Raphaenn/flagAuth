using Api.Dto;
using MediatR;

namespace App.Auth.Commands;

public record struct RefreshTokenCommand(string RefreshToken) : IRequest<AuthTokenResponse>;