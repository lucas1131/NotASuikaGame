using UnityEngine;

public interface ISpawner {
    public void SpawnInitialPieces();
	void SpawnPieceFromMerge(int pieceId, Vector3 position);
	Piece SpawnPiece();
	void RemoveFromList(int id);
}
