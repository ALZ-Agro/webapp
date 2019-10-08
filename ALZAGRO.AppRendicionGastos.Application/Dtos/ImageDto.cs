using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {
    public class ImageDto: IDto {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public string Path { get; set; }
    }
}
