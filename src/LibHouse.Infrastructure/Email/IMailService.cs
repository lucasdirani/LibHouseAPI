using LibHouse.Business.Monads;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Email
{
    public interface IMailService
    {
        Task<Result> SendEmailAsync(MailRequest mailRequest);
    }
}