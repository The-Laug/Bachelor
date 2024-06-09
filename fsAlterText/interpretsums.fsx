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




let powerFold (N: int) (I: string) : string =
    List.init N (fun _ -> I)
    |> List.reduce (fun acc elem -> acc + "*" + elem)







let powerSum (filepath) (power: int)  =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let powerString = string power
    if not (domainExists ("domain powerSumDomain" + powerString) (filepath)) then
        writer.WriteLine("
domain powerSumDomain" + (powerString) + " {
// uninterpreted function
function powerSum" + (powerString) + "(i: Int, N: Int): Int

axiom emptyPowerSum" + powerString + "{
    powerSum" + (powerString) + "(0,0) == 0
}

axiom iGreaterThanNPowerSum" + powerString + " {
    forall i:Int, N:Int::
        i > N ==> powerSum" + (powerString) + "(i,N) == 0
}

axiom rightRecursionPowerSum" + powerString + " {
    forall i:Int, N: Int :: 
        0 <= i < N ==> powerSum" + (powerString) + "(i,N) == powerSum" + (powerString) + "(i,N-1) + " + (powerFold power "N") + "
}

axiom leftRecursionPowerSum" + powerString + " {
    forall i:Int, N: Int :: 
        0 <= i < N ==> powerSum" + (powerString) + "(i,N) == " + (powerFold power "i") + " + powerSum" + (powerString) + "(i+1,N)
}
}



")



















let setifySumNoBounds (filepath, indexVariable : string,innerFunction :string,name :string,lowerBound,upperBound) :string =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
    let line = "
domain setify" + (name) + " {
    // uninterpreted function
    function setifySum" + (name) + "(lowerBound:Int, upperBound:Int) : Multiset[Int]

    axiom setifyInSet" + (name) + " {
        forall lowerBound:Int,upperBound: Int, " + (indexVariable) + ": Int ::
            lowerBound<= " + (indexVariable) + " <=upperBound ==> (" + (innerFunction) + " in setifySum" + (name) + "(lowerBound,upperBound))>=1
    }
    
    axiom setifyNotInSet" + (name) + " {
        forall lowerBound:Int,upperBound: Int, " + (indexVariable) + " : Int ::
            " + (indexVariable) + " < lowerBound || " + (indexVariable) + " > upperBound ==> !( (" + (innerFunction) + " in setifySum" + (name) + "(lowerBound,upperBound))>=1 )
    }

    }
    
    
    
    
    "
    if not (domainExists ("domain setify" + (name)) (filepath)) then
        writer.WriteLine(line) 
    "setifySum" + (name) + "(" + lowerBound + "," + upperBound + ")"



let setifySum (filepath, indexVariable : string,innerFunction :string,name :string,lowerBound,upperBound) :string =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
    let line = "
domain setify" + (name) + " {
    // uninterpreted function
    function setifySum" + (name) + (count) + "() : Multiset[Int]

    axiom setifyInSet" + (name) + " {
        forall " + (indexVariable) + ": Int ::
            " + (lowerBound) + "<= " + (indexVariable) + " <=" + (upperBound) + " ==> (" + (innerFunction) + " in setifySum" + (name) + (count) + "())==1
    }
    
    axiom setifyNotInSet" + (name) + " {
        forall " + (indexVariable) + " : Int ::
            " + (indexVariable) + " < " + (lowerBound) + " ==> ( (" + (innerFunction) + " in setifySum" + (name) + (count) + "())==0 )
    }

    axiom setifyAlsoNotInSet" + (name) + " {
        forall " + (indexVariable) + " : Int ::
            " + (indexVariable) + " > " + (upperBound) + " ==> ( (" + (innerFunction) + " in setifySum" + (name) + (count) + "())==0 )
    }

    }
    
    
    
    
    "
    if not (domainExists ("domain setify" + (name)) (filepath)) then
        writer.WriteLine(line) 
    "setifySum" + (name) + (count) + "()"
        






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
}
"
// axiom rightRecursionGenericSum" + (count) + " {
//     forall i:Int, N: Int :: 
//         0 <= i < N ==> genericSum" + (count) + "(i,N) == genericSum" + (count) + "(i,N-1) + " + interpretInnerFunction(innerFunction,indexVariable,"N") + "
// }

// axiom leftRecursionGenericSum" + (count) + " {
//     forall i:Int, N: Int :: 
//         0 <= i < N ==> genericSum" + (count) + "(i,N) == " + interpretInnerFunction(innerFunction,indexVariable,"i") + " + genericSum" + (count) + "(i+1,N)
// }

