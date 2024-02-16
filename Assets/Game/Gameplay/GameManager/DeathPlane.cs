using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour, IDeathPlane {
    void OnTriggerEnter2D(Collider2D collider){
        Piece other = collider.gameObject.GetComponent<Piece>();
        if(other == null) return;

        // TODO make player actually lose
        Debug.Log("Player lost");
    }

    public void Enable(){
        gameObject.SetActive(true);
    }

    public void Disable(){
        gameObject.SetActive(false);
    }
}
