namespace Checkerboard

open System.Numerics
open FSharp.Extensions

type move<'Piece, 'BoardSize when 'BoardSize :> INumber<'BoardSize>> = square<'Piece, 'BoardSize> * square<'Piece, 'BoardSize>

module Move =
    let getShift (move: move<'Piece, 'BoardSize>) : 'BoardSize * 'BoardSize =
        let start = (fst move).coordinates
        let finish = (snd move).coordinates
        Coordinates.getDistanceBetweenCoordinates start finish
    let getPieceAtDestination (move: move<'Piece, 'BoardSize>) : 'Piece option =
        snd move
        |> fun square -> square.piece
    let getMovedPiece (move: move<'Piece, 'BoardSize>) : 'Piece =
        fst move
        |> Square.getPiece
