using System.Security.Cryptography;
using Api.Dto;
using App.Users.Queries;
using MediatR;
using Api.Extensions;
using App.Auth.Commands;
using App.Auth.DTOs;
using App.Users.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using CompleteUserRequest = App.Auth.DTOs.CompleteUserRequest;

namespace Api.Abstractions.EndpointsDefinitions;

public class AuthEndpointDef : IEndpointsDefinitions
{
    private record Result(User User, string Token);

    private record VerifyRequest(string Email);
    
    private record LoginRequest(string Email, string? Password);

    private record VerifyUserResponse(User? User, string AccStatus, string? Token);
    
    private record UpdatePhoneRequest(string Token, string Url1, string Url2, string Url3);
    
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/auth/social-signup", async (HttpContext context, IMediator mediator) =>
        {
            foreach (var us in context.User.Claims)
            {
                Console.WriteLine(us.Value);
            }
            
            // if (!context.User.Identity?.IsAuthenticated ?? false)
            //     return Results.Unauthorized();

            var userMail = context.User.FindFirst("emailAddress")?.Value;

             try
             {
                 // Validate toke with public key
                 // RSA publicKey = PublicKeyLoader.LoadPublicKey();
                 // var validatedClaims = JwtTokenValidator.ValidateJwtToken(token, publicKey);
            
                 if (userMail == null)
                 {
                     return Results.BadRequest("Invalid token data");
                 }
                 
                 // Check user data
                 GetUserQuery userQuery = new GetUserQuery
                 {
                     Email = userMail
                 };
                 
                 User? userData = await mediator.Send(userQuery);
            
                 // Validate if user exists
                 if (userData == null)
                 {
                     return Results.BadRequest("User not found");
                 }
                 
                 // create the real auth token
                 CreateAuthCommand createToken = new CreateAuthCommand
                 {
                     Id = userData.Id,
                     Email = userData.Email,
                     Name = userData.Name
                 };
                 string createdToken = await mediator.Send(createToken);
            
                 Result res = new Result(userData, createdToken);
                 
                 return Results.Ok(res);
             }
             catch (Exception e)
             {
                 return Results.BadRequest(e.Message);
             }
        }).RequireAuthorization();

        app.MapPost("/auth/email-signup", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                // get body
                var request = await context.Request.ReadFromJsonAsync<SignUpRequest>();
            
                // search by user mail
                GetUserQuery userQuery = new GetUserQuery
                {
                    Email = request.Email
                };

                User? userData = await mediator.Send(userQuery);

                // return if already exists
                if (userData != null)
                {
                    return Results.BadRequest("User already exists");
                }
            
                // create new user
                CreateUserCommand userCmd = new CreateUserCommand(request.Email);
                User response = await mediator.Send(userCmd);
            
                // Create a session token
                CreateAuthCommand createToken = new CreateAuthCommand
                {
                    Id = response.Id,
                    Email = request.Email,
                };
                string createdToken = await mediator.Send(createToken);
                
                // Anonymous object 
                // var user = new
                // {
                //     id = 123,
                //     name = "Lionel Messi",
                //     email = "messie@gmail.com"
                // };
                //
                // return Results.Created($"/users/{user.id}", new { user });
            
                return Results.Created("/auth/signup", new Result(User: response, Token: createdToken));
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        app.MapPost("/auth/verify", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                var request = await context.Request.ReadFromJsonAsync<VerifyRequest>();

                if (request.Email == null)
                {
                    return Results.BadRequest("Invalid email");
                }

                GetUserQuery getUserQuery = new GetUserQuery
                {
                    Email = request.Email
                };

                User user = await mediator.Send(getUserQuery);
                
                // return an specific acc status if user wasn't registered
                if (user == null)
                {
                    return Results.Ok(new VerifyUserResponse(User: null, AccStatus: "needRegisterAccount", Token: null));
                }

                // if user exists and isn't incomplete
                if (user.Name is null)
                {
                    CreateAuthCommand createSession = new CreateAuthCommand
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name
                    };

                    string createdSession = await mediator.Send(createSession);
                    
                    return Results.Created("/auth/verify", new VerifyUserResponse(User: user, AccStatus: "incompleteAccount", Token: createdSession));
                }
                
                return Results.Ok(new VerifyUserResponse(User: user, AccStatus: "needCompleteAuth", Token: null));
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        
        app.MapPost("/auth/signin/email", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                LoginRequest request = await context.Request.ReadFromJsonAsync<LoginRequest>();

                GetUserQuery userQuery = new GetUserQuery
                {
                    Email = request.Email
                };

                User? user = await mediator.Send(userQuery);

                if (user == null)
                {
                    return Results.BadRequest("User not found");
                }
                
                CreateAuthCommand createToken = new CreateAuthCommand
                {
                    Id = user.Id,
                    Email = user.Email,
                };
                
                string createdToken = await mediator.Send(createToken);

                if (createdToken == null)
                {
                    return Results.BadRequest("Create token error");
                }
                
                string refreshToken = Guid.NewGuid().ToString();
                DateTime expires = DateTime.UtcNow.AddDays(7);

                // Salva o refresh token no banco, junto com data de expiração e o userId
                await mediator.Send(new SaveRefreshTokenCommand(user.Id, refreshToken, expires));
            
                Result res = new Result(user, createdToken);
                
                return Results.Ok(res);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        
        app.MapPost("/auth/signin/email-password", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                LoginRequest request = await context.Request.ReadFromJsonAsync<LoginRequest>();

                GetUserQuery userQuery = new GetUserQuery
                {
                    Email = request.Email
                };

                User? user = await mediator.Send(userQuery);

                if (user == null || user.Password == null)
                {
                    return Results.Unauthorized();;
                }

                var hasher = new PasswordHasher<object>();
                var result = hasher.VerifyHashedPassword(null, user.Password, request.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return Results.Unauthorized();
                }
                
                CreateAuthCommand createToken = new CreateAuthCommand
                {
                    Id = user.Id,
                    Email = user.Email,
                };
                
                string createdToken = await mediator.Send(createToken);
            
                string refreshToken = Guid.NewGuid().ToString();
                DateTime expires = DateTime.UtcNow.AddDays(7);

                // Salva o refresh token no banco, junto com data de expiração e o userId
                await mediator.Send(new SaveRefreshTokenCommand(user.Id, refreshToken, expires));
                
                var data = new
                {
                    AccessToken = createdToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expires,
                    User = user
                };
            
                return Results.Ok(data);
                
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        app.MapPost("/auth/complete", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                var request = await context.Request.ReadFromJsonAsync<CompleteUserRequest>();
                
                if (string.IsNullOrWhiteSpace(request.Password))
                    return Results.BadRequest("Password cannot be empty");


                if (request is null || request.Id is null)
                {
                    return Results.Unauthorized();
                }
                
                var hasher = new PasswordHasher<object>();
                string passwordHash = hasher.HashPassword(null, request.Password);

                UpdateUserCommand completeUser = new UpdateUserCommand
                {
                    Id = request.Id,
                    Name = request.Name,
                    BirthDate = request.BirthDate,
                    Country = request.Country,
                    City = request.City,
                    Sexuality = request.Sexuality,
                    SexualOrientation = request.SexualOrientation,
                    Password = passwordHash,
                    Height = request.Height,
                    Weight = request.Weight,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                };

                User user = await mediator.Send(completeUser);

                return Results.Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        });

        app.MapPost("/auth/update-photos", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                var request = await context.Request.ReadFromJsonAsync<UpdatePhoneRequest>();
                string userEmail = context.User.FindFirst("emailAddress")?.Value;
                
                // call command to send photos
                
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        
        app.MapPost("/auth/refresh", async (
            RefreshTokenRequest req,
            IMediator mediator
            ) =>
        {
            try
            {
                var result = await mediator.Send(new RefreshTokenCommand(req.RefreshToken));
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
    }
}