using MeeterBeeperApp.Data.Helpers;
using MeeterBeeperApp.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeeterBeeperApp.APIClients.Helpers
{
    public class HttpResponsHelper
    {
        public static event EventHandler LogOut;
        private static HttpResponsHelper httpResponsHelper;

        public HttpResponsHelper()
        {
            httpResponsHelper = this;
        }

        #region Public Implementation

        public static async Task<T> GetObjectFor<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var serialized = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(serialized);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorMessage = await GetErrorMessage(response);
                string displayMessage = GetDisplayMessage(errorMessage);
                if (!string.IsNullOrWhiteSpace(displayMessage))
                {
                    UserDialogsHelper.ShowNotification(displayMessage.ToString(), NotificationTypeEnum.Error, TimeSpan.FromSeconds(3));
                }

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                LogOut?.Invoke(httpResponsHelper, new EventArgs());
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                UserDialogsHelper.ShowNotification("There was an error in processing your request.", NotificationTypeEnum.Error);
            }
            else if (!response.IsSuccessStatusCode)
            {
                UserDialogsHelper.ShowNotification("There was an error while fetching data.", NotificationTypeEnum.Error);
            }
            return default(T);
        }

        private static string GetDisplayMessage(ErrorMessage errorMessage)
        {
            StringBuilder displayMessage = new StringBuilder();
            if (!string.IsNullOrEmpty(errorMessage.Message))
            {
                displayMessage.AppendLine(errorMessage.Message);
            }
            return displayMessage.ToString();
        }

        public static async Task<string> GetAsString(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        public static async Task<ErrorMessage> GetErrorMessage(HttpResponseMessage response)
        {
            var message = await response.Content.ReadAsStringAsync();
            ErrorMessage errorMessage = new ErrorMessage();
            if (message.StartsWith("{"))
            {
                errorMessage = JsonConvert.DeserializeObject<ErrorMessage>(message ?? string.Empty);
            }
            else if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
            {
                errorMessage.Message = message;
            }
            return errorMessage;
        }

        #endregion
    }
}
