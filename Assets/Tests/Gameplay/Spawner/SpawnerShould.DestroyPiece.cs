using NUnit.Framework;
using NSubstitute;
using FluentAssertions;

public partial class SpawnerShould
{
    void WhenDestroyingPiece(IPieceController piece) => spawner.DestroyPiece(piece);

    void ThenPieceShouldHaveBeenDestroyed(IPieceController piece)
    {
        piece.Received(1).Destroy();
    }

    [Test]
    public void DestroyPiece()
    {
        GivenDefaultTripleMergeEnabledGameConfig();
        GivenGameConfigPiecesList();
        GivenRngSelectedPiece(0);
        var piece = defaultTestPieceController;
        GivenNewPieceIsInstantiated(piece);
        GivenInitialPiecesWereSpawned();

        // TODO revisit this test after creating some classes to control the piece queue
        // this should become properly testable when I implement a proper NextPiecesListQueue to handle these next pieces previews
        // even the RemoveFromList method should become testable then
        WhenDestroyingPiece(piece);

        // Then piece should not be in pieces list
        ThenPieceShouldHaveBeenDestroyed(piece);
    }
}
