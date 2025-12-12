using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Database.Interfaces;

public interface IDatabaseInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
