# Checkerboard

This library is for creating, modifying and accessing a checkboard.
A checkboard is a 2D grid of squares, and games that use a checkboard typically have pieces that are placed in the squares available. e.g. Chess and Checkers.
Checkerboard is written functionally in F# and does not rely on external packages.

## Types and Modules

The Checkerboard library contains 4 types, each with a module to perform functions on the type.

### Board

The board type is a 2D array of squares of a generic piece type.

```type board<'Piece> = square<'Piece>[,]```

The Board module contains submodules for creating a board, getting square(s) and updating a board.

### Square

The square type is a record type of an optional generic piece type, and the coordinates it has on a board.

```type square<'Piece> = {piece: 'Piece option; coordinates: int * int}```

The Square module contains functions for getting its properties, and updating the piece it contains.

### Move

The move type is a tuple of two squares.

```type move<'Piece> = square<'Piece> * square<'Piece>```

It is useful for representing when a piece moves between two squares.
The Move module contains functions for getting the pieces on the starting and destination squares, and finding the shift between the two squares.

### Coordinates

The coordinates type is a tuple of ints.

```type coordinates = int * int```

The Coordinates module can parse rank and file naming convention into coordinates.

e.g. "a1" = (0, 0), "c4" = (2, 3)