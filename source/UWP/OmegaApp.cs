﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Notifications;

using keep.grass.Helpers;

namespace keep.grass.UWP
{
    public class OmegaApp : AlphaApp
    {
        public OmegaApp()
        {
        }

        public override string getLanguage()
        {
            return Windows.System.UserProfile.GlobalizationPreferences.Languages[0].Split('-')[0];
        }

        public override void UpdateAlerts()
        {
            base.UpdateAlerts();

            if
            (
                String.IsNullOrWhiteSpace(Settings.UserName) ||
                null == Main.LastPublicActivity
            )
            {
                TileUpdateManager.GetTemplateContent(TileTemplateType.TileWidePeekImageAndText02);
            }
            else
            {
                var Limit = Main.LastPublicActivity.Value.AddHours(24);
                var LastPublicActivityInfo = L["Last Stamp: "] + Main.LastPublicActivity.Value.ToString("HH:mm");

                TileUpdateManager.GetTemplateContent(TileTemplateType.TileWidePeekImageAndText02);

            }
        }
        string MakeToastId(int id)
        {
            return "Toast" + id.ToString();
        }

        public override void ShowAlert(string title, string body, int id, DateTime notifyTime)
        {
            //  こんなコードを自前で用意しない為に Plugin.LocalNotifications を導入したのだが。。。
            //  UWP にも対応してるハズなのになぜか動作してくれない。

            CancelAlert(id);

            //  base code https://msdn.microsoft.com/ja-jp/library/windows/desktop/windows.ui.notifications.scheduledtoastnotification?cs-save-lang=1&cs-lang=csharp#code-snippet-1

            // Set up the notification text.
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode(title));
            strings[1].AppendChild(toastXml.CreateTextNode(body));

            // Create the toast notification object.
            var toast = new ScheduledToastNotification(toastXml, notifyTime);
            toast.Id = MakeToastId(id);

            // Add to the schedule.
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
        }
        public override void CancelAlert(int id)
        {
            var ToastId = MakeToastId(id);
            var Manager = ToastNotificationManager
                .CreateToastNotifier();
            foreach (var toast in Manager
                .GetScheduledToastNotifications()
                .Where(t => t.Id == ToastId))
            {
                Manager.RemoveFromSchedule(toast);
            }
        }
    }
}