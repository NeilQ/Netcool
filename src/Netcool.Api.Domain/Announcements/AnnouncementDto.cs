using System;
using System.ComponentModel.DataAnnotations;
using Netcool.Core.Helpers;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Announcements
{
    public class AnnouncementDto : AnnouncementSaveInput
    {
        public AnnouncementStatus Status { get; set; }

        public DateTime UpdateTime { get; set; }

        public string StatusDescription => Reflection.GetEnumDescription(Status);

        public string NotifyTargetTypeDescription => Reflection.GetEnumDescription(NotifyTargetType);
    }

    public class AnnouncementSaveInput : EntityDto
    {
        [StringLength(32, MinimumLength = 1)]
        public string Title { get; set; }

        public string Body { get; set; }

        public NotifyTargetType NotifyTargetType { get; set; }
    }
}
