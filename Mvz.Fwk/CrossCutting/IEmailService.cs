namespace ALZAGRO.AppRendicionGastos.Fwk.CrossCutting {

    public interface IEmailService {

        void SendGenericMail(string to, string subject, string message);

        void SendGenericMail(string subject, string message);
    }
}