using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO can probably use new input system to deal with input events here
public class MouseController : MonoBehaviour, IMouseController {

    [SerializeField] Piece piece;
    [SerializeField, Range(0.1f, 10f)] float maxSpeed = 3f;
    [SerializeField, Range(0f, 1f)] float smoothTime = 0.2f;
    ISpawner spawner;
    IDeathPlane deathPlane;
    Vector2 velocity;

    public void SetControlledObject(Piece piece){
        this.piece = piece;
    }

    public void Setup(ISpawner spawner, IDeathPlane deathPlane){
        this.spawner = spawner;
        this.deathPlane = deathPlane;
        deathPlane.Enable();
    }

    void Update(){
        if(piece == null) return;

        // TODO limit piece movement to inside game field boundaries
        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        Vector2 target = new Vector2(mouseX, piece.gameObject.transform.position.y);
        piece.gameObject.transform.position = Vector2.SmoothDamp(piece.gameObject.transform.position, target, ref velocity, smoothTime, maxSpeed);

        if(Input.GetMouseButtonDown(0)){
            deathPlane.Disable();
            piece.PlayPiece();
            piece = null;

            StartCoroutine(DropAndGetNextPiece());
        }
    }

    IEnumerator DropAndGetNextPiece(){
        yield return new WaitForSeconds(1f);
        deathPlane.Enable();
        SetControlledObject(spawner.SpawnPiece());
    }
}
