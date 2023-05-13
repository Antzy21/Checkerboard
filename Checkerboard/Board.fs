namespace Checkerboard

open System.Numerics
open FSharp.Extensions

type board = bitMap list

/// A board a an 8x8 collection of generic squares.
/// It is stored as a list of uint64.
///
/// Functions in this module will usually need to be given a "parser" or "converter" between the square bitmap representation and more well defined types.
module Board =

    /// Initialise a board
    let init (squareMapsLength: int) : board =
        List.init squareMapsLength (fun _ -> 0UL)

    /// Checks if given coordinates are on the board
    let private isOnBoard ((x, y): coordinates) (board: board) : bool =
        let dim = 
            board[0]
            |> BitMap.getDimensionFromIntegerType
        x >= 0 && x < dim && y >= 0 && y < dim
        
    module GetSquare =

        let fromCoordinatesResult (board: board) ((i,j): coordinates) : squareBitMap result =
            if isOnBoard (i,j) board then
                board
                |> List.map (fun map ->
                   BitMap.getValueAtCoordinatesResult (i,j) map
                )
                |> List.filterResults
                |> Ok
            else
                Error $"The coordinates ({i}, {j}) are not on the board."

        let fromCoordinatesOption (board: board) (coords: coordinates) : squareBitMap option =
            fromCoordinatesResult board coords |> Result.toOption

        let fromCoordinates (board: board) (coords: coordinates) : squareBitMap =
            fromCoordinatesResult board coords |> Result.failOnError

    module GetCoordinates =

        /// Returns a new collection containing only the coordinates<int> that are on the board
        let onBoard (board: board) (coordinatesList: coordinates list) : coordinates list =
            coordinatesList
            |> List.filter (fun coords ->
                isOnBoard coords board
            )

        let afterShifts (start: coordinates<int>) (board: board) (shifts: (struct (int*int)) seq) : coordinates<int> list =
            shifts
            |> Coordinates.getAfterShifts start
            |> Seq.toList
            |> onBoard board
        let rec afterRepeatedShift (shift: struct (int*int)) (start: coordinates<int>) (board: board) : coordinates<int> list =
            let isNotOnBoard = fun coords -> isOnBoard coords board |> not
            Coordinates.afterRepeatedShift isNotOnBoard shift start
            |> onBoard board
        let rec afterRepeatedShiftWithStopper (shift: struct (int*int)) (start: coordinates<int>) (stopAt: squareBitMap -> bool) (board: board) : coordinates<int> list =
            let stopperFunction coords = 
                GetSquare.fromCoordinatesOption board coords
                |> Option.map stopAt
                |> Option.defaultValue true // If None, then coords are out of board, so stop
            Coordinates.afterRepeatedShift stopperFunction shift start
            |> onBoard board
        let getAfterShiftInAllDirections (shift: struct (int*int)) (start: coordinates<int>) (board: board) : coordinates<int> list =
            Coordinates.getAfterShiftInAllDirections start shift
            |> Seq.toList
            |> onBoard board
        
    module GetSquares =

        let fromCoordinates (board: board) (coordinatesList : coordinates list) : squareBitMap list =
            coordinatesList
            |> List.map (GetSquare.fromCoordinatesResult board)
            |> List.filterResults

        let afterShifts (start: coordinates) (board: board) (shifts: (struct (int*int)) list) : squareBitMap list =
            shifts
            |> Coordinates.getAfterShifts start
            |> Seq.toList
            |> fromCoordinates board

        let rec afterRepeatedShift (shift: struct (int*int)) (start: coordinates) (board: board) : squareBitMap list =
            let isNotOnBoard coords = not <| isOnBoard coords board
            Coordinates.afterRepeatedShift isNotOnBoard shift start
            |> fromCoordinates board

        let rec afterRepeatedShiftWithStopper (shift: struct (int*int)) (start: coordinates) (stopAt: squareBitMap -> bool) (board: board) : squareBitMap list =
            GetCoordinates.afterRepeatedShiftWithStopper shift start stopAt board
            |> fromCoordinates board

        let getAfterShiftInAllDirections (shift: struct (int*int)) (start: coordinates) (board: board) : squareBitMap list =
            Coordinates.getAfterShiftInAllDirections shift start
            |> Seq.toList
            |> fromCoordinates board

        let adjacent (start: coordinates) (board: board) : squareBitMap list =
            Coordinates.getAdjacentCoordinates start
            |> Seq.toList
            |> fun list -> fromCoordinates board list


    let updateSquare (coords: coordinates) (square: squareBitMap) (board: board) : board =
        square
        |> List.mapi (fun i boolVal ->
            BitMap.updateValueAtCoordinates boolVal coords board[i]
        )

    /// Folds the array, starting in the top left and moving right.
    let foldij (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7]
        |> List.fold (fun accRow i ->
            [0..7]
            |> List.fold (fun acc j ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the bottom left and moving right.
    let foldijback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow i ->
            [0..7]
            |> List.fold (fun acc j ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state

    /// Folds the array, starting in the top right and moving left.
    let foldibackj (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7]
        |> List.fold (fun accRow i ->
            [0..7] |> List.rev
            |> List.fold (fun acc j ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state

    /// Folds the array, starting in the bottom right and moving left.
    let foldibackjback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow i ->
            [0..7] |> List.rev
            |> List.fold (fun acc j ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state

    /// Folds the array, starting in the top left and moving down.
    let foldji (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7]
        |> List.fold (fun accRow j ->
            [0..7]
            |> List.fold (fun acc i ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the top right and moving down.
    let foldjiback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow j ->
            [0..7]
            |> List.fold (fun acc i ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the bottom left and moving up.
    let foldjbacki (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7]
        |> List.fold (fun accRow j ->
            [0..7] |> List.rev
            |> List.fold (fun acc i ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the bottom right and moving up.
    let foldjbackiback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow j ->
            [0..7] |> List.rev
            |> List.fold (fun acc i ->
                GetSquare.fromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Iterates through the array, starting in the top left and moving right.
    let iterij (func: squareBitMap -> unit) (board: board) : unit =
        foldij (fun _ _ square -> func square) () board

    /// Try to find a value in an array using a filtering function.
    let tryFind (predicate: squareBitMap -> bool) (board: board) : squareBitMap option =
        board
        |> foldij (fun _ oValue squareBitMap ->
            if Option.isSome oValue then
                oValue
            elif predicate squareBitMap then
                Some squareBitMap
            else
                None    
        ) None
        
    /// Try to find a value in an array using a filtering function.
    let tryFindCoordinates (predicate: squareBitMap -> bool) (board: board) : coordinates option =
        board
        |> foldij (fun coords oCoords squareBitMap ->
            if Option.isSome oCoords then
                oCoords
            elif predicate squareBitMap then
                Some coords
            else
                None    
        ) None

    /// Returns a new collection containing only the elements of the collection
    /// for which the given predicate returns "true"
    let filter (predicate: squareBitMap -> bool) (board: board) : squareBitMap list =
        board
        |> foldij (fun _ accList squareBitMap ->
            if predicate squareBitMap then
                squareBitMap :: accList
            else
                accList    
        ) []
        
    /// Returns a new collection containing only the coordinates of the elements of the collection
    /// for which the given predicate returns "true"
    let filterCoordinates (predicate: squareBitMap -> bool) (board: board) : coordinates list =
        board
        |> foldij (fun coords accList squareBitMap ->
            if predicate squareBitMap then
                coords :: accList
            else
                accList    
        ) []