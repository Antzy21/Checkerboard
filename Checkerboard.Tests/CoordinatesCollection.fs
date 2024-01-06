namespace CoordinatesCollection

open Checkerboard
open Xunit
module Construct =
    
    [<Fact>]
    let ``Construct gives empty list of coordinates`` () =
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.toList
        Assert.Empty(result)

module Append =
    
    [<Fact>]
    let ``Appending value to empty collection results in single item list`` () =
        let coords = { value = 1UL }
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.append coords
            |> CoordinatesCollection.toList
        Assert.Single(result)

    [<Fact>]
    let ``Appending Error value to empty collection results in empty collection`` () =
        let coords = Error "No coordinates"
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.appendResult coords
            |> CoordinatesCollection.toList
        Assert.Empty(result)

    [<Fact>]
    let ``Appending Ok value to empty collection results in single item list`` () =
        let coords = Ok { value = 1UL }
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.appendResult coords
            |> CoordinatesCollection.toList
        Assert.Single(result)

module Remove =
    
    [<Fact>]
    let ``Removing value from a single item list results in empty collection`` () =
        let coords = { value = 1UL }
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.append coords
            |> CoordinatesCollection.remove coords
            |> CoordinatesCollection.toList
        Assert.Empty(result)
    
module Filter =
    
    [<Fact>]
    let ``Filtering a collection with itself results in the original collection`` () =
        let coords = { value = 1UL }
        let cc =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.append coords
        let result =
            cc
            |> CoordinatesCollection.filter cc
        Assert.Equal(result, cc)