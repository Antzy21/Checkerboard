namespace Checkerboard

type board<'Piece> = square<'Piece>[,]

module Board =
    module Create =
        let empty (size: int) : board<'Piece> =
            Array2D.init size size (fun i j ->
                {piece = None; coordinates = (i, j)}
            )

    module getSquares =
        let private isOnBoard (position: int * int) (board: board<'Piece>) : bool =
            let x, y = position
            x >= 0 && x < Array2D.base1 board && y >= 0 && y < Array2D.base2 board
        let afterShift (shift: int * int) (start: int * int) (board: board<'Piece>) : square<'Piece> option =
            let (i, j), (x, y) = shift, start
            if isOnBoard (x+i, y+j) board then
                Some board.[x+i,y+j]
            else None
        let rec afterRepeatedShift (shift: int * int) (start: int * int) (stopAt: ('Piece -> bool) option) (board: board<'Piece>) : square<'Piece> list =
            match afterShift shift start board with
            | None -> []
            | Some square ->
                match stopAt, square.piece with
                | Some stopAtPieceFunc, Some piece when stopAtPieceFunc piece -> []
                | _ -> List.append [square] <| afterRepeatedShift shift square.coordinates stopAt board
        
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
        let afterAllShiftDirections (start: square<'Piece>) (i: int) (j: int) (board: board<'Piece>) : square<'Piece> list =
            if i = 0 || j = 0 then
                [
                    afterShift start.coordinates (j+i,0) board;
                    afterShift start.coordinates (-j-i,0) board;
                    afterShift start.coordinates (0,i+j) board;
                    afterShift start.coordinates (0,-i-j) board
                ]                
            elif i = j then
                [
                    afterShift start.coordinates (i,i) board;
                    afterShift start.coordinates (i,-i) board;
                    afterShift start.coordinates (-i,i) board;
                    afterShift start.coordinates (-i,-i) board
                ]
            else
                let x, y = 
                    if i > j then i, j
                    else j, i
                [
                    afterShift start.coordinates (x,y) board;
                    afterShift start.coordinates (x,-y) board;
                    afterShift start.coordinates (-x,y) board;
                    afterShift start.coordinates (-x,-y) board
                    afterShift start.coordinates (y,x) board;
                    afterShift start.coordinates (y,-x) board;
                    afterShift start.coordinates (-y,x) board;
                    afterShift start.coordinates (-y,-x) board
                ]
            |> List.filter Option.isSome
            |> List.map Option.get
                
                
