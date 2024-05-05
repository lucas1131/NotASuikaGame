using UnityEngine;
using NUnit.Framework;
using NSubstitute;

public partial class PieceMergerShould
{
    Triplet<IPieceController> GivenTriplet(IPieceController piece1, IPieceController piece2, IPieceController piece3 = null) =>
        new Triplet<IPieceController>(piece1, piece2, piece3);

    void GivenPiecesWereRegisteredForMerging(IPieceController piece1, IPieceController piece2)
    {
        merger.RegisterPieces(piece1, piece2);
    }

    void WhenMerging(Triplet<IPieceController> triplet) => merger.Merge(spawnerMock, triplet);

    void ThenSpawnerShouldSpawnPieceWithOrder(int order)
    {
        spawnerMock.Received(1).SpawnAndPlayPiece(order, Arg.Any<Vector3>());
    }

    void ThenNoPieceShouldBeSpawned()
    {
        spawnerMock.Received(0).SpawnAndPlayPiece(Arg.Any<int>(), Arg.Any<Vector3>());
    }

    void ThenSpawnerShouldDestroyPiece(IPieceController piece)
    {
        spawnerMock.Received(1).DestroyPiece(piece);
    }

    void ThenSpawnerShouldNotDestroyPiece(IPieceController piece)
    {
        spawnerMock.Received(0).DestroyPiece(piece);
    }


    [Test]
    public void CreateNewPieceWithPieceOrderOneMoreWhenMergingTwoPieces()
    {
        GivenTripleMergeDisabledGameConfig();
        GivenGameConfigMaximumPieceOrderIs(8);
        var piece1 = GivenPiece(0, 1);
        var piece2 = GivenPiece(1, 1);
        var triplet = GivenTriplet(piece1, piece2);
        GivenPiecesWereRegisteredForMerging(piece1, piece2);

        WhenMerging(triplet);

        ThenSpawnerShouldDestroyPiece(piece1);
        ThenSpawnerShouldDestroyPiece(piece2);
        ThenSpawnerShouldSpawnPieceWithOrder(2);
    }

    [Test]
    public void IgnoreThirdPieceWhenMergingIfTripleMergeIsDisabled()
    {
        GivenTripleMergeDisabledGameConfig();
        GivenGameConfigMaximumPieceOrderIs(8);

        var piece1 = GivenPiece(0, 1);
        var piece2 = GivenPiece(1, 1);
        var piece3 = GivenPiece(2, 1);
        var triplet = GivenTriplet(piece1, piece2, piece3);

        GivenPiecesWereRegisteredForMerging(piece1, piece2);
        GivenPiecesWereRegisteredForMerging(piece1, piece3);

        WhenMerging(triplet);

        ThenSpawnerShouldDestroyPiece(piece1);
        ThenSpawnerShouldDestroyPiece(piece2);
        ThenSpawnerShouldNotDestroyPiece(piece3);
    }

    [Test]
    public void SkipOnePieceOrderWhenMergingThreePieces()
    {
        GivenTripleMergeEnabledGameConfig();
        GivenGameConfigMaximumPieceOrderIs(8);
        var piece1 = GivenPiece(0, 1);
        var piece2 = GivenPiece(1, 1);
        var piece3 = GivenPiece(2, 1);
        var triplet = GivenTriplet(piece1, piece2, piece3);
        GivenPiecesWereRegisteredForMerging(piece1, piece2);
        GivenPiecesWereRegisteredForMerging(piece1, piece3);

        WhenMerging(triplet);

        ThenSpawnerShouldDestroyPiece(piece1);
        ThenSpawnerShouldDestroyPiece(piece2);
        ThenSpawnerShouldDestroyPiece(piece3);
        ThenSpawnerShouldSpawnPieceWithOrder(3);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void NotMergeIfPieceOrderWillExceedMaximum_WithTripleMergeAllowed(bool tripleMergeAllowed)
    {
        GivenTripleMergeGameConfig(tripleMergeAllowed);
        int pieceOrder = tripleMergeAllowed ? 6 : 7;
        GivenGameConfigMaximumPieceOrderIs(12);
        var piece1 = GivenPiece(0, pieceOrder);
        var piece2 = GivenPiece(1, pieceOrder);
        var piece3 = GivenPiece(2, pieceOrder);
        var triplet = GivenTriplet(piece1, piece2, piece3);
        GivenPiecesWereRegisteredForMerging(piece1, piece2);
        GivenPiecesWereRegisteredForMerging(piece1, piece3);

        WhenMerging(triplet);

        ThenNoPieceShouldBeSpawned();
    }
}
