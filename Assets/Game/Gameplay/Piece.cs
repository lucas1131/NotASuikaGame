using System;
using UnityEngine;

public partial class Piece : MonoBehaviour, IPiece, IEquatable<Piece>, IComparable<Piece> {

    Spawner spawner;
    Rigidbody2D rb;
    int pieceId;
    bool isMerging;
    bool isInPlay;

    public int PieceId { get => pieceId; }

    void Awake(){
        isMerging = false;
        rb = GetComponent<Rigidbody2D>();
        rb.mass *= (1f + pieceId)*10f;
        SetGravityScale(0f);
    }

    public void Setup(Spawner spawner, int pieceId, float gravityScale){
        this.spawner = spawner;
        this.pieceId = pieceId;

        // SetGravityScale(gravityScale);
        // EnablePhysics(false);
    }

    void SetGravityScale(float scale) => rb.gravityScale = scale;
    void EnablePhysics(bool enable) => rb.isKinematic = !enable;
    public void PlayPiece() {
        EnablePhysics(true);
        isInPlay = true;
    }

    public Vector3 GetPosition() => transform.position;

    // TODO dropped piece above lose plane
    // TODO create a piece merger who knows how many pieces there are in total so we dont merge the last pieces
    // TODO better way to known when a piece is in play so we know
    void OnCollisionEnter2D(Collision2D collision){
        Piece other = collision.gameObject.GetComponent<Piece>();
        if(other == null || other.pieceId != pieceId) return;

        bool isAnyPieceMerging = isMerging | other.isMerging;
        if(isAnyPieceMerging) return;

        bool areBothPiecesInPlay = isInPlay & other.isInPlay;
        if(!areBothPiecesInPlay) return;

        isMerging = true;
        other.isMerging = true;

        Destroy(other.gameObject);
        Destroy(gameObject);

        // TODO instantiate next piece -- need to ask spawner to instantiate?
        spawner.SpawnPieceFromMerge(pieceId+1, transform.position); // TODO need to make spawn position halfway between two pieces
    }

    public override string ToString(){
        return pieceId.ToString();
    }
}
