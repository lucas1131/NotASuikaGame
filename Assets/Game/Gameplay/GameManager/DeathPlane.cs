using System;
using UnityEngine;

public class DeathPlane : MonoBehaviour, IDeathPlane
{
    public event Action OnPlayerLost;

    private void OnDestroy()
    {
        OnPlayerLost = null;
    }

    void OnTriggerEnter2D(Collider2D collider){
        PieceGraphics other = collider.gameObject.GetComponent<PieceGraphics>();
        if(other == null) return;

        OnPlayerLost?.Invoke();
    }

    public void Enable(){
        gameObject.SetActive(true);
    }

    public void Disable(){
        gameObject.SetActive(false);
    }
}
