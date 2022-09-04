module Tests.Board

open Checkerboard
open Helpers.Data
open Xunit

[<Fact>]
let ``Update square with piece`` () =
    let board : board<string> = Board.Create.empty 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.example1 board
    Assert.Equal(board.[3,2], {piece = Some Pieces.example1; coordinates = Coordinates._3_2})

[<Fact>]
let ``Apply move to board`` () =
    let board : board<string> = Board.Create.empty 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.example1 board
    Board.Update.applyMove Moves.example2 board
    Assert.Equal(board.[3,2], {piece = None; coordinates = Coordinates._3_2})
    Assert.Equal(board.[5,0], {piece = Some Pieces.example1; coordinates = Coordinates._5_0})
