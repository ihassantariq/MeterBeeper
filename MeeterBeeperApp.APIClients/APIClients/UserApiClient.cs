using MeeterBeeperApp.APIClients.APIClients.Interfaces;
using MeeterBeeperApp.APIClients.Helpers;
using MeeterBeeperApp.Data.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeeterBeeperApp.APIClients.APIClients
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        #region Interfaces

        internal interface IUserApi
        {
            [Get("/users/{user}")]
            Task<HttpResponseMessage> GetUser(string user);

            [Get("/users/{user}/repos")]
            Task<HttpResponseMessage> GetUserRepos(string user);
        }

        #endregion

        #region Constructors

        public UserApiClient(HttpClientProvider httpClientProvider) : base(httpClientProvider)
        { }

        #endregion

        #region Public Implementation

        public async Task<User> GetUser(string user)
        {
            try
            {
                if (!await IsConnectionAvailable())
                {
                    return null;
                }
                var response = await RestService
                    .For<IUserApi>(await GetHttpClient())
                    .GetUser(user);
                return await HttpResponsHelper.GetObjectFor<User>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Repository>> GetUserRepos(string user)
        {
            try
            {
                if (!await IsConnectionAvailable())
                {
                    return null;
                }
                var response = await RestService
                    .For<IUserApi>(await GetHttpClient())
                    .GetUserRepos(user);
                return await HttpResponsHelper.GetObjectFor<List<Repository>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
