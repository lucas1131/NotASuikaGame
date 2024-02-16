using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceMerger : IPieceMerger {

    IGameConfig config;
    bool allowTripleMerge;
    HashSet<Triplet<Piece>> mergeSet;


    public PieceMerger(IGameConfig config){
        this.config = config;
        this.allowTripleMerge = config.AllowTripleMerge;
        mergeSet = new HashSet<Triplet<Piece>>();
    }

    public void RegisterPieces(Piece piece1, Piece piece2){
        if(allowTripleMerge){
            RegisterForTripleMerge(piece1, piece2);
        } else {
            RegisterForDoubleMerge(piece1, piece2);
        }
    }

    void RegisterForTripleMerge(Piece piece1, Piece piece2){
        if(piece1.PieceOrder >= config.GetHighestPieceOrder()) {
            return;
        }

        foreach(Triplet<Piece> triplet in mergeSet){
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

        mergeSet.Add(new Triplet<Piece>(piece1, piece2));
    }

    void RegisterForDoubleMerge(Piece piece1, Piece piece2){
        mergeSet.Add(new Triplet<Piece>(piece1, piece2));
    }

    public Triplet<Piece> Consume(){
        // There isnt a simple way to just get any one element
        foreach(var triplet in mergeSet){
            mergeSet.Remove(triplet);
            return triplet;
        }
        return null;
    }

    public void Merge(ISpawner spawner, Triplet<Piece> triplet){
        if(allowTripleMerge){
            MergeTriple(spawner, triplet);
        } else {
            MergeDouble(spawner, triplet);
        }
    }

    void MergeTriple(ISpawner spawner, Triplet<Piece> triplet){
        Piece p1 = triplet.v1;
        Piece p2 = triplet.v2;
        Piece p3 = triplet.v3;

        if(p1 == null) {
            MergeDouble(spawner, new Triplet<Piece>(triplet.v2, triplet.v3));
            return;
        } else if(p2 == null) {
            MergeDouble(spawner, new Triplet<Piece>(triplet.v1, triplet.v3));
            return;
        } else if(p3 == null) {
            MergeDouble(spawner, new Triplet<Piece>(triplet.v1, triplet.v2));
            return;
        }

        bool isAnyMerging = p1.IsMerging & p2.IsMerging & p3.IsMerging;
        if(isAnyMerging) {
            return;
        }

        p1.IsMerging = true;
        p2.IsMerging = true;
        p3.IsMerging = true;

        // dont like this, the object is instantiated somewhere else but is destroyed here, its not consistent and easy to lose track of references this way
        p1.DestroyPiece();
        p2.DestroyPiece();
        p3.DestroyPiece();

        Vector3 position = (p1.Position + p2.Position + p2.Position)/3f;
        spawner.SpawnPieceFromMerge(p1.PieceOrder+2, position);
    }

    void MergeDouble(ISpawner spawner, Triplet<Piece> triplet){
        Piece p1 = triplet.v1;
        Piece p2 = triplet.v2;

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

    public void TestTriplet(){
        // only using for debug, should probably convert this into an unit test
        // Piece p1 = new Piece();
        // p1.Setup(null, 1, 0f);
        // Piece p2 = new Piece();
        // p2.Setup(null, 2, 0f);
        // Piece p3 = new Piece();
        // p3.Setup(null, 3, 0f);
        // Piece p4 = new Piece();
        // p4.Setup(null, 4, 0f);
        // Piece p5 = new Piece();
        // p5.Setup(null, 5, 0f);
        // Piece p6 = new Piece();
        // p6.Setup(null, 6, 0f);
        // Piece p7 = new Piece();
        // p7.Setup(null, 7, 0f);
        // Piece p8 = new Piece();
        // p8.Setup(null, 8, 0f);


        // // these are all working
        // var t123 = new Triplet<Piece>(p1, p2, p3);
        // var t132 = new Triplet<Piece>(p1, p3, p2);
        // var t312 = new Triplet<Piece>(p3, p1, p2);
        // var t321 = new Triplet<Piece>(p3, p2, p1);
        // var t178 = new Triplet<Piece>(p1, p7, p8);

        // // null seems sus
        // var t1n3 = new Triplet<Piece>(p1, null, p3);
        // var t13n = new Triplet<Piece>(p1, p3, null);
        // var t31n = new Triplet<Piece>(p3, p1, null);
        // var t3n1 = new Triplet<Piece>(p3, null, p1);
        // var tn13 = new Triplet<Piece>(null, p1, p3);
        // var tnn1 = new Triplet<Piece>(null, null, p1);
        // var tn1n = new Triplet<Piece>(null, p1, null);
        // var t1nn = new Triplet<Piece>(p1, null, null);
        // var tnnn = new Triplet<Piece>(null, null, null);
        // var tnnn_2 = new Triplet<Piece>(null, null, null);
        // var t34n = new Triplet<Piece>(p3, p4, null);

        // mergeSet.Add(t123);
        // mergeSet.Add(t123);
        // mergeSet.Add(t132);
        // mergeSet.Add(t312);
        // mergeSet.Add(t321);
        // mergeSet.Add(t178);
        // mergeSet.Add(t1n3);
        // mergeSet.Add(t13n);
        // mergeSet.Add(t31n);
        // mergeSet.Add(t3n1);
        // mergeSet.Add(tn13);
        // mergeSet.Add(tnn1);
        // mergeSet.Add(tn1n);
        // mergeSet.Add(t1nn);
        // mergeSet.Add(tnnn);
        // mergeSet.Add(tnnn_2);
        // mergeSet.Add(t34n);

        // mergeSet.TryGetValue

        // foreach(var triplet in mergeSet){
        // }
    }
}
