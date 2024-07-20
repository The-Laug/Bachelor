open System.IO
open System
open System.Text.RegularExpressions

let rec interpretSum (indexVariable:string) (lowerBound:string) (upperBound:string) (innerFunc:string) : string =
    match innerFunc with
    | _"+"_ -> sprintf "to"
    | "n" -> (sprintf "simplesum(%s, %s, %s)" indexVariable lowerBound upperBound)
    | "n^2" -> sprintf "squaredsum(%s, %s, %s)" indexVariable lowerBound upperBound
    | "C*n" -> sprintf "coefficientsum(%s, %s, %s)" indexVariable lowerBound upperBound
    | _ ->  sprintf "ERROR: InnerFunc is: (%s)" innerFunc


let rec processLine (line: string) =
    let pattern = @"for ([a-z]+?)=(.*?) to (.*?) sum \((.*?)\)"
    let matchResult = Regex.Match(line, pattern)
    if matchResult.Success then
        let indexVariable = matchResult.Groups.[1].Value
        let lowerBound = matchResult.Groups.[2].Value
        let upperBound = matchResult.Groups.[3].Value
        let innerFunc = matchResult.Groups.[4].Value
        let transformed = interpretSum indexVariable lowerBound upperBound innerFunc
        let line = line.Replace(matchResult.Value, transformed)
        processLine line
    else
        line // Return the line unchanged if no match is found


let readLines filePath = System.IO.File.ReadLines(filePath);;



let writeStringsToFile (filePath: string) (strings: seq<string>) =
    use writer = new StreamWriter(filePath)
    strings |> Seq.iter (fun str -> writer.WriteLine(processLine str))

writeStringsToFile "output.txt" (readLines "example.txt")