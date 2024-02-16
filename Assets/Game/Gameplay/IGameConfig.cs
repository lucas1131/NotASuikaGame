public interface IGameConfig {
    float GravityScale { get; }
    float PieceSizeFactor { get; }
    public float PieceMassFactor { get; }
    public bool AllowTripleMerge { get; }
    int NextPiecesListSize { get; }
    float[] Chance { get; }
    Piece[] Prefabs { get; }

    int GetHighestPieceOrder();
}
