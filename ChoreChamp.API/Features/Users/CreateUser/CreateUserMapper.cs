using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Security;
using FastEndpoints;

namespace ChoreChamp.API.Features.Users.CreateUser;

public class CreateUserMapper : Mapper<CreateUserRequest, CreateUserResponse, User>
{
    public override CreateUserResponse FromEntity(User e)
    {
        return new CreateUserResponse(e.Id, e.Name, e.Email, e.IsAdmin, e.Points.Value);
    }
}
