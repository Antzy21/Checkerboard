module internal Helpers.Data

open Checkerboard

module Coordinates =
    
    let _5_0 = (5,0)
    let _3_2 = (3,2)
    let _0_0 = (0,0)

module Pieces =
    
    let example1 = "Piece1ofStringType"
    let example2 = "Piece2ofStringType"

module Moves =

    let example1 =
        {piece = Some Pieces.example1; coordinates = Coordinates._3_2},
        {piece = None; coordinates = Coordinates._5_0}

    let example2 =
        {piece = Some Pieces.example1; coordinates = Coordinates._3_2},
        {piece = Some Pieces.example2; coordinates = Coordinates._5_0}


module Boards = 

    let exampleSize1 : board<string, int> =
        Board.init 1

    let exampleSize6 : board<string, int> =
        Board.init 6