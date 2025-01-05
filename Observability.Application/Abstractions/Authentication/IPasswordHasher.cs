﻿namespace Observability.Application.Abstractions.Authentication;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}