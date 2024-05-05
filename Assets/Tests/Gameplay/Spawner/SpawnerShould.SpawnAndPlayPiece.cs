using NUnit.Framework;
using NSubstitute;
using FluentAssertions;
using UnityEngine;

public partial class SpawnerShould
{
    void GivenInstantiatedPieceHasOrder(IPieceController newPiece, int order) {
        newPiece.Order.Returns(order);
    }

    IPieceController WhenSpawningPieceFromMerge(int pieceOrder) => spawner.SpawnAndPlayPiece(pieceOrder, Vector3.zero);

    void ThenPieceShouldHaveOrder(IPieceController piece, int order)
    {
        piece.Order.Should().Be(order);
    }

    [Test]
    public void SpawnAndPlayPieceWhenSpawnedFromMerge()
    {
        GivenGameConfigPiecesList();
        IPieceController newPiece = defaultTestPieceController;
        GivenNewPieceIsInstantiated(newPiece);
        GivenInstantiatedPieceHasOrder(newPiece, 2);

        // this should become properly testable when I implement a proper NextPiecesListQueue to handle these next pieces previews
        // even the RemoveFromList method should become testable then
        var piece = WhenSpawningPieceFromMerge(2);

        // ThenPiecesListShouldShift(); untestable for now
        ThenPieceShouldHaveOrder(piece, 2);
    }
}


