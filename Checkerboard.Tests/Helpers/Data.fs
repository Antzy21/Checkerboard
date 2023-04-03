module internal Helpers.Data

open Checkerboard

module Coordinates =
    
    let _4_0 = (4,0)
    let _3_2 = (3,2)
    let _0_0 = (0,0)

module Pieces =
    
    let exampleA = "Piece1ofStringType"
    let exampleB = "Piece2ofStringType"

module Moves =

    let example1 =
        {piece = Some Pieces.exampleA; coordinates = Coordinates._3_2},
        {piece = None; coordinates = Coordinates._4_0}

    let example2 =
        {piece = Some Pieces.exampleA; coordinates = Coordinates._3_2},
        {piece = Some Pieces.exampleB; coordinates = Coordinates._4_0}


module Boards = 

    let exampleSize1 : board<string, int> =
        Board.init 1

    let exampleSize6 : board<string, int> =
        Board.init 6