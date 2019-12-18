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