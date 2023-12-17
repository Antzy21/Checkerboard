namespace Coordinates

open Checkerboard
open Xunit
open FSharp.Extensions

module Parse =

    [<Fact>]
    let ``"a1" => (0,0)`` () =
        let result = 
            Coordinates.parse "a1"
            |> Result.map Coordinates.getName
        Assert.Equal(Ok "a1", result)
    
    [<Fact>]
    let ``"h8" => (7, 7)`` () =
        let result = 
            Coordinates.parse "h8"
            |> Result.map Coordinates.getName
        Assert.Equal(Ok "h8", result)
    
module GetFile =
    
    [<Fact>]
    let ``(0,0) = 0`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getFile
        Assert.Equal(0, result)

    [<Fact>]
    let ``(7,0) = 7`` () =
        let result = 
            Coordinates.construct 7 0 |> Result.failOnError
            |> Coordinates.getFile
        Assert.Equal(7, result)

    [<Fact>]
    let ``(2,4) = 4`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getFile
        Assert.Equal(2, result)
   
module GetRow =
    
    [<Fact>]
    let ``(0,0) = 0`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getRow
        Assert.Equal(0, result)

    [<Fact>]
    let ``(0,7) = 7`` () =
        let result = 
            Coordinates.construct 0 7 |> Result.failOnError
            |> Coordinates.getRow
        Assert.Equal(7, result)

    [<Fact>]
    let ``(2,4) = 4`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getRow
        Assert.Equal(4, result)   
 
module GetRowNumber =
    
    [<Fact>]
    let ``(0,0) = "1"`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getRowNumber
        Assert.Equal("1", result)

    [<Fact>]
    let ``(0,7) = "8"`` () =
        let result = 
            Coordinates.construct 0 7 |> Result.failOnError
            |> Coordinates.getRowNumber
        Assert.Equal("8", result)

    [<Fact>]
    let ``(2,4) = "5"`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getRowNumber
        Assert.Equal("5", result)   
    

module GetFileLetter =
    
    [<Fact>]
    let ``(0,0) = "a"`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getFileLetter
        Assert.Equal("a", result)

    [<Fact>]
    let ``(7,0) = "h"`` () =
        let result = 
            Coordinates.construct 7 0 |> Result.failOnError
            |> Coordinates.getFileLetter
        Assert.Equal("h", result)

    [<Fact>]
    let ``(2,4) = "c"`` () =
        let result = 
            Coordinates.construct 2 4 |> Result.failOnError
            |> Coordinates.getFileLetter
        Assert.Equal("c", result)
    
module Construct =
    
    [<Fact>]
    let ``Construct 0 0 = 1UL`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
        Assert.Equal({value = 1UL}, result)
    
    [<Fact>]
    let ``Construct 1 0 = 2UL`` () =
        let result = 
            Coordinates.construct 1 0 |> Result.failOnError
        Assert.Equal({value = 2UL}, result)

module Shift =
    
    [<Fact>]
    let ``Shift (0,0) 0 0 = (0,0)`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> fun c -> Coordinates.shift c 0 0 |> Result.failOnError
        Assert.Equal({value = 1UL}, result)

    [<Fact>]
    let ``Shift (0,0) 1 0 = (1,0)`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> fun c -> Coordinates.shift c 1 0 |> Result.failOnError
        Assert.Equal({value = 2UL}, result)

module GetName =

    [<Fact>]
    let ``(0,0) => "a1"`` () =
        let result = 
            Coordinates.construct 0 0 |> Result.failOnError
            |> Coordinates.getName
        Assert.Equal("a1", result)
    