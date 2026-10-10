using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserActionRepository : IRepositoryBase<UserAction>
{
    Task<bool> UpsertAsync(UserAction entity, bool isUpdating);
    Task<bool> CreateAsync(UserAction entity);
    Task<bool> DeleteAsync(UserAction entity);
    Task<IEnumerable<UserAction>> ReadAsync();
    Task<UserAction> FindAsync(int id);
    Task<bool> UpdateAsync(UserAction entity);
    Task<bool> UpdateManyAsync(IEnumerable<UserAction> entities);
    Task<bool> ExistsAsync(UserAction entity);
}

public class UserActionRepository : RepositoryBase<UserAction>, IUserActionRepository
{
    public override async Task<UserAction> FindAsync(int id)
    {
        try
        {
            // The key of this table is numeric(18,0) (decimal), so it must be searched with a decimal.
            return await DbContext.Set<UserAction>().FindAsync((decimal)id);
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }
}
