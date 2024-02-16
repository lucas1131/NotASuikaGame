using System;
using UnityEngine;

public partial class Piece : MonoBehaviour, IPiece, IEquatable<Piece>, IComparable<Piece> {

    ISpawner spawner;
    IPieceMergerManager merger;
    Rigidbody2D rb;
    Collider2D collider;
    int pieceId;
    int pieceOrder;
    float gravity;
    bool isMerging;
    bool isInPlay;

    public int PieceId => pieceId;
    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }

    void Awake(){
        isMerging = false;

        // Make sure object has no physics before enabling physics -- just setting isKinematic still
        collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = false;

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void OnDestroy(){
        spawner.RemoveFromList(pieceOrder);
    }

    public void Setup(
        ISpawner spawner,
        IPieceMergerManager merger,
        int pieceId,
        int pieceOrder,
        float scaleFactor,
        float massFactor,
        float gravity
    ){
        this.spawner = spawner;
        this.merger = merger;
        this.pieceId = pieceId;
        this.pieceOrder = pieceOrder;
        this.gravity = gravity;

        transform.localScale *= Mathf.Pow(scaleFactor, pieceOrder+1);
        rb.mass = (1f + pieceOrder)*massFactor;
    }

    void EnablePhysics(bool enable) {
        collider.enabled = enable;
        rb.isKinematic = !enable;
        rb.gravityScale = gravity * (enable ? 1f : 0f);
    }

    public void PlayPiece() {
        EnablePhysics(true);
        isInPlay = true;
    }

    // TODO dropped piece above lose plane
    // TODO create a piece merger who knows how many pieces there are in total so we dont merge the last pieces
    // TODO better way to known when a piece is in play so we know
    void OnCollisionEnter2D(Collision2D collision){
        Piece other = collision.gameObject.GetComponent<Piece>();
        if(other == null || other.pieceOrder != pieceOrder) return;

        bool isAnyPieceMerging = isMerging | other.isMerging;
        if(isAnyPieceMerging) return;

        bool areBothPiecesInPlay = isInPlay & other.isInPlay;
        if(!areBothPiecesInPlay) return;

        merger.RegisterPieces(this, other);
        isMerging = true;
        other.isMerging = true;

        // dont like this, the object is instantiated somewhere else but is destroyed here, its not consistent and easy to lose track of references this way
        Destroy(other.gameObject);
        Destroy(gameObject);

        // TODO instantiate next piece -- need to ask spawner to instantiate?
        spawner.SpawnPieceFromMerge(pieceOrder+1, transform.position); // TODO need to make spawn position halfway between two pieces
    }

    public override string ToString(){
        return pieceOrder.ToString();
    }
}
