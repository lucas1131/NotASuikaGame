public interface IGameConfig {
    float GravityScale { get; }
    float PieceSizeFactor { get; }
    int NextPiecesListSize { get; }
    float[] Chance { get; }
    Piece[] Prefabs { get; }

    int GetHighestPieceId();
}
