namespace Coordinates

open Checkerboard
open Xunit

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
    let ``(0,0) = 'a'`` () =
        let result = 
            Coordinates.construct 0 0
            |> Coordinates.getFile
        Assert.Equal("a", result)

    [<Fact>]
    let ``(7,0) = 'h'`` () =
        let result = 
            Coordinates.construct 7 0
            |> Coordinates.getFile
        Assert.Equal("h", result)

    [<Fact>]
    let ``(2,4) = 'c'`` () =
        let result = 
            Coordinates.construct 2 4
            |> Coordinates.getFile
        Assert.Equal("c", result)
    
module GetRow =
    
    [<Fact>]
    let ``(0,0) = "1"`` () =
        let result = 
            Coordinates.construct 0 0
            |> Coordinates.getRow
        Assert.Equal("1", result)

    [<Fact>]
    let ``(0,7) = "8"`` () =
        let result = 
            Coordinates.construct 0 7
            |> Coordinates.getRow
        Assert.Equal("8", result)

    [<Fact>]
    let ``(2,4) = "5"`` () =
        let result = 
            Coordinates.construct 2 4
            |> Coordinates.getRow
        Assert.Equal("5", result)   
    
module GetName =

    [<Fact>]
    let ``(0,0) => "a1"`` () =
        let result = 
            Coordinates.construct 0 0
            |> Coordinates.getName
        Assert.Equal("a1", result)
    