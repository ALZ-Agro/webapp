using ALZAGRO.AppRendicionGastos.Fwk.Domain.Entities;
using System;

namespace ALZAGRO.AppRendicionGastos.Application.Dtos {

    public class RequestDto : IDto {

        public long Id { get; set; }

        public string Description { get; set; }

        public long UserId { get; set; }

        public string Username { get; set; }

        public DateTime AccidentDate { get; set; }

        public DateTime AgreementDate { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public string AccidentDateString { get; set; }

        public string AgreementDateString { get; set; }

        public string BirthDateString { get; set; }

        public double Compensation { get; set; }

        public double Apu { get; set; }

        public double Aditional { get; set; }
    }
}