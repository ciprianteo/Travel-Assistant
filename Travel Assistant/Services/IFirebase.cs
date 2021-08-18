using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Models;

namespace Travel_Assistant.Services
{
    public interface IFirebase
    {
        void AddUserDocument(User user);
        void AddBadgeDocument(Badge badge);
        void AddTicket(Ticket ticket);
        void RemoveTicket(string TicketID);
        string GetUserEmail();
        Task<Dictionary<string, Ticket>> GetUserTickets();
        Task<User> GetUserDetails();
        Task<Badge> GetUserBadge();
        
    }
}
