namespace Checkerboard

open System.Numerics
open FSharp.Extensions

type board<'Piece, 'Size when 'Size :> INumber<'Size>> = square<'Piece, 'Size>[,]

module Board =

    let init (size: 'Size when 'Size :> INumber<'Size>) : board<'Piece, 'Size> =
        let intSize = (INumber.CreateTruncating(size))
        Array2D.init intSize intSize (fun i j ->
            {piece = None; coordinates = Coordinates.createTruncating (i, j)}
        )
            
    let private isOnBoard (position: coordinates<'Size>) (board: board<'Piece, 'Size>) : bool =
        let x, y = Coordinates.createTruncating position
        x >= 'Size.Zero && x < 'Size.CreateChecked(Array2D.length1 board) && y >= 'Size.Zero && y < 'Size.CreateChecked(Array2D.length2 board)
    
    let containsPiece (coords: coordinates<'Size>) (board: board<'Piece, 'Size>) : bool =
        let i, j = Coordinates.createTruncating coords
        board.[i,j].piece
        |> Option.isSome

    module GetSquare =
        let fromCoordinates (c: coordinates<'Size>) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> =
            let (i,j) = Coordinates.createTruncating c
            board.[int(i),int(j)]
        let fromCoordinatesName (name: string) : board<'Piece, 'Size> -> square<'Piece, 'Size> =
            Coordinates.parse name
            |> fromCoordinates
        let afterShift (shift: 'Size * 'Size) (start: coordinates<'Size>) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> option =
            let newCoordinates = Coordinates.getAfterShift shift start
            if isOnBoard newCoordinates board then
                Some <| fromCoordinates newCoordinates board
            else None        

    module GetSquares =
        let fromCoordinates (coordinatesList : coordinates<'Size> list) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> list =
            coordinatesList
            |> List.map (fun coordinates -> GetSquare.fromCoordinates coordinates board) 
        let afterShifts (start: coordinates<'Size>) (board: board<'Piece, 'Size>) (shifts: ('Size*'Size) list) : square<'Piece, 'Size> list =
            shifts
            |> List.map (fun shift -> GetSquare.afterShift shift start board)
            |> List.filter Option.isSome
            |> List.map Option.get
        let rec afterRepeatedShift (shift: 'Size * 'Size) (start: coordinates<'Size>) (stopAt: ('Piece -> bool) option) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> list =
            match GetSquare.afterShift shift start board with
            | None -> []
            | Some square ->
                match stopAt, square.piece with
                | Some stopAtPieceFunc, Some piece when stopAtPieceFunc piece -> [square]
                | _ -> List.append [square] <| afterRepeatedShift shift square.coordinates stopAt board
        let afterAllShiftDirections (start: coordinates<'Size>) ((i, j) : 'Size * 'Size) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> list =
            if i = 'Size.Zero || j = 'Size.Zero then
                [
                    (j+i,'Size.Zero);
                    (-(j+i),'Size.Zero);
                    ('Size.Zero,i+j);
                    ('Size.Zero,-(i+j))
                ]                
            elif i = j then
                [
                    (i,i);
                    (i,-i);
                    (-i,i);
                    (-i,-i)
                ]
            else
                let x, y = 
                    if i > j then i, j
                    else j, i
                [
                    (x,y);
                    (x,-y);
                    (-x,y);
                    (-x,-y)
                    (y,x);
                    (y,-x);
                    (-y,x);
                    (-y,-x)
                ]
            |> afterShifts start board
        
        let adjacent (start: coordinates<'Size>) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> list =
            [
                ('Size.Zero,'Size.One);
                ('Size.One,'Size.One);
                ('Size.One,'Size.Zero);
                ('Size.One,-'Size.One);
                ('Size.Zero,-'Size.One);
                (-'Size.One,'Size.Zero);
                (-'Size.One,-'Size.One);
                (-'Size.One,'Size.One)]
            |> afterShifts start board
        let onDiagonals (start: coordinates<'Size>) (stopAt: ('Piece -> bool) option) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> list =
            [
                (-'Size.One,'Size.One); 
                ('Size.One,'Size.One);
                (-'Size.One,-'Size.One);
                ('Size.One,-'Size.One)
            ]
            |> List.fold (fun s direction ->
                s |> List.append (afterRepeatedShift direction start stopAt board)
            ) List.empty<square<'Piece, 'Size>>
        let onRowAndFile (start: coordinates<'Size>) (stopAt: ('Piece -> bool) option) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> list =
            [('Size.One,'Size.Zero); (-'Size.One,'Size.Zero); ('Size.Zero,'Size.One); ('Size.Zero,-'Size.One)]
            |> List.fold (fun s direction ->
                s |> List.append (afterRepeatedShift direction start stopAt board)
            ) List.empty<square<'Piece, 'Size>>
        let onRowFileAndDiagonals (start: coordinates<'Size>) (stopAt: ('Piece -> bool) option) (board: board<'Piece, 'Size>) : square<'Piece, 'Size> list =
            List.append (onRowAndFile start stopAt board) (onDiagonals start stopAt board)            
        
    module GetPiece =
        let fromCoordinates (coordinates: coordinates<'Size>) (board: board<'Piece, 'Size>) : 'Piece option =
            GetSquare.fromCoordinates coordinates board
            |> fun sqr -> sqr.piece
        let fromCoordinatesName (name: string) : board<'Piece, 'Size> -> 'Piece option =
            Coordinates.parse name
            |> fromCoordinates
        
    let hasPieceOnSquare (squareName : string) (piece: 'Piece) (board : board<'Piece, 'Size>) : bool =
        GetSquare.fromCoordinatesName squareName board
        |> fun square -> square.piece
        |> (=) (Some piece)

    module Update =
        module Square =
            let withPieceOption (c: coordinates<'Size>) (piece: 'Piece option) (board: board<'Piece, 'Size>) =
                let (i,j) = Coordinates.createTruncating c
                board[i,j] <- {piece = piece; coordinates = c}
            let withPiece (coordinates: coordinates<'Size>) (piece: 'Piece) (board: board<'Piece, 'Size>) =
                withPieceOption coordinates (Some piece) board
            let removePiece (c: coordinates<'Size>) (board: board<'Piece, 'Size>) =
                let (i,j) = Coordinates.createTruncating c
                board[i,j] <- {piece = None; coordinates = c}
        let applyMove ((startingSquare, endingSquare): move<'Piece, 'Size>) (board: board<'Piece, 'Size>) =
            Square.withPieceOption endingSquare.coordinates startingSquare.piece board
            Square.removePiece startingSquare.coordinates board
        let undoMove ((startingSquare, endingSquare): move<'Piece, 'Size>) (board: board<'Piece, 'Size>) =
            Square.withPieceOption endingSquare.coordinates endingSquare.piece board
            Square.withPieceOption startingSquare.coordinates startingSquare.piece board