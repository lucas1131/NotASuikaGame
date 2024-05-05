using UnityEngine;

public interface IViewControllerFactory
{
    IPieceController CreatePiece(
        ISpawner spawner,
        IPieceMerger merger,
        int pieceId,
        int pieceOrder,
        Vector3 position,
        float scaleFactor,
        float massFactor,
        float gravity,
        bool enablePhysics);
}
