using System.Threading;
using System.Threading.Tasks;
using Fahem.Models;

namespace Fahem.Repositories.Orders
{
    public interface IOrderMongoRepository
    {
        Task InsertAsync(Order order, CancellationToken cancellationToken = default);
    }
}
