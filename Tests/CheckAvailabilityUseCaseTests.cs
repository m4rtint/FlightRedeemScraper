using System;
using System.Collections.Generic;
using CathayDomain;
using Xunit;

namespace CathayScraperApp.Tests
{
    public class CheckAvailabilityUseCaseTests
    {
        [Fact]
        public void Execute_ShouldReturnEmptyArray_WhenNoAvailabilitiesProvided()
        {
            // Given
            var useCase = new CheckAvailabilityUseCase();
            var dateRange = new DateRange { FromDate = new DateTime(2023, 10, 1) };
            var availabilities = Array.Empty<Availability>();

            // When
            var result = useCase.Execute(dateRange, availabilities);

            // Then
            Assert.Empty(result);
        }

        [Fact]
        public void Execute_ShouldReturnEmptyArray_WhenAllAvailabilitiesAreNotAvailable()
        {
            // Given
            var useCase = new CheckAvailabilityUseCase();
            var dateRange = new DateRange { FromDate = new DateTime(2023, 10, 1) };
            var availabilities = new[]
            {
                new Availability { Date = new DateTime(2023, 10, 1), SeatsAvailability = SeatsAvailability.NotAvailable },
                new Availability { Date = new DateTime(2023, 10, 2), SeatsAvailability = SeatsAvailability.NotAvailable }
            };

            // When
            var result = useCase.Execute(dateRange, availabilities);

            // Then
            Assert.Empty(result);
        }

        [Fact]
        public void Execute_ShouldReturnMatchingAvailabilities_WhenToDateIsNull()
        {
            // Given
            var useCase = new CheckAvailabilityUseCase();
            var dateRange = new DateRange { FromDate = new DateTime(2023, 10, 1) };
            var availabilities = new[]
            {
                new Availability { Date = new DateTime(2023, 10, 1), SeatsAvailability = SeatsAvailability.HighAmount },
                new Availability { Date = new DateTime(2023, 10, 2), SeatsAvailability = SeatsAvailability.LowAmount }
            };

            // When
            var result = useCase.Execute(dateRange, availabilities);

            // Then
            Assert.Single(result);
            Assert.Equal(new DateTime(2023, 10, 1), result[0].Date);
        }

        [Fact]
        public void Execute_ShouldReturnMatchingAvailabilities_WhenToDateIsSet()
        {
            // Given
            var useCase = new CheckAvailabilityUseCase();
            var dateRange = new DateRange { FromDate = new DateTime(2023, 10, 1), ToDate = new DateTime(2023, 10, 2) };
            var availabilities = new[]
            {
                new Availability { Date = new DateTime(2023, 10, 1), SeatsAvailability = SeatsAvailability.HighAmount },
                new Availability { Date = new DateTime(2023, 10, 2), SeatsAvailability = SeatsAvailability.LowAmount },
                new Availability { Date = new DateTime(2023, 10, 3), SeatsAvailability = SeatsAvailability.HighAmount }
            };

            // When
            var result = useCase.Execute(dateRange, availabilities);

            // Then
            Assert.Equal(2, result.Length);
            Assert.Equal(new DateTime(2023, 10, 1), result[0].Date);
            Assert.Equal(new DateTime(2023, 10, 2), result[1].Date);
        }

        [Fact]
        public void Execute_ShouldIgnoreNotAvailableEntries_WhenToDateIsSet()
        {
            // Given
            var useCase = new CheckAvailabilityUseCase();
            var dateRange = new DateRange { FromDate = new DateTime(2023, 10, 1), ToDate = new DateTime(2023, 10, 2) };
            var availabilities = new[]
            {
                new Availability { Date = new DateTime(2023, 10, 1), SeatsAvailability = SeatsAvailability.HighAmount },
                new Availability { Date = new DateTime(2023, 10, 2), SeatsAvailability = SeatsAvailability.NotAvailable },
                new Availability { Date = new DateTime(2023, 10, 3), SeatsAvailability = SeatsAvailability.LowAmount }
            };

            // When
            var result = useCase.Execute(dateRange, availabilities);

            // Then
            Assert.Single(result);
            Assert.Equal(new DateTime(2023, 10, 1), result[0].Date);
        }
    }
}