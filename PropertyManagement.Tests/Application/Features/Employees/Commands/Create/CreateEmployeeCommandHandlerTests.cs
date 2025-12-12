using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using EmployeeManagement.Application.Core.Abstractions;
using EmployeeManagement.Application.Features.Employees.Commands.Create;
using EmployeeManagement.Domain.Companies;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Shared.Results;

namespace EmployeeManagement.Tests.Application.Features.Employees.Commands.Create;

[TestFixture]
public class CreateEmployeeCommandHandlerTests
{
    private IEmployeeRepository _employeeRepository = null!;
    private ICompanyRepository _companyRepository = null!;
    private IPortalRepository _portalRepository = null!;
    private IUnitOfWork _unitOfWork = null!;
    private CreateEmployeeCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _employeeRepository = Substitute.For<IEmployeeRepository>();
        _companyRepository = Substitute.For<ICompanyRepository>();
        _portalRepository = Substitute.For<IPortalRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new CreateEmployeeCommandHandler(
            _employeeRepository,
            _companyRepository,
            _portalRepository,
            _unitOfWork);
    }

    [Test]
    public async Task Handle_Should_Return_Success_When_Employee_Created_Successfully()
    {
        // Arrange
        var command = new CreateEmployeeCommand(
            CompanyId: 1,
            PortalId: 10,
            RoleId: 1,
            Username: "ndjfo",
            Email: "ndjfo@example.com",
            Password: "Password123!",
            Name: "Nicolas De Jesus",
            Telephone: "809-555-1234",
            Fax: null);

        var portal = new Portal
        {
            Id = 10,
            CompanyId = 1,
            Name = "Portal Principal",
            Company = new Company { Id = 1, Name = "Acme Corporation" }
        };

        _companyRepository.ExistsAsync(command.CompanyId, Arg.Any<CancellationToken>())
            .Returns(true);

        _portalRepository.GetByIdWithCompanyAsync(command.PortalId, Arg.Any<CancellationToken>())
            .Returns(portal);

        _employeeRepository.IsUsernameUniqueAsync(command.Username, null, Arg.Any<CancellationToken>())
            .Returns(true);

        _employeeRepository.IsEmailUniqueAsync(command.Email, null, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Value.Should().NotBeNull();

        _employeeRepository.Received(1).Add(Arg.Is<Employee>(e =>
            e.CompanyId == command.CompanyId &&
            e.PortalId == command.PortalId &&
            e.Username == command.Username &&
            e.Email == command.Email &&
            e.Name == command.Name &&
            e.Telephone == command.Telephone &&
            e.Fax == command.Fax));

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_Should_Return_Error_When_Company_Does_Not_Exist()
    {
        // Arrange
        var command = new CreateEmployeeCommand(
            CompanyId: 999,
            PortalId: 10,
            RoleId: 1,
            Username: "mgarcia",
            Email: "mgarcia@example.com",
            Password: "Password123!",
            Name: "Maria Garcia",
            Telephone: null,
            Fax: null);

        _companyRepository.ExistsAsync(command.CompanyId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be(ErrorTypeEnum.NotFound);

        _employeeRepository.DidNotReceive().Add(Arg.Any<Employee>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_Should_Return_Error_When_Portal_Does_Not_Exist()
    {
        // Arrange
        var command = new CreateEmployeeCommand(
            CompanyId: 1,
            PortalId: 999,
            RoleId: 1,
            Username: "jrodriguez",
            Email: "jrodriguez@example.com",
            Password: "Password123!",
            Name: "Juan Rodriguez",
            Telephone: null,
            Fax: null);

        _companyRepository.ExistsAsync(command.CompanyId, Arg.Any<CancellationToken>())
            .Returns(true);

        _portalRepository.GetByIdWithCompanyAsync(command.PortalId, Arg.Any<CancellationToken>())
            .Returns((Portal?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be(ErrorTypeEnum.NotFound);

        _employeeRepository.DidNotReceive().Add(Arg.Any<Employee>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_Should_Return_Error_When_Portal_Does_Not_Belong_To_Company()
    {
        // Arrange
        var command = new CreateEmployeeCommand(
            CompanyId: 1,
            PortalId: 10,
            RoleId: 1,
            Username: "aperez",
            Email: "aperez@exaple.com",
            Password: "Password123!",
            Name: "Ana Perez",
            Telephone: null,
            Fax: null);

        var portal = new Portal
        {
            Id = 10,
            CompanyId = 2, 
            Name = "Portal Secundario",
            Company = new Company { Id = 2, Name = "TechStart Inc" }
        };

        _companyRepository.ExistsAsync(command.CompanyId, Arg.Any<CancellationToken>())
            .Returns(true);

        _portalRepository.GetByIdWithCompanyAsync(command.PortalId, Arg.Any<CancellationToken>())
            .Returns(portal);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be(ErrorTypeEnum.Validation);

        _employeeRepository.DidNotReceive().Add(Arg.Any<Employee>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_Should_Return_Error_When_Username_Already_Exists()
    {
        // Arrange
        var command = new CreateEmployeeCommand(
            CompanyId: 1,
            PortalId: 10,
            RoleId: 1,
            Username: "cmartinez",
            Email: "carlos.nuevo@example.co",
            Password: "Password123!",
            Name: "Carlos Martinez",
            Telephone: null,
            Fax: null);

        var portal = new Portal
        {
            Id = 10,
            CompanyId = 1,
            Name = "Portal Principal",
            Company = new Company { Id = 1, Name = "Acme Corporation" }
        };

        _companyRepository.ExistsAsync(command.CompanyId, Arg.Any<CancellationToken>())
            .Returns(true);

        _portalRepository.GetByIdWithCompanyAsync(command.PortalId, Arg.Any<CancellationToken>())
            .Returns(portal);

        _employeeRepository.IsUsernameUniqueAsync(command.Username, null, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be(ErrorTypeEnum.Conflict);

        _employeeRepository.DidNotReceive().Add(Arg.Any<Employee>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_Should_Return_Error_When_Email_Already_Exists()
    {
        // Arrange
        var command = new CreateEmployeeCommand(
            CompanyId: 1,
            PortalId: 10,
            RoleId: 1,
            Username: "lhernandez",
            Email: "laura.existente@example.com",
            Password: "Password123!",
            Name: "Laura Hernandez",
            Telephone: null,
            Fax: null);

        var portal = new Portal
        {
            Id = 10,
            CompanyId = 1,
            Name = "Portal Principal",
            Company = new Company { Id = 1, Name = "Acme Corporation" }
        };

        _companyRepository.ExistsAsync(command.CompanyId, Arg.Any<CancellationToken>())
            .Returns(true);

        _portalRepository.GetByIdWithCompanyAsync(command.PortalId, Arg.Any<CancellationToken>())
            .Returns(portal);

        _employeeRepository.IsUsernameUniqueAsync(command.Username, null, Arg.Any<CancellationToken>())
            .Returns(true);

        _employeeRepository.IsEmailUniqueAsync(command.Email, null, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be(ErrorTypeEnum.Conflict);

        _employeeRepository.DidNotReceive().Add(Arg.Any<Employee>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
