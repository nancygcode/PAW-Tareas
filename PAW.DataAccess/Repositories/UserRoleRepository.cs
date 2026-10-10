using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserRoleRepository : IRepositoryBase<UserRole>
{
    Task<bool> UpsertAsync(UserRole entity, bool isUpdating);
    Task<bool> CreateAsync(UserRole entity);
    Task<bool> DeleteAsync(UserRole entity);
    Task<IEnumerable<UserRole>> ReadAsync();
    Task<UserRole> FindAsync(int id);
    Task<bool> UpdateAsync(UserRole entity);
    Task<bool> UpdateManyAsync(IEnumerable<UserRole> entities);
    Task<bool> ExistsAsync(UserRole entity);
}

public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
{
    public override async Task<UserRole> FindAsync(int id)
    {
        try
        {
            // The key of this table is numeric(18,0) (decimal), so it must be searched with a decimal.
            return await DbContext.Set<UserRole>().FindAsync((decimal)id);
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }
}