//     }
    
    
    
    
//     "


    if not (domainExists ("domain genericSum" + (indexVariable) + (count)) (filepath)) then
        writer.WriteLine(line)
















let (|IndexIntPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| indexVar; power |] when System.Int32.TryParse(power, &powermut) && indexVar=indexVariable -> Some(power)
        | _ -> None


let (|AnyIntPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| powervar; power |] when System.Int32.TryParse(power, &powermut) && not (powervar=indexVariable) -> Some(power,powervar)
        | _ -> None


let (|IndexVarPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| indexVar; power |] when indexVar=indexVariable -> Some(power)
        | _ -> None


let (|AnyVarPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| powervar; power |] when  not (powervar=indexVariable) -> Some(power,powervar)
        | _ -> None



let (|CoefficientMatch|_|) (indexVariable:string) (lowerBound) (upperBound) (str: string) =
    let mutable coefficientmut = 0
    let parts = str.Split('*',2)
    match parts with
        | [| coefficient; innerFunction |] when System.Int32.TryParse(coefficient, &coefficientmut) -> Some(coefficient,innerFunction)
        | _ -> None


let (|CoefficientMatchAny|_|) (indexVariable:string) (lowerBound) (upperBound) (str: string) =
    let parts = str.Split('*',2)
    match parts with
        | [| multiplicand; rest |] -> Some(multiplicand,rest)
        | _ -> None


// let (|AdditionMatch|_|) (str: string) =
//     let parts = str.Split('+')
//     match parts with
//         | [| firstTerm; rest |] -> Some(firstTerm,rest)
//         | _ -> None


let rec (|AdditionMatch|_|) (str: string) =
    let parts = str.Split('+', 2) // Split only at the first occurrence of '+'
    match parts with
    | [| firstTerm; rest |] ->
        match rest with
        | AdditionMatch(innerFirst, innerRest) ->
            Some(firstTerm, innerFirst + "+" + innerRest)
        | _ ->
            Some(firstTerm, rest)
    | _ -> None




let rec (|SubtractionMatch|_|) (str: string) =
    let parts = str.Split('-', 2) // Split only at the first occurrence of '-'
    match parts with
    | [| firstTerm; rest |] ->
        match rest with
        | SubtractionMatch(innerFirst, innerRest) ->
            Some(firstTerm, innerFirst + "-" + innerRest)
        | _ ->
            Some(firstTerm, rest)
    | _ -> None



let strContainsVariable (s:string) = s |> Seq.exists Char.IsAsciiLetter






let rec interpretSum (indexVariable:string) (lowerBound:string) (upperBound:string) (innerFunc:string) (summationpath) (isSet:bool ) : string =
    let outputpath = "./outputs/" + summationpath
    let mutable intHolder = 0
    let mutable charHolder = 'a'
    let rec interpretTerm (term:string) =
        match term.Trim() with
        | (c: string) when c=indexVariable  -> // SimpleSum
            simpleSumDomain (outputpath) |> ignore
            if isSet then 
                let count = GlobalCounterModule.getCounter()
                GlobalCounterModule.incrementCounter() 
                setifySum(outputpath,indexVariable,term.Trim(),"simpleSum" + (string count),lowerBound,upperBound)
            else 
                sprintf "simplesum(%s, %s)" lowerBound upperBound
        | c when System.Int32.TryParse(c,&intHolder)  ->  // Constant sum
            if isSet then 
                let count = GlobalCounterModule.getCounter()
                GlobalCounterModule.incrementCounter() 
                setifySum(outputpath,indexVariable,term.Trim(),"constantSum" + (string count),lowerBound,upperBound)
            else
                sprintf "(%d*(%s-%s+1))" intHolder upperBound lowerBound
        | (IndexIntPowerMatch indexVariable powerString) when (not (strContainsVariable powerString)) -> // where n is the indexvariable n^expression, where expression does not contain a ascii character
            let power = int powerString
            powerSum outputpath power |> ignore
            if isSet then 
                let count = GlobalCounterModule.getCounter()
                GlobalCounterModule.incrementCounter() 
                setifySum(outputpath,indexVariable,(powerFold (int power) indexVariable),"power" + (powerString.Trim()) + "Sum" + (string count),lowerBound,upperBound)
            else 
                sprintf "powerSum%s(%s,%s)"  (powerString.Trim()) lowerBound upperBound
        | (AnyIntPowerMatch indexVariable powerString) -> // where n is the indexvariable n^expression where expression can contain an ascii character
            let (power,powerVar) = powerString
            if isSet then 
                let count = GlobalCounterModule.getCounter()
                GlobalCounterModule.incrementCounter() 
                setifySum(outputpath,indexVariable,(powerFold (int power) powerVar),"power" + (power) + "SumAny" + (string count),lowerBound,upperBound) 
            else 
                sprintf "%s*(%s-%s)"  (powerFold (int power) powerVar) upperBound lowerBound
        | (CoefficientMatch indexVariable lowerBound upperBound interpretedSum) -> 
            let (coefficient,innerFunction) = interpretedSum
            sprintf "%s * %s"  coefficient (interpretTerm innerFunction)
        | (CoefficientMatchAny indexVariable lowerBound upperBound interpretedSum) -> 
            let (multiplicand,rest) = interpretedSum
            sprintf "%s * %s"  (interpretTerm multiplicand) (interpretTerm rest)
        | (AdditionMatch interpretedSum) -> 
            let (firstTerm: string,rest) = interpretedSum
            sprintf "%s + %s"  (interpretTerm firstTerm) (interpretTerm rest)
        | (SubtractionMatch interpretedSum) -> 
            let (firstTerm: string,rest) = interpretedSum
            sprintf "%s - %s"  (interpretTerm firstTerm) (interpretTerm rest)
        | (c: string) when Char.TryParse(c,&charHolder)  -> 
            sprintf "(%c*(%s-%s+1))" (charHolder) lowerBound upperBound
        | _ -> 
            printf "%s |" (term.Trim())
            let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
            genericSum(outputpath,indexVariable,innerFunc)
            // setifySum(outputpath,indexVariable,innerFunc,"genericSum" + count) |> ignore
            GlobalCounterModule.incrementCounter()
            // if isSet then 
            //     setifySum(outputpath,indexVariable,term,"genericSum",lowerBound,upperBound) 
            // else 
            sprintf "genericSum%s(%s, %s)" count lowerBound upperBound
    interpretTerm innerFunc




    // match innerFunc.Split('+') with
    // | terms ->
    //     let interpretedTerms = terms |> Array.map interpretTerm
    //     String.concat " + " interpretedTerms
    // match innerFunc.Split('-') with
    // | terms ->
    //     let interpretedTerms = terms |> Array.map interpretTerm
    //     String.concat " - " interpretedTerms






