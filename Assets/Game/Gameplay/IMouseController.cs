using UnityEngine;

public interface IMouseController {
	public void SetControlledObject(IPieceController obj);
    public void Setup(ISpawner spawner, IDeathPlane deathPlane, GameObject leftWall, GameObject rightWall);
}
