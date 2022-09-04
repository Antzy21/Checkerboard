module Tests.Move

open Checkerboard
open Helpers.Data
open Xunit

[<Fact>]
let ``getShift`` () =
    let result = Move.getShift Moves.example1
    Assert.Equal((-2, 2), result)
    
[<Fact>]
let ``getMovedPiece`` () =
    let result = Move.getMovedPiece Moves.example1
    Assert.Equal(Pieces.example1, result)
    
[<Fact>]
let ``getDestinationPiece`` () =
    let result = Move.getPieceAtDestination Moves.example1
    Assert.Equal(Some Pieces.example2, result)
