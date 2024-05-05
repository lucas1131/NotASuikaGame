using NUnit.Framework;
using NSubstitute;
using FluentAssertions;

public partial class PieceControllerShould
{
    void WhenPlayingPiece() => piece.Play();

    void ThenPieceShouldBeActive() {
        pieceGraphicsMock.Received(1).EnablePhysics(true);
        piece.IsInPlay.Should().BeTrue();
    }

    [Test]
    public void ActivatePiece(){
        WhenPlayingPiece();

        ThenPieceShouldBeActive();
    }
}
