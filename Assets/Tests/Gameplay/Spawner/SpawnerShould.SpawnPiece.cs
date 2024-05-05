using NUnit.Framework;
using FluentAssertions;

public partial class SpawnerShould
{
    int GivenPieceLimit() => Spawner.maximumSpawnedObjects;
    IPieceController WhenSpawningPiece() => spawner.SpawnPiece();

    void GivenInitialPiecesWereSpawned()
    {
        spawner.SpawnInitialPieces();
    }

    void GivenSpawnerSpawnedMultiplePieces(int n)
    {
        for (int i = 0; i < n - gameConfigMock.NextPiecesListSize; i++)
        {
            spawner.SpawnPiece();
        }
    }

    void ThenPieceShouldBe(IPieceController piece, IPieceController expectedPiece)
    {
        piece.Should().BeEquivalentTo(expectedPiece);
    }

    void ThenPieceShouldBeNull(IPieceController piece)
    {
        piece.Should().BeNull();
    }


    [Test]
    public void SpawnNewPiece()
    {
        GivenDefaultTripleMergeEnabledGameConfig();
        GivenGameConfigPiecesList();
        GivenRngSelectedPiece(0);
        var instantiatedPiece = defaultTestPieceController;
        GivenNewPieceIsInstantiated(instantiatedPiece);
        GivenInitialPiecesWereSpawned();

        // TODO revisit this test after creating some classes to control the piece queue
        // this should become properly testable when I implement a proper NextPiecesListQueue to handle these next pieces previews
        // even the RemoveFromList method should become testable then
        var piece = WhenSpawningPiece();

        // ThenPiecesListShouldShift(); untestable for now
        ThenPieceShouldBe(piece, instantiatedPiece);
    }

    [Test]
    public void ReturnNullIfPieceLimitReached()
    {
        GivenDefaultTripleMergeEnabledGameConfig();
        GivenGameConfigPiecesList();
        GivenRngSelectedPiece(0);
        var instantiatedPiece = defaultTestPieceController;
        GivenNewPieceIsInstantiated(instantiatedPiece);
        int limit = GivenPieceLimit();
        GivenInitialPiecesWereSpawned();
        GivenSpawnerSpawnedMultiplePieces(limit);

        var piece = WhenSpawningPiece();

        ThenPieceShouldBeNull(piece);
    }
}
