using System;
using UnityEngine;

public partial class Piece : MonoBehaviour, IPiece, IEquatable<Piece>, IComparable<Piece> {

    public static bool operator == (Piece p1, Piece p2){
        if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null)) {
        	return false;
        }

        if (ReferenceEquals(p1, p2)){
            return true;
        }

        return p1.pieceId == p2.pieceId;
    }

    public static bool operator != (Piece p1, Piece p2) => !(p1 == p2);

	public bool Equals(Piece other){
        if (other is null) return false;
        return pieceId == other.pieceId;
    }

    public override int GetHashCode(){
        return pieceId;
    }

    public int CompareTo(Piece other){
        // Make null behave as the biggest value to work with hash calculation later
        if (other == null) return -1;
        return pieceId.CompareTo(other.pieceId);
    }
}
