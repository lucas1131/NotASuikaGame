using System;
using System.Collections.Generic;

public interface IPieceMergerManager {

}



public class PieceMergerManager : IPieceMergerManager {

    bool allowTripleMerge;
    HashSet<Triplet<Piece>> mergeSet;

    public PieceMergerManager(bool allowTripleMerge){
        this.allowTripleMerge = allowTripleMerge;
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

        mergeSet.Add(new Triplet<Piece>(piece1, piece2, null));
    }

    void RegisterForDoubleMerge(Piece piece1, Piece piece2){
        mergeSet.Add(new Triplet<Piece>(piece1, piece2, null));
    }

    public void UnregisterPieces(Piece piece1, Piece piece2){

    }

    public void DoMerge(){

    }
}
