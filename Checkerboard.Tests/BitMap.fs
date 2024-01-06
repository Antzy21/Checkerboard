namespace BitMap

open Checkerboard
open Helpers.Data
open Xunit
open FsCheck.Xunit
open System
open FSharp.Extensions

module ToString =

    [<Fact>]
    let ``0 => 0...000`` () =
        let result = BitMap.toString 0UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000000", result)

    [<Fact>]
    let ``1 => 0...001`` () =
        let result = BitMap.toString 1UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000001", result)

    [<Fact>]
    let ``2 => 0...010`` () =
        let result = BitMap.toString 2UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000010", result)

    [<Fact>]
    let ``3 => 0...011`` () =
        let result = BitMap.toString 3UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000011", result)

    [<Fact>]
    let ``4 => 0...100`` () =
        let result = BitMap.toString 4UL
        Assert.Equal("0000000000000000000000000000000000000000000000000000000000000100", result)

module FromString =

    [<Fact>]
    let ``0...000 => 0`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000000"
            |> Result.failOnError
        Assert.Equal(0UL, result)

    [<Fact>]
    let ``0...001 => 1`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000001"
            |> Result.failOnError
        Assert.Equal(1UL, result)

    [<Fact>]
    let ``0...010 => 2`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000010"
            |> Result.failOnError
        Assert.Equal(2UL, result)

    [<Fact>]
    let ``0...011 => 3`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000011"
            |> Result.failOnError
        Assert.Equal(3UL, result)

    [<Fact>]
    let ``0...100 => 4`` () =
        let result = 
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000100"
            |> Result.failOnError
        Assert.Equal(4UL, result)
        
    [<Fact>]
    let FromString_WithTooShortString_ReturnsError () =
        let result =
            BitMap.fromString 0UL ""
        Assert.True(Result.isError result)
        
    [<Fact>]
    let FromString_WithStringOfBadChars_ReturnsError () =
        let result =
            BitMap.fromString 0UL "0000000000000000000000000000000000000000000000000000000000000002"
        Assert.True(Result.isError result)

module Inverses =

    [<Property>]
    let ``UL: toString and fromString`` (n: bitMap) =
        let result = n |> BitMap.toString |> BitMap.fromString 0UL
        Assert.Equal(Ok n, result)

module GetValueAtCoordinates = 

    [<Fact>]
    let ``Coordinates (0,0) is true for 1`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.getValueAtCoordinates c 1UL
        Assert.Equal(true, result)

    [<Property>]
    let ``0 returns false for all coords`` (i_b: int) (j_b: int) =
        let i = abs(i_b % 8)
        let j = abs(j_b % 8)
        let c = Coordinates.construct i j |> Result.failOnError
        let result = BitMap.getValueAtCoordinates c 0UL
        Assert.Equal(false, result)

    [<Property>]
    let ``MaxValue returns true for all 8x8 coords`` (i_b: int) (j_b: int) =
        let i = abs(i_b % 8)
        let j = abs(j_b % 8)
        let c = Coordinates.construct i j |> Result.failOnError
        let result = BitMap.getValueAtCoordinates c UInt64.MaxValue
        Assert.Equal(true, result)
    
    [<Fact>]
    let ``Chess BitMap number is correct`` () =
        let result = 
            BitMap.toString BitMapNumbers.startingChessPiecePosition
        Assert.Equal(
            "1111111111111111000000000000000000000000000000001111111111111111",
            result)

    [<Property>]
    let ``Returns true only for starting chess position 8x8 coords`` (randomInt: int) =
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
    let ``Returns true only for starting chess white position 8x8 coords`` (randomInt: int) =
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
    let ``Update from 0 to 1 AT (0,0)`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.setValueAtCoordinates true c 0UL
        Assert.Equal(1UL, result)
        
    [<Fact>]
    let ``Update from 1 to 0 AT (0,0)`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.setValueAtCoordinates false c 0UL
        Assert.Equal(0UL, result)

module SwitchValueAtCoordinates =

    [<Fact>]
    let ``Switch at (0,0)`` () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.switchValueAtCoordinates c 0UL
        Assert.Equal(1UL, result)

module IsOnAtCoordinates =
    
    [<Fact>]
    let IsOnAtCoordinates_CheckAt_0_0_ForEmptyBitmap_ReturnsFalse () =
        let c = Coordinates.construct 0 0 |> Result.failOnError
        let result = BitMap.isOnAtCoordinates c 0UL
        Assert.False(result)
        