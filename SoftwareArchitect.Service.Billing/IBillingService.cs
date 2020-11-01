using System;
using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Service.Billing.Models;
using SoftwareArchitect.Service.Billing.Storage;

namespace SoftwareArchitect.Service.Billing
{
    internal interface IBillingService
    {
        Task<Result<decimal>> DepositAsync(long userId, decimal amount);

        Task<Result<decimal>> WithdrawAsync(long userId, decimal amount);
    }

    internal class BillingService : IBillingService
    {
        private readonly IAccountStorage accountStorage;

        public BillingService(IAccountStorage accountStorage)
        {
            this.accountStorage = accountStorage;
        }

        public async Task<Result<decimal>> DepositAsync(long userId, decimal amount)
        {
            var getAccountResult = await accountStorage.GetAsync(userId).ConfigureAwait(false);

            if (getAccountResult.HasError<NotFound>())
                return getAccountResult.ToResult<decimal>();

            var account = getAccountResult.Value;

            account.Amount += amount;

            var saveAccountResult = await accountStorage.CreateOrUpdateAsync(account).ConfigureAwait(false);

            if (saveAccountResult.IsSuccess)
                return Result.Ok(account.Amount);
            // todo(kulikov): send event

            return saveAccountResult.ToResult<decimal>();
        }

        public async Task<Result<decimal>> WithdrawAsync(long userId, decimal amount)
        {
            var getAccountResult = await accountStorage.GetAsync(userId).ConfigureAwait(false);

            if (getAccountResult.HasError<NotFound>())
                return getAccountResult.ToResult<decimal>();

            var account = getAccountResult.Value;

            var newAccountAmount = account.Amount - amount;
            if (newAccountAmount < 0)
                return Result.Fail(new NotEnoughFunds());

            account.Amount -= amount;

            var saveAccountResult = await accountStorage.CreateOrUpdateAsync(account).ConfigureAwait(false);

            if (saveAccountResult.IsSuccess)
                return Result.Ok(account.Amount);
            // todo(kulikov): send event

            return saveAccountResult.ToResult<decimal>();
        }
    }

    internal class Account
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public decimal Amount { get; set; }
    }
}