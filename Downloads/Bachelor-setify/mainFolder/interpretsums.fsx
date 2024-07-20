open System.IO
open System
open System.Text.RegularExpressions

//Checks whether a given domain already exists in the given file
let domainExists (domain :String)  (filePath :String)   =
    File.ReadLines(filePath)
    |> Seq.exists (fun line -> line.Contains(domain))

// Creates a counter used to count the different summation domains
module GlobalCounterModule =

    // Define a mutable global counter
    let mutable counter = 0

    // Function to increment the counter
    let incrementCounter () =
        counter <- counter + 1

    // Function to get the current value of the counter
    let getCounter () =
        counter




//Appends the domain corresponding to the sum of the simple arithmetic progression to the given file
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
        0 <= i <= N ==> simplesum(i,N) == simplesum(i,N-1) + N
}

axiom leftRecursion  {
    forall i:Int, N: Int :: 
        0 <= i <= N ==> simplesum(i,N) == i + simplesum(i+1,N)
}

axiom rightRecursionNeg  {
    forall i:Int, N: Int :: 
        i <= N <=0 ==> simplesum(i,N) == simplesum(i,N-1) + N
}

axiom leftRecursionNeg  {
    forall i:Int, N: Int :: 
        i <= N <=0  ==> simplesum(i,N) == i + simplesum(i+1,N)
}

axiom closedForm  {
    forall  N: Int :: 
        simplesum(1,N) == (N*(N+1))/2
}

axiom positiveTerms  {
    forall  i:Int, N: Int :: 
        0 <= i < N ==> simplesum(i,N) >= i
}

axiom largerIndex  {
    forall  i:Int, N: Int, N2: Int :: 
        0 <= i < N < N2 ==> simplesum(i,N) <= simplesum(i,N2)
}

axiom indexShiftOnZero  {
    forall  i:Int, N: Int :: 
        i < 0 < N ==> simplesum(i,N) == simplesum(i,0) + simplesum(0,N)
}


}



")



//Folds out the exponent
let powerFold (N: int) (I: string) : string =
    List.init N (fun _ -> I)
    |> List.reduce (fun acc elem -> acc + "*" + elem)






//Appends the domain corresponding to the sum of the simple arithmetic progression to the given power to the given file
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
        0 <= i <= N ==> powerSum" + (powerString) + "(i,N) == powerSum" + (powerString) + "(i,N-1) + " + (powerFold power "N") + "
}

axiom leftRecursionPowerSum" + powerString + " {
    forall i:Int, N: Int :: 
        0 <= i <= N ==> powerSum" + (powerString) + "(i,N) == " + (powerFold power "i") + " + powerSum" + (powerString) + "(i+1,N)
}

axiom positiveTermsPowerSum" + powerString + "  {
    forall  i:Int, N: Int :: 
        0 <= i < N ==> powerSum" + (powerString) + "(i,N) >= " + (powerFold power "i") + "
}

axiom largerIndexPowerSum" + powerString + "  {
    forall  i:Int, N: Int, N2: Int :: 
        0 <= i < N < N2 ==> powerSum" + (powerString) + "(i,N) <= powerSum" + (powerString) + "(i,N2)
}

axiom indexShiftOnZeroPowerSum" + powerString + "  {
    forall  i:Int, N: Int :: 
        i < 0 < N ==> powerSum" + (powerString) + "(i,N) == powerSum" + (powerString) + "(i,0) + powerSum" + (powerString) + "(0,N)
}

}



")



