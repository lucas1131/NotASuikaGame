using UnityEngine;

public interface IPiece {
	int PieceId { get; }
	void Setup(Spawner spawner, int pieceId, float gravityScale);
	void PlayPiece();
	Vector3 GetPosition();
}