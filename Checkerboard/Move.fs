namespace Checkerboard

type move<'Piece> = square<'Piece> * square<'Piece>

module Move =
    let getShift (move: move<'Piece>) : int * int =
        let start = (fst move).coordinates
        let finish = (snd move).coordinates
        Coordinates.getShiftBetween start finish
    let getPieceAtDestination (move: move<'Piece>) : 'Piece option =
        snd move
        |> fun square -> square.piece
    let getMovedPiece (move: move<'Piece>) : 'Piece =
        fst move
        |> Square.getPiece
