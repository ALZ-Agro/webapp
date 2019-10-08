using System;

namespace ALZAGRO.AppRendicionGastos.WebUI.Infrastructure.Core {

    public class FileUploadResult {

        public String LocalFilePath { get; set; }

        public String FileName { get; set; }

        public Int64 FileLength { get; set; }
    }
}