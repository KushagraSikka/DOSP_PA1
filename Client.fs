module Client

open System
open System.Net.Sockets
open System.Text

let startClient port =
    use client = new TcpClient("localhost", port)
    use stream = client.GetStream()
    let buffer = Array.zeroCreate 1024
    let encoding = Encoding.UTF8

    // Receive the initial greeting from the server
    let bytesRead = stream.Read(buffer, 0, buffer.Length)
    let serverMessage = encoding.GetString(buffer, 0, bytesRead).Trim()
    printfn "Server response: %s" serverMessage

    // Loop to send commands and receive responses
    while true do
        printf "Enter command: "
        let userCommand = Console.ReadLine()

        // Send the command to the server
        let commandBytes = encoding.GetBytes userCommand
        stream.Write(commandBytes, 0, commandBytes.Length)

        // Receive the server's response
        let bytesRead = stream.Read(buffer, 0, buffer.Length)
        let serverMessage = encoding.GetString(buffer, 0, bytesRead).Trim()

        // Display the server's response
        printfn "Server response: %s" serverMessage

        // Handle error codes and termination
        match serverMessage with
        | "-1" -> printfn "Incorrect operation command."
        | "-2" -> printfn "Number of inputs is less than two."
        | "-3" -> printfn "Number of inputs is more than four."
        | "-4" -> printfn "One or more of the inputs contain(s) non-number(s)."
        | "-5" -> 
            printfn "Exit."
            breakLoop
        | _ -> ()

    // Exit the program
    printfn "Client is exiting."
