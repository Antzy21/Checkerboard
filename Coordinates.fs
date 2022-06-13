namespace Checkerboard

type coordinates = int * int

module Coordinates =
    let getName ((i,j) : coordinates) : string =
        sprintf "%c%d" ['a'..'z'].[i] (j+1)
    let fromName (name: string) : coordinates =
        let j, c = System.Char.GetNumericValue name[1] |> int, name[0]
        let i = List.findIndex (fun letter -> letter = c) ['a'..'z']
        (i, j)