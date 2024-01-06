# Checkerboard 5.0

This library is for efficient storing of creating, modifying and accessing a checkerboard. A checkerboard is an 8x8 board of squares used in games such as Chess and Checkers. Checkerboard is implemented functionally using F#. The library mainly comprises two modules: `Coordinates` and `BitMap`, each with their associated functions.

## Types and Modules

### Coordinates

The `coordinates` type plays a vital role within the library, represented as a `UInt64`. It mainly deals with the creation, manipulation, and extraction of checkerboard square coordinates.

#### Module Functions:

- `construct`: Constructs a `coordinates` instance from given row and column values, represented as integers.

- `getFile`: Returns the file (column) of any given `coordinates` type as an alphanumeric character.

- `getRow`: Returns the row of the `coordinates` instance as a numeric value.

- `getName`: Returns the name of the `coordinates` instance as a string, which is the combination of its file and row.
  
- `parse`: Takes a string representation of a coordinate (in "[file][row]" format, e.g., "a2") and transforms it into a `coordinates` instance.

### BitMap

The `bitMap` type, essentially a `UInt64`, represents checkerboard states effectively. A checkerboard layer composed of multiple `bitMaps` can denote custom square states over an 8x8 board.

#### Module Functions:

- `getValueAtCoordinates (coords: coordinates) (bitMap: bitMap)`: Get the bit value at a specific coordinate from the provided bitmap.

- `setValueAtCoordinates (value: bool) (coords: coordinates) (bitMap: bitMap)`: Sets the designated value at a specific coordinate in the provided bitmap.

- `switchValueAtCoordinates (coords: coordinates) (bitMap: bitMap)`: Switches the bit value at a specific coordinate.

- `isolateValues (bitMap: bitMap)`: Isolates the coordinates of positive values in bitmap format. Returns a list of `bitMaps` where each has a single bit in common with the given bitmap.

### CoordinatesCollection Module

The `CoordinatesCollection` type manages collections of `Coordinates` instances. Each `CoordinatesCollection` is a `UInt64` under the hood, which serves as a compact way to store multiple coordinates at once.

#### Module Functions

- `construct`: Constructs an empty `coordinatesCollection`.

- `toList`: Converts a `coordinatesCollection` into a List of `coordinates`.

- `append`: Adds a `coordinates` instance to a `coordinatesCollection`.

- `appendResult`: Tries to append a `coordinates result` value to a `coordinatesCollection`. If `coordinates result` is an error, this function ignores it and returns the `coordinatesCollection` as-is.

- `remove`: Deletes a `coordinates` instance from a `coordinatesCollection`.

- `filter`: Filters a `coordinatesCollection`, such that only coordinates that also exist in a provided mask `CoordinatesCollection` remain in the output.

