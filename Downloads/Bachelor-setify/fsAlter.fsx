open System.Text.RegularExpressions

let interpretSum (result: string array) =
    let indexVariable = result.[1]
    let lowerBound = result.[2]
    let upperBound = result.[3]
    let innerFunction = result.[4]
    match innerFunction with
    | "n" -> sprintf "simplesum(%s, %s, %s)" indexVariable lowerBound upperBound
    | "n^2" -> sprintf "squaredsum(%s, %s, %s)" indexVariable lowerBound upperBound
    | "C*n" -> sprintf "coefficientsum(%s, %s, %s)" indexVariable lowerBound upperBound
    | _ -> printfn "ERROR"; sprintf "%s" result


let searchAndReplace (filePath: string) =
    use writeFile: System.IO.StreamWriter = System.IO.File.CreateText("newFile.txt")
    writeFile.WriteLine("import \"sumdomains.vpr\"")
    let readFile = System.IO.File.ReadAllText(filePath)
    let searchString = "(sum\\[(.?)\\]\\((.*?),(.*?),(.*?)\\))"
    let result = Regex.Matches(readFile, searchString)
    for sum in result do
        let newSum = interpretSum (sum.Groups.[1..].Values |> Seq.map string)
        // createDomain(newSum)
        printfn "%s" newSum
        let updatedFile = readFile.Replace(sum.Value, newSum)
        writeFile.Write(updatedFile)

// Example usage
let filePath = "example.txt"
searchAndReplace filePath
