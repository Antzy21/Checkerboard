module Tests.Board

open Checkerboard
open Helpers.Data
open Xunit

[<Fact>]
let ``Update square with piece`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.example1 board
    Assert.Equal(board.[3,2], {piece = Some Pieces.example1; coordinates = Coordinates._3_2})

[<Fact>]
let ``Apply move to board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.example1 board
    Board.applyMove Moves.example2 board
    Assert.Equal(board.[3,2], {piece = None; coordinates = Coordinates._3_2})
    Assert.Equal(board.[5,0], {piece = Some Pieces.example1; coordinates = Coordinates._5_0})
    
[<Fact>]
let ``Apply capture move to board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.example1 board
    Board.Update.Square.withPiece Coordinates._5_0 Pieces.example2 board
    Board.applyMove Moves.example1 board
    Assert.Equal(board.[3,2], {piece = None; coordinates = Coordinates._3_2})
    Assert.Equal(board.[5,0], {piece = Some Pieces.example1; coordinates = Coordinates._5_0})
    
[<Fact>]
let ``Undo move on board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.example1 board
    Board.applyMove Moves.example1 board
    Board.undoMove Moves.example1 board
    Assert.Equal(board.[3,2], {piece = Some Pieces.example1; coordinates = Coordinates._3_2})
    Assert.Equal(board.[5,0], {piece = None; coordinates = Coordinates._5_0})
    
[<Fact>]
let ``Undo capture move on board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.example1 board
    Board.applyMove Moves.example2 board
    Board.undoMove Moves.example2 board
    Assert.Equal(board.[3,2], {piece = Some Pieces.example1; coordinates = Coordinates._3_2})
    Assert.Equal(board.[5,0], {piece = Some Pieces.example2; coordinates = Coordinates._5_0})

