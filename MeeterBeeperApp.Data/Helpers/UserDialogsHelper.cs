using Acr.UserDialogs;
using MeeterBeeperApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Color = Xamarin.Forms.Color;

namespace MeeterBeeperApp.Data.Helpers
{
    public class UserDialogsHelper
    {
        #region Public Implementation

        public static IDisposable ShowNotification(string message, NotificationTypeEnum notificationType, TimeSpan? duration = null)
        {
            return UserDialogs.Instance.Toast(new ToastConfig(message.ToUpper())
            {
                BackgroundColor = GetBackgroundColorFor(notificationType),
                MessageTextColor = GetMessageTextColorFor(notificationType),
                Duration = duration ?? TimeSpan.FromSeconds(2.5),
                Position = ToastPosition.Bottom
            });
        }

        #endregion

        #region Private Implementation

        private static Color GetBackgroundColorFor(NotificationTypeEnum notificationType)
        {
            switch (notificationType)
            {
                case NotificationTypeEnum.Info:
                    return Color.FromHex("#BDE5F8");
                case NotificationTypeEnum.Success:
                    return Color.FromHex("#DFF2BF");
                case NotificationTypeEnum.Warning:
                    return Color.FromHex("#FEEFB3");
                case NotificationTypeEnum.Error:
                    return Color.FromHex("#FFD2D2");
                case NotificationTypeEnum.Network:
                    return Color.Black;
                default:
                    return Color.LightPink;
            }
        }

        private static Color GetMessageTextColorFor(NotificationTypeEnum notificationType)
        {
            switch (notificationType)
            {
                case NotificationTypeEnum.Info:
                    return Color.FromHex("#00529B");
                case NotificationTypeEnum.Success:
                    return Color.FromHex("#4F8A10");
                case NotificationTypeEnum.Warning:
                    return Color.FromHex("£9F6000");
                case NotificationTypeEnum.Error:
                    return Color.FromHex("#D8000C");
                case NotificationTypeEnum.Network:
                    return Color.White;
                default:
                    return Color.Pink;
            }
        }

        #endregion
    }
}
