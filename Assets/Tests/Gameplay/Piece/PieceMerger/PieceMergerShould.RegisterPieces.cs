using NUnit.Framework;
using FluentAssertions;
using System.Linq;

public partial class PieceMergerShould
{
    IPieceController GivenPiece(int id, int order) => CreateTestPiece(id, order);

    void WhenRegisteringPieces(IPieceController piece1, IPieceController piece2) =>
        merger.RegisterPieces(piece1, piece2);

    void ThenFirstQueuedPiecesShouldHaveSize(int expectedSize)
    {
        merger.GetQueuedPieces().Should().HaveCount(expectedSize);
    }

    void ThenFirstQueuedPiecesShouldContainPieces(params IPieceController[] pieces)
    {
        foreach (var piece in pieces)
        {
            merger.GetQueuedPieces().First().Should().Contain(piece);
        }
    }

    void ThenFirstQueuedPiecesShouldNotContainPieces(params IPieceController[] pieces)
    {
        foreach (var piece in pieces)
        {
            merger.GetQueuedPieces().First().Should().NotContain(piece);
        }
    }

    [Test]
    public void ShouldQueuePiecesForDoubleMerge()
    {
        GivenTripleMergeDisabledGameConfig();
        var piece1 = GivenPiece(1, 1);
        var piece2 = GivenPiece(2, 1);

        WhenRegisteringPieces(piece1, piece2);

        ThenFirstQueuedPiecesShouldHaveSize(1);
        ThenFirstQueuedPiecesShouldContainPieces(piece2, piece1);
    }

    [Test]
    public void ShouldNotQueuePiecesForDoubleMergeIfAnyIsAlreadyQueued()
    {
        GivenTripleMergeDisabledGameConfig();
        var piece1 = GivenPiece(1, 1);
        var piece2 = GivenPiece(2, 1);
        var piece3 = GivenPiece(3, 1);

        WhenRegisteringPieces(piece1, piece2);
        WhenRegisteringPieces(piece1, piece3);
        WhenRegisteringPieces(piece2, piece3);

        ThenFirstQueuedPiecesShouldHaveSize(1);
        ThenFirstQueuedPiecesShouldContainPieces(piece2, piece1);
        ThenFirstQueuedPiecesShouldNotContainPieces(piece3);
    }

    [Test]
    public void ShouldNotQueuePiecesForTripleMergeIfAnyIsAlreadyQueuedForTripleMerge()
    {
        GivenTripleMergeEnabledGameConfig();
        var piece1 = GivenPiece(1, 1);
        var piece2 = GivenPiece(2, 1);
        var piece3 = GivenPiece(3, 1);
        var piece4 = GivenPiece(4, 1);

        WhenRegisteringPieces(piece1, piece2);
        WhenRegisteringPieces(piece1, piece3);
        WhenRegisteringPieces(piece1, piece4);
        WhenRegisteringPieces(piece2, piece4);
        WhenRegisteringPieces(piece3, piece4);

        ThenFirstQueuedPiecesShouldHaveSize(1);
        ThenFirstQueuedPiecesShouldContainPieces(piece2, piece1, piece3);
        ThenFirstQueuedPiecesShouldNotContainPieces(piece4);
    }

    [Test]
    public void ShouldAddThirdPieceToTripleMergeIfAnyIsQueuedForDoubleMerge()
    {
        GivenTripleMergeEnabledGameConfig();
        var piece1 = GivenPiece(1, 1);
        var piece2 = GivenPiece(2, 1);
        var piece3 = GivenPiece(3, 1);

        WhenRegisteringPieces(piece1, piece2);
        WhenRegisteringPieces(piece1, piece3);

        ThenFirstQueuedPiecesShouldHaveSize(1);
        ThenFirstQueuedPiecesShouldContainPieces(piece2, piece1, piece3);
    }

    [Test]
    public void ShouldQueueTwoPiecesForTripleMerge()
    {
        GivenTripleMergeEnabledGameConfig();
        var piece1 = GivenPiece(1, 1);
        var piece2 = GivenPiece(2, 1);

        WhenRegisteringPieces(piece1, piece2);

        ThenFirstQueuedPiecesShouldHaveSize(1);
        ThenFirstQueuedPiecesShouldContainPieces(piece2, piece1);
    }

    [Test]
    public void ShouldQueueThreePiecesForTripleMerge()
    {
        GivenTripleMergeEnabledGameConfig();
        var piece1 = GivenPiece(1, 1);
        var piece2 = GivenPiece(2, 1);
        var piece3 = GivenPiece(3, 1);

        WhenRegisteringPieces(piece1, piece2);
        WhenRegisteringPieces(piece1, piece3);

        ThenFirstQueuedPiecesShouldHaveSize(1);
        ThenFirstQueuedPiecesShouldContainPieces(piece2, piece1, piece3);
    }
}
