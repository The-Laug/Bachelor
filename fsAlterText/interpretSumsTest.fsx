open System.IO
open System
open System.Text.RegularExpressions

// let createFile (path) =
//     use writer: StreamWriter = new StreamWriter ("summationDomain.vpr")
//     writer.Write("")
//     writer.Close

let domainExists (domain :String)  (filePath :String)   =
    File.ReadLines(filePath)
    |> Seq.exists (fun line -> line.Contains(domain))


module GlobalCounterModule =

    // Define a mutable global counter
    let mutable counter = 0

    // Function to increment the counter
    let incrementCounter () =
        counter <- counter + 1

    // Function to get the current value of the counter
    let getCounter () =
        counter





let simpleSumDomain (filepath)  =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
    if not (domainExists ("domain simpleSums") (filepath)) then
        writer.WriteLine("
domain simpleSums {
// uninterpreted function
function simplesum(i: Int, N: Int): Int

axiom emptySum {
    simplesum(0,0) == 0
}

axiom iGreaterThanN  {
    forall i:Int, N:Int::
        i > N ==> simplesum(i,N) == 0
}

axiom rightRecursion  {
    forall i:Int, N: Int :: 
        0 <= i < N ==> simplesum(i,N) == simplesum(i,N-1) + N
}

axiom leftRecursion  {
    forall i:Int, N: Int :: 
        0 <= i < N ==> simplesum(i,N) == i + simplesum(i+1,N)
}


}



")











let squareSumDomain (filepath)  =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
    if not (domainExists ("domain squareSums") (filepath)) then
        writer.WriteLine("
domain squareSums" + count + " {
// uninterpreted function
function squaresum(i: Int, N: Int): Int

axiom emptySquareSum{
    squaresum(0,0) == 0
}

axiom iGreaterThanNSquareSum {
    forall i:Int, N:Int::
        i > N ==> squaresum(i,N) == 0
}

axiom rightRecursionSquareSum {
    forall i:Int, N: Int :: 
        0 <= i < N ==> squaresum(i,N) == squaresum(i,N-1) + N*N
}

axiom leftRecursionSquareSum {
    forall i:Int, N: Int :: 
        0 <= i < N ==> squaresum(i,N) == i*i + squaresum(i+1,N)
}
}



")



















let setifySum (filepath, indexVariable : string,innerFunction :string,name :string)  =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
    let line = "
domain setify" + (name) + " {
    // uninterpreted function
    function setifySum" + (name) + "( lowerBound:Int, upperBound:Int) : Set[Int]

    axiom setifyInSet" + (name) + " {
        forall lowerBound:Int,upperBound: Int, " + (indexVariable) + ": Int ::
            lowerBound<= " + (indexVariable) + " <=upperBound ==> " + (innerFunction) + " in setifySum" + (name) + "(lowerBound,upperBound)
    }
    
    axiom setifyNotInSet" + (name) + " {
        forall lowerBound:Int,upperBound: Int, " + (indexVariable) + " : Int ::
            " + (indexVariable) + " < lowerBound || " + (indexVariable) + " > upperBound ==> !( " + (innerFunction) + " in setifySum" + (name) + "(lowerBound,upperBound))
    }

    }
    
    
    
    
    "


    if not (domainExists ("domain setifySums" + (indexVariable) + (count)) (filepath)) then
        writer.WriteLine(line)







let genericSum (filepath, indexVariable : string,innerFunction :string)  =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
    let interpretInnerFunction (innerfunc : String, indexVariable: String,value : String) =
        innerfunc.Replace(indexVariable,value)
    let line = "
domain genericSum" + (indexVariable) + (count) + " {
    // uninterpreted function
    function genericSum" + (count) + "( lowerBound:Int, upperBound:Int) : Int

axiom emptyGenericSum" + (count) + "{
    genericSum" + (count) + "(0,0) == 0
}

axiom iGreaterThanNGenericSum" + (count) + " {
    forall i:Int, N:Int::
        i > N ==> genericSum" + (count) + "(i,N) == 0
}

axiom rightRecursionGenericSum" + (count) + " {
    forall i:Int, N: Int :: 
        0 <= i < N ==> genericSum" + (count) + "(i,N) == genericSum" + (count) + "(i,N-1) + " + interpretInnerFunction(innerFunction,indexVariable,"N") + "
}

axiom leftRecursionGenericSum" + (count) + " {
    forall i:Int, N: Int :: 
        0 <= i < N ==> genericSum" + (count) + "(i,N) == " + interpretInnerFunction(innerFunction,indexVariable,"i") + " + genericSum" + (count) + "(i+1,N)
}

    }
    
    
    
    
    "


    if not (domainExists ("domain genericSum" + (indexVariable) + (count)) (filepath)) then
        writer.WriteLine(line)






















let rec interpretSum (indexVariable:string) (lowerBound:string) (upperBound:string) (innerFunc:string) (summationpath): string =
    let outputpath = "./outputs/" + summationpath
    let rec interpretTerm (term:string) =
        match term with
        | c when c=indexVariable -> 
            let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
            simpleSumDomain (outputpath) |> ignore
            setifySum(outputpath,indexVariable,innerFunc,"simpleSum" + count) |> ignore
            GlobalCounterModule.incrementCounter()
            sprintf "simplesum(%s, %s)" lowerBound upperBound
        | c when c = sprintf "%s^2" indexVariable -> 
            let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
            squareSumDomain (outputpath) |> ignore
            setifySum(outputpath,indexVariable,innerFunc,"squareSum" + count) |> ignore
            GlobalCounterModule.incrementCounter()
            sprintf "squaresum(%s, %s)" lowerBound upperBound
        | "C*n" ->
            let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
            GlobalCounterModule.incrementCounter()
            sprintf "coefficientsum(%s, %s, %s)" indexVariable lowerBound upperBound
        | _ -> 
            let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
            genericSum(outputpath,indexVariable,innerFunc)
            setifySum(outputpath,indexVariable,innerFunc,"genericSum" + count) |> ignore
            GlobalCounterModule.incrementCounter()
            sprintf "genericSum%s(%s, %s)" count lowerBound upperBound

    match innerFunc.Split('+') with
    | terms ->
        let interpretedTerms = terms |> Array.map interpretTerm
        String.concat " + " interpretedTerms

    // match innerFunc.Split('-') with
    // | terms ->
    //     let interpretedTerms = terms |> Array.map interpretTerm
    //     String.concat " - " interpretedTerms






let rec processLine (line: string) (summationPath)=
    let pattern = @"for ([a-z]+?)=(.*?) to (.*?) sum \((.*?)\)"
    let matchResult = Regex.Match(line, pattern)
    if matchResult.Success then
        let indexVariable = matchResult.Groups.[1].Value
        let lowerBound = matchResult.Groups.[2].Value
        let upperBound = matchResult.Groups.[3].Value
        let innerFunc = matchResult.Groups.[4].Value
        let transformed = interpretSum indexVariable lowerBound upperBound innerFunc summationPath
        let line = line.Replace(matchResult.Value, transformed)
        processLine line summationPath
    else
        line // Return the line unchanged if no match is found


let readLines filePath = System.IO.File.ReadLines(filePath);;



let writeStringsToFile (outputFilePath: string) (inputFilePath: string) =
    if not (Directory.Exists("./outputs")) then Directory.CreateDirectory("./outputs") |> ignore
    let strings = System.IO.File.ReadLines(inputFilePath)
    use writer: StreamWriter = new StreamWriter(outputFilePath)
    let summationPath = Path.GetFileNameWithoutExtension(inputFilePath) + "_summationDomain.vpr"
    writer.WriteLine("import \"" + summationPath + "\"" )
    // createFile ("./outputs/" + summationPath) |> ignore
    // File.Delete("summationDomain.vpr") |> ignore
    strings |> Seq.iter (fun str -> writer.WriteLine(processLine str summationPath))

printf "Please enter path to Viper file to interpret: \nExample: \"sumFile.vpr\" \n>"
let path = Console.ReadLine()
let fileName = Path.GetFileNameWithoutExtension(path)

let outputFilePath = Path.Combine("./outputs", fileName + "_output.vpr")

writeStringsToFile outputFilePath path