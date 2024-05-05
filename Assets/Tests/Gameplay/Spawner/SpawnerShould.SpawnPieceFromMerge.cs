using NUnit.Framework;
using FluentAssertions;
using UnityEngine;

public partial class SpawnerShould
{
    IPieceController WhenSpawningPieceFromMerge(int pieceOrder) => spawner.SpawnPieceFromMerge(pieceOrder, Vector3.zero);

    void ThenPieceShouldHaveOrder(IPieceController piece, int order)
    {

    }

    [Test]
    public void SpawnPieceFromMerge()
    {
        GivenGameConfigPiecesList();
        GivenNewPieceIsInstantiated(defaultTestPieceController);

        // this should become properly testable when I implement a proper NextPiecesListQueue to handle these next pieces previews
        // even the RemoveFromList method should become testable then
        var piece = WhenSpawningPieceFromMerge(2);

        // ThenPiecesListShouldShift(); untestable for now
        ThenPieceShouldHaveOrder(piece, 2);
    }
}


