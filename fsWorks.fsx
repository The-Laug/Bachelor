open System.IO
open System
open System.Text.RegularExpressions

let rec interpretSum (indexVariable:string) (lowerBound:string) (upperBound:string) (innerFunc:string) : string =
    printf "%s" indexVariable
    match innerFunc with
    | "n" -> (sprintf "simplesum(%s, %s, %s)" indexVariable lowerBound upperBound)
    | _ ->  sprintf "(%s)" innerFunc


let processLine (line: string) =
    let pattern = @"for ([a-z]*?)=(.*?) to (.*?) sum \((.*)\)"
    let indexVariable = "$1"
    printf "%s" indexVariable
    Regex.Replace(line, pattern, interpretSum "$1" "$2" "$3" "$4")


let readLines filePath = System.IO.File.ReadLines(filePath);;



let writeStringsToFile (filePath: string) (strings: seq<string>) =
    use writer = new StreamWriter(filePath)
    strings |> Seq.iter (fun str -> writer.WriteLine(processLine str))

writeStringsToFile "output.txt" (readLines "example.txt")