using System;
using UnityEngine;

public interface IPieceController : IEquatable<IPieceController>, IComparable<IPieceController> {
	int PieceId { get; }
	int PieceOrder { get; }
	bool IsMerging { get; set; }
    public bool IsInPlay { get; }
	float Radius { get; }
	Vector3 Position { get; set; }

	void PlayPiece();
	void DestroyPiece();
    void ApplyScaleFactor();
    void OnCollision(IPieceController other);
}