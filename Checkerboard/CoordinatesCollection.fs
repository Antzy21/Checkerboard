namespace Checkerboard

open System

type coordinatesCollection = UInt64
    
module CoordinatesCollection =
    
    let construct () = 0UL

    let toList (cc: coordinatesCollection) : coordinates List =
        cc
        |> BitMap.IsolateValues
        |> List.map Coordinates.constructFromValue

    let append (c: coordinates) (cc: coordinatesCollection) : coordinatesCollection =
        cc ||| c.value

    let remove (c: coordinates) (cc: coordinatesCollection) : coordinatesCollection =
        cc &&& ~~~c.value

    let filter (positiveMatches: coordinatesCollection) (cc: coordinatesCollection) : coordinatesCollection =
        positiveMatches &&& cc
