namespace Checkerboard

open System
open System.Numerics
open FSharp.Extensions

type bitMap = uint64        

module BitMap =

    /// Is a given read value inside a bitMap
    let private isReadableValue (n: int) : bool =
        n >= 0 && n < 64

    let rec private getBits (depth: int) (n: uint64) : bool array =
        match depth with
        | 0 -> [||]
        | d ->
            let boolVal = UInt64.IsOddInteger(n)
            let ary = getBits (d-1) <| UInt64.RotateRight(n, 1)
            Array.append ary [|boolVal|]

    let private readAt (bitMap: uint64) (n: int) : bool result =
        if not <| isReadableValue n then
            Error $"The read value {n} is not in the bitMap."
        else
            (bitMap, n)
            |> UInt64.RotateRight
            |> UInt64.IsOddInteger
            |> Ok

    /// Assumes read position is contained in the bitMap, keep private to ensure this is not incorrectly used.
    let private updateValueAtReadPosition (value: bool) (readPosition: int) (bitMap: uint64) : UInt64 =
        (bitMap, readPosition)
        |> UInt64.RotateRight
        |> (fun n ->
            let isOdd = UInt64.IsOddInteger(n)
            match value, isOdd with
            | true, true
            | false, false -> n
            | true, false -> n+1UL
            | false, true -> n-1UL
        )
        |> fun i -> UInt64.RotateLeft(i, readPosition)

    /// Get
    let getValueAtCoordinates (coords: coordinates) (bitMap: uint64) : bool =
        coords.value &&& bitMap > 0UL

    // Set
    let setValueAtCoordinates (value: bool) (coords: coordinates) (bitMap: uint64) : UInt64 =
        if value then
            coords.value ||| bitMap
        else
            (~~~ coords.value) ||| bitMap

    // Switch
    let switchValueAtCoordinates (coords: coordinates) (bitMap: uint64) : UInt64 =
        coords.value ^^^ bitMap
        
    /// Isolate the coordinates of the positive values in bitMap form
    let IsolateValues (bitMap: bitMap) : bitMap list =
        [0..63]
        |> List.fold (fun accRow i ->
            (uint64)(2.**i) &&& bitMap
            |> fun value ->
                if value > 0UL then
                    value :: accRow
                else
                    accRow
        ) []

    let toString (n: uint64) : string =
        getBits 64 n
        |> Array.map (fun b ->
            match b with
            | true -> "1"
            | false -> "0"
        )
        |> String.Concat

    let fromString (initial: uint64) (str : string) : UInt64 result =
        if str.Length <> 64 then
            Error $"Number of string bits {str.Length} not equal to bit count of Int Type {initial.GetType().ToString()}"
        else
            Seq.fold (fun acc (c: char) ->
                UInt64.RotateLeft(acc, 1)
                |> fun v ->
                    match c with
                    | '1' ->
                        v + 1UL
                    | '0' -> 
                        v
                    | _ -> failwith $"Invalid character '{c}' in binary string '{str}'"
            ) initial str
            |> Ok

    let print (map: bitMap) : unit =
        map |> toString |> String.toBlock |> printfn "%s"
