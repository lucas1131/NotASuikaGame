using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;

public partial class SpawnerShould
{
    void GivenRngSelectedPiece(int index) => rngMock.SelectIndex(Arg.Any<float[]>()).Returns(index);

    (IPieceController, List<IPieceController>) WhenSpawningInitialPieces() => spawner.SpawnInitialPieces();

    void ThenCurrentPieceShouldNotBeNull(IPieceController currentPiece)
    {
        currentPiece.Should().NotBeNull();
    }

    void ThenMouseControllerShouldHavePieceAttached(IPieceController currentPiece)
    {
        mouseControllerMock.Received(1).SetControlledObject(currentPiece);
    }

    void ThenNextPiecesShouldHaveSize(List<IPieceController> nextPieces, int expectedSize)
    {
        nextPieces.Should().NotBeNull();
        nextPieces.Count.Should().Be(expectedSize);
    }

    [Test]
    public void AttachNewPieceToControllerWhenSpawningPieces()
    {
        GivenDefaultTripleMergeEnabledGameConfig();
        GivenRngSelectedPiece(0);

        (IPieceController currentPiece, List<IPieceController> _) = WhenSpawningInitialPieces();

        ThenCurrentPieceShouldNotBeNull(currentPiece);
        ThenMouseControllerShouldHavePieceAttached(currentPiece);
    }

    [Test]
    public void GenerateNextPiecesList()
    {
        GivenDefaultTripleMergeEnabledGameConfig();
        GivenRngSelectedPiece(0);

        (IPieceController _, List<IPieceController> nextPiecesList) = WhenSpawningInitialPieces();

        ThenNextPiecesShouldHaveSize(nextPiecesList, 2);
    }
}
