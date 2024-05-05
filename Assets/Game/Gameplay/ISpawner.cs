using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    (IPieceController, List<IPieceController>) SpawnInitialPieces();
    IPieceController SpawnAndPlayPiece(int pieceOrder, Vector3 position);
    void ConsumeMerges();
    IPieceController SpawnPiece();
    void DestroyPiece(IPieceController piece);
    void RemoveFromList(int id);
}
