using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;

public partial class SpawnerShould
{
    void GivenNewPieceIsInstantiated(IPieceController piece)
    {
        vcFactory.CreatePieceController(
                Arg.Any<ISpawner>(),
                Arg.Any<IPieceMerger>(),
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<Vector3>(),
                Arg.Any<float>(),
                Arg.Any<float>(),
                Arg.Any<float>(),
                Arg.Any<bool>())
            .Returns(piece);
    }

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
    public void AttachNewPieceToController()
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

        ThenNextPiecesShouldHaveSize(nextPiecesList, 1);
    }
}