let geometricSum (filepath) (power: int)  =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let powerString = string power
    if not (domainExists ("domain geometricSumDomain" + powerString) (filepath)) then
        writer.WriteLine("
domain geometricSumDomain" + (powerString) + " {
// uninterpreted function
function geometricSum" + (powerString) + "(i: Int, N: Int): Int

axiom iGreaterThanNgeometricSum" + powerString + " {
    forall i:Int, N:Int::
        i > N ==> geometricSum" + (powerString) + "(i,N) == 0
}

axiom emptygeometricSum" + powerString + "{
    forall i:Int, N:Int::
    i > 0 && N > i==> geometricSum" + (powerString) + "(i,N) == 0
}

axiom nonEmptygeometricSum" + powerString + "{
    forall i:Int, N:Int::
    i < 0 < N ==> geometricSum" + (powerString) + "(i,N) == 1
}

}



")


















//Adds a set-domain to the specified path, which defines a multiset corresponding to the relevant sum. The NoBounds means that the domain does not take the given bounds into account
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




        





//Creates the domain for the sum if it is not matched to any of the other patterns. That is why this domain only contains the definition of the sum.
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
// axiom rightRecursionGenericSum" + (count) + " {
//     forall i:Int, N: Int :: 
//         0 <= i <= N ==> genericSum" + (count) + "(i,N) == genericSum" + (count) + "(i,N-1) + " + interpretInnerFunction(innerFunction,indexVariable,"N") + "
// }

// axiom leftRecursionGenericSum" + (count) + " {
//     forall i:Int, N: Int :: 
//         0 <= i <= N ==> genericSum" + (count) + "(i,N) == " + interpretInnerFunction(innerFunction,indexVariable,"i") + " + genericSum" + (count) + "(i+1,N)
// }

//     }
    
    
    
    
"


    if not (domainExists ("domain genericSum" + (indexVariable) + (count)) (filepath)) then
        writer.WriteLine(line)





//Adds a set-domain to the specified path, which defines a multiset corresponding to the relevant sum
let setifySum (filepath, indexVariable : string,innerFunction :string,name :string,lowerBound,upperBound) :string =
    // Add simplesum domain to the summationDomain.vpr file
    use writer: StreamWriter = File.AppendText(filepath)
    let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
    let uninterpretedFunction = "setifySum" + (name) + (count) + "()"
    let line = ""
    if name.StartsWith("constantSum") then // For the constant sum, the multiplicity is the difference between the upperbound and the lowerbound
        if not (domainExists ("domain setify" + (name)) (filepath)) then
            writer.WriteLine("
    domain setify" + (name) + " {
    // uninterpreted function
    function " + uninterpretedFunction + " : Multiset[Int]

    axiom setifyInSet" + (name) + " {
            (" + (innerFunction) + " in " + uninterpretedFunction + ")== " + upperBound + "-" + lowerBound + "
    }
    
    axiom setifyNotInSet" + (name) + " {
        forall n : Int ::
            n != " + innerFunction + " ==> ( (n in " + uninterpretedFunction + ")==0 )
    }

    }
    
    
    ")
    elif name.StartsWith("variableSum") then // For the variable sum, the multiplicity is the difference between the upperbound and the lowerbound
        if not (domainExists ("domain setify" + (name)) (filepath)) then
            writer.WriteLine("
    domain setify" + (name) + " {
    // uninterpreted function
    function " + uninterpretedFunction + " : Multiset[Int]

    axiom setifyInSet" + (name) + " {
        forall " + (innerFunction) + " : Int  ::
            (" + (innerFunction) + " in " + uninterpretedFunction + ")== " + upperBound + "-" + lowerBound + "
    }
    
    axiom setifyNotInSet" + (name) + " {
        forall n : Int, " + (innerFunction) + " : Int ::
            n != " + innerFunction + " ==> ( (n in " + uninterpretedFunction + ")==0 )
    }

    }
    
    
    ")
    // elif name.StartsWith("simpleSum") then // 
    //     if not (domainExists ("domain setify" + (name)) (filepath)) then
    //         writer.WriteLine("
    // domain setify" + (name) + " {
    // // uninterpreted function
    // function " + uninterpretedFunction + " : Set[Int]

    // axiom setifyInSet" + (name) + " {
    //         (" + (innerFunction) + " in " + uninterpretedFunction + ")== " + upperBound + "-" + lowerBound + "
    // }
    
    // axiom setifyNotInSet" + (name) + " {
    //     forall n : Int ::
    //         n != " + indexVariable + " ==> ( (" + (indexVariable) + " in " + uninterpretedFunction + ")==0 )
    // }

    // }
    
    
    // ")
    else 
            if not (domainExists ("domain setify" + (name)) (filepath)) then //For other inner functions the multiplicity is one for every value of 
                writer.WriteLine( "
    domain setify" + (name) + " {
    // uninterpreted function
    function " + uninterpretedFunction + " : Multiset[Int]

    axiom setifyInSet" + (name) + " {
        forall " + (indexVariable) + ": Int ::
            " + (lowerBound) + "<= " + (indexVariable) + " <=" + (upperBound) + " ==> (" + (innerFunction) + " in " + uninterpretedFunction + ")==1
    }
    
    axiom setifyNotInSet" + (name) + " {
        forall " + (indexVariable) + " : Int ::
            " + (indexVariable) + " < " + (lowerBound) + " ==> ( (" + (innerFunction) + " in " + uninterpretedFunction + ")==0 )
    }

    axiom setifyAlsoNotInSet" + (name) + " {
        forall " + (indexVariable) + " : Int ::
            " + (indexVariable) + " > " + (upperBound) + " ==> ( (" + (innerFunction) + " in " + uninterpretedFunction + ")==0 )
    }

    }
        
    ") 
    uninterpretedFunction









// This is an active pattern, which matches with inputs of the form n^I, where n is the index variable and a is Int type
let (|IndexIntPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| indexVar; power |] when System.Int32.TryParse(power, &powermut) && indexVar=indexVariable -> Some(power)
        | _ -> None


// This is an active pattern, which matches with inputs of the form a^I, where n is NOT the index variable and a is Int type
let (|AnyIntPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| powervar; power |] when System.Int32.TryParse(power, &powermut) && not (powervar=indexVariable) -> Some(power,powervar)
        | _ -> None


// This is an active pattern, which matches with inputs of the form n^a, where n is the index variable and a is any type, most often a variable
let (|IndexVarPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| indexVar; power |] when indexVar=indexVariable -> Some(power)
        | _ -> None


// This is an active pattern, which matches with inputs of the form a^n, where n is the index variable and a is any type, most often a variable
let (|IndexVarGeometricMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| power; indexVar: string |] when indexVar=indexVariable -> Some(power)
        | _ -> None


// This is an active pattern, which matches with inputs of the form a^a where a is any type, most often a variable
let (|AnyVarPowerMatch|_|) (indexVariable:string) (str: string) =
    let mutable powermut = 0
    let parts = str.Split('^', 2)
    match parts with
        | [| powervar; power |] when  not (powervar=indexVariable) -> Some(power,powervar)
        | _ -> None


//This is an active pattern, which matches with inputs of the form C*n where n is the indexVariable and C is an integer. n*C also works
let (|CoefficientMatch|_|) (indexVariable:string) (lowerBound) (upperBound) (str: string) =
    let mutable coefficientmut = 0
    let parts = str.Split('*',2)
    match parts with
        | [| coefficient; innerFunction |] when System.Int32.TryParse(coefficient, &coefficientmut) -> Some(coefficient,innerFunction)
        | [| innerFunction; coefficient  |] when System.Int32.TryParse(coefficient, &coefficientmut) -> Some(coefficient,innerFunction)
        | _ -> None


let (|CoefficientMatchAny|_|) (indexVariable:string) (lowerBound) (upperBound) (str: string) =
    let mutable coefficientmut = 0
    let parts = str.Split('*',2)
    match parts with
        | [| coefficient; innerFunction |] when not (coefficient = indexVariable) -> Some(coefficient,innerFunction)
        | [| innerFunction; coefficient  |] when not (coefficient = indexVariable) -> Some(coefficient,innerFunction)
        | _ -> None


// let (|CollectPowers|_|) (indexVariable:string) (lowerBound) (upperBound) (str: string) =
//     let parts = str.Split('*',2)
//     match parts with
//         | [| multiplicand; rest |] -> Some(multiplicand,rest)
//         | _ -> None


// let (|AdditionMatch|_|) (str: string) =
//     let parts = str.Split('+')
//     match parts with
//         | [| firstTerm; rest |] -> Some(firstTerm,rest)
//         | _ -> None


//This is an active pattern, which matches with inputs of the form f(n)+f(n) where n is the indexVariable 
let rec (|AdditionMatch|_|) (str: string) =
    let parts = str.Split('+', 2) // Split only at the first occurrence of '+'
    match parts with
    | [| firstTerm; rest |] -> Some(firstTerm, rest)
    | _ -> None




//This is an active pattern, which matches with inputs of the form f(n)-f(n) where n is the indexVariable 
let rec (|SubtractionMatch|_|) (str: string) =
    let parts = str.Split('-', 2) // Split only at the first occurrence of '-'
    match parts with
    | [| firstTerm; rest |] -> Some(firstTerm, rest)
    | _ -> None


//This is an auxilary function, which checks whether a variable is contain in a string
let strContainsVariable (s:string) = s |> Seq.exists Char.IsAsciiLetter
let strContainsSpecificVariable (s:string) (charToLookFor :char) = s |> Seq.exists (fun c -> c = charToLookFor)





//This is the main function, which takes a summand function, processes it and creates files with the relevant viper domains
let rec interpretSum (indexVariable:string) (lowerBound:string) (upperBound:string) (innerFunc:string) (summationpath) (isSet:bool ) : string =
    let outputpath = "./outputs/" + summationpath
    //These mutables are used to hold Ints and Chars
    let mutable intHolder = 0
    let mutable charHolder = 'a'
    // This is the recursive function, which splits and matches the terms of the summand function to create the correct domains
    let rec interpretTerm (term:string) =
        match term.Trim() with
        | (AdditionMatch interpretedSum) -> 
            let (firstTerm: string,rest) = interpretedSum
            sprintf "%s + %s"  (interpretTerm firstTerm) (interpretTerm rest)
        | (SubtractionMatch interpretedSum) -> 
            let (firstTerm: string,rest) = interpretedSum
            sprintf "%s - %s"  (interpretTerm firstTerm) (interpretTerm rest)
        | (CoefficientMatch indexVariable lowerBound upperBound interpretedSum) -> 
            let (coefficient,innerFunction) = interpretedSum
            sprintf "%s * %s"  coefficient (interpretTerm innerFunction)
        | (CoefficientMatchAny indexVariable lowerBound upperBound interpretedSum) -> 
            let (coefficient,innerFunction) = interpretedSum
            sprintf "%s * %s"  coefficient (interpretTerm innerFunction)
        | (IndexIntPowerMatch indexVariable powerString) when (not (strContainsVariable powerString)) -> // This is an active pattern, which matches with inputs of the form n^I, where n is the index variable and a is Int type
            let power = int powerString
            powerSum outputpath power |> ignore
            sprintf "powerSum%s(%s,%s)"  (powerString.Trim()) lowerBound upperBound
        // | (IndexVarGeometricMatch indexVariable powerString) when (not (strContainsVariable powerString)) -> // where n is the indexvariable n^expression, where expression does not contain a ascii character
        //     let power = int powerString
        //     geometricSum outputpath power |> ignore
        //     sprintf "geometricSum%s(%s,%s)"  (powerString.Trim()) lowerBound upperBound
        | (c: string) when c=indexVariable  -> // SimpleSum
            simpleSumDomain (outputpath) |> ignore
            sprintf "simplesum(%s, %s)" lowerBound upperBound
        | c when System.Int32.TryParse(c,&intHolder)  ->  // Constant sum
            sprintf "(%d*(%s-%s+1))" intHolder upperBound lowerBound
        | (c: string) when Char.TryParse(c,&charHolder)  -> 
            sprintf "(%c*(%s-%s+1))" (charHolder) upperBound lowerBound
        | _ -> 
            printf "%s |" (term.Trim())
            let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
            genericSum(outputpath,indexVariable,innerFunc)
            GlobalCounterModule.incrementCounter()
            sprintf "genericSum%s(%s, %s)" count lowerBound upperBound
    let rec interpretTermSet (term:string) =
        match term.Trim() with
        | (IndexIntPowerMatch indexVariable powerString) when (not (strContainsVariable powerString)) -> // This is an active pattern, which matches with inputs of the form n^I, where n is the index variable and a is Int type
            let power = int powerString
            let count = GlobalCounterModule.getCounter()
            GlobalCounterModule.incrementCounter() 
            setifySum(outputpath,indexVariable,(powerFold (int power) indexVariable),"power" + (powerString.Trim()) + "Sum" + (string count),lowerBound,upperBound)
        | (IndexVarGeometricMatch indexVariable powerString) when (not (strContainsVariable powerString)) -> // where n is the indexvariable n^expression, where expression does not contain a ascii character
            let power = int powerString
            let count = GlobalCounterModule.getCounter()
            GlobalCounterModule.incrementCounter() 
            setifySum(outputpath,indexVariable,(powerFold (int power) indexVariable),"geometric" + (powerString.Trim()) + "Sum" + (string count),lowerBound,upperBound)
        | (c: string) when c=indexVariable  -> // SimpleSum
                let count = GlobalCounterModule.getCounter()
                GlobalCounterModule.incrementCounter() 
                setifySum(outputpath,indexVariable,term.Trim(),"simpleSum" + (string count),lowerBound,upperBound)
        | c when System.Int32.TryParse(c,&intHolder)  ->  // Constant sum
            let count = GlobalCounterModule.getCounter()
            GlobalCounterModule.incrementCounter() 
            setifySum(outputpath,indexVariable,term.Trim(),"constantSum" + (string count),lowerBound,upperBound)
        | c when (indexVariable.Trim().Length=1) && not (strContainsSpecificVariable (c) (indexVariable.[0]))  ->  // Variable sum
            let count = GlobalCounterModule.getCounter()
            GlobalCounterModule.incrementCounter() 
            setifySum(outputpath,indexVariable,term.Trim(),"variableSum" + (string count),lowerBound,upperBound)
        | _ -> 
            printf "%s |" (term.Trim())
            let count: string = string (sprintf "%d" (GlobalCounterModule.getCounter() ))
            // genericSum(outputpath,indexVariable,innerFunc)
            GlobalCounterModule.incrementCounter()
            setifySum(outputpath,indexVariable,term,"genericSum" + (string count),lowerBound,upperBound)
            // sprintf "genericSum%s(%s, %s)" count lowerBound upperBound
    if isSet then 
        interpretTermSet innerFunc
    else interpretTerm innerFunc



    // | (AnyIntPowerMatch indexVariable powerString) -> // where n is not the indexvariable n^expression where expression can contain an ascii character
    //     let (power,powerVar) = powerString
    //     printf "here:%s" powerVar
    //     if isSet then 
    //         let count = GlobalCounterModule.getCounter()
    //         GlobalCounterModule.incrementCounter() 
    //         setifySum(outputpath,indexVariable,(powerFold (int power) powerVar),"power" + (power) + "SumAny" + (string count),lowerBound,upperBound) 
    //     else 
    //         sprintf "%s*(%s-%s)"  (powerFold (int power) powerVar) upperBound lowerBound




//This is the function which processes a line looking for instances of the custom notation. When it finds a sum it interprets the sum with the function above, and replaces the instance with something Viper can understand with the new domain 
let rec processLine (line: string) (summationPath)=
    let pattern = @"for ([a-z]+?)=(.*?) to (.*?) sum \((.*?)\)"
    let matchResult = Regex.Match(line, pattern)
    let setPattern = @"for ([a-z]+?)=(.*?) to (.*?) setsum \((.*?)\)"
    let matchSetResult = Regex.Match(line, setPattern)
    //If it matches with the "non-set" custom notation
    if matchResult.Success then
        //Here the regex groups are used to group the relevant information for the interpretation function
        let indexVariable = matchResult.Groups.[1].Value
        let lowerBound = matchResult.Groups.[2].Value
        let upperBound = matchResult.Groups.[3].Value
        let innerFunc = matchResult.Groups.[4].Value
        //Here the interpretation function is used
        let transformed = interpretSum indexVariable lowerBound upperBound innerFunc summationPath false
        let line = line.Replace(matchResult.Value, transformed)
        processLine line summationPath
    //If it matches with the SET custom notation
    elif matchSetResult.Success then
        //Here the regex groups are used to group the relevant information for the interpretation function
        let indexVariable = matchSetResult.Groups.[1].Value
        let lowerBound = matchSetResult.Groups.[2].Value
        let upperBound = matchSetResult.Groups.[3].Value
        let innerFunc = matchSetResult.Groups.[4].Value
        //Here the interpretation function is used where the isSet is true
        let transformed = interpretSum indexVariable lowerBound upperBound innerFunc summationPath true
        let line = line.Replace(matchSetResult.Value, transformed)
        processLine line summationPath 
    else
        line // Return the line unchanged if no match is found
    


// let readLines filePath = System.IO.File.ReadLines(filePath);;


// This function creates the outputs directory and writes the import statement in the beginning of the new file and starts copying the input file onto the output, creating changes using the processLine function
let writeStringsToFile (outputFilePath: string) (inputFilePath: string) =
    if not (Directory.Exists("./outputs")) then Directory.CreateDirectory("./outputs") |> ignore
    let strings = System.IO.File.ReadLines(inputFilePath)
    use outputWriter: StreamWriter = new StreamWriter(outputFilePath)
    let summationPath = Path.GetFileNameWithoutExtension(inputFilePath) + "_summationDomain.vpr"
    outputWriter.WriteLine("import \"" + summationPath + "\"" )
    File.Delete("./outputs/" + summationPath) |> ignore
    //Here it copies the inputfile onto the output file while processing it for potential changes to write
    strings |> Seq.iter (fun str -> outputWriter.WriteLine(processLine str summationPath))
    if not (File.Exists("./outputs/" + summationPath)) then File.Create("./outputs/" + summationPath) |> ignore

//Main function
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

