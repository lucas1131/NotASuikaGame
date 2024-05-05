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

    IPieceController GivenPiece(int id, int order)
    {
        IPieceController pieceMock = Substitute.For<IPieceController>();
        pieceMock.Id.Returns(id);
        pieceMock.Order.Returns(order);
        pieceMock.Position.Returns(UnityEngine.Vector3.zero);
        return pieceMock;
    }

    void GivenTripleMergeGameConfig(bool allowed) => gameConfigMock.AllowTripleMerge.Returns(allowed);
    void GivenTripleMergeDisabledGameConfig() => GivenTripleMergeGameConfig(false);
    void GivenTripleMergeEnabledGameConfig() => GivenTripleMergeGameConfig(true);
    void GivenGameConfigMaximumPieceOrderIs(int order) => gameConfigMock.GetHighestPieceOrder().Returns(8);

}
