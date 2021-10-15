using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Prisms.Core.Test
{
    public class AppTests
    {
        private readonly App _app;
        private readonly Mock<IStorage> _mockStorage;
        private static readonly string _userId = "+12223334444";
        private readonly UserMessage _userMessage = new(_userId, DateTime.Now, "");

        public AppTests()
        {
            _mockStorage = new Mock<IStorage>();
            _mockStorage.Setup(it => it.ReadUserCommandsAsync(_userId)).ReturnsAsync(new List<Command>());
            _app = App.Create(_mockStorage.Object);
        }

        [Fact]
        public async Task WritesMessageToStorageAsync()
        {
            var result = await _app.ProcessAsync(_userMessage);

            result.Should().BeOfType<Result.Success>();
            _mockStorage.Verify(s => s.WriteAsync(It.IsAny<Shard>()));
        }
    }
}
