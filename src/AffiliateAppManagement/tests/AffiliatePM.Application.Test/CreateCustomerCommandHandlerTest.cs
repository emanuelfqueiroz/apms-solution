using AffiliatePMS.Application.AffiliateCustomers.CreateCustomer;
using AffiliatePMS.Application.Common;
using AffiliatePMS.Domain.AffiliateCustomers;
using AffiliatePMS.Domain.Common;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;


namespace AffiliatePMS.Application.Test
{
    public class CreateCustomerCommandHandlerTest
    {
        private const int newCustomerId = 3;
        private const int currentUserId = 10;
        private readonly Fixture fixture;
        private readonly Mock<IRepository<AffiliateCustomer>> repository = new();
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IIdentifierService> identifier = new();
        private readonly CancellationToken cancellationToken = new();

        public CreateCustomerCommandHandlerTest()
        {
            var config = new AutoMoqCustomization()
            {
                ConfigureMembers = true
            };

            identifier.Setup(p => p.GetUserId()).Returns(currentUserId);
            repository.Setup(p => p.AddAsync(It.IsAny<AffiliateCustomer>())).Callback<AffiliateCustomer>(a => a.Id = newCustomerId); ;


            fixture = new Fixture();
            fixture.Customize(config);
            fixture.Customize<CreateAffiliateCustomerCommand>(c => c.With(p => p.BirthDate, new DateOnly(2000, 1, 1)));
            fixture.Inject(repository.Object);
            fixture.Inject(unitOfWork.Object);
            fixture.Inject(identifier.Object);
        }


        [Fact]
        public async Task RegisterCustomer_Should_AddAndSaveEntity()
        {
            var commandHandler = fixture.Create<CreateAffiliateCustomerHandler>();
            var response = await commandHandler.Handle(fixture.Create<CreateAffiliateCustomerCommand>(), cancellationToken);


            repository.Verify(p => p.AddAsync(It.IsAny<AffiliateCustomer>()), Times.Once);
            unitOfWork.Verify(p => p.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterCustomer_ShouldAdd_AffilateId()
        {
            var commandHandler = fixture.Create<CreateAffiliateCustomerHandler>();
            var command = fixture.Create<CreateAffiliateCustomerCommand>();
            var response = await commandHandler.Handle(command, cancellationToken);
            repository.Verify(p => p.AddAsync(It.Is<AffiliateCustomer>(d => command.AffiliateId == d.AffiliateId)));
        }


        [Fact]
        public async Task RegisterCustomer_ShouldReturn_NewId()
        {
            var commandHandler = fixture.Create<CreateAffiliateCustomerHandler>();
            var command = fixture.Create<CreateAffiliateCustomerCommand>();
            var response = await commandHandler.Handle(command, cancellationToken);

            Assert.True(response.IsSuccess);
            Assert.Equal(newCustomerId, response.Data!.Id);
        }
    }
}