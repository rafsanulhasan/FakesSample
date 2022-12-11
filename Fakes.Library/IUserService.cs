using System;

namespace MsFakes.Library
{
    public interface IUserService
    {
        public User CreateUser(string name, string email)
        {
            var user = new User() { Name = name, Email = email };
            return user;
        }

        public User UpdateUser(User user)
        {
            user.LastModifiedDate = DateTime.UtcNow;
            return user;
        }
    }
}
