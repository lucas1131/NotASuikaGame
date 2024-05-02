public interface IGameConfig
{
    float GravityScale { get; }
    float PieceSizeFactor { get; }
    float PieceMassFactor { get; }
    bool AllowTripleMerge { get; }
    int NextPiecesListSize { get; }
    float[] Chance { get; }
    PieceGraphics[] Prefabs { get; }

    int GetHighestPieceOrder();
}
