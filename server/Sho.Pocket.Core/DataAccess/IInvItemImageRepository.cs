using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IInvItemImageRepository
    {
        Task<IEnumerable<InvItemImage>> GetByItemIdAsync(Guid itemId);

        Task<InvItemImage> GetAsync(Guid id);

        Task<InvItemImage> AddAsync(Guid itemId, string fileName, byte[] image);

        Task<bool> RemoveAsync(Guid id, Guid itemId, string fileName);
    }
}
