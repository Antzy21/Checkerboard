namespace Checkerboard

type board<'Piece> = square<'Piece>[,]

module Board =
    module Create =
        let empty (size: int) : board<'Piece> =
            Array2D.init size size (fun i j ->
                {piece = None; coordinates = (i, j)}
            )
            
    let private isOnBoard (position: coordinates) (board: board<'Piece>) : bool =
        let x, y = position
        x >= 0 && x < Array2D.length1 board && y >= 0 && y < Array2D.length2 board

    module GetSquare =
        let fromCoordinates ((i, j): coordinates) (board: board<'Piece>) : square<'Piece> =
            board.[i,j]
        let fromCoordinatesName (name: string) : board<'Piece> -> square<'Piece> =
            Coordinates.fromName name
            |> fromCoordinates
        let afterShift (shift: int * int) (start: coordinates) (board: board<'Piece>) : square<'Piece> option =
            let newCoordinates = Coordinates.afterShift shift start
            if isOnBoard newCoordinates board then
                Some <| fromCoordinates newCoordinates board
            else None        

    module GetSquares =
        let fromCoordinates (coordinatesList : coordinates list) (board: board<'Piece>) : square<'Piece> list =
            coordinatesList
            |> List.map (fun coordinates -> GetSquare.fromCoordinates coordinates board) 
        let afterShifts (start: coordinates) (board: board<'Piece>) (shifts: (int*int) list) : square<'Piece> list =
            shifts
            |> List.map (fun shift -> GetSquare.afterShift shift start board)
            |> List.filter Option.isSome
            |> List.map Option.get
        let rec afterRepeatedShift (shift: int * int) (start: coordinates) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            match GetSquare.afterShift shift start board with
            | None -> []
            | Some square ->
                match stopAt, square.piece with
                | Some stopAtPieceFunc, Some piece when stopAtPieceFunc piece -> [square]
                | _ -> List.append [square] <| afterRepeatedShift shift square.coordinates stopAt board
        let afterAllShiftDirections (start: coordinates) ((i, j) : int * int) (board: board<'Piece>) : square<'Piece> list =
            if i = 0 || j = 0 then
                [
                    (j+i,0);
                    (-j-i,0);
                    (0,i+j);
                    (0,-i-j)
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
        
        let adjacent (start: coordinates) (board: board<'Piece>) : square<'Piece> list =
            [(0,1); (1,1); (1,0); (1,-1); (0,-1); (-1,0); (-1,-1); (-1,1)]
            |> afterShifts start board
        let onDiagonals (start: coordinates) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            [(-1,1); (1,1); (-1,-1); (1,-1)]
            |> List.fold (fun s direction ->
                s |> List.append (afterRepeatedShift direction start stopAt board)
            ) List.empty<square<'Piece>>
        let onRowAndFile (start: coordinates) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            [(1,0); (-1,0); (0,1); (0,-1)]
            |> List.fold (fun s direction ->
                s |> List.append (afterRepeatedShift direction start stopAt board)
            ) List.empty<square<'Piece>>
        let onRowFileAndDiagonals (start: coordinates) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            List.append (onRowAndFile start stopAt board) (onDiagonals start stopAt board)            
        
    module GetPiece =
        let fromCoordinates (coordinates: coordinates) (board: board<'Piece>) : 'Piece option =
            GetSquare.fromCoordinates coordinates board
            |> fun sqr -> sqr.piece
        let fromCoordinatesName (name: string) : board<'Piece> -> 'Piece option =
            Coordinates.fromName name
            |> fromCoordinates
        
    let hasPieceOnSquare (squareName : string) (piece: 'Piece) (board : board<'Piece>) : bool =
        GetSquare.fromCoordinatesName squareName board
        |> fun square -> square.piece
        |> (=) (Some piece)

    module Update =
        module Square =
            let withPieceOption ((i,j): coordinates) (piece: 'Piece option) (board: board<'Piece>) =
                board[i,j] <- {piece = piece; coordinates = (i,j)}
            let withPiece (coordinates: coordinates) (piece: 'Piece) (board: board<'Piece>) =
                withPieceOption coordinates (Some piece) board
            let removePiece ((i,j): coordinates) (board: board<'Piece>) =
                board[i,j] <- {piece = None; coordinates = (i,j)}
        let applyMove ((startingSquare, endingSquare): move<'Piece>) (board: board<'Piece>) =
            Square.withPieceOption endingSquare.coordinates startingSquare.piece board
            Square.removePiece startingSquare.coordinates board