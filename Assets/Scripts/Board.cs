using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls elements of the board
public class Board : MonoBehaviour {

	Dictionary<HexCoordinates, Tile> tiles = new Dictionary<HexCoordinates, Tile>();

    public Tile GetTile(HexCoordinates coordinates) {
        if (!tiles.ContainsKey(coordinates)) {
            return null;
        }
        return tiles[coordinates];
    }

    public void SetTile(HexCoordinates coordinates, Tile tile) {
        if (!tiles.ContainsKey(coordinates)) {
            tiles.Add(coordinates, tile);
            return;
        }
        tiles[coordinates] = tile;
    }
}
