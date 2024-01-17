namespace AffiliatePMS.WebAPI.Tests.Admin;
using _AffiliatePMS.WebAPI.Areas.Admin;
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

public class AffiliatesControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<AffiliatesController>> _mockLogger;
    private readonly AffiliatesController _controller;
    private readonly CancellationToken cancellationToken = new();
    private readonly Fixture fixture = new();

    public AffiliatesControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<AffiliatesController>>();
        _controller = new AffiliatesController(_mockMediator.Object, _mockLogger.Object);
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
        var result = await _controller.Add(command);

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
        var result = await _controller.Add(command);

        // Assert
        Assert.Equal(500, ((ObjectResult)result).StatusCode);
    }


}