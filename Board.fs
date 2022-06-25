namespace Checkerboard

type board<'Piece> = square<'Piece>[,]

module Board =
    module Create =
        let empty (size: int) : board<'Piece> =
            Array2D.init size size (fun i j ->
                {piece = None; coordinates = (i, j)}
            )

    module getSquares =
        let private isOnBoard (position: coordinates) (board: board<'Piece>) : bool =
            let x, y = position
            x >= 0 && x < Array2D.length1 board && y >= 0 && y < Array2D.length2 board
        let afterShift (shift: int * int) (start: coordinates) (board: board<'Piece>) : square<'Piece> option =
            let (i, j), (x, y) = shift, start
            if isOnBoard (x+i, y+j) board then
                Some board.[x+i,y+j]
            else None
        let afterShifts (start: coordinates) (board: board<'Piece>) (shifts: (int*int) list) : square<'Piece> list =
            shifts
            |> List.map (fun shift -> afterShift shift start board)
            |> List.filter Option.isSome
            |> List.map Option.get
        let rec afterRepeatedShift (shift: int * int) (start: coordinates) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            match afterShift shift start board with
            | None -> []
            | Some square ->
                match stopAt, square.piece with
                | Some stopAtPieceFunc, Some piece when stopAtPieceFunc piece -> [square]
                | _ -> List.append [square] <| afterRepeatedShift shift square.coordinates stopAt board
        
        let adjacent (start: square<'Piece>) (board: board<'Piece>) : square<'Piece> list =
            [(0,1); (1,1); (1,0); (1,-1); (0,-1); (-1,0); (-1,-1); (-1,1)]
            |> afterShifts start.coordinates board
        let getDiagonals (start: square<'Piece>) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            [(-1,1); (1,1); (-1,-1); (1,-1)]
            |> List.fold (fun s direction ->
                s |> List.append (afterRepeatedShift direction start.coordinates stopAt board)
            ) List.empty<square<'Piece>>
        let getRowAndFile (start: square<'Piece>) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            [(1,0); (-1,0); (0,1); (0,-1)]
            |> List.fold (fun s direction ->
                s |> List.append (afterRepeatedShift direction start.coordinates stopAt board)
            ) List.empty<square<'Piece>>
        let afterAllShiftDirections (start: square<'Piece>) ((i, j) : int * int) (board: board<'Piece>) : square<'Piece> list =
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
            |> afterShifts start.coordinates board
        
    let getSquare ((i, j): coordinates) (board: board<'Piece>) : square<'Piece> =
        board.[i,j]
    let getSquareFromCoordinatesName (name: string) (board: board<'Piece>) : square<'Piece> =
        let coordinates = name |> Coordinates.fromName
        getSquare coordinates board

    let hasPieceOnSquare (squareName : string) (piece: 'Piece) (board : board<'Piece>) : bool =
        getSquareFromCoordinatesName squareName board
        |> fun square -> square.piece
        |> (=) <| Some piece

    let updateWithPiece ((i,j): coordinates)  (piece: 'Piece) (board: board<'Piece>) : board<'Piece> =
        board[i,j] <- {piece = Some piece; coordinates = (i,j)}
        board
    let removePiece ((i,j): coordinates) (board: board<'Piece>) : board<'Piece> =
        board[i,j] <- {piece = None; coordinates = (i,j)}
        board
    let movePiece (move: move<'Piece>) (board: board<'Piece>) : board<'Piece> =
        let (startingSquare, endingSquare) = move
        let piece = Square.getPiece startingSquare
        board
        |> Array2D.copy
        |> updateWithPiece endingSquare.coordinates piece
        |> removePiece startingSquare.coordinates