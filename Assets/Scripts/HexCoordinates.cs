using System;
using UnityEngine;

public struct HexCoordinates {
    int x;
    int y;
    int z;

    public HexCoordinates(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // public static bool operator== (HexCoordinates a, HexCoordinates b) {
    //     return a.x == b.x && a.y == b.y && a.z == b.z;
    // }

    // public static bool operator!= (HexCoordinates a, HexCoordinates b) {
    //     return !(a==b);
    // }

    public static HexCoordinates operator+ (HexCoordinates a, HexCoordinates b) {
        return new HexCoordinates(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static HexCoordinates operator- (HexCoordinates a, HexCoordinates b) {
        return new HexCoordinates(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static int Length(HexCoordinates coordinates) {
        return (Math.Abs(coordinates.x) + Math.Abs(coordinates.y) + Math.Abs(coordinates.z))/2;
    }

    public static int Distance(HexCoordinates from, HexCoordinates to) {
        return Length(to - from);
    }

    public static HexCoordinates[] hexDirections = {
        new HexCoordinates(1, 0, -1),
        new HexCoordinates(1, -1, 0),
        new HexCoordinates(0, -1, 1),
        new HexCoordinates(-1, 0, 1),
        new HexCoordinates(-1, 1, 0),
        new HexCoordinates(0, 1, -1)
    };

    public HexCoordinates GetNeighbor(int directionIndex) {
        if (directionIndex < 0 || directionIndex > 5) {
            Debug.LogError("Direction index out of bounds");
        }
        return this + hexDirections[directionIndex];
    }
}