using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType 
{
    Simple4Directions, //OneCell: Up,Down,Left,Right
    Simple8Directions, //OneCell: Up,Down,Left,Right,Diagonals
    Double4Directions, //TwoCells: Up,Down,Left,Right
    Double8Directions, //TwoCells: Up,Down,Left,Right,Diagonals

    Rook, //AnyCells: Up,Down,Left,Right
    Bishop, //AnyCells: Diagonals
    Knight, //L type movement
    Queen //AnyCells: Up,Down,Left,Right,Diagonals
}
