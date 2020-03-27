using System.Threading.Tasks;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface ICustomer2FAVerificationService : IBaseService<Customer2FAVerification>
  {
    string GenerateVerificationCode();
    Task<bool> CheckVerficationCode(string phoneNo, string verificationCode);
    Task<bool> VerifyCode(string phoneNo, string verificationCode);
    Task<bool> InvalidateAllVerificationCode(string phoneNo);
  }
}