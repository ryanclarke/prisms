namespace Prisms.Core.Test;

public class AppTests
{
    private readonly App _app;
    private readonly Mock<IDatabase> _mockDatabase;
    private static readonly string _userId = "+12223334444";
    private readonly UserMessage _userMessage = new(_userId, DateTime.Now, "");

    public AppTests()
    {
        _mockDatabase = new Mock<IDatabase>();
        _mockDatabase.Setup(it => it.GetAllOfDataTypeAsync(_userId, It.IsAny<string>())).ReturnsAsync(Array.Empty<Shard>());
        _app = App.Create(_mockDatabase.Object);
    }

    [Fact]
    public async Task WritesMessageToStorageAsync()
    {
        var result = await _app.ProcessAsync(_userMessage with { Message = "message" });

        _mockDatabase.Verify(s => s.CreateOrUpdateAsync(It.IsAny<Shard>()));
        
        result.Should().BeOfType<Result.Success>();
    }
}
