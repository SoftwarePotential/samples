using IssueNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueNotification
{
    public static class InMemoryLicenseNotificationRepository
    {
        static InMemoryLicenseNotificationRepository()
        {
            LicenseNotifications = new List<IssueNotificationModel>();
        }
        public static List<IssueNotificationModel> LicenseNotifications { get; set; }
    }
}