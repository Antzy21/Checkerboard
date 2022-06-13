namespace Checkerboard

type square<'Piece> = {piece: 'Piece option; coordinates: int * int}

module Square =
    let getPiece (square: square<'piece>) : 'piece =
        match square.piece with
        | None -> failwith $"No piece in on position {Coordinates.getName square.coordinates}"
        | Some piece -> piece
    let getCoordinatesName (square: square<'piece>) : string =
        square.coordinates |> Coordinates.getName
    let updateWithPiece (piece: 'Piece) (square: square<'Piece>) : square<'Piece> =
        {piece = Some piece; coordinates = square.coordinates}
    let removePiece (square: square<'Piece>) : square<'Piece> =
        {piece = None; coordinates = square.coordinates}