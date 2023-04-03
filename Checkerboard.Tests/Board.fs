module Tests.Board

open Checkerboard
open Helpers.Data
open Xunit

[<Fact>]
let ``Piece exists check`` () =
    let board : board<string, int> = Boards.exampleSize6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.exampleA board
    Assert.True(Board.containsPiece Coordinates._3_2 board)

[<Fact>]
let ``Piece does not exist check`` () =
    Assert.False(Board.containsPiece Coordinates._3_2 Boards.exampleSize6)

[<Fact>]
let ``Update square with piece`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.exampleA board
    Assert.Equal(board.[3,2], {piece = Some Pieces.exampleA; coordinates = Coordinates._3_2})

[<Fact>]
let ``Apply move to board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.exampleA board
    Board.applyMove Moves.example2 board
    Assert.Equal(board.[3,2], {piece = None; coordinates = Coordinates._3_2})
    Assert.Equal(board.[4,0], {piece = Some Pieces.exampleA; coordinates = Coordinates._4_0})
    
[<Fact>]
let ``Apply capture move to board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.exampleA board
    Board.Update.Square.withPiece Coordinates._4_0 Pieces.exampleB board
    Board.applyMove Moves.example1 board
    Assert.Equal(board.[3,2], {piece = None; coordinates = Coordinates._3_2})
    Assert.Equal(board.[4,0], {piece = Some Pieces.exampleA; coordinates = Coordinates._4_0})
    
[<Fact>]
let ``Undo move on board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.exampleA board
    Board.applyMove Moves.example1 board
    Board.undoMove Moves.example1 board
    Assert.Equal(board.[3,2], {piece = Some Pieces.exampleA; coordinates = Coordinates._3_2})
    Assert.Equal(board.[4,0], {piece = None; coordinates = Coordinates._4_0})
    
[<Fact>]
let ``Undo capture move on board`` () =
    let board : board<string, int> = Board.init 6
    Board.Update.Square.withPiece Coordinates._3_2 Pieces.exampleA board
    Board.applyMove Moves.example2 board
    Board.undoMove Moves.example2 board
    Assert.Equal(board.[3,2], {piece = Some Pieces.exampleA; coordinates = Coordinates._3_2})
    Assert.Equal(board.[4,0], {piece = Some Pieces.exampleB; coordinates = Coordinates._4_0})

