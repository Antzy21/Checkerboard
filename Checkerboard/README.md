# Checkerboard 5.0

This library is for efficient storing of creating, modifying and accessing a checkerboard.
A checkerboard is an 8x8 board of squares.

Games that use a checkerboard typically have pieces that are placed in the squares available. e.g. Chess and Checkers.
Checkerboard is written functionally in F#.

## Types and Modules

The Checkerboard library contains 2 types Coordinates and Bitmap.
Both types come with a module to perform related functions.

### BitMap

The `bitMap` type is a `UInt64`.
The idea is to build up a board with layers of `bitMaps` to represent each square with multiple bits, each at the same location within the `UInt64`.
A 3 layered `bitMap` board can store up to 8 different states of a square on the board.

#### Module Functions:

Get the bit value as a boolean at given coordinates
`getValueAtCoordinates (coords: coordinates) (bitMap: uint64)`

Set a given boolean value at given coordinates
`setValueAtCoordinates (value: bool) (coords: coordinates) (bitMap: uint64)`

Switch the bit value at given coordinates
`switchValueAtCoordinates (coords: coordinates) (bitMap: uint64)`

Isolate the coordinates of the positive values in bitMap form.
Returns a list of coordinates
`IsolateValues (bitMap: bitMap)`

### Coordinates

The `coordinates` type is stored as a UInt64

#### Module Functions:

`construct`
Constructs a coordinates struct from given coordinates

`getFile`
Returns the corresponding file (column) as a string, represented by a letter of the alphabet.

`getRow`
Returns the corresponding row numeric value as a string.

`getName`
Returns the corresponding name as a string, which is the concatenation of the file and row.

`parse`
Takes a string representation of a coordinate name and returns a coordinates<'Size> value.
The string must be in the format "[file][row]", where the file is a letter of the alphabet and the row is a number.

`tryParse`
Converts the string representation of some coordinates into a coordinates type.
The return value is some optional coordinates.