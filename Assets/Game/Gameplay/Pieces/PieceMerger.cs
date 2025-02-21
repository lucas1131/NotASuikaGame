using System.Collections.Generic;
using UnityEngine;

public class PieceMerger : IPieceMerger
{
    IGameConfig config;
    ILogger logger;
    IScoreController scoreController;
    HashSet<Triplet<IPieceController>> mergeSet;

    public PieceMerger(IGameConfig config, ILogger logger, IScoreController scoreController)
    {
        this.config = config;
        this.logger = logger;
        this.scoreController = scoreController;
        mergeSet = new HashSet<Triplet<IPieceController>>();
    }

    public void RegisterPieces(IPieceController piece1, IPieceController piece2)
    {
        if (config.AllowTripleMerge)
        {
            RegisterForTripleMerge(piece1, piece2);
        }
        else
        {
            RegisterForDoubleMerge(piece1, piece2);
        }
    }

    void RegisterForTripleMerge(IPieceController piece1, IPieceController piece2)
    {
        string mergeFailWarningMsg = "[PieceMerger] Pair ({0}, {1}) is already queued for merge.";

        foreach (Triplet<IPieceController> triplet in mergeSet)
        {
            // Triplet that has either piece 1 or 2, check if there is a null value to write the other piece onto
            if (triplet.Contains(piece1))
            {
                if (triplet.Contains(piece2))
                {
                    logger.LogWarning(string.Format(mergeFailWarningMsg, piece1, piece2, piece1));
                    return;
                }

                mergeSet.Remove(triplet);
                triplet.AddValueOverNull(piece2);
                mergeSet.Add(triplet);
                return;

            }
            else if (triplet.Contains(piece2))
            {
                if (triplet.Contains(piece1))
                {
                    logger.LogWarning(string.Format(mergeFailWarningMsg, piece1, piece2, piece1));
                    return;
                }

                mergeSet.Remove(triplet);
                triplet.AddValueOverNull(piece1);
                mergeSet.Add(triplet);
                return;
            }
        }

        // Completely new triplet
        mergeSet.Add(new Triplet<IPieceController>(piece1, piece2));
    }

    void RegisterForDoubleMerge(IPieceController piece1, IPieceController piece2)
    {
        string mergeFailWarningMsg = "Cannot register pieces ({0}, {1}) for merging, {2} is already queued for merge in another set.";

        foreach (Triplet<IPieceController> queuedPieces in mergeSet)
        {
            if (queuedPieces.Contains(piece1))
            {
                logger.LogWarning(string.Format(mergeFailWarningMsg, piece1, piece2, piece1));
                return;
            }
            else if (queuedPieces.Contains(piece2))
            {
                logger.LogWarning(string.Format(mergeFailWarningMsg, piece1, piece2, piece2));
                return;
            }
        }

        mergeSet.Add(new Triplet<IPieceController>(piece1, piece2));
    }

    public Triplet<IPieceController> Consume()
    {
        // There isnt a simple way to just get any one element
        foreach (var triplet in mergeSet)
        {
            mergeSet.Remove(triplet);
            return triplet;
        }
        return null;
    }

    public void Merge(ISpawner spawner, Triplet<IPieceController> triplet)
    {
        if (config.AllowTripleMerge)
        {
            MergeTriple(spawner, triplet);
        }
        else
        {
            MergeDouble(spawner, triplet);
        }
    }

    void MergeTriple(ISpawner spawner, Triplet<IPieceController> triplet)
    {
        IPieceController p1 = triplet.v1;
        IPieceController p2 = triplet.v2;
        IPieceController p3 = triplet.v3;
        if (p1 == null)
        {
            MergeDouble(spawner, new Triplet<IPieceController>(triplet.v2, triplet.v3));
            return;
        }
        else if (p2 == null)
        {
            MergeDouble(spawner, new Triplet<IPieceController>(triplet.v1, triplet.v3));
            return;
        }
        else if (p3 == null)
        {
            MergeDouble(spawner, new Triplet<IPieceController>(triplet.v1, triplet.v2));
            return;
        }

        if (p1.Order + 2 >= config.GetHighestPieceOrder())
        {
            logger.LogWarning($"[PieceMerger] Trying to merge triplet {triplet} (resulting order: {p1.Order + 2}) will exceeded maximum piece order ({config.GetHighestPieceOrder()})");
            return;
        }

        bool isAnyMerging = p1.IsMerging || p2.IsMerging || p3.IsMerging;
        if (isAnyMerging)
        {
            logger.LogWarning($"[PieceMerger] Trying to merge merge a piece (either {p1} or {p2}) that is already merging!");
            return;
        }

        p1.IsMerging = true;
        p2.IsMerging = true;
        p3.IsMerging = true;

        spawner.DestroyPiece(p1);
        spawner.DestroyPiece(p2);
        spawner.DestroyPiece(p3);

        Vector3 position = (p1.Position + p2.Position + p2.Position) / 3f;
        spawner.SpawnAndPlayPiece(p1.Order + 2, position);

        IncrementScore(p1.Order, 3);
    }

    void MergeDouble(ISpawner spawner, Triplet<IPieceController> triplet)
    {
        if (triplet.v3 != null)
        {
            logger.LogWarning("[PieceMerger] Triplet has 3 pieces registered but merger is trying to merge double, third piece will be ignored");
            triplet.v3 = null;
        }

        IPieceController p1 = triplet.v1;
        IPieceController p2 = triplet.v2;

        if (p1.Order + 1 >= config.GetHighestPieceOrder())
        {
            logger.LogWarning($"[PieceMerger] Trying to merge triplet {triplet} (resulting order: {p1.Order + 1}) will exceeded maximum piece order ({config.GetHighestPieceOrder()})");
            return;
        }

        bool isAnyMerging = p1.IsMerging || p2.IsMerging;
        if (isAnyMerging)
        {
            logger.LogWarning($"[PieceMerger] Trying to merge merge a piece (either {p1} or {p2}) that is already merging!");
            return;
        }

        // These IsMerging control variables dont have any effect the way the code is now, everything runs completely sync and this flag is set
        // in the same method the pieces are destroyed. However, when I have animations for pieces being merged/destroyed, these animations will
        // probably run async and these flags will become more useful.
        p1.IsMerging = true;
        p2.IsMerging = true;

        Vector3 position = (p1.Position + p2.Position) / 2f;
        int newPieceOrder = p1.Order + 1;

        spawner.DestroyPiece(p1);
        spawner.DestroyPiece(p2);
        spawner.SpawnAndPlayPiece(newPieceOrder, position);
        
        IncrementScore(p1.Order, 2);
    }

    private void IncrementScore(int pieceOrder, int mergeCount)
    {
        scoreController.IncrementScore(config.BaseScore * (pieceOrder + mergeCount));
    }

    public List<Triplet<IPieceController>> GetQueuedPieces() => new List<Triplet<IPieceController>>(mergeSet);
}
