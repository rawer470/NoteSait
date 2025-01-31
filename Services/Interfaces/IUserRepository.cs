using System;
using NoteSait.Models;
using NoteSait.Services.Interfaces;

namespace NoteSait.Services.Interfaces;

public interface IUserRepository : IRepository<User>
{
    void Update(User obj);
}
