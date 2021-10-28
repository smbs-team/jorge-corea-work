using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class UserJobNotification
    {
        public int JobNotificationId { get; set; }
        public Guid UserId { get; set; }
        public int? JobId { get; set; }
        public string JobType { get; set; }
        public string JobNotificationText { get; set; }
        public string JobNotificationType { get; set; }
        public string JobNotificationPayload { get; set; }
        public string ErrorMessage { get; set; }
        public bool Dismissed { get; set; }
        public DateTime CreatedTimestamp { get; set; }

        public virtual Systemuser User { get; set; }
    }
}
