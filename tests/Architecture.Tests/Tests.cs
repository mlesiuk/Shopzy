using FluentAssertions;
using MediatR;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class Tests
{
    [Fact]
    public void Domain_ShouldNot_HaveDepenedencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Shopzy.Domain.AssemblyReference).Assembly;

        var projects = new[]
        {
            Consts.ApplicationNamespace,
            Consts.InfrastructureNamespace,
            Consts.ApiNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(projects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNot_HaveDepenedencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Shopzy.Application.DependencyInjection).Assembly;

        var projects = new[]
        {
            Consts.InfrastructureNamespace,
            Consts.ApiNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(projects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNot_HaveDepenedencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Shopzy.Application.DependencyInjection).Assembly;

        var projects = new[]
        {
            Consts.ApiNamespace
        };

        // Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(projects)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Should_HaveNameEndingWithCommand()
    {
        // Arrange
        var assembly = typeof(Shopzy.Application.DependencyInjection).Assembly;

        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(Consts.ApplicationCommandsNamespace)
            .And()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .HaveNameEndingWith(Consts.Command)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlers_Should_HaveNameEndingWithHandler()
    {
        // Arrange
        var assembly = typeof(Shopzy.Application.DependencyInjection).Assembly;

        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(Consts.ApplicationCommandsNamespace)
            .And()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Should()
            .HaveNameEndingWith(Consts.Handler)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_Should_HaveNameEndingWithQuery()
    {
        // Arrange
        var assembly = typeof(Shopzy.Application.DependencyInjection).Assembly;

        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(Consts.ApplicationQueriesNamespace)
            .And()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .HaveNameEndingWith(Consts.Query)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_Should_HaveNameEndingWithHandler()
    {
        // Arrange
        var assembly = typeof(Shopzy.Application.DependencyInjection).Assembly;

        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(Consts.ApplicationQueriesNamespace)
            .And()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Should()
            .HaveNameEndingWith(Consts.Handler)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validators_Should_HaveNameEndingWithQuery()
    {
        // Arrange
        var assembly = typeof(Shopzy.Application.DependencyInjection).Assembly;

        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace(Consts.ApplicationValidatorsNamespace)
            .And()
            .ImplementInterface(typeof(IRequest<>))
            .Should()
            .HaveNameEndingWith(Consts.Validator)
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}