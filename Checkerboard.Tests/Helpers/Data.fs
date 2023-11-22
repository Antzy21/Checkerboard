module internal Helpers.Data

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
