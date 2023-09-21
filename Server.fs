module Server

open System
open System.Net
open System.Net.Sockets
open System.Text
open System.Threading.Tasks

let mutable isServerRunning = true  // Flag to control the server loop

let handleClient (client: TcpClient) =
    use stream = client.GetStream()
    let buffer = Array.zeroCreate 1024
    let encoding = Encoding.UTF8

    let greeting = "Hello!"
    let greetingBytes = encoding.GetBytes greeting
    stream.Write(greetingBytes, 0, greetingBytes.Length)

    while true do
        let bytesRead = stream.Read(buffer, 0, buffer.Length)
        let clientMessage = encoding.GetString(buffer, 0, bytesRead).Trim()

        let response = 
            match clientMessage.Split(' ') with
            | [|"add"; nums|] -> 
                try 
                    let sum = nums |> Array.map Int32.Parse |> Array.sum
                    sum.ToString()
                with
                | :? FormatException -> "-4"
            | [|"subtract"; nums|] -> 
                try 
                    let diff = nums |> Array.map Int32.Parse |> Array.reduce (-)
                    diff.ToString()
                with
                | :? FormatException -> "-4"
            | [|"multiply"; nums|] -> 
                try 
                    let product = nums |> Array.map Int32.Parse |> Array.reduce (*)
                    product.ToString()
                with
                | :? FormatException -> "-4"
            | [|"bye"|] -> 
                isServerRunning <- false
                "-5"
            | _ -> "-1"

        let responseBytes = encoding.GetBytes response
        stream.Write(responseBytes, 0, responseBytes.Length)

        if clientMessage = "bye" then breakLoop

let startServer port =
    let listener = new TcpListener(IPAddress.Any, port)
    listener.Start()
    printfn "Server is running and listening on port %d." port

    while true do
        let client = listener.AcceptTcpClientAsync().Result
        let _ = Task.Run(fun () -> handleClient client)
    ()
