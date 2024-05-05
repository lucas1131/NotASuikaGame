using NUnit.Framework;
using NSubstitute;

public partial class PieceControllerShould
{
    void WhenDestroyingPiece() => piece.Destroy();

    void ThenPieceGraphicsShouldBeDestroyed() {
        pieceGraphicsMock.Received(1).Destroy();
    }

    void ThenPieceShouldBeRemovedFromList() {
        spawnerMock.Received(1).RemoveFromList(piece.Order);
    }

    [Test]
    public void DestroyPiece(){
        WhenDestroyingPiece();

        ThenPieceGraphicsShouldBeDestroyed();
        ThenPieceShouldBeRemovedFromList();
    }
}
