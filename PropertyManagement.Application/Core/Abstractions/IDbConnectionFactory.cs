using System.Data;

namespace EmployeeManagement.Application.Core.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
