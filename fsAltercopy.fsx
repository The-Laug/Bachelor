open System.IO
open System

// Function to replace '1' with '2' in a word
let processWord (word: string) =
    word

// Custom function to read words from a StreamReader
let rec readWord (reader: StreamReader) (acc: string) =
    match reader.Peek() with
    | -1 -> None // End of file
    | c when System.Char.IsWhiteSpace(char c) ->
        reader.Read() |> ignore // Consume whitespace character
        if acc <> "" then Some acc else readWord reader "" // Skip multiple whitespaces
    | c ->
        reader.Read() |> ignore
        readWord reader (acc + string c)

// Process the file word by word
let processFile (inputFile: string) (outputFile: string) =
    use reader = new StreamReader(inputFile)
    use writer = new StreamWriter(outputFile)

    let rec processWords () =
        match readWord reader "" with
        | Some word ->
            let modifiedWord = processWord word
            printfn "%s" ( modifiedWord)
            writer.Write(modifiedWord + " ")
            processWords()
        | None -> () // End of file

    processWords()

// Read content from the file
let inputFile = "example.txt"
let outputFile = "output.txt"

// Process the content
processFile inputFile outputFile

printfn "File processing completed."
