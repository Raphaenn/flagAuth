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

    private record struct GetUserRequest(string Id);
    
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
                Status = user.Status.ToString(),
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

        app.MapGet("/user/photos", async (HttpContext context, IMediator mediator) =>
        {
            string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            GetUserPhotosCommand cmd = new GetUserPhotosCommand(userId);
            var res = await mediator.Send(cmd);
            
            return Results.Ok(res);
        }).RequireAuthorization();

        // app.MapDelete("user/photo/:id", async (HttpContext ContextCallback, IMediator mediator) =>
        // {
        //     string 
        // }).RequireAuthorization();
    }
}