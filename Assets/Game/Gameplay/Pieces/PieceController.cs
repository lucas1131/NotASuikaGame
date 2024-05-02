using UnityEngine;

public partial class PieceController : IPieceController {

    public int PieceId => pieceId;
    public int PieceOrder => pieceOrder;
    public float Radius => pieceGraphics.Radius;
    public Vector3 Position {
        get => pieceGraphics.Position;
        set => pieceGraphics.Position = value;
    }
    public bool IsMerging { get; set; }
    public bool IsInPlay { get; private set; }

    readonly IPieceGraphics pieceGraphics;
    readonly ISpawner spawner;
    readonly IPieceMerger merger;
    readonly int pieceId;
    readonly int pieceOrder;
    readonly float scaleFactor;
    readonly float massFactor;

    public PieceController(
        IPieceGraphics pieceGraphics,
        ISpawner spawner,
        IPieceMerger merger,
        int pieceId,
        int pieceOrder,
        float scaleFactor,
        float massFactor
    ){
        this.pieceGraphics = pieceGraphics;
        this.spawner = spawner;
        this.merger = merger;
        this.pieceId = pieceId;
        this.pieceOrder = pieceOrder;
        this.scaleFactor = scaleFactor;
        this.massFactor = massFactor;

        IsMerging = false;
    }

    public void PlayPiece() {
        pieceGraphics.EnablePhysics(true);
        IsInPlay = true;
    }

    public void DestroyPiece(){
        spawner.RemoveFromList(PieceOrder);
        pieceGraphics.Destroy();
    }

    public void ApplyScaleFactor(){
        pieceGraphics.ApplyScaleFactor(Mathf.Pow(scaleFactor, PieceOrder+1));
        pieceGraphics.SetMass((1f + PieceOrder)*massFactor);
    }

    public void OnCollision(IPieceController other){
        if(other == null || other.PieceOrder != PieceOrder) return;

        bool isAnyPieceMerging = IsMerging | other.IsMerging;
        if(isAnyPieceMerging) return;

        bool areBothPiecesInPlay = IsInPlay & other.IsInPlay;
        if(!areBothPiecesInPlay) return;

        merger.RegisterPieces(this, other);
    }

    public override string ToString()
    {
        return PieceOrder.ToString();
    }
}