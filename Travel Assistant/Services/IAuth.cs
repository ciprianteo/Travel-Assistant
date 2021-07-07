using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Assistant.Services
{
    public interface IAuth
    {
        Task<string> LogIn(string email, string password);
        Task<string> Register(string email, string password);

        bool SignOut();
        bool IsSignedIn();
    }
}
