using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IUserBankAuthDataRepository
    {
        Task<UserBankAuthData> AlterAsync(Guid userId, string bankName, string token);

        Task<UserBankAuthData> GetAsync(Guid userId, Guid id);
    }
}
