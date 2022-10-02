namespace Checkerboard

type coordinates = int * int

module Coordinates =
    let rec private numberToAlphabet (n: int) : string =
        if n%26 = n then ""
        else numberToAlphabet (n/26-1)
        + string (['a'..'z'].[n%26])
    let getFile ((i,_) : coordinates) : string =
        numberToAlphabet i
    let getRow ((_,j) : coordinates) : string =
        sprintf "%d" (j+1)
    let getName ((i,j) : coordinates) : string =
        sprintf "%s%d" (numberToAlphabet i) (j+1)
    let fromName (name: string) : coordinates =
        let startOfNum = Seq.findIndex (fun c -> System.Char.IsDigit(c)) name
        let chars, num = name.[..startOfNum-1], name.[startOfNum..]
        let i = Seq.fold (fun s c -> 26 * s + List.findIndex ((=) c) ['a'..'z']+1) 0 chars
        let j = num |> int
        (i-1, j-1)
    let tryParse (name: string) : coordinates option =
        try
            fromName name 
            |> Some
        with
        _ -> None
    let afterShift ((i, j): int * int) ((x,y): coordinates) : coordinates =
        (x+i, y+j)
    let getShiftBetween ((x1,y1): coordinates) ((x2,y2): coordinates) : coordinates =
        (x2 - x1, y2 - y1)