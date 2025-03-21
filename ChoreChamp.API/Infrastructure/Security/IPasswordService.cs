﻿namespace ChoreChamp.API.Infrastructure.Security;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
