namespace Checkerboard

open System
open FSharp.Extensions

type coordinatesCollection = UInt64
    
module CoordinatesCollection =
    
    /// Construct an empty coordinatesCollection
    let construct () = 0UL

    let toList (cc: coordinatesCollection) : coordinates List =
        cc
        |> BitMap.IsolateValues
        |> List.map Coordinates.constructFromValue

    let append (c: coordinates) (cc: coordinatesCollection) : coordinatesCollection =
        cc ||| c.value
        
    /// Appends a coordinates value if Ok, ignores if Error.
    let appendResult (coordsResult: coordinates result) (cc: coordinatesCollection) : coordinatesCollection =
        match coordsResult with
        | Ok coords -> append coords cc
        | Error _ -> cc

    let remove (c: coordinates) (cc: coordinatesCollection) : coordinatesCollection =
        cc &&& ~~~c.value

    let filter (positiveMatches: coordinatesCollection) (cc: coordinatesCollection) : coordinatesCollection =
        positiveMatches &&& cc
