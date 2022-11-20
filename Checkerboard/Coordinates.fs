namespace Checkerboard

open System.Numerics

type coordinates<'Size when 'Size :> INumber<'Size>> = 'Size * 'Size

module Coordinates =

    let internal createTruncating ((i, j) : coordinates<'Size1>) : coordinates<'Size2> =
        'Size2.CreateTruncating i, 'Size2.CreateTruncating j
    let rec private numberToAlphabet (n: 'Size when 'Size :> INumber<'Size>) : string =
        let alphabetLength : 'Size = 'Size.CreateTruncating(26)
        let x : 'Size = n % alphabetLength
        if n % alphabetLength = n then ""
        else numberToAlphabet (n/(alphabetLength-'Size.One))
        + string (['a'..'z'].[System.Int32.CreateChecked(x)])

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
                26 * s + List.findIndex ((=) c) ['a'..'z']+1
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
    let afterShift<'Size when 'Size :> INumber<'Size>> ((i, j): 'Size * 'Size) ((x, y): coordinates<'Size>) : coordinates<'Size> =
        (x+i, y+j)
    let getShiftBetween ((x1,y1): coordinates<'Size>) ((x2,y2): coordinates<'Size>) : coordinates<'Size> =
        (x2-x1,y2-y1)
        