namespace Checkerboard

type square<'Piece> = {piece: 'Piece option; coordinates: int * int}

module Square =
    let getName (square: square<'piece>) : string =
        let x, y = square.coordinates
        sprintf "%c%d" ['a'..'h'].[x] y
    let getPiece (square: square<'piece>) : 'piece =
        match square.piece with
        | None -> failwith $"No piece in on position {getName square}"
        | Some piece -> piece
    let updateWithPiece (piece: 'Piece) (square: square<'Piece>) : square<'Piece> =
        {piece = Some piece; coordinates = square.coordinates}
    let removePiece (square: square<'Piece>) : square<'Piece> =
        {piece = None; coordinates = square.coordinates}