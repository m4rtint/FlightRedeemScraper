using CathayDomain;
using CathayScraperApp.Assets.Domain.Repository;
using CathayScraperApp.Assets.Domain.UseCases;
using Moq;
using Xunit;

namespace CathayScraperApp.Tests
{
    public class CanSendEmailUseCaseTests
    {
        private static readonly DateTime StaticTime = new DateTime(2023, 10, 1, 12, 0, 0);

        [Fact]
        public void Execute_ShouldReturnTrue_WhenWithinTwoHoursOfFirstSend()
        {
            // Given
            var mockRepository = new Mock<IRateLimitEmailRepository>();
            var request = new FlightEntryToScanRequest { Id = "testFlightId" };
            var emailData = new RateLimitEmailData
            {
                FirstSentEmail = StaticTime.AddHours(-1),
                LastSentEmail = StaticTime.AddHours(-1)
            };
            mockRepository.Setup(r => r.GetEmailSentTimes(request.Id)).Returns(emailData);
            var useCase = new CanSendEmailUseCase(mockRepository.Object);

            // When
            var result = useCase.Execute(request);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void Execute_ShouldReturnFalse_WhenMoreThanTwoHoursFromFirstSendAndLessThanThreeHoursFromLastSend()
        {
            // Given
            var mockRepository = new Mock<IRateLimitEmailRepository>();
            var request = new FlightEntryToScanRequest { Id = "testFlightId" };
            var emailData = new RateLimitEmailData
            {
                FirstSentEmail = StaticTime.AddHours(-3),
                LastSentEmail = StaticTime.AddHours(-1)
            };
            mockRepository.Setup(r => r.GetEmailSentTimes(request.Id)).Returns(emailData);
            var useCase = new CanSendEmailUseCase(mockRepository.Object);

            // When
            var result = useCase.Execute(request);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void Execute_ShouldReturnTrue_WhenMoreThanTwoHoursFromFirstSendAndMoreThanThreeHoursFromLastSend()
        {
            // Given
            var mockRepository = new Mock<IRateLimitEmailRepository>();
            var request = new FlightEntryToScanRequest { Id = "testFlightId" };
            var emailData = new RateLimitEmailData
            {
                FirstSentEmail = StaticTime.AddHours(-4),
                LastSentEmail = StaticTime.AddHours(-4)
            };
            mockRepository.Setup(r => r.GetEmailSentTimes(request.Id)).Returns(emailData);
            var useCase = new CanSendEmailUseCase(mockRepository.Object);

            // When
            var result = useCase.Execute(request);

            // Then
            Assert.True(result);
        }
    }
}