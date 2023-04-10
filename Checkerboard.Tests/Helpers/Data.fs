module internal Helpers.Data

open Checkerboard

module Coordinates =
    
    let _4_0 = (4,0)
    let _3_2 = (3,2)
    let _0_0 = (0,0)


module PieceParsers =
    
    let private toCharPiece (boolAry: bool list) : char =
        match boolAry[0], boolAry[1], boolAry[2] with
        | true, true, true -> 'a'
        | true, true, false -> 'b'
        | true, false, true -> 'c'
        | true, false, false -> 'd'
        | false, true, true -> 'e'
        | false, true, false -> 'f'
        | false, false, true -> 'g'
        | false, false, false -> 'h'

    let private fromCharPiece (c: char) : bool list =
        match c with
        | 'a' -> [true; true; true]
        | 'b' -> [true; true; false]
        | 'c' -> [true; false; true]
        | 'd' -> [true; false; false]
        | 'e' -> [false; true; true]
        | 'f' -> [false; true; false]
        | 'g' -> [false; false; true]
        | 'h' -> [false; false; false]
        | invalidChar -> failwith $"Not valid char: '{invalidChar}'"

    let toChar = Parsers.addOptionalLayerToParser toCharPiece

    let fromChar = Parsers.addOptionToConverter 3 fromCharPiece
       

module Pieces =
    
    let exampleA = Some 'a' |> PieceParsers.fromChar
    let exampleB = Some 'b' |> PieceParsers.fromChar

module BitMapString =
    
    let stringOf1 =
        "10000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"

    let stringOfMaxUInt64 =
        "11111111\n"+
        "11111111\n"+
        "11111111\n"+
        "11111111\n"+
        "11111111\n"+
        "11111111\n"+
        "11111111\n"+
        "11111111\n"

    let stringOfMaxUInt16 =
        "1111\n"+
        "1111\n"+
        "1111\n"+
        "1111\n"

    let startingChessPiecePositions =
        "11111111\n"+
        "11111111\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "11111111\n"+
        "11111111\n"
        
    let startingWhitePiecePositions =
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "00000000\n"+
        "11111111\n"+
        "11111111\n"

module BitMapNumbers =
    
    let startingChessPiecePosition = 18446462598732906495UL

    let startingWhitePiecePositions = 65535UL

module Boards = 

    let exampleChar : board =
        Board.init 4
