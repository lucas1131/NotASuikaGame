using UnityEngine;

public partial class PieceController : IPieceController {

    public int Id => id;
    public int Order => order;
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
    readonly int id;
    readonly int order;
    readonly float scaleFactor;
    readonly float massFactor;

    public PieceController(
        IPieceGraphics pieceGraphics,
        ISpawner spawner,
        IPieceMerger merger,
        int id,
        int order,
        float scaleFactor,
        float massFactor
    ){
        this.pieceGraphics = pieceGraphics;
        this.spawner = spawner;
        this.merger = merger;
        this.id = id;
        this.order = order;
        this.scaleFactor = scaleFactor;
        this.massFactor = massFactor;

        IsMerging = false;
    }

    public void Play() {
        pieceGraphics.EnablePhysics(true);
        IsInPlay = true;
    }

    public void Destroy(){
        spawner.RemoveFromList(Order);
        pieceGraphics.Destroy();
    }

    public void ApplyScaleFactor(){
        pieceGraphics.ApplyScaleFactor(Mathf.Pow(scaleFactor, Order+1));
        pieceGraphics.SetMass((1f + Order)*massFactor);
    }

    public void OnCollision(IPieceController other){
        if(other == null || other.Order != Order) return;

        bool isAnyPieceMerging = IsMerging || other.IsMerging;
        if(isAnyPieceMerging) return;

        bool areBothPiecesInPlay = IsInPlay && other.IsInPlay;
        if(!areBothPiecesInPlay) return;

        merger.RegisterPieces(this, other);
    }

    public override string ToString()
    {
        return $"[Piece id:{Id} order:{Order}]";
    }
}