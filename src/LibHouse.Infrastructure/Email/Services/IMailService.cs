using LibHouse.Business.Monads;
using LibHouse.Infrastructure.Email.Models;
using System.Threading.Tasks;

namespace LibHouse.Infrastructure.Email.Services
{
    public interface IMailService
    {
        Task<Result> SendEmailAsync(MailRequest mailRequest);
    }
}