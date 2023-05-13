namespace Coordinates

open Checkerboard
open Xunit

module Parse =

    [<Fact>]
    let ``"a1" => (0,0)`` () =
        let result = Coordinates.parse "a1"
        Assert.Equal(Ok struct (0, 0), result)
    
    [<Fact>]
    let ``"az1" => (51, 0)`` () =
        let result = Coordinates.parse "az1"
        Assert.Equal(Ok struct (51,0), result)

    [<Fact>]
    let ``"z26" => (25, 25)`` () =
        let result = Coordinates.parse "z26"
        Assert.Equal(Ok struct (25, 25), result)

    [<Fact>]
    let ``"aa676" => (26,675)`` () =
        let result = Coordinates.parse "aa676"
        Assert.Equal(Ok struct (26, 675), result)

    [<Fact>]
    let ``"za1" => (676,0)`` () =
        let result = Coordinates.parse "za1"
        Assert.Equal(Ok struct (676, 0), result)
    
module GetFile =
    
    [<Fact>]
    let ``(0,0) = 'a'`` () =
        let result = Coordinates.getFile (0,0)
        Assert.Equal("a", result)

    [<Fact>]
    let ``(7,0) = 'h'`` () =
        let result = Coordinates.getFile (7,0)
        Assert.Equal("h", result)

    [<Fact>]
    let ``(2,4) = 'c'`` () =
        let result = Coordinates.getFile (2,4)
        Assert.Equal("c", result)
    
module GetRow =
    
    [<Fact>]
    let ``(0,0) = "1"`` () =
        let result = Coordinates.getRow (0,0)
        Assert.Equal("1", result)

    [<Fact>]
    let ``(0,7) = "8"`` () =
        let result = Coordinates.getRow (0,7)
        Assert.Equal("8", result)

    [<Fact>]
    let ``(2,4) = "5"`` () =
        let result = Coordinates.getRow (2,4)
        Assert.Equal("5", result)   
    
module GetName =

    [<Fact>]
    let ``(0,0) => "a1"`` () =
        let result = Coordinates.getName (0, 0)
        Assert.Equal("a1", result)
    
    [<Fact>]
    let ``(51, 0) => "az1"`` () =
        let result = Coordinates.getName (51,0)
        Assert.Equal("az1", result)

    [<Fact>]
    let ``(25, 25) => "z26"`` () =
        let result = Coordinates.getName (25, 25)
        Assert.Equal("z26", result)

    [<Fact>]
    let ``(26,675) => "aa676"`` () =
        let result = Coordinates.getName (26,675)
        Assert.Equal("aa676", result)

    [<Fact>]
    let ``(676, 0) => "za1"`` () =
        let result = Coordinates.getName (676, 0)
        Assert.Equal("za1", result)
    