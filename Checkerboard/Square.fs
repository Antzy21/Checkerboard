namespace Checkerboard

open System.Numerics
open FSharp.Extensions

type square<'Piece, 'BoardSize when 'BoardSize :> INumber<'BoardSize>> = {piece: 'Piece option; coordinates: coordinates<'BoardSize>}

module Square =
    let getPiece (square: square<'Piece, 'Size>) : 'Piece =
        match square.piece with
        | None -> failwith $"No piece in on position {Coordinates.getName square.coordinates}"
        | Some piece -> piece
    let getCoordinates (square: square<'Piece, 'Size>) : coordinates<'Size> =
        square.coordinates
    let getFile (square: square<'Piece, 'Size>) : string =
        square.coordinates |> Coordinates.getFile
    let getRow (square: square<'Piece, 'Size>) : string =
        square.coordinates |> Coordinates.getRow
    let getCoordinatesName (square: square<'Piece, 'Size>) : string =
        square.coordinates |> Coordinates.getName
    let updateWithPiece (piece: 'Piece) (square: square<'Piece, 'Size>) : square<'Piece, 'Size> =
        {piece = Some piece; coordinates = square.coordinates}
    let removePiece (square: square<'Piece, 'Size>) : square<'Piece, 'Size> =
        {piece = None; coordinates = square.coordinates}
    let hasPiece (piece: 'Piece) (square: square<'Piece, 'Size>) : bool =
        square.piece
        |> (=) (Some piece)