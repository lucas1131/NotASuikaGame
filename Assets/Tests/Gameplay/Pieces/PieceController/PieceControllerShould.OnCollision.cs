using NUnit.Framework;
using NSubstitute;
using FluentAssertions;

public partial class PieceControllerShould
{
    void GivenPieceIsInPlay() => piece.Play();
    PieceController GivenOtherCollidingPiece(int id, int order, bool isInPlay, bool isMerging) =>
        CreateOtherCollidingPiece(id, order, isInPlay, isMerging);

    void WhenColliding(IPieceController other) => piece.OnCollision(other);

    void ThenMergerShouldNotBeCalled(){
        pieceMergerMock.Received(0).RegisterPieces(Arg.Any<IPieceController>(), Arg.Any<IPieceController>());
    }

    void ThenPiecesShouldBeQueuedToMerger(IPieceController other){
        pieceMergerMock.Received(1).RegisterPieces(piece, other);
    }


    [Test]
    public void DoNothingIfAnyCollidingPieceIsAlreadyMerging(){
        GivenPieceIsInPlay();
        var otherPiece = GivenOtherCollidingPiece(id: 2, order: 2, isInPlay: true, isMerging: true);

        WhenColliding(otherPiece);

        ThenMergerShouldNotBeCalled();
    }

    [Test]
    public void DoNothingIsAnyCollidingPiecesIsNotInPlay(){
        GivenPieceIsInPlay();
        var otherPiece = GivenOtherCollidingPiece(id: 2, order: 2, isInPlay: false, isMerging: false);

        WhenColliding(otherPiece);

        ThenMergerShouldNotBeCalled();
    }

    [Test]
    public void DoNothingIfPiecesOrderAreDifferent(){
        GivenPieceIsInPlay();
        var otherPiece = GivenOtherCollidingPiece(id: 2, order: 1, isInPlay: true, isMerging: false);

        WhenColliding(otherPiece);

        ThenMergerShouldNotBeCalled();
    }

    [Test]
    public void QueueToMergerIfPiecesHaveSameOrderAndNotMergingAndInPlay(){
        GivenPieceIsInPlay();
        var otherPiece = GivenOtherCollidingPiece(id: 2, order: 2, isInPlay: true, isMerging: false);

        WhenColliding(otherPiece);

        ThenPiecesShouldBeQueuedToMerger(otherPiece);
    }
}

