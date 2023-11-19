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

    // Only call if 100% sure that the coordinates are on the board
    let getSquareFromCoordinates (board: board) ((i,j): coordinates) : squareBitMap =
        board
        |> List.map (fun map ->
            BitMap.getValueAtCoordinates (i,j) map
        )
        
    // Call this if it is not gaurenteed that the coordinates are on the board
    let getSquareFromCoordinatesResult (board: board) ((i,j): coordinates) : squareBitMap result =
        if BitMap.containsCoordinates (i,j) then
            getSquareFromCoordinates board (i,j)
            |> Ok
        else
            Error $"The coordinates ({i}, {j}) are not on the board."

    /// Returns a new collection containing only the coordinates<int> that are on the board
    let filterForCoordinatesOnBoard (board: board) (coordinatesList: coordinates array) : coordinates array =
        coordinatesList
        |> Array.filter (fun coords ->
            BitMap.containsCoordinates coords
        )

    let getCoordinatesAfterShifts (start: coordinates<int>) (board: board) (shifts: (struct (int*int)) seq) : coordinates<int> array =
        shifts
        |> Coordinates.getAfterShifts start
        |> Seq.toArray
        |> filterForCoordinatesOnBoard board

    let rec getCoordinatesAfterRepeatedShift (shift: struct (int*int)) (start: coordinates<int>) (board: board) : coordinates<int> array =
        let isNotOnBoard = BitMap.containsCoordinates >> not
        Coordinates.afterRepeatedShift isNotOnBoard shift start
        |> Array.ofList
        |> filterForCoordinatesOnBoard board

    let rec getCoordinatesAfterRepeatedShiftWithStopper (shift: struct (int*int)) (start: coordinates<int>) (stopAt: squareBitMap -> bool) (board: board) : coordinates<int> array =
        let stopperFunction coords = 
            getSquareFromCoordinatesResult board coords
            |> Result.map stopAt
            |> Result.defaultValue true // If None, then coords are out of board, so stop
        Coordinates.afterRepeatedShift stopperFunction shift start
        |> Array.ofList
        |> filterForCoordinatesOnBoard board

    let getCoordinatesAfterShiftInAllDirections (shift: struct (int*int)) (start: coordinates<int>) (board: board) : coordinates<int> array =
        Coordinates.getAfterShiftInAllDirections start shift
        |> Seq.toArray
        |> filterForCoordinatesOnBoard board
        
    /// Creates a copy of the board with the new square data at given coordinates
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
                getSquareFromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the bottom left and moving right.
    let foldijback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow i ->
            [0..7]
            |> List.fold (fun acc j ->
                getSquareFromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state

    /// Folds the array, starting in the top right and moving left.
    let foldibackj (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7]
        |> List.fold (fun accRow i ->
            [0..7] |> List.rev
            |> List.fold (fun acc j ->
                getSquareFromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state

    /// Folds the array, starting in the bottom right and moving left.
    let foldibackjback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow i ->
            [0..7] |> List.rev
            |> List.fold (fun acc j ->
                getSquareFromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state

    /// Folds the array, starting in the top left and moving down.
    let foldji (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7]
        |> List.fold (fun accRow j ->
            [0..7]
            |> List.fold (fun acc i ->
                getSquareFromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the top right and moving down.
    let foldjiback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow j ->
            [0..7]
            |> List.fold (fun acc i ->
                getSquareFromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the bottom left and moving up.
    let foldjbacki (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7]
        |> List.fold (fun accRow j ->
            [0..7] |> List.rev
            |> List.fold (fun acc i ->
                getSquareFromCoordinates board (i, j)
                |> folder (i, j) acc
            ) accRow
        ) state
    
    /// Folds the array, starting in the bottom right and moving up.
    let foldjbackiback (folder: coordinates -> 'S -> squareBitMap -> 'S) (state: 'S) (board: board)=
        [0..7] |> List.rev
        |> List.fold (fun accRow j ->
            [0..7] |> List.rev
            |> List.fold (fun acc i ->
                getSquareFromCoordinates board (i, j)
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