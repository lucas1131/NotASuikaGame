using NUnit.Framework;
using NSubstitute;

public partial class PieceControllerShould
{
    void WhenApplyingScale() => piece.ApplyScaleFactor();

    void ThenPieceGraphicsShouldApplyScaleAndSetMass() {
        pieceGraphicsMock.Received(1).ApplyScaleFactor(8f);
        pieceGraphicsMock.Received(1).SetMass(6f);
    }

    [Test]
    public void ScalePiecePhysicsParameters(){
        WhenApplyingScale();

        ThenPieceGraphicsShouldApplyScaleAndSetMass();
    }
}

