namespace Coordinates

open Checkerboard
open Xunit
open FSharp.Extensions
module Parse =

    [<Fact>]
    let ``Parsing "a1" gives (0,0)`` () =
        let result = 
            Coordinates.parse "a1"
            |> Result.map Coordinates.getName
        Assert.Equal(Ok "a1", result)

    [<Fact>]
    let ``Parsing "h8" gives (7,7)`` () =
        let result = 
            Coordinates.parse "h8"
            |> Result.map Coordinates.getName
        Assert.Equal(Ok "h8", result)

    [<Fact>]
    let ``Parsing bad input returns error`` () =
        let result = 
            Coordinates.parse "bad input"
            |> Result.map Coordinates.getName
        Assert.True(Result.isError result)

module GetFile =
    
    [<Fact>]
    let ``(0,0) file is 0`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getFile
        Assert.Equal(0, result)

    [<Fact>]
    let ``(7,0) file is 7`` () =
        let result = 
            Coordinates.construct 7 0 |> Result.failOnError
            |> Coordinates.getFile
        Assert.Equal(7, result)

    [<Fact>]
    let ``(2,4) file is 2`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getFile
        Assert.Equal(2, result)
   
module GetRow =
    
    [<Fact>]
    let ``(0,0) row is 0`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getRow
        Assert.Equal(0, result)

    [<Fact>]
    let ``(0,7) row is 7`` () =
        let result = 
            Coordinates.construct 0 7 |> Result.failOnError
            |> Coordinates.getRow
        Assert.Equal(7, result)

    [<Fact>]
    let ``(2,4) row is 4`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getRow
        Assert.Equal(4, result)

module GetRowNumber =
    
    [<Fact>]
    let ``Row number of (0,0) is "1"`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getRowNumber
        Assert.Equal("1", result)

    [<Fact>]
    let ``Row number of (0,7) is "8"`` () =
        let result = 
            Coordinates.construct 0 7 |> Result.failOnError
            |> Coordinates.getRowNumber
        Assert.Equal("8", result)

    [<Fact>]
    let ``Row number of (2,4) is "5"`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getRowNumber
        Assert.Equal("5", result)

module GetFileLetter =
    
    [<Fact>]
    let ``File letter of (0,0) is "a"`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getFileLetter
        Assert.Equal("a", result)

    [<Fact>]
    let ``File letter of (7,0) is "h"`` () =
        let result = 
            Coordinates.construct 7 0 |> Result.failOnError
            |> Coordinates.getFileLetter
        Assert.Equal("h", result)

    [<Fact>]
    let ``File letter of (2,4) is "c"`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getFileLetter
        Assert.Equal("c", result)
module Construct =

    [<Fact>]
    let ``Construct coordinates at (0,0) results in 1UL`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
        Assert.Equal({value = 1UL}, result)

    [<Fact>]
    let ``Construct coordinates at (1,0) results in 2UL`` () =
        let result = 
            Coordinates.construct 1 0 |> Result.failOnError
        Assert.Equal({value = 2UL}, result)

module Shift =
    
    [<Fact>]
    let ``Shifting (0,0) by (0,0) gives (0,0)`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> fun c -> Coordinates.shift c 0 0 |> Result.failOnError
        Assert.Equal({value = 1UL}, result)

    [<Fact>]
    let ``Shifting (0,1) by (0,0) gives (0,1)`` () =
        let coords = Coordinates.construct 0 1 |> Result.failOnError
        let result = 
            coords
            |> fun c -> Coordinates.shift c 0 0 |> Result.failOnError
        Assert.Equal(coords, result)

    [<Fact>]
    let ``Shifting (0,0) by (1,0) gives (1,0)`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> fun c -> Coordinates.shift c 1 0 |> Result.failOnError
        Assert.Equal({value = 2UL}, result)

module GetName =

    [<Fact>]
    let ``Getting the name of (0,0) gives "a1"`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getName
        Assert.Equal("a1", result)
    