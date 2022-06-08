namespace Checkerboard

type square<'Piece> = {piece: 'Piece option; coordinates: int * int}

module Square =
    let getName (square: square<'piece>) : string =
        let x, y = square.coordinates
        sprintf "%c%d" ['a'..'h'].[x] (y+1)
    let getFromName (name: string) : square<'Piece> =
        let j, c = System.Char.GetNumericValue name[1] |> int, name[0]
        let i = List.findIndex (fun letter -> letter = c) ['a'..'h']
        {piece = None; coordinates = (i, j-1)}

    let getPiece (square: square<'piece>) : 'piece =
        match square.piece with
        | None -> failwith $"No piece in on position {getName square}"
        | Some piece -> piece
    let updateWithPiece (piece: 'Piece) (square: square<'Piece>) : square<'Piece> =
        {piece = Some piece; coordinates = square.coordinates}
    let removePiece (square: square<'Piece>) : square<'Piece> =
        {piece = None; coordinates = square.coordinates}