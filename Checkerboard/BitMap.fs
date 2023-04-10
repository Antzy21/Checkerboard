namespace Checkerboard

open System
open System.Numerics
open FSharp.Extensions

type bitMap = uint64

module BitMap =

    let mapSize = 8
    
    /// Are some given coordinates are inside a bitMap
    let containsCoordinates ((i,j): coordinates) : bool =
        i >= 0 && i <= mapSize && j >= 0 && j <= mapSize

    /// Is a given read value inside a bitMap
    let isReadableValue (n: int) : bool =
        n >= 0 && n < mapSize*mapSize
         
    let getDimensionFromIntegerType (n: 'N when 'N :> IBinaryInteger<'N>) : int =
        n.GetByteCount() * 8 |> Math.Sqrt |> int

    let getReadPositionFromCoordinatesResult ((i, j): coordinates) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : int result =
        if not <| containsCoordinates (i,j) then
            Error $"The coordinates ({i}, {j}) are not on the bitMap."
        else
            let d = getDimensionFromIntegerType bitMap
            Ok (i*d + j)
    let getReadPositionFromCoordinates (coords: coordinates) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : int =
        getReadPositionFromCoordinatesResult coords bitMap |> Result.failOnError

    let getCoordinatesFromReadPositionResult (n: int) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : coordinates result =
        if not <| isReadableValue n then
            Error $"The read value {n} is not on the bitMap."
        else
            let d = getDimensionFromIntegerType bitMap
            let i, j = n % mapSize, n / mapSize
            Ok (i, j)
    let getCoordinatesFromReadPosition (n: int) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : coordinates =
        getCoordinatesFromReadPositionResult n bitMap |> Result.failOnError

    let rec getBits (depth: int) (n: 'N when 'N :> IBinaryInteger<'N>) : bool array =
        match depth with
        | 0 -> [||]
        | d ->
            let boolVal = 'N.IsOddInteger(n)
            let ary = getBits (d-1) <| 'N.RotateRight(n, 1)
            Array.append ary [|boolVal|]

    let toString (n: 'N when 'N :> IBinaryInteger<'N>) : string =
        let bitCount = n.GetByteCount() * mapSize
        getBits bitCount n
        |> Array.map (fun b ->
            match b with
            | true -> "1"
            | false -> "0"
        )
        |> String.Concat

    let fromString (initial: 'N when 'N :> IBinaryInteger<'N>) (str : string) : 'N result =
        if initial.GetByteCount() * mapSize <> str.Length then
            Error $"Number of string bits {str.Length} not equal to bit count of Int Type {initial.GetType().ToString()}"
        else
            Seq.fold (fun acc (c: char) ->
                'N.RotateLeft(acc, 1)
                |> fun v ->
                    match c with
                    | '1' ->
                        v + 'N.One
                    | '0' -> 
                        v
                    | _ -> failwith $"Invalid character '{c}' in binary string '{str}'"
            ) initial str
            |> Ok

    let toStringBlock (n: 'N when 'N :> IBinaryInteger<'N>) = n |> toString |> String.toBlock

    let print (map: bitMap) : unit =
        map |> toString |> String.toBlock |> printfn "%s"

    let readAt (bitMap: 'N when 'N :> IBinaryInteger<'N>) (n: int) : bool result =
        if not <| isReadableValue n then
            Error $"The read value {n} is not in the bitMap."
        else
            (bitMap, n)
            |> 'N.RotateRight
            |> 'N.IsOddInteger
            |> Ok

    let getValueAtCoordinatesResult (coords: coordinates) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : bool result =
        getReadPositionFromCoordinatesResult coords bitMap
        |> Result.bind (readAt bitMap) 
    let getValueAtCoordinates (coords: coordinates) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : bool =
        getValueAtCoordinatesResult coords bitMap |> Result.failOnError

    let getValuesAtCoordinatesList (bitMap: 'N when 'N :> IBinaryInteger<'N>) (coordsList: coordinates list) : bool list =
        coordsList
        |> List.map (fun coords ->
            getValueAtCoordinatesResult coords bitMap
        )
        |> List.filterResults

    /// Assumes read position is contained in the bitMap, keep private to ensure this is not incorrectly used.
    let private updateValueAtReadPosition (value: bool) (readPosition: int) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : 'N =
        (bitMap, readPosition)
        |> 'N.RotateRight
        |> (fun n ->
            let isOdd = 'N.IsOddInteger(n)
            match value, isOdd with
            | true, true
            | false, false -> n
            | true, false -> n+'N.One
            | false, true -> n-'N.One
        )
        |> fun i -> 'N.RotateLeft(i, readPosition)

    let updateValueAtCoordinatesResult (value: bool) (coords: coordinates) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : 'N result =
        getReadPositionFromCoordinatesResult coords bitMap
        |> Result.map (fun readPos ->
            updateValueAtReadPosition value readPos bitMap
        )

    let updateValueAtCoordinates (value: bool) (coords: coordinates) (bitMap: 'N when 'N :> IBinaryInteger<'N>) =
        bitMap
        |> updateValueAtCoordinatesResult value coords
        |> Result.failOnError

    let switchValueAtCoordinates (coords: coordinates) (bitMap: 'N when 'N :> IBinaryInteger<'N>) : 'N result =
        getValueAtCoordinatesResult coords bitMap
        |> Result.map (fun value ->
            updateValueAtCoordinates (not value) coords bitMap
        )
