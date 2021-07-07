using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Travel_Assistant.Models;

namespace Travel_Assistant.Services
{
    public interface IFirebase
    {
        void AddUserDocument(User user);
        void AddBadgeDocument(Badge badge);
        string GetUserEmail();
    }
}
