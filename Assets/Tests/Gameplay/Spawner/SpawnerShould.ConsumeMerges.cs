using NUnit.Framework;
using FluentAssertions;
using NSubstitute;

public partial class SpawnerShould
{
    void WhenConsumingMerges() => spawner.ConsumeMerges();

    void GivenMergerHas3PiecesToMerge() =>
        pieceMergerMock.Consume().Returns(defaultTriplet, defaultTriplet, defaultTriplet, nullTriplet);

    void ThenSpawnerShouldConsumeNumberOfTimes(int n)
    {
        pieceMergerMock.Received(n).Consume();
    }

    void ThenNumberOfTimesMergeShouldBeCalledIs(int n)
    {
        pieceMergerMock.Received(n).Merge(spawner, Arg.Any<Triplet<IPieceController>>());
    }

    [Test]
    public void ConsumeMerges()
    {
        GivenMergerHas3PiecesToMerge();

        WhenConsumingMerges();

        ThenSpawnerShouldConsumeNumberOfTimes(4);
        ThenNumberOfTimesMergeShouldBeCalledIs(3);
    }

    Triplet<IPieceController> defaultTriplet => new Triplet<IPieceController>(default, default, default);
    Triplet<IPieceController> nullTriplet => (Triplet<IPieceController>)null;
}


