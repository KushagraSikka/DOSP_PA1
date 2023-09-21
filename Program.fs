module Program

open System

[<EntryPoint>]
let main argv =
    match argv with
    | [| "server"; port |] -> 
        try
            let portNum = Int32.Parse port
            Server.startServer portNum  // Assuming you have a function called startServer in your Server module
            0  // Return an integer exit code
        with
        | :? FormatException -> 
            printfn "Invalid port number."
            1  // Return an integer exit code

    | [| "client"; port |] -> 
        try
            let portNum = Int32.Parse port
            Client.startClient portNum  // Assuming you have a function called startClient in your Client module
            0  // Return an integer exit code
        with
        | :? FormatException -> 
            printfn "Invalid port number."
            1  // Return an integer exit code

    | _ -> 
        printfn "Usage: dotnet run -- (server|client) <port>"
        1  // Return an integer exit code