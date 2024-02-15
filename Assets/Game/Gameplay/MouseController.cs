using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    [SerializeField] Piece obj;
    [SerializeField, Range(0.1f, 10f)] float maxSpeed = 3f;
    [SerializeField, Range(0f, 1f)] float smoothTime = 0.2f;
    Spawner spawner;

    public void SetControlledObject(Piece obj){
        this.obj = obj;
    }

    public void SetSpawner(Spawner spawner){
        this.spawner = spawner;
    }

    Vector2 velocity;

    void Update(){
        if(obj == null) return;

        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        Vector2 target = new Vector2(mouseX, obj.gameObject.transform.position.y);
        obj.gameObject.transform.position = Vector2.SmoothDamp(obj.gameObject.transform.position, target, ref velocity, smoothTime, maxSpeed);

        if(Input.GetMouseButtonDown(0)){
            Debug.Log("Mouse clicking, dropping piece and getting next");
            // drop piece
            obj.SetGravityScale(0.4f);
            obj = null;

            StartCoroutine(GetNextPiece());
        }
    }

    IEnumerator GetNextPiece(){
        yield return new WaitForSeconds(1f);
        // TODO Lerp this piece moving into position during this time?
        SetControlledObject(spawner.GetNextPiece());
    }
}
