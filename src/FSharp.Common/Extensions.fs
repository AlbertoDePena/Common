namespace Numaka.FSharp.Common

[<RequireQualifiedAccess>]
module String =
    open System

    let createOptional value =
        if String.IsNullOrWhiteSpace value then
           None
        else Some value

    let createRequired name value =
        if String.IsNullOrWhiteSpace value then
            Error (sprintf "'%s' is required." name)
        else Ok value

[<RequireQualifiedAccess>]
module Environment =
    open System

    let GetEnvironmentVariableAsInt variableName =
        let value = Environment.GetEnvironmentVariable variableName
        let isInt, intValue = Int32.TryParse value
        match isInt with
        | true -> intValue
        | false -> 0

    let GetEnvironmentVariableAsLong variableName =
        let value = Environment.GetEnvironmentVariable variableName
        let isLong, longValue = Int64.TryParse value
        match isLong with
        | true -> longValue
        | false -> 0L

    let GetEnvironmentVariableAsBool variableName =
        let value = Environment.GetEnvironmentVariable variableName
        let isBool, boolValue = bool.TryParse value
        match isBool with
        | true -> boolValue
        | false -> false

[<RequireQualifiedAccess>]
module Async =
    open System
    open System.Threading

    let bind f x = async.Bind(x, f)

    let retn x = async.Return x

    let map f x = x |> bind (f >> retn)

    /// Runs the asynchronous computation in a loop until 'Ctrl+C' is pressed.
    ///
    /// milliseconds: The number of milliseconds to sleep between each asynchronous computation execution.
    ///
    /// task: The asynchronous computation.
    let RunUntilCancelKeyPressed milliseconds task =
        let looper =
            async {
                while (not <| Async.DefaultCancellationToken.IsCancellationRequested) do
                    do! task
                    do! Async.Sleep milliseconds
            }

        try
            use cancellationSource = new CancellationTokenSource()    
            let callback _ (args : ConsoleCancelEventArgs) = 
                args.Cancel <- true
                cancellationSource.Cancel() 
            let handler = ConsoleCancelEventHandler(callback) 
            Console.CancelKeyPress.AddHandler handler            
            Async.RunSynchronously (looper, cancellationToken = cancellationSource.Token)
        with
        | :? OperationCanceledException -> () 

[<RequireQualifiedAccess>]
module Result =

    let switch f =
        f >> Ok

    let either ok error x =
        match x with
        | Ok o -> ok o
        | Error e -> error e

    let ofChoice x =
        match x with
        | Choice1Of2 o -> Ok o
        | Choice2Of2 e -> Error e

    let ofOption e x =
        match x with
        | Some t -> Ok t
        | None -> Error e
        
    let toOption x =
        match x with
        | Ok o -> Some o
        | Error _ -> None
        
    let toErrorOption =
        function
        | Ok _ -> None
        | Error e -> Some e

    let tee f x =
        f x; x

    let tryCatch f handler x =
        try
            f x |> Ok
        with
        | ex -> handler ex |> Error

    let doubleMap ok error =
        either (ok >> Ok) (error >> Error)

    let valueOr f x =
        match x with
        | Ok o -> o
        | Error e -> f e

    let defaultValue x =
        function
        | Ok o -> o
        | Error _ -> x

[<RequireQualifiedAccess>]
module Console =
    open System
    
    let private log =
        let lockObj = obj()
        fun color text ->
            lock lockObj (fun _ ->
                Console.ForegroundColor <- color
                printfn "%s" text
                Console.ResetColor())

    let complete = log ConsoleColor.Magenta

    let ok = log ConsoleColor.Green

    let info = log ConsoleColor.Cyan

    let warn = log ConsoleColor.Yellow
    
    let error = log ConsoleColor.Red
