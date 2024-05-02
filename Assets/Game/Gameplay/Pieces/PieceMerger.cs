using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceMerger : IPieceMerger {

    IGameConfig config;
    bool allowTripleMerge;
    HashSet<Triplet<IPieceController>> mergeSet;


    public PieceMerger(IGameConfig config){
        this.config = config;
        this.allowTripleMerge = config.AllowTripleMerge;
        mergeSet = new HashSet<Triplet<IPieceController>>();
    }

    public void RegisterPieces(IPieceController piece1, IPieceController piece2){
        if(allowTripleMerge){
            RegisterForTripleMerge(piece1, piece2);
        } else {
            RegisterForDoubleMerge(piece1, piece2);
        }
    }

    void RegisterForTripleMerge(IPieceController piece1, IPieceController piece2){
        foreach(Triplet<IPieceController> triplet in mergeSet){
            if(triplet.Contains(piece1)){

                if(triplet.Contains(piece2)){
                    return;
                }

                mergeSet.Remove(triplet);
                triplet.AddValueOverNull(piece2);
                mergeSet.Add(triplet);
                return;

            } else if(triplet.Contains(piece2)){

                if(triplet.Contains(piece1)){
                    return;
                }

                mergeSet.Remove(triplet);
                triplet.AddValueOverNull(piece1);
                mergeSet.Add(triplet);
                return;
            }
        }

        mergeSet.Add(new Triplet<IPieceController>(piece1, piece2));
    }

    void RegisterForDoubleMerge(IPieceController piece1, IPieceController piece2){
        mergeSet.Add(new Triplet<IPieceController>(piece1, piece2));
    }

    public Triplet<IPieceController> Consume(){
        // There isnt a simple way to just get any one element
        foreach(var triplet in mergeSet){
            mergeSet.Remove(triplet);
            return triplet;
        }
        return null;
    }

    public void Merge(ISpawner spawner, Triplet<IPieceController> triplet){
        if(allowTripleMerge){
            MergeTriple(spawner, triplet);
        } else {
            MergeDouble(spawner, triplet);
        }
    }

    void MergeTriple(ISpawner spawner, Triplet<IPieceController> triplet){
        IPieceController p1 = triplet.v1;
        IPieceController p2 = triplet.v2;
        IPieceController p3 = triplet.v3;

        if(p1 == null) {
            MergeDouble(spawner, new Triplet<IPieceController>(triplet.v2, triplet.v3));
            return;
        } else if(p2 == null) {
            MergeDouble(spawner, new Triplet<IPieceController>(triplet.v1, triplet.v3));
            return;
        } else if(p3 == null) {
            MergeDouble(spawner, new Triplet<IPieceController>(triplet.v1, triplet.v2));
            return;
        }

        if(p1.PieceOrder+2 >= config.GetHighestPieceOrder()) {
            return;
        }

        bool isAnyMerging = p1.IsMerging & p2.IsMerging & p3.IsMerging;
        if(isAnyMerging) {
            return;
        }

        p1.IsMerging = true;
        p2.IsMerging = true;
        p3.IsMerging = true;

        p1.DestroyPiece();
        p2.DestroyPiece();
        p3.DestroyPiece();

        Vector3 position = (p1.Position + p2.Position + p2.Position)/3f;
        spawner.SpawnPieceFromMerge(p1.PieceOrder+2, position);
    }

    void MergeDouble(ISpawner spawner, Triplet<IPieceController> triplet){
        IPieceController p1 = triplet.v1;
        IPieceController p2 = triplet.v2;

        if(p1.PieceOrder+1 >= config.GetHighestPieceOrder()) {
            return;
        }

        bool isAnyMerging = p1.IsMerging & p2.IsMerging;
        if(isAnyMerging) {
            return;
        }

        p1.IsMerging = true;
        p2.IsMerging = true;

        // dont like this, the object is instantiated somewhere else but is destroyed here, its not consistent and easy to lose track of references this way
        p1.DestroyPiece();
        p2.DestroyPiece();

        Vector3 position = (p1.Position + p2.Position)/2f;
        spawner.SpawnPieceFromMerge(p1.PieceOrder+1, position);
    }
}
