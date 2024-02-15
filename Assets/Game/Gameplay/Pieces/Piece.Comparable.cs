using System;
using UnityEngine;

public partial class Piece : MonoBehaviour, IPiece, IEquatable<Piece>, IComparable<Piece> {

    public static bool operator == (Piece p1, Piece p2){
        bool isP1Null = ReferenceEquals(p1, null);
        bool isP2Null = ReferenceEquals(p2, null);
        bool areBothNull = isP1Null && isP2Null;
        bool isOneNull = isP1Null || isP2Null;

        if (areBothNull) {
        	return true;
        }

        if(isOneNull){
            return false;
        }

        return p1.pieceId == p2.pieceId;
    }

    public static bool operator != (Piece p1, Piece p2) => !(p1 == p2);

    public override bool Equals(object other){
        Piece otherPiece = other as Piece;
        if (otherPiece == null) return false;
        return pieceId == otherPiece.pieceId;
    }

	public bool Equals(Piece other){
        if (other == null) return false;
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
