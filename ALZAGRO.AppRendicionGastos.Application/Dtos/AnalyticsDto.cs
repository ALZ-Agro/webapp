using System.Collections.Generic;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class AnalyticsDto {
        public double TotalFuel { get; set; }
        public double TotalHotel { get; set; }
        public double TotalFood { get; set; }
        public double TotalOther { get; set; }
        public List<VendorAnalyticsDto> Vendors { get; set; }
    }
}
