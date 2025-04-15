using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType 
{
    Single4Directions, //OneCell: Up,Down,Left,Right
    Single8Directions, //OneCell: Up,Down,Left,Right,Diagonals

    Double4DirectionsObstructed, //TwoCells: Up,Down,Left,Right
    Double8DirectionsObstructed, //TwoCells: Up,Down,Left,Right,Diagonals
    Triple4DirectionsObstructed, //ThreeCells: Up,Down,Left,Right
    Triple8DirectionsObstructed, //ThreeCells: Up,Down,Left,Right,Diagonals

    Double4DirectionsJump, //TwoCells: Up,Down,Left,Right
    Double8DirectionsJump, //TwoCells: Up,Down,Left,Right,Diagonals
    Triple4DirectionsJump, //ThreeCells: Up,Down,Left,Right
    Triple8DirectionsJump, //ThreeCells: Up,Down,Left,Right,Diagonals

    RookObstructed, //AnyCells: Up,Down,Left,Right
    RookJump, //AnyCells: Up,Down,Left,Right

    BishopObstructed, //AnyCells: Diagonals
    BishopJump, //AnyCells: Diagonals

    QueenObstructed, //AnyCells: Up,Down,Left,Right,Diagonals
    QueenJump, //AnyCells: Up,Down,Left,Right,Diagonals

    Knight, //L type movement

    Immovable
}
