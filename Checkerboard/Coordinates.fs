namespace Checkerboard

open System
open FSharp.Extensions

type coordinates = UInt64

module Coordinates =

    let private alphabet = ['a'..'h']

    let internal constructFromReadValue (r: int) : coordinates = 
        UInt64.RotateLeft(1UL, r)

    let construct (i: int) (j: int) : coordinates result =
        if not (i >= 0 && i < 8 && j >= 0 && j < 8) then
            Error "Invalid coordinates"
        else
            Ok (constructFromReadValue ((j * 8) + i))

    let private getLetterIndex (c: char) : int result =
        List.tryFindIndex ((=) c) alphabet
        |> Result.fromOption "Char not found in alphabet"

    let getReadValue (c: coordinates) : int = 
        c
        |> Numerics.BigInteger.Log2
        |> int

    let getFile (c : coordinates) : int =
        getReadValue c
        |> fun r -> r % 8

    let getRow (c : coordinates) : int =
        getReadValue c
        |> fun r -> r / 8

    let getRowNumber = getRow >> (+) 1 >> string

    let getFileLetter = getFile >> (fun file -> alphabet[file]) >> string

    let getName (c : coordinates) : string =
        $"{getFileLetter c}{getRowNumber c}"

    // Shift coordinates by i and j
    let shift (c: coordinates) (i: int) (j: int) : coordinates result =
        construct (i + getFile c) (j + getRow c)

    let parse (name: string) : coordinates result =
        let rowFileSplitIndexResult =
            let rowFileSplitIndexOption =
                Seq.tryFindIndex Char.IsDigit name
            match rowFileSplitIndexOption with
            | Some rowFileSplitIndex ->
                Ok rowFileSplitIndex
            | None ->
                Error "No digit found in parsed coordinates name"

        rowFileSplitIndexResult
        |> Result.bind (fun rowFileSplitIndex ->
            let chars = name[..rowFileSplitIndex-1]
            let num = name[rowFileSplitIndex..]
            let j = Int32.Parse num
            let i = 
                Seq.foldResult (fun acc c ->
                    getLetterIndex c
                    |> Result.map (fun letterIndex ->
                        letterIndex + acc * alphabet.Length + 1
                    )
                ) 0 chars
            i
            |> Result.bind (fun i -> construct (i-1) (j-1))
        )