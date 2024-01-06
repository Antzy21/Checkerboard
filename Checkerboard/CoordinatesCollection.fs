namespace Checkerboard

open System
open FSharp.Extensions

type coordinatesCollection = UInt64
    
module CoordinatesCollection =
    
    /// Construct an empty coordinatesCollection
    let construct () = 0UL

    let toList (cc: coordinatesCollection) : coordinates List =
        cc
        |> BitMap.isolateValues

    let append (c: coordinates) (cc: coordinatesCollection) : coordinatesCollection =
        cc ||| c
        
    /// Appends a coordinates value if Ok, ignores if Error.
    let appendResult (cResult: coordinates result) (cc: coordinatesCollection) : coordinatesCollection =
        match cResult with
        | Ok c -> append c cc
        | Error _ -> cc

    let remove (c: coordinates) (cc: coordinatesCollection) : coordinatesCollection =
        cc &&& ~~~c

    let filter (positiveMatches: coordinatesCollection) (cc: coordinatesCollection) : coordinatesCollection =
        positiveMatches &&& cc
