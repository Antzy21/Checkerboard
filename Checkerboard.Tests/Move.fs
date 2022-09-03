module Tests.Move

open Checkerboard
open Xunit

let exampleMove1 =
    {piece = Some "myPiece1"; coordinates = (6,2)},
    {piece = Some "myPiece2"; coordinates = (3,5)}

[<Fact>]
let ``getShift`` () =
    let result = Move.getShift exampleMove1
    Assert.Equal((-3, 3), result)
    
[<Fact>]
let ``getMovedPiece`` () =
    let result = Move.getMovedPiece exampleMove1
    Assert.Equal("myPiece1", result)
    
[<Fact>]
let ``getDestinationPiece`` () =
    let result = Move.getPieceAtDestination exampleMove1
    Assert.Equal(Some "myPiece2", result)
