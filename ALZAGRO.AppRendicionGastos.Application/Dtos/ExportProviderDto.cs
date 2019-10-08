namespace ALZAGRO.AppRendicionGastos.Application.Dtos
{
    public class ExportProviderDto {
        public string LegalName { get; set; }
        public long Cuit { get; set; }
        public string LegalCondition { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string ContactFullName { get; set; }
        public string Error { get; set; }
    }
}
