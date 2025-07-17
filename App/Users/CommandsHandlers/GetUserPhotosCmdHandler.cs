using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class GetUserPhotosCmdHandler : IRequestHandler<GetUserPhotosCommand, List<UserPhotos>>
{
    private readonly IUserPhotoRepository _userPhotoRepository;

    public GetUserPhotosCmdHandler(IUserPhotoRepository userPhotoRepository)
    {
        _userPhotoRepository = userPhotoRepository;
    }
    
    public async Task<List<UserPhotos>> Handle(GetUserPhotosCommand request, CancellationToken cancellationToken)
    {
        Guid parsedUserId = Guid.Parse(request.UserId);
        List<UserPhotos> userPhotos = await _userPhotoRepository.GetUserPhotos(parsedUserId);
        return userPhotos;
    }
}