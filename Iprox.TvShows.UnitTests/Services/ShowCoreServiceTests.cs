using Moq;
using Iprox.Domain.Entities;
using Iprox.Domain.Interface.IRepositories;
using Microsoft.Extensions.Logging;
using Iprox.Domain.Core;
using Iprox.Domain.Enums;

namespace Iprox.TvShows.UnitTests.Services;

public class ShowCoreServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILogger<ShowCoreService>> _mockLogger;
    private readonly ShowCoreService _service;

    public ShowCoreServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLogger = new Mock<ILogger<ShowCoreService>>();
        _service = new ShowCoreService(_mockUnitOfWork.Object, _mockLogger.Object);
    }

    /// <summary>
    /// This test validates that when an exception is thrown "Database error" during the creation of a TV show,
    /// the error message "An error occurred while creating the TV show." is logged as expected.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldLogError_WhenExceptionIsThrown()
    {
        var tvShow = new TvShow
        {
            Name = "Breaking Bad",
            Language = "English",
            Premiered = DateOnly.FromDateTime(DateTime.UtcNow),
            Genres = new List<Genre> {
                new Genre
                {
                    Id = (int)GenreType.Drama, Name = GenreType.Drama.ToString()
                },
                new Genre
                {
                    Id = (int)GenreType.ScienceFiction, Name = GenreType.ScienceFiction.ToString()
                }
            },
            Summary = "A high school chemistry teacher turned methamphetamine manufacturer teams up with a former student to navigate the dangers of the drug trade.",
            TvMazeId = 12345
        };


        _mockUnitOfWork.Setup(uow => uow.TvShowRepository.AddAsync(tvShow))
            .ThrowsAsync(new Exception("Database error"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(tvShow));

        _mockLogger.Verify(
            log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while creating the TV show.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }
}
