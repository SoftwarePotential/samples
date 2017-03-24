using IssueNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueNotification
{
    public static class InMemoryLicenseEventRepository
    {
        static InMemoryLicenseEventRepository()
        {
            LicenseEvents = new List<IssueEventModel>();
        }
        public static List<IssueEventModel> LicenseEvents { get; set; }
    }
}