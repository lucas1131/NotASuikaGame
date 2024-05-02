using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    (IPieceController, List<IPieceController>) SpawnInitialPieces();
    void SpawnPieceFromMerge(int pieceOrder, Vector3 position);
    void ConsumeMerges();
    IPieceController SpawnPiece();
    void RemoveFromList(int id);
}
