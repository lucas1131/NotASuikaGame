using UnityEngine;

public interface IPiece {
	int PieceId { get; }
	int PieceOrder { get; }
	Vector3 Position { get; set; }
	bool IsMerging { get; set; }

    public void Setup(ISpawner spawner,
        IPieceMerger merger,
        int pieceId,
        int pieceOrder,
        float scaleFactor,
        float massFactor,
        float gravity
    );

	void PlayPiece();
	void DestroyPiece();
}