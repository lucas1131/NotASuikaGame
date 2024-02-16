using System;
using UnityEngine;

public partial class Piece : MonoBehaviour, IPiece, IEquatable<Piece>, IComparable<Piece> {

    ISpawner spawner;
    IPieceMerger merger;

    Rigidbody2D rb;
    Collider2D collider;

    int pieceId;
    int pieceOrder;
    float gravity;
    bool isInPlay;

    public int PieceId => pieceId;
    public int PieceOrder => pieceOrder;
    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }
    public bool IsMerging { get; set; }

    void Awake(){
        IsMerging = false;

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
        IPieceMerger merger,
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
    // TODO better way to known when a piece is in play so we know
    void OnCollisionEnter2D(Collision2D collision){
        Piece other = collision.gameObject.GetComponent<Piece>();
        if(other == null || other.pieceOrder != pieceOrder) return;

        bool isAnyPieceMerging = IsMerging | other.IsMerging;
        if(isAnyPieceMerging) return;

        bool areBothPiecesInPlay = isInPlay & other.isInPlay;
        if(!areBothPiecesInPlay) return;

        merger.RegisterPieces(this, other);
    }

    public void DestroyPiece(){
        Destroy(gameObject);
    }

    public override string ToString(){
        return pieceOrder.ToString();
    }
}
