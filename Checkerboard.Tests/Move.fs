module Tests.Move

open Checkerboard
open Helpers.Data
open Xunit

[<Fact>]
let ``getShift`` () =
    let result = Move.getShift Moves.example1
    Assert.Equal((1, -2), result)
    
[<Fact>]
let ``getMovedPiece`` () =
    let result = Move.getMovedPiece Moves.example1
    Assert.Equal(Pieces.exampleA, result)
    
[<Fact>]
let ``getDestinationPiece`` () =
    let result = Move.getPieceAtDestination Moves.example2
    Assert.Equal(Some Pieces.exampleB, result)
