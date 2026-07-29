using System;

namespace api.varietyshop.com.tr.Core.Entities
{
    public class UserLoginLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // Hangi ajan?
        public string IpAddress { get; set; } = string.Empty; // Hangi IP?
        public string DeviceInfo { get; set; } = string.Empty; // Mac mi Windows mu, Hangi Tarayıcı?
        public DateTime LoginDate { get; set; } = DateTime.UtcNow; // Saat kaçta?
    }
}