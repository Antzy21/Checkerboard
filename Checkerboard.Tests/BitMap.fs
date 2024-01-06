namespace BitMap

open Checkerboard
open Helpers.Data
open Xunit
open FsCheck.Xunit
open System
open FSharp.Extensions

module ToString =

    [<Fact>]
    let ``Given 0UL output is a string of zeros`` () =
        let result = BitMap.toString 0UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", result)

    [<Fact>]
    let ``Given 1UL output string has last character one`` () =
        let result = BitMap.toString 1UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000001", result)

    [<Fact>]
    let ``Given 2UL output string has second last character one`` () =
        let result = BitMap.toString 2UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000010", result)

    [<Fact>]
    let ``Given 3UL output string has last two characters one`` () =
        let result = BitMap.toString 3UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000011", result)

    [<Fact>]
    let ``Given 4UL output string has third last character one`` () =
        let result = BitMap.toString 4UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000100", result)

module FromString =

    [<Fact>]
    let ``String of zeros maps to 0UL`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000000"
            |> Result.failOnError
        Assert.Equal(0UL, result)

    [<Fact>]
    let ``String with last character one maps to 1UL`` () =
        let result =
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000001"
            |> Result.failOnError
        Assert.Equal(1UL, result)

    [<Fact>]
    let ``String with second last character one maps to 2UL`` () =
        let result =
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000010"
            |> Result.failOnError
        Assert.Equal(2UL, result)

    [<Fact>]
    let ``String with last two characters one maps to 3UL`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000011"
            |> Result.failOnError
        Assert.Equal(3UL, result)

    [<Fact>]
    let ``String with third last character one maps to 4UL`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000100"
            |> Result.failOnError
        Assert.Equal(4UL, result)

    [<Fact>]
    let ``Too short input string returns error`` () =
        let result = BitMap.fromString 0UL ""
        Assert.True(Result.isError result)

    [<Fact>]
    let ``Input string with bad characters returns error`` () =
        let result = BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000002"
        Assert.True(Result.isError result)

module Inverses =

    [<Property>]
    let ``BitMap toString and fromString are inverses`` (n: bitMap) =
        let result = n |> BitMap.toString |> BitMap.fromString 0UL
        Assert.Equal(Ok n, result)

module GetValueAtCoordinates = 

    [<Fact>]
    let ``Coordinates (0,0) for 1UL results in true`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.getValueAtCoordinates c 1UL
        Assert.Equal(true, result)

    [<Property>]
    let ``Coordinate accessing with an empty bitmap always results in false`` (i_b: int) (j_b: int) =
        let i = abs(i_b % 8)
        let j = abs(j_b % 8)
        let c = Coordinates.construct i j |> Result.failOnError
        let result = BitMap.getValueAtCoordinates c 0UL
        Assert.Equal(false, result)

    [<Property>]
    let ``Coordinate accessing with a full bitmap always results in true`` (i_b: int) (j_b: int) =
        let i = abs(i_b % 8)
        let j = abs(j_b % 8)
        let c = Coordinates.construct i j |> Result.failOnError
        let result = BitMap.getValueAtCoordinates c UInt64.MaxValue
        Assert.Equal(true, result)

    [<Fact>]
    let ``Starting chess position is correct`` () =
        let result = BitMap.toString BitMapNumbers.startingChessPiecePosition
        Assert.Equal("1111111111111111000000000000000000000000000000001111111111111111", result)

    [<Property>]
    let ``Starting chess position only gives true for specific coordinates`` (randomInt: int) =
        let i = abs(randomInt % 8)
        List.iter (fun j ->
            let c = Coordinates.construct i j |> Result.failOnError
            let result = BitMap.getValueAtCoordinates c BitMapNumbers.startingChessPiecePosition
            Assert.Equal(false, result)
        ) [2; 3; 4; 5]
        List.iter (fun j ->
            let c = Coordinates.construct i j |> Result.failOnError
            let result = BitMap.getValueAtCoordinates c BitMapNumbers.startingChessPiecePosition
            Assert.Equal(true, result)
        ) [0; 1; 6; 7]

    [<Property>]
    let ``Starting white chess position only gives true for specific coordinates`` (randomInt: int) =
        let i = abs(randomInt % 8)
        List.iter (fun j ->
            let c = Coordinates.construct i j |> Result.failOnError
            let result = BitMap.getValueAtCoordinates c BitMapNumbers.startingWhitePiecePositions
            Assert.Equal(true, result)
        ) [0; 1]
        List.iter (fun j ->
            let c = Coordinates.construct i j |> Result.failOnError
            let result = BitMap.getValueAtCoordinates c BitMapNumbers.startingWhitePiecePositions
            Assert.Equal(false, result)
        ) [2; 3; 4; 5; 6; 7]

module SetValueAtCoordinates =

    [<Fact>]
    let ``Update value from 0 to 1 at (0,0)`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.setValueAtCoordinates true c 0UL
        Assert.Equal(1UL, result)
        
    [<Fact>]
    let ``Update value from 1 to 0 at (0,0)`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.setValueAtCoordinates false c 0UL
        Assert.Equal(0UL, result)
        
module SwitchValueAtCoordinates =

    [<Fact>]
    let ``Switch value at (0,0)`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.switchValueAtCoordinates c 0UL
        Assert.Equal(1UL, result)

module IsOnAtCoordinates =
    
    [<Fact>]
    let ``Empty bitmap returns false for IsOnAtCoordinates (0,0)`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.isOnAtCoordinates c 0UL
        Assert.False(result)