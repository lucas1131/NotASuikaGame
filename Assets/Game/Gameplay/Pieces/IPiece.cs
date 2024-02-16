using UnityEngine;

public interface IPiece {
	int PieceId { get; }
	Vector3 Position { get; set; }

    public void Setup(ISpawner spawner, IPieceMergerManager merger, int pieceId, int pieceOrder, float scaleFactor, float massFactor, float gravity);
	void PlayPiece();
}