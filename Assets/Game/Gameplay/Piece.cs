using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    Spawner spawner;
    int pieceId;
    [SerializeField] float defaultGravityScale;
    Rigidbody2D rb;
    bool isMerging;


    public int PieceId { get => pieceId; }

    void Awake(){
        isMerging = false;
        rb = GetComponent<Rigidbody2D>();
        rb.mass *= (1f + pieceId)*10f;
        SetGravityScale(0f);

    }

    public void Setup(int pieceId, Spawner spawner, float defaultGravityScale=0.4f){
        this.pieceId = pieceId;
        this.spawner = spawner;
        this.defaultGravityScale = defaultGravityScale;
    }

    public void SetGravityScale() => rb.gravityScale = defaultGravityScale;
    public void SetGravityScale(float scale) => rb.gravityScale = scale;

    // TODO dropped piece above lose plane
    // TODO create a piece merger who knows how many pieces there are in total so we dont merge the last pieces
    void OnCollisionEnter2D(Collision2D collision){
        Piece other = collision.gameObject.GetComponent<Piece>();
        if(other == null || other.pieceId != pieceId || isMerging || other.isMerging) return;

        isMerging = true;
        other.isMerging = true;

        Debug.Log($"this is: {gameObject.name} other is: {other.gameObject.name}");
        Debug.Log($"this id is: {PieceId} other id is: {other.PieceId}");

        Destroy(other.gameObject);
        Destroy(gameObject);

        // TODO instantiate next piece -- need to ask spawner to instantiate?
        spawner.SpawnPieceFromMerge(pieceId+1, transform.position); // TODO need to make spawn position halfway between two pieces
    }
}
