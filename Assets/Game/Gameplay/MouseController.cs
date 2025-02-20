using System.Collections;
using UnityEngine;

// TODO can probably use new input system to deal with input events here
public class MouseController : MonoBehaviour, IMouseController {

    [SerializeField] IPieceController piece;
    [SerializeField, Range(0.1f, 10f)] float maxSpeed = 3f;
    [SerializeField, Range(0f, 1f)] float smoothTime = 0.2f;
    ISpawner spawner;
    IDeathPlane deathPlane;
    GameObject leftWall;
    GameObject rightWall;
    Vector2 velocity;

    public void SetControlledObject(IPieceController piece){
        this.piece = piece;
    }

    // This setup for bounds is really bad but quick to do, we have no Interfaces for GameObjects to inject here
    // and this is dependent on assigning things on editor instead of calculating the field boundaries
    public void Setup(ISpawner spawner, IDeathPlane deathPlane, GameObject leftWall, GameObject rightWall){
        this.spawner = spawner;
        this.deathPlane = deathPlane;
        this.leftWall = leftWall;
        this.rightWall = rightWall;
        deathPlane.Enable();
    }

    void Update(){
        if(piece == null) return;

        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float rightBound = GetRightBound();
        float leftBound = GetLeftBound();
        if(mouseX > rightBound) mouseX = rightBound - piece.Radius;
        if(mouseX < leftBound) mouseX = leftBound + piece.Radius;

        Vector2 target = new Vector2(mouseX, piece.Position.y);

        piece.Position = Vector2.SmoothDamp(piece.Position, target, ref velocity, smoothTime, maxSpeed);

        if(Input.GetMouseButtonDown(0)){
            // deathPlane.Disable();
            piece.Play();
            piece = null;

            StartCoroutine(DropAndGetNextPiece());
        }
    }

    float GetRightBound(){
        return rightWall.transform.position.x - rightWall.transform.lossyScale.x/2.0f;
    }

    float GetLeftBound(){
        return leftWall.transform.position.x + leftWall.transform.lossyScale.x/2.0f;
    }

    IEnumerator DropAndGetNextPiece(){
        yield return new WaitForSeconds(1f);
        // deathPlane.Enable();
        SetControlledObject(spawner.SpawnPiece());
    }
}
