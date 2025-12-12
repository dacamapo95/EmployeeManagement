using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Shared.Primitives;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Domain.Employees;

public sealed class Employee : AuditableEntity<Guid>
{
    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public int PortalId { get; set; }
    public Portal Portal { get; set; } = default!;

    public int RoleId { get; private set; } = (int)RoleEnum.Employee;
    public Role Role { get; set; } = default!;

    public int StatusId { get; private set; } = (int)EmployeeStatusEnum.Active;
    public EmployeeStatus Status { get; set; } = default!;

    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Telephone { get; set; }
    public string? Fax { get; set; }

    public DateTime? LastLogin { get; private set; }
    public DateTime? DeletedOn { get; private set; }

  
    public Result UpdateProfile(string name, string email, string? telephone, string? fax)
    {
        if (DeletedOn.HasValue)
            return Error.Validation("Cannot update a deleted employee.");

        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            return Error.Validation("Email cannot be empty.");

        Name = name;
        Email = email;
        Telephone = telephone;
        Fax = fax;

        return Result.Success();
    }

    
    public Result ChangeRole(RoleEnum newRole)
    {
        if (DeletedOn.HasValue)
            return Error.Validation("Cannot change role of a deleted employee.");

        RoleId = (int)newRole;
        return Result.Success();
    }

   
    public Result ChangeStatus(EmployeeStatusEnum newStatus)
    {
        if (DeletedOn.HasValue)
            return Error.Validation("Cannot change status of a deleted employee.");

        if ((EmployeeStatusEnum)StatusId == EmployeeStatusEnum.Terminated && newStatus != EmployeeStatusEnum.Terminated)
            return Error.Validation("Cannot change status after termination.");

        StatusId = (int)newStatus;
        return Result.Success();
    }

   
    public void RecordLogin()
    {
        LastLogin = DateTime.UtcNow;
    }
}
