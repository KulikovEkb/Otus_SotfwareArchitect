using System.Threading.Tasks;
using FluentResults;

namespace SoftwareArchitect.Service.Billing
{
    internal interface IBillingService
    {
    }

    internal class BillingService : IBillingService
    {
        public async Task<Result> DepositAsync(long userId, decimal amount)
        {
            
        }
    }
    
    internal class 
}