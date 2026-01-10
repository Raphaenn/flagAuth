using System.Security.Claims;
using Api.Dto;
using App.Users.Commands;
using App.Users.DTOs;
using App.Users.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Api.Abstractions.EndpointsDefinitions;

public class UserEndpointDef : IEndpointsDefinitions
{
    private record UpdatePhoneRequest(string Token, string Url1, string Url2, string Url3);

    public record UploadImage(bool IsPublic);
    
    private record struct ChangeStatus(string Status);

    private record struct UpdateLocation(string Location);
    private record struct ChangeName(string UserId, string Name);
    private record struct ChangeEmail(string Email);
    private record struct ChangePassword(string UserId, string OldPass, string NewPass);
    
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/users/create", async (HttpContext context, IMediator mediator) =>
        {
            var request = await context.Request.ReadFromJsonAsync<CreateUserRequest>();
            CreateUserCommand userCmd = new CreateUserCommand(request.Email);
            User response = await mediator.Send(userCmd);
            return Results.Ok(response);
        }).AllowAnonymous();

        app.MapGet("/user/info/{id}", async (string id, HttpContext context, IMediator mediator) =>
        {
            GetUserByIdQuery userQuery = new GetUserByIdQuery(id);
            GetUserPhotosCommand cmd = new GetUserPhotosCommand(id);
            
            User user = await mediator.Send(userQuery);
            List<UserPhotos> photos = await mediator.Send(cmd);

            GetUserResponse parsedRes = new GetUserResponse
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Name = user.Name,
                Birthdate = user.Birthdate,
                Country = user.Country,
                City = user.City,
                Sexuality = user.Sexuality.ToString(),
                SexualOrientation = user.SexualOrientation.ToString(),
                Height = user.Height,
                Weight = user.Weight,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                Status = user.Status,
                Pics = photos
            };
            return Results.Ok(parsedRes);
        });

        app.MapPut("/user/update", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                var request = await context.Request.ReadFromJsonAsync<CompleteUserRequest>();
                
                if (string.IsNullOrWhiteSpace(request?.Password))
                    return Results.BadRequest("Password cannot be empty");

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
                    Longitude = request.Longitude,
                    Status = request.Status
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
        
        app.MapPut("/user/change-name", async (HttpContext context, IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var request = await context.Request.ReadFromJsonAsync<ChangeName>(ct);
                
                if (string.IsNullOrWhiteSpace(request.Name))
                    return Results.BadRequest("Name cannot be empty");

                UpdateUserNameCommand userCommand = new UpdateUserNameCommand(Guid.Parse(request.UserId), request.Name);

                bool res = await mediator.Send(userCommand, ct);

                if (!res)
                {
                    throw new Exception("Invalid argument");
                }

                return Results.Ok(res);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
        
        app.MapPut("/user/change-password", async (HttpContext context, IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var request = await context.Request.ReadFromJsonAsync<ChangePassword>(ct);
                
                if (string.IsNullOrWhiteSpace(request.NewPass))
                    return Results.BadRequest("Password cannot be empty");

                var hasher = new PasswordHasher<object>();
                string passwordHash = hasher.HashPassword(null, request.NewPass);

                ChangePasswordCommand userCommand = new ChangePasswordCommand(Guid.Parse(request.UserId), request.OldPass, passwordHash);

                bool res = await mediator.Send(userCommand, ct);

                if (!res)
                {
                    throw new Exception("Invalid argument");
                }

                return Results.Ok(res);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        app.MapPost("/user/change-status", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var request = await context.Request.ReadFromJsonAsync<ChangeStatus>();

                if (userId == null)
                {
                    throw new Exception("Invalid user");
                }

                Guid parsedUserId = Guid.Parse(userId);
                UserStatus status = Enum.Parse<UserStatus>(request.Status, ignoreCase: true);
                Console.WriteLine(status);

                ChangeUserStatusCommand cmd = new ChangeUserStatusCommand(parsedUserId, status);
                User res = await mediator.Send(cmd);
                return Results.Ok(res);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }).RequireAuthorization();
        
        app.MapPost("/user/upload-photos", async (HttpContext context, IFormFile file, IMediator mediator) =>
        {
            try
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Results.BadRequest("User not found");
                } 
                
                var formData = context.Request.Form;
                var profile = formData["isProfile"].ToString();
                bool parsedIsProfile = bool.Parse(profile);
                
                var fileFolder = Path.Combine(Directory.GetCurrentDirectory(), "files");
                
                // Create folder if hot exists
                if (!Directory.Exists(fileFolder))
                    Directory.CreateDirectory(fileFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(fileFolder, fileName);

                // save file
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                UploadUserPhotosCommand upload = new UploadUserPhotosCommand(userId, fullPath, parsedIsProfile);
                UserPhotos photo = await mediator.Send(upload);

                return Results.Ok(new { FilePath = $"/files/{fileName}", data = photo });
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        }).RequireAuthorization();

        app.MapPost("/admin/upload-photos/{id}", async (string id, HttpContext context, IFormFileCollection files, IMediator mediator) =>
        {
            var formData = context.Request.Form;
            var profile = formData["isProfile"].ToString();
            bool parsedIsProfile = bool.Parse(profile);
            
            var fileFolder = Path.Combine(Directory.GetCurrentDirectory(), "files");
            var uploadedPhotos = new List<object>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue;
                }
                
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(fileFolder, fileName);
                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var upload = new UploadUserPhotosCommand(id, fullPath, parsedIsProfile);
                UserPhotos photo = await mediator.Send(upload);

                uploadedPhotos.Add(new
                {
                    FilePath = $"/files/{fileName}",
                    Data = photo
                });
                
            }
            
            return Results.Ok(uploadedPhotos);
        });

        app.MapGet("/user/photos", async (HttpContext context, IMediator mediator) =>
        {
            string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GetUserPhotosCommand cmd = new GetUserPhotosCommand(userId);
            var res = await mediator.Send(cmd);
            
            return Results.Ok(res);
        }).RequireAuthorization();

        app.MapPut("/user/location/{id}", async (IMediator mediator, UpdateLocation body, string id) =>
        {
            try
            {
                UpdateUserLocationCommand cmd = new UpdateUserLocationCommand(id, body.Location);
                await mediator.Send(cmd);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        // app.MapDelete("user/photo/:id", async (HttpContext ContextCallback, IMediator mediator) =>
        // {
        //     string 
        // }).RequireAuthorization();
    }
}
