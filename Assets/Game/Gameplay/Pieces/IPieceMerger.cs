public interface IPieceMerger {
    void RegisterPieces(Piece piece1, Piece piece2);
    void Merge(ISpawner spawner, Triplet<Piece> triplet);
    Triplet<Piece> Consume();
}
