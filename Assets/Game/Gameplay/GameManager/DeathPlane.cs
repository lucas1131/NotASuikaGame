using UnityEngine;

public class DeathPlane : MonoBehaviour, IDeathPlane {
    void OnTriggerEnter2D(Collider2D collider){
        PieceGraphics other = collider.gameObject.GetComponent<PieceGraphics>();
        if(other == null) return;

        // TODO make player actually lose probably via game manager
        Debug.Log("Player lost");
    }

    public void Enable(){
        gameObject.SetActive(true);
    }

    public void Disable(){
        gameObject.SetActive(false);
    }
}
