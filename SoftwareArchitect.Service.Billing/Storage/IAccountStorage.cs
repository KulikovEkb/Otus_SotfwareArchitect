using System.Threading.Tasks;
using FluentResults;

namespace SoftwareArchitect.Service.Billing.Storage
{
    internal interface IAccountStorage
    {
        Task<Result> CreateOrUpdateAsync(Account account);

        Task<Result<Account>> GetAsync(long userId);
    }
}