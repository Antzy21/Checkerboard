namespace Board

open Checkerboard
open Helpers.Data
open Xunit

//module ContainsPiece =

//    [<Fact>]
//    let ``Piece exists check`` () =
//        let board : board = 
//            Boards.exampleCharOption
//            |> Board.Update.Square.withPiece PieceParsers.fromChar Coordinates._3_2 Pieces.exampleA
//        Assert.True(Board.containsPiece Coordinates._3_2 board)
    
//    [<Fact>]
//    let ``Piece does not exist check`` () =
//        Assert.False(Board.containsPiece Coordinates._3_2 Boards.exampleCharOption)

module UpdateSquareWithPiece =

    [<Fact>]
    let ``Update square with piece`` () =
        let board : board = 
            Boards.exampleChar
            |> Board.updateSquare Coordinates._3_2 Pieces.exampleA
        let pieceAt_3_2 =
            Board.getSquareFromCoordinates board Coordinates._3_2
        Assert.Equal<squareBitMap>(Pieces.exampleA, pieceAt_3_2)

//module ApplyMove =

//    [<Fact>]
//    let ``Apply move to board`` () =
//        let board : board  =
//            Boards.exampleCharOption
//            |> Board.Update.Square.withPiece PieceParsers.fromChar Coordinates._3_2 Pieces.exampleA
//            |> Board.applyMove Coordinates._3_2 Coordinates._4_0
//        Assert.Equal(
//            Grid.fromCoordinates Coordinates._3_2 board,
//            None
//        )
//        Assert.Equal(
//            Grid.fromCoordinates Coordinates._4_0 board,
//            Some Pieces.exampleA
//        )

//    [<Fact>]
//    let ``Apply capture move to board`` () =
//        let board : board  =
//            Boards.exampleCharOption
//            |> Board.Update.Square.withPiece PieceParsers.fromChar Coordinates._3_2 Pieces.exampleA
//            |> Board.Update.Square.withPiece PieceParsers.fromChar Coordinates._4_0 Pieces.exampleB
//            |> Board.applyMove Coordinates._3_2 Coordinates._4_0
//        Assert.Equal(
//            Grid.fromCoordinates Coordinates._3_2 board,
//            None
//        )
//        Assert.Equal(
//            Grid.fromCoordinates Coordinates._4_0 board,
//            Some Pieces.exampleA
//        )
