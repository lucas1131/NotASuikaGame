using NUnit.Framework;
using NSubstitute;

public partial class PieceMergerShould
{
    IPieceMerger merger;

    IGameConfig gameConfigMock;
    ISpawner spawnerMock;
    ILogger loggerMock;

    [SetUp]
    public void Setup()
    {
        gameConfigMock = Substitute.For<IGameConfig>();
        spawnerMock = Substitute.For<ISpawner>();
        loggerMock = Substitute.For<ILogger>();

        merger = new PieceMerger(gameConfigMock, loggerMock);
    }

    PieceController CreateTestPiece(int id, int order) =>
        new PieceController(null, null, null, id, order, 1.4f, 1.4f);

    void GivenTripleMergeDisabledGameConfig() => gameConfigMock.AllowTripleMerge.Returns(false);
    void GivenTripleMergeEnabledGameConfig() => gameConfigMock.AllowTripleMerge.Returns(true);
}
