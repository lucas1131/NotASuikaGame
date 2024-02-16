using UnityEngine;

public interface ISpawner {
    void SpawnInitialPieces();
    void SpawnPieceFromMerge(int pieceOrder, Vector3 position);
	void ConsumeMerges();
	Piece SpawnPiece();
	void RemoveFromList(int id);
}
