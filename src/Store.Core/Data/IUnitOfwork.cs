using System.Threading.Tasks;

namespace Store.Core.Data
{
    public interface IUnitOfwork
    {
        Task<bool> Commit();
    }
}
