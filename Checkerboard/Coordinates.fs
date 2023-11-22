namespace Checkerboard

open System
open FSharp.Extensions

[<Struct>]
type coordinates = 
    {
        value: UInt64
    }

module Coordinates =

    let private alphabet = ['a'..'z']

    let construct (i: int) (j: int) : coordinates =
        if not (i >= 0 && i <= 8 && j >= 0 && j <= 8) then
            failwith "Invalid coordinates"
        {value = UInt64.RotateLeft(1UL, (i * 8) + j)}

    let private getLetterIndex (c: char) : int result =
        List.tryFindIndex ((=) c) alphabet
        |> Result.fromOption "Char not found in alphabet"

    let rec private numberToAlphabet (n: int) : string =
        let remainder = n % alphabet.Length
        if remainder = n then
            ""
        else
            numberToAlphabet ((n/alphabet.Length)-1)
        + string alphabet.[remainder]

    let getReadValue (c: coordinates) : int = 
        c.value
        |> Numerics.BigInteger.Log2
        |> (int)

    let getFile (c : coordinates) : string =
        getReadValue c
        |> fun i -> i % 8
        |> numberToAlphabet

    let getRow (c : coordinates) : string =
        getReadValue c
        |> fun i -> i / 8
        |> fun j -> (j+1).ToString()

    let getName (coords : coordinates) : string =
        getFile coords + getRow coords

    let parse (name: string) : coordinates result =
        let rowFileSplitIndexResult =
            let rowFileSplitIndexOption =
                Seq.tryFindIndex System.Char.IsDigit name
            match rowFileSplitIndexOption with
            | Some rowFileSplitIndex ->
                Ok rowFileSplitIndex
            | None ->
                Error "No digit found in parsed coordinates name"

        rowFileSplitIndexResult
        |> Result.bind (fun rowFileSplitIndex ->
            let chars = name.[..rowFileSplitIndex-1]
            let num = name.[rowFileSplitIndex..]
            let j = Int32.Parse num
            let i = 
                Seq.foldResult (fun acc c ->
                    getLetterIndex c
                    |> Result.map (fun letterIndex ->
                        letterIndex + acc * alphabet.Length + 1
                    )
                ) 0 chars
            i
            |> Result.map (fun i -> construct (i-1) (j-1))
        )

    let tryParse (name: string) : coordinates option =
        parse name
        |> Result.toOption
