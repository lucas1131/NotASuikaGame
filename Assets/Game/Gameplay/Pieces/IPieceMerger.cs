using System.Collections.Generic;

public interface IPieceMerger
{
    void RegisterPieces(IPieceController piece1, IPieceController piece2);
    void Merge(ISpawner spawner, Triplet<IPieceController> triplet);
    Triplet<IPieceController> Consume();
    List<Triplet<IPieceController>> GetQueuedPieces();
}
