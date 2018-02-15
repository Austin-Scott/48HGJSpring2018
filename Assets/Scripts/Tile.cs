using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls elements of the board
public class Tile : MonoBehaviour {

    /// Location of the tile
    HexCoordinates location;

    /// The deck holder on the tile
    DeckHolder deckHolder = null;

    /// Cards on the tile
    List<Card> cards;

    /// The board this tile is on.
    Board board;

    public void Initialize(HexCoordinates location, DeckHolder deckHolder = null) {
        cards = new List<Card>();
        this.deckHolder = deckHolder;
        this.location = location;
    }

    public Tile GetNeighbor(int directionIndex) {
        return board.GetTile(location.GetNeighbor(directionIndex));
    }
}