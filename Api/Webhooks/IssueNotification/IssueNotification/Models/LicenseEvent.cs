using System;
using System.Linq;

namespace IssueNotification.Models
{
    public class IssueEventModel
    {
        public string Action { get; set; }
        public Guid LicenseRefId { get; set; }
        public string ActivationKey { get; set; }
        public Guid? CustomerId { get; set; }
    }

    public class WebHookNotification
    {
        public Guid Id { get; set; }
        public int Attempt { get; set; }
        public IssueEventModel[] Notifications { get; set; }
    }
}