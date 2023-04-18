namespace Checkerboard

open System
open FSharp.Extensions

type coordinates = coordinates<int>

module Coordinates =

    let private alphabet = ['a'..'z']

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

    let getFile ((i, _) : coordinates) : string =
        numberToAlphabet i

    let getRow ((_, j) : coordinates) : string =
        (j+1).ToString()

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
            |> Result.map (fun i -> (i-1, j-1))
        )

    let tryParse (name: string) : coordinates option =
        parse name
        |> Result.toOption