let rec processLine (line: string) (summationPath)=
    let pattern = @"for ([a-z]+?)=(.*?) to (.*?) sum \((.*?)\)"
    let matchResult = Regex.Match(line, pattern)
    let setPattern = @"for ([a-z]+?)=(.*?) to (.*?) setsum \((.*?)\)"
    let matchSetResult = Regex.Match(line, setPattern)
    if matchResult.Success then
        let indexVariable = matchResult.Groups.[1].Value
        let lowerBound = matchResult.Groups.[2].Value
        let upperBound = matchResult.Groups.[3].Value
        let innerFunc = matchResult.Groups.[4].Value
        let transformed = interpretSum indexVariable lowerBound upperBound innerFunc summationPath false
        let line = line.Replace(matchResult.Value, transformed)
        processLine line summationPath
    elif matchSetResult.Success then
        let indexVariable = matchSetResult.Groups.[1].Value
        let lowerBound = matchSetResult.Groups.[2].Value
        let upperBound = matchSetResult.Groups.[3].Value
        let innerFunc = matchSetResult.Groups.[4].Value
        let transformed = interpretSum indexVariable lowerBound upperBound innerFunc summationPath true
        let line = line.Replace(matchSetResult.Value, transformed)
        processLine line summationPath 
    else
        line // Return the line unchanged if no match is found
    


let readLines filePath = System.IO.File.ReadLines(filePath);;



let writeStringsToFile (outputFilePath: string) (inputFilePath: string) =
    if not (Directory.Exists("./outputs")) then Directory.CreateDirectory("./outputs") |> ignore
    let strings = System.IO.File.ReadLines(inputFilePath)
    use outputWriter: StreamWriter = new StreamWriter(outputFilePath)
    let summationPath = Path.GetFileNameWithoutExtension(inputFilePath) + "_summationDomain.vpr"
    outputWriter.WriteLine("import \"" + summationPath + "\"" )
    File.Delete("./outputs/" + summationPath) |> ignore
    strings |> Seq.iter (fun str -> outputWriter.WriteLine(processLine str summationPath))

let main (argv:string array)  =
    if argv.Length = 0 then
        printfn "Please provide a file path"
        1
    else
        let path = argv.[0]
        let fileName = Path.GetFileNameWithoutExtension(path)
        let outputFilePath = Path.Combine("./outputs", fileName + "_output.vpr")
        writeStringsToFile outputFilePath path
        0


main fsi.CommandLineArgs.[1..] |> ignore

