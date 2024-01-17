namespace AffiliatePMS.WebAPI.Tests.Admin;
using _AffiliatePMS.WebAPI.Areas.Admin;
using AffiliatePMS.Application.AffiliateCustomers.CreateCustomer;
using AffiliatePMS.Application.Common;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CustomersControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CustomersController>> _mockLogger;
    private readonly CustomersController _controller;
    private readonly CancellationToken cancellationToken = new();
    private readonly Fixture fixture = new();

    public CustomersControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CustomersController>>();
        _controller = new CustomersController(_mockMediator.Object, _mockLogger.Object);
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        fixture.Customize<CreateAffiliateCustomerCommand>(c => c.With(p => p.BirthDate, new DateOnly(2000, 1, 1)));
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
        var result = await _controller.Add(affiliateId, command);

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
        var result = await _controller.Add(affiliateId, command);

        // Assert
        Assert.Equal(500, ((ObjectResult)result).StatusCode);
    }

}