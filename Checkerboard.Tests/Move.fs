namespace Move

open Checkerboard
open Helpers.Data
open Xunit

module getShift =

    [<Fact>]
    let ``(3,2)->(4,0)=>(1,-2)`` () =
        let result = Move.getShift Moves.example1
        Assert.Equal((1, -2), result)
    
module getMovedPiece =
    
    [<Fact>]
    let ``Is correct piece on board`` () =
        let result = Move.getMovedPiece Moves.example1
        Assert.Equal(Pieces.exampleA, result)
    
module getDestinationPiece =

    [<Fact>]
    let ``Is correct option type of piece`` () =
        let result = Move.getPieceAtDestination Moves.example2
        Assert.Equal(Some Pieces.exampleB, result)
