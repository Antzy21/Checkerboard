namespace Checkerboard

type coordinates = int * int

module Coordinates =
    let getName ((i,j) : coordinates) : string =
        sprintf "%c%d" ['a'..'z'].[i] (j+1)
    let fromName (name: string) : coordinates =
        let j, c = System.Char.GetNumericValue name[1] |> int, name[0]
        let i = List.findIndex (fun letter -> letter = c) ['a'..'z']
        (i, j-1)
    let tryParse (name: string) : coordinates option =
        try
            fromName name 
            |> Some
        with
        _ -> None
    let afterShift ((i, j): int * int) ((x,y): coordinates) : coordinates =
        (x+i, y+j)