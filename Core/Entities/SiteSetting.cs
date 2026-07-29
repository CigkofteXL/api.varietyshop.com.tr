using System;

namespace api.varietyshop.com.tr.Core.Entities
{
    public class SiteSetting
    {
        public int Id { get; set; } = 1; // Tek satırlık ayar tablosu
        public bool IsMaintenanceMode { get; set; } = false; // Bakım modu şalteri
        public string MaintenanceMessage { get; set; } = "SİBER AĞ GÜNCELLENİYOR. 10 DAKİKA SONRA BURADAYIZ.";
    }
}