using IssueNotification.Models;
using Microsoft.AspNet.WebHooks;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IssueNotification.WebHooks
{
    public class Handler : WebHookHandler
    {
        public override Task ExecuteAsync( string receiver, WebHookHandlerContext context )
        {
            string action = context.Actions.First();
            if ( action == "LicenseIssue" || action == "LicenseReissue" )
            {
                var data = context.GetDataOrDefault<WebHookNotification>();
                InMemoryLicenseNotificationRepository.LicenseNotifications.AddRange( data.Notifications.Where( x => x != null ) );
            }
            return Task.FromResult( true );
        }
    }
}