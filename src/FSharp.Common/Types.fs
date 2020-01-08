namespace Numaka.FSharp.Common


type NonEmptyString = private NonEmptyString of string

[<RequireQualifiedAccess>]
module NonEmptyString =

    let value (NonEmptyString x) = x

    let createOptional = 
        String.createOptional
        >> Option.map NonEmptyString

    let createRequired name value =
        String.createRequired name value
        |> Result.map NonEmptyString

type NonNegativeInt = private NonNegativeInt of int

[<RequireQualifiedAccess>]
module NonNegativeInt =
    
    let value (NonNegativeInt x) = x

    let createOptional x =
        if x < 0
        then None
        else NonNegativeInt x |> Some