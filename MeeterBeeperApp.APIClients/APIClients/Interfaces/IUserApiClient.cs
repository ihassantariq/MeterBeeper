using MeeterBeeperApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeeterBeeperApp.APIClients.APIClients.Interfaces
{
    public interface IUserApiClient
    {
        Task<User> GetUser(string user);
        Task<List<Repository>> GetUserRepos(string user);
    }
}
