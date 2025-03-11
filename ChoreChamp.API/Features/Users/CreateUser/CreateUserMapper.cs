﻿using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Users.CreateUser;

public class CreateUserMapper : Mapper<CreateUserRequest, CreateUserResponse, User>
{
    public override User ToEntity(CreateUserRequest r)
    {
        return new User
        {
            Name = r.Name,
            Email = r.Email,
            IsParent = r.IsParent
        };
    }

    public override CreateUserResponse FromEntity(User e)
    {
        return new CreateUserResponse(e.Id, e.Name, e.Email, e.IsParent, e.Points);
    }
}

