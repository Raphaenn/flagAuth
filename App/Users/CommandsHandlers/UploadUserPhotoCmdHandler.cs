using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class UploadUserPhotoCmdHandler : IRequestHandler<UploadUserPhotosCommand, UserPhotos>
{
    private readonly IUserPhotoRepository _userPhotoRepository;
    
    public UploadUserPhotoCmdHandler(IUserPhotoRepository userPhotoRepository)
    {
        _userPhotoRepository = userPhotoRepository;
    }
    
    public async Task<UserPhotos> Handle(UploadUserPhotosCommand request, CancellationToken cancellationToken)
    {
        Guid parsedUserId = Guid.Parse(request.UserId);
        var userPhoto = UserPhotos.UploadPhoto(parsedUserId, request.Url, request.IsProfile);
        await _userPhotoRepository.UploadPhoto(userPhoto);
        return userPhoto;
    }
}