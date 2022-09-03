module Tests.Coordinates

open System
open Checkerboard
open Xunit

[<Fact>]
let ``Parse "a1"`` () =
    let result = Coordinates.fromName "a1"
    Assert.Equal((0, 0), result)
    
[<Fact>]
let ``Parse "az1"`` () =
    let result = Coordinates.fromName "az1"
    Assert.Equal((51, 0), result)

[<Fact>]
let ``Parse "z26"`` () =
    let result = Coordinates.fromName "z26"
    Assert.Equal((25, 25), result)

[<Fact>]
let ``Parse "aa676"`` () =
    let result = Coordinates.fromName "aa676"
    Assert.Equal((26, 675), result)

[<Fact>]
let ``Parse "za1"`` () =
    let result = Coordinates.fromName "za1"
    Assert.Equal((676, 0), result)
