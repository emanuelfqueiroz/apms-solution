namespace AffiliatePMS.WebAPI.Tests.Admin;
using _AffiliatePMS.WebAPI.Admin;
using AffiliatePMS.Application.AffiliateCustomers.CreateCustomer;
using AffiliatePMS.Application.Affiliates.Create;
using AffiliatePMS.Application.Affiliates.Query;
using AffiliatePMS.Application.Common;
using AffiliatePMS.Application.Contracts;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AffiliateControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<AffiliateController>> _mockLogger;
    private readonly AffiliateController _controller;
    private readonly CancellationToken cancellationToken = new();
    private readonly Fixture fixture = new();

    public AffiliateControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<AffiliateController>>();
        _controller = new AffiliateController(_mockMediator.Object, _mockLogger.Object);
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        fixture.Customize<CreateAffiliateCustomerCommand>(c => c.With(p => p.BirthDate, new DateOnly(2000, 1, 1)));
    }

    [Fact]
    public async Task Head_ShouldReturnOk_WhenCalled()
    {
        // Arrange
        _mockMediator.Setup(mediator => mediator.Send(It.IsAny<GetAffiliateHeadQuery>(), cancellationToken))
            .ReturnsAsync(10);

        // Act
        var result = await _controller.Head();

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task List_ShouldReturnOk_WhenCalled()
    {
        // Arrange
        var results = Enumerable.Repeat(new AffiliateResponse(), 10).ToList();
        var pagination = new Pagination { PageSize = 10 };
        _mockMediator.Setup(mediator => mediator.Send(It.IsAny<GetAffiliateQuery>(), default))
            .ReturnsAsync(results);

        // Act
        var result = await _controller.List(pagination);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<AffiliateResponse>>(okResult.Value);
        Assert.Equal(results.Count, ((List<AffiliateResponse>)okResult.Value).Count);
    }

    [Fact]
    public async Task CreateAffiliate_ShouldReturnOk_WhenCalled()
    {
        // Arrange
        var command = fixture.Create<CreateAffiliateCommand>();
        _mockMediator.Setup(mediator => mediator.Send(It.IsAny<CreateAffiliateCommand>(), cancellationToken))
            .ReturnsAsync(CommandResponse<EntityCreated>.Success(new()));

        // Act
        var result = await _controller.CreateAffiliate(command);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task CreateAffiliate_ShouldReturnConflict_WhenError()
    {
        // Arrange
        var command = fixture.Create<CreateAffiliateCommand>();
        _mockMediator.Setup(mediator => mediator.Send(It.IsAny<CreateAffiliateCommand>(), cancellationToken))
            .ReturnsAsync(CommandResponse<EntityCreated>.Error(""));

        // Act
        var result = await _controller.CreateAffiliate(command);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task CreateAffiliateCustomer_ShouldReturnOk_WhenCalled()
    {
        // Arrange
        var affiliateId = 2;
        var command = fixture.Build<CreateAffiliateCustomerCommand>()
            .With(p => p.AffiliateId, affiliateId)
            .With(p => p.BirthDate, new DateOnly(2000, 1, 1))
            .Create();

        _mockMediator.Setup(mediator => mediator.Send(It.IsAny<CreateAffiliateCustomerCommand>(), cancellationToken))
            .ReturnsAsync(CommandResponse<EntityCreated?>.Success(new()));

        // Act
        var result = await _controller.CreateCustomer(affiliateId, command);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task CreateAffiliateCustomer_ShouldReturnConflict_WhenError()
    {
        // Arrange
        var affiliateId = 2;
        var command = fixture.Build<CreateAffiliateCustomerCommand>()
            .With(p => p.AffiliateId, affiliateId)
            .With(p => p.BirthDate, new DateOnly(2000, 1, 1))
            .Create();

        _mockMediator.Setup(mediator => mediator.Send(It.IsAny<CreateAffiliateCustomerCommand>(), cancellationToken))
            .ReturnsAsync(CommandResponse<EntityCreated?>.Error(""));

        // Act
        var result = await _controller.CreateCustomer(affiliateId, command);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);
    }
}