namespace Checkerboard

type squareBitMap = bool list
type pieceBitMap = bool list

module Parsers =

    let addLayerToParser (additionalLayerParser: bool -> 'BinaryType) (parser: squareBitMap -> 'Piece) : squareBitMap -> 'BinaryType * 'Piece =
        fun (wholeMap: squareBitMap) ->
            match wholeMap with
            | [] ->
                failwith "bitmap list not large enough to contain data for square"
            | additionalLayerBitmap :: pieceBitMaps ->
                let binaryType = additionalLayerParser additionalLayerBitmap
                let piece = parser pieceBitMaps
                binaryType, piece

    let addOptionalLayerToParser (parser: squareBitMap -> 'Piece) : squareBitMap -> 'Piece option =
        fun (wholeMap: squareBitMap) ->
            match wholeMap with
            | [] ->
                failwith "bitmap list not large enough to contain data for square"
            | optionBitMap :: pieceBitMaps ->
                match optionBitMap with
                | true -> 
                    parser pieceBitMaps
                    |> Some
                | false ->
                    None

    let addOptionToConverter (defautLength: int) (bitMapConstructor: 'Piece -> squareBitMap) : 'Piece option -> squareBitMap =
        fun (pieceOption: 'Piece option) ->
            match pieceOption with
            | None -> false :: (List.init defautLength (fun _ -> false))
            | Some piece -> true :: bitMapConstructor piece

    let addSomeOptionToConverter (bitMapConstructor: 'Piece -> squareBitMap) : 'Piece option -> squareBitMap =
        fun (pieceOption: 'Piece option) ->
            true :: bitMapConstructor (Option.get pieceOption)

