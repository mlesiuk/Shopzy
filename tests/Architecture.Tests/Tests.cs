using FluentAssertions;
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
    public void Commands_ShouldNot_HaveDepenedencyOnOtherProjects()
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
}