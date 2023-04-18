namespace BitMap

open Checkerboard
open Helpers.Data
open Xunit
open FsCheck.Xunit
open System

module ToString =

    [<Fact>]
    let ``0s => 0000000000000000`` () =
        let result = BitMap.toString 0s
        Assert.Equal("0000000000000000", result)

    [<Fact>]
    let ``1s => 0000000000000001`` () =
        let result = BitMap.toString 1s
        Assert.Equal("0000000000000001", result)

    [<Fact>]
    let ``2s => 0000000000000010`` () =
        let result = BitMap.toString 2s
        Assert.Equal("0000000000000010", result)

    [<Fact>]
    let ``3s => 0000000000000011`` () =
        let result = BitMap.toString 3s
        Assert.Equal("0000000000000011", result)

    [<Fact>]
    let ``4s => 0000000000000100`` () =
        let result = BitMap.toString 4s
        Assert.Equal("0000000000000100", result)

module FromString =

    [<Fact>]
    let ``0000000000000000 => 0s`` () =
        let result = BitMap.fromString 0s "0000000000000000"
        Assert.Equal(Ok 0s, result)

    [<Fact>]
    let ``0000000000000001 => 1s`` () =
        let result = BitMap.fromString 0s "0000000000000001"
        Assert.Equal(Ok 1s, result)

    [<Fact>]
    let ``0000000000000010 => 2s`` () =
        let result = BitMap.fromString 0s "0000000000000010"
        Assert.Equal(Ok 2s, result)

    [<Fact>]
    let ``0000000000000011 => 3s`` () =
        let result = BitMap.fromString 0s "0000000000000011"
        Assert.Equal(Ok 3s, result)

    [<Fact>]
    let ``0000000000000100 => 4s`` () =
        let result = BitMap.fromString 0s "0000000000000100"
        Assert.Equal(Ok 4s, result)

module ToStringBlock =

    [<Fact>]
    let ``0s => all zeros`` () =
        let result = BitMap.toStringBlock 0s
        Assert.Equal("0000\n0000\n0000\n0000\n", result)

    [<Fact>]
    let ``1s, 2s are correct`` () =
        let result = BitMap.toStringBlock 1s
        Assert.Equal("1000\n0000\n0000\n0000\n", result)
        let result = BitMap.toStringBlock 2s
        Assert.Equal("0100\n0000\n0000\n0000\n", result)

    [<Fact>]
    let ``8x8 1`` () =
        let result = BitMap.toStringBlock 1UL
        Assert.Equal(BitMapString.stringOf1, result)

    [<Fact>]
    let ``8x8 maxValue`` () =
        let result = BitMap.toStringBlock UInt64.MaxValue
        Assert.Equal(BitMapString.stringOfMaxUInt64, result)

    [<Fact>]
    let ``4x4 maxValue`` () =
        let result = BitMap.toStringBlock UInt16.MaxValue
        Assert.Equal(BitMapString.stringOfMaxUInt16, result)

    [<Fact>]
    let ``Chess Starting Position`` () =
        let result = BitMap.toStringBlock BitMapNumbers.startingChessPiecePosition
        Assert.Equal(BitMapString.startingChessPiecePositions, result)

module Inverses =

    [<Property>]
    let ``s: toString and fromString`` (n: int16) =
        let result = n |> BitMap.toString |> BitMap.fromString 0s
        Assert.Equal(Ok n, result)

    [<Property>]
    let ``UL: toString and fromString`` (n: uint64) =
        let result = n |> BitMap.toString |> BitMap.fromString 0UL
        Assert.Equal(Ok n, result)

module GetValueAtCoordinates = 

    [<Fact>]
    let ``Coordinates (0,0) is true for 1`` () =
        let result = BitMap.getValueAtCoordinates (0,0) 1
        Assert.True result

    [<Property>]
    let ``0 returns false for all coords`` (i_b: int) (j_b: int) =
        let i = abs(i_b % 8)
        let j = abs(j_b % 8)
        let result = BitMap.getValueAtCoordinates (i,j) 0
        Assert.False result

    [<Property>]
    let ``MaxValue returns true for all 8x8 coords`` (i_b: int) (j_b: int) =
        let i = abs(i_b % 8)
        let j = abs(j_b % 8)
        let result = BitMap.getValueAtCoordinates (i,j) UInt64.MaxValue
        Assert.True result

    [<Property>]
    let ``MaxValue returns true for all 4x4 coords`` (i_b: int) (j_b: int) =
        let i = abs(i_b % 4)
        let j = abs(j_b % 4)
        let result = BitMap.getValueAtCoordinates (i,j) UInt16.MaxValue
        Assert.True result
    
    [<Property>]
    let ``Returns true only for starting chess position 8x8 coords`` (j_b: int) =
        let j = abs(j_b % 8)
        List.iter (fun i ->
            let result = BitMap.getValueAtCoordinates (i,j) BitMapNumbers.startingChessPiecePosition
            Assert.True result
        ) [0; 1; 6; 7]
        List.iter (fun i ->
            let result = BitMap.getValueAtCoordinates (i,j) BitMapNumbers.startingChessPiecePosition
            Assert.False result
        ) [2; 3; 4; 5]

    [<Property>]
    let ``Returns true only for starting chess white position 8x8 coords`` (j_b: int) =
        let j = abs(j_b % 8)
        List.iter (fun i ->
            let result = BitMap.getValueAtCoordinates (i,j) BitMapNumbers.startingWhitePiecePositions
            Assert.True result
        ) [0; 1]
        List.iter (fun i ->
            let result = BitMap.getValueAtCoordinates (i,j) BitMapNumbers.startingWhitePiecePositions
            Assert.False result
        ) [2; 3; 4; 5; 6; 7]

module GetDimensionFromIntegerType =

    [<Fact>]
    let ``int16 square has side length 4`` () =
        let result = BitMap.getDimensionFromIntegerType 0us
        Assert.Equal(4, result)

    [<Fact>]
    let ``int64 square has side length 8`` () =
        let result = BitMap.getDimensionFromIntegerType 0UL
        Assert.Equal(8, result)

module UpdateValueAtCoordinates =

    [<Fact>]
    let ``Update from 0 to 1`` () =
        let result = BitMap.updateValueAtCoordinates true (0,0) 0UL
        Assert.Equal(1UL, result)

