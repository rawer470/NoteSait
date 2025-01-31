using System;
using NoteSait.Data;
using NoteSait.Models;
using NoteSait.Services.Interfaces;

namespace NoteSait.Services;

public class UserRepository : Repository<User>, IUserRepository
{
     private readonly Context context;

    public UserRepository(Context context) : base(context)
    {
        this.context = context;
    }

    public void Update(User obj)
    {

    }
}
