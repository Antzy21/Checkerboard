namespace Checkerboard

open System.Numerics
open FSharp.Extensions

type coordinates = coordinates<int>

module Coordinates =

    let private alphabet = ['a'..'z']

    let internal createTruncating ((i, j) : coordinates<'Size1>) : coordinates<'Size2> =
        'Size2.CreateTruncating i, 'Size2.CreateTruncating j
    let rec private numberToAlphabet (n: 'Size when 'Size :> INumber<'Size>) : string =
        let alphabetLength : 'Size = 'Size.CreateTruncating(26)
        let x : 'Size = n % alphabetLength
        if n % alphabetLength = n then ""
        else numberToAlphabet (n/(alphabetLength-'Size.One))
        + string (alphabet.[System.Int32.CreateChecked(x)])

    let getFile ((i, _) : coordinates<'Size>) : string =
        numberToAlphabet i

    let getRow ((_, j) : coordinates<'Size>) : string =
        (j+'Size.One).ToString()

    let getName ((i, j) : coordinates<'Size>) : string =
        $"{(numberToAlphabet i)}{(j+'Size.One)}"

    let parse (name: string) : coordinates<'Size> =
        let alphabetLength : 'Size = 'Size.CreateTruncating(26)
        let startOfNum = Seq.findIndex (fun c -> System.Char.IsDigit(c)) name
        let chars, num = name.[..startOfNum-1], name.[startOfNum..]
        let i : 'Size = 
            Seq.fold (fun s c ->
                26 * s + List.findIndex ((=) c) alphabet+1
            ) 0 chars
            |> 'Size.CreateChecked
        let j = 'Size.Parse(num, null)
        (i-'Size.One, j-'Size.One)

    let tryParse (name: string) : coordinates<'Size> option =
        try
            parse name 
            |> Some
        with
        _ -> None
        