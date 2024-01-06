namespace CoordinatesCollection

open Checkerboard
open Xunit

module Construct =
    
    [<Fact>]
    let Construct_ByDefault_IsEmptyListOfCoordinates () =
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.toList
            
        Assert.Empty(result)
        
module Append =
    
    [<Fact>]
    let Append_ValueToEmptyCollection_ReturnsSingularList () =
        let coords = { value = 1UL }
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.append coords
            |> CoordinatesCollection.toList
            
        Assert.Single(result)
        
    [<Fact>]
    let AppendResult_ErrorValueToEmptyCollection_ReturnsEmptyCollection () =
        let coords = Error "No coordinates"
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.appendResult coords
            |> CoordinatesCollection.toList
            
        Assert.Empty(result)
        
    [<Fact>]
    let AppendResult_OkValueToEmptyCollection_ReturnsSingularList () =
        let coords = Ok { value = 1UL }
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.appendResult coords
            |> CoordinatesCollection.toList
            
        Assert.Single(result)
        
module Remove =
    
    [<Fact>]
    let Remove_ValueOfSingularList_ReturnsEmptyCollection () =
        let coords = { value = 1UL }
        let result =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.append coords
            |> CoordinatesCollection.remove coords
            |> CoordinatesCollection.toList
            
        Assert.Empty(result)
        
module Filter =
    
    [<Fact>]
    let Filter_ACollectionOnItself_ReturnsItself () =
        let coords = { value = 1UL }
        let cc =
            CoordinatesCollection.construct ()
            |> CoordinatesCollection.append coords
        let result =
            cc
            |> CoordinatesCollection.filter cc
            
        Assert.Equal(result, cc)