using UnityEngine;

public interface IPieceGraphics {
	float Radius { get; }
	Vector3 Position { get; set; }

	void Setup(IPieceController pieceController, bool enablePhysics, float gravity);
    void Destroy();
    void EnablePhysics(bool enable);
	void ApplyScaleFactor(float scale);
	void SetMass(float mass);
	void SetGravity(float gravity);
}