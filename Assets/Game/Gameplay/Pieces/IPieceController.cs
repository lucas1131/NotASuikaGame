using System;
using UnityEngine;

public interface IPieceController : IEquatable<IPieceController>, IComparable<IPieceController> {
	int Id { get; }
	int Order { get; }
	bool IsMerging { get; set; }
    public bool IsInPlay { get; }
	float Radius { get; }
	Vector3 Position { get; set; }

	void Play();
	void Destroy();
    void ApplyScaleFactor();
    void OnCollision(IPieceController other);
}