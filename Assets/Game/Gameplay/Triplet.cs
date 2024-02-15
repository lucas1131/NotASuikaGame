using System;


/*
    Triplet structure made to work with Sets -- maybe call this TripletSet? the order of the values
    doesn't matter for this structure, ie: triple(1, 2, 3) is the same as triple(3, 1, 2) and triple(3, 2, 1)
*/
public sealed class Triplet<T> : IEquatable<Triplet<T>> where T : IEquatable<T> {

    T v1;
    T v2;
    T v3;

    public Triplet(T v1, T v2, T v3){
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
    }

    public bool Contains(T val){
        bool isValNull = ReferenceEquals(val, null);
        return ((this.v1?.Equals(val) ?? isValNull) ||
                (this.v2?.Equals(val) ?? isValNull) ||
                (this.v3?.Equals(val) ?? isValNull));
    }

    public bool Equals(Triplet<T> other){
        if (ReferenceEquals(other, null))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return other.Contains(v1) &&
               other.Contains(v2) &&
               other.Contains(v3);
    }

    public override int GetHashCode(){
        T[] set = new T[] { v1, v2, v3 };

        Array.Sort(set);

        int hash = 0;
        for(int i = 0; i < set.Length; i++){
            T val = set[i];
            if(val == null) {
                hash += Int32.MaxValue/3; // Maximum value becomes triplet of 3 nulls
            } else {
                hash += val.GetHashCode() << i;
            }
        }

        return hash;
    }

    public override string ToString(){
        return $"({v1?.ToString() ?? "null"}, {v2?.ToString() ?? "null"}, {v3?.ToString() ?? "null"})";
    }
}