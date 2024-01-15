using AffiliatePMS.Application.Affiliates.Create;
using AffiliatePMS.Application.Common;
using AffiliatePMS.Domain.Affiliates;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;


namespace AffiliatePMS.Application.Test
{
    public class CreateAffiliateCommandHandlerTest
    {
        private const int newCustomerId = 3;
        private const int currentUserId = 10;
        private readonly Fixture fixture;
        private readonly Mock<IAffiliateRepository> repository = new();
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IIdentifierService> identifier = new();
        private readonly CancellationToken cancellationToken = new();

        public CreateAffiliateCommandHandlerTest()
        {
            var config = new AutoMoqCustomization()
            {
                ConfigureMembers = true
            };

            identifier.Setup(p => p.GetUserId()).Returns(currentUserId);
            repository.Setup(p => p.AddAsync(It.IsAny<Affiliate>())).Callback<Affiliate>(a => a.Id = newCustomerId);
            repository.Setup(p => p.IsEmailUsedAsync(It.IsAny<string>())).ReturnsAsync(false);

            fixture = new Fixture();
            fixture.Customize(config);
            fixture.Inject(repository.Object);
            fixture.Inject(unitOfWork.Object);
            fixture.Inject(identifier.Object);
        }
        [Fact]
        public async Task RegisterAffiliate_ShouldReturnError_WhenEmailExist_OnDatabase()
        {
            //Arrange
            repository.Setup(p => p.IsEmailUsedAsync(It.IsAny<string>())).ReturnsAsync(true); //email already used
            var command = fixture.Create<CreateAffiliateProfileCommand>();
            var commandHandler = fixture.Create<CreateAffiliateHandler>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task RegisterAffiliate_Should_AddAndSaveEntity()
        {
            //Arrange
            var command = fixture.Create<CreateAffiliateCommand>();
            var commandHandler = fixture.Create<CreateAffiliateHandler>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            repository.Verify(p => p.AddAsync(It.IsAny<Affiliate>()), Times.Once);
            unitOfWork.Verify(p => p.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAffiliate_ShouldAdd_UserCreatedId()
        {
            //Arrange
            var command = fixture.Create<CreateAffiliateCommand>();
            var commandHandler = fixture.Create<CreateAffiliateHandler>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            repository.Verify(p => p.AddAsync(It.Is<Affiliate>(d => identifier.Object.GetUserId() == d.UserCreatedId)));

        }

        [Fact]
        public async Task RegisterAffiliate_ShouldAdd_InnerInstancies()
        {
            //Arrange
            var command = fixture.Create<CreateAffiliateCommand>();
            var commandHandler = fixture.Create<CreateAffiliateHandler>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            repository.Verify(p => p.AddAsync(It.Is<Affiliate>(d => d.AffiliateDetail!.Email!.Equals(command.Email))));
            repository.Verify(p => p.AddAsync(It.Is<Affiliate>(d => d.AffiliateSocialMedia.Any())));
        }

        [Fact]
        public async Task RegisterAffiliate_ShouldReturn_NewId()
        {
            //Arrange
            var command = fixture.Create<CreateAffiliateCommand>();
            var commandHandler = fixture.Create<CreateAffiliateHandler>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            Assert.True(response.IsSuccess);
            Assert.Equal(newCustomerId, response.Data!.Id);
        }
    }
    public class CreateAffiliateProfileCommandHandlerTest
    {
        private const int newCustomerId = 3;
        private const int currentUserId = 10;
        private const string sameEmail = "user@email.com";
        private readonly Fixture fixture;
        private readonly Mock<IAffiliateRepository> repository = new();
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IIdentifierService> identifier = new();
        private readonly CancellationToken cancellationToken = new();

        public CreateAffiliateProfileCommandHandlerTest()
        {
            var config = new AutoMoqCustomization()
            {
                ConfigureMembers = true
            };

            identifier.Setup(p => p.GetUserId()).Returns(currentUserId);
            identifier.Setup(p => p.GetEmail()).Returns(sameEmail);
            repository.Setup(p => p.AddAsync(It.IsAny<Affiliate>())).Callback<Affiliate>(a => a.Id = newCustomerId); ;

            fixture = new Fixture();
            fixture.Customize<CreateAffiliateProfileCommand>(c => c.With(p => p.Email, sameEmail));
            fixture.Customize(config);
            fixture.Inject(repository.Object);
            fixture.Inject(unitOfWork.Object);
            fixture.Inject(identifier.Object);
        }
        [Fact]
        public async Task RegisterProfile_ShouldReturnError_When_Email_IsDifferentFrom_UserEmail()
        {
            //Arrange
            identifier.Setup(p => p.GetEmail()).Returns("signedUserEmail@user.com"); // Signed User

            var command = fixture.Build<CreateAffiliateProfileCommand>().With(c => c.Email, "tryingAnotherEmail@user.com").Create();
            var commandHandler = fixture.Create<CreateAffiliateHandler>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task RegisterProfile_ShouldReturnError_WhenEmailExist_OnDatabase()
        {
            //Arrange
            repository.Setup(p => p.IsEmailUsedAsync(It.IsAny<string>())).ReturnsAsync(true); //email already used
            var command = fixture.Create<CreateAffiliateProfileCommand>();
            var commandHandler = fixture.Create<CreateAffiliateHandler>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task RegisterAffiliate_Should_AddAndSaveEntityd()
        {
            //Arrange
            var commandHandler = fixture.Create<CreateAffiliateHandler>();
            var command = fixture.Create<CreateAffiliateProfileCommand>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            repository.Verify(p => p.AddAsync(It.IsAny<Affiliate>()), Times.Once);
            unitOfWork.Verify(p => p.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAffiliate_Should_Save_UserCreatedId()
        {
            //Arrange
            var commandHandler = fixture.Create<CreateAffiliateHandler>();
            var command = fixture.Create<CreateAffiliateProfileCommand>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            repository.Verify(p => p.AddAsync(It.Is<Affiliate>(d => identifier.Object.GetUserId() == d.UserCreatedId)));

        }

        [Fact]
        public async Task RegisterAffiliate_ShouldAdd_InnerInstancies()
        {
            //Arrange
            var commandHandler = fixture.Create<CreateAffiliateHandler>();
            var command = fixture.Create<CreateAffiliateProfileCommand>();

            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            repository.Verify(p => p.AddAsync(It.Is<Affiliate>(d => d.AffiliateDetail!.Email!.Equals(command.Email))));
            repository.Verify(p => p.AddAsync(It.Is<Affiliate>(d => d.AffiliateSocialMedia.Any())));
        }

        [Fact]
        public async Task RegisterAffiliate_ShouldReturn_NewId()
        {
            //Arrange
            var commandHandler = fixture.Create<CreateAffiliateHandler>();
            var command = fixture.Create<CreateAffiliateProfileCommand>();


            //Act
            var response = await commandHandler.Handle(command, cancellationToken);

            //Assert
            Assert.True(response.IsSuccess);
            Assert.Equal(newCustomerId, response.Data!.Id);
        }
    }
}