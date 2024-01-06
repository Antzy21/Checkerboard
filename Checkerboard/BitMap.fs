namespace Checkerboard

open System
open FSharp.Extensions

type bitMap = UInt64        

module BitMap =

    let rec private getBits (depth: int) (bitMap: bitMap) : bool array =
        match depth with
        | 0 -> [||]
        | d ->
            let boolVal = UInt64.IsOddInteger(bitMap)
            let ary = getBits (d-1) <| UInt64.RotateRight(bitMap, 1)
            Array.append ary [|boolVal|]

    /// Get
    let getValueAtCoordinates (c: coordinates) (bitMap: bitMap) : bool =
        c &&& bitMap > 0UL

    // Set
    let setValueAtCoordinates (value: bool) (c: coordinates) (bitMap: bitMap) : bitMap =
        if value then
            c ||| bitMap
        else
            (~~~ c) &&& bitMap

    // Switch
    let switchValueAtCoordinates (c: coordinates) (bitMap: bitMap) : bitMap =
        c ^^^ bitMap
        
    /// Isolate the coordinates of the positive values in bitMap form
    let isolateValues (bitMap: bitMap) : bitMap list =
        [0..63]
        |> List.fold (fun accRow i ->
            let value = uint64 (2.**i)
            if getValueAtCoordinates value bitMap then
                value :: accRow
            else
                accRow
        ) []

    let toString (bitMap: bitMap) : string =
        getBits 64 bitMap
        |> Array.map (fun b ->
            match b with
            | true -> "1"
            | false -> "0"
        )
        |> String.Concat

    let parse (str : string) : bitMap result =
        if str.Length <> 64 then
            Error $"Number of string bits {str.Length} not equal to bit count of UInt64"
        else
            Seq.fold (fun accResult (c: char) ->
                accResult |> Result.bind (fun acc ->
                    UInt64.RotateLeft(acc, 1)
                    |> fun v ->
                        match c with
                        | '1' ->
                            Ok (v + 1UL)
                        | '0' -> 
                            Ok v
                        | _ -> Error $"Invalid character '{c}' in binary string '{str}'"
                )
            ) (Ok 0UL) str
