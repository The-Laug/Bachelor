module Parse

open FSharp.Text.Lexing
open System
open AST

exception ParseError of Position * string * Exception

let parse parser src =
    let lexbuf = LexBuffer<char>.FromString src

    let parser = parser Lexer.tokenize

    try
        Ok(parser lexbuf)
    with
    | e ->
        let pos = lexbuf.EndPos
        let line = pos.Line
        let column = pos.Column
        let message = e.Message
        let lastToken = new String(lexbuf.Lexeme)
        eprintf "Parse failed at line %d, column %d:\n" line column
        eprintf "Last token: %s" lastToken
        eprintf "\n"
        Error(ParseError(pos, lastToken, e))

let rec prettyA a =
    match a with 
     | NumA(a) -> string a
     | Var(a) -> a
     | Plus(c1,c2) -> prettyA c1 + "+" + prettyA c2
     | Minus(c1,c2) -> prettyA c1 + "-" + prettyA c2
     | Times(c1,c2) -> prettyA c1 + "*" + prettyA c2
     | Div(c1,c2) -> prettyA c1 + "/" + prettyA c2
     | Pow(c1,c2) -> prettyA c1 + "^" + prettyA c2
     | UMinus(c1) -> "-" + prettyA c1
     | ParsA(c1) -> "(" + prettyA c1 + ")" 
     | ArrayIndex(c1,c2) -> c1 + "[" + prettyA c2 + "]"
     | _ -> failwith "ERROR with Arithmetic"
and prettyBool Bool =
    match Bool with 
     | True -> "TRUE"
     | False -> "FALSE"
     | BandB(b1,b2) -> prettyBool b1 + "&&" + prettyBool b2
     | BorB(b1,b2)  -> prettyBool b1 + "||" + prettyBool b2
     | BbitandB(b1,b2) -> prettyBool b1 + "&" + prettyBool b2
     | BbitorB(b1,b2) -> prettyBool b1 + "|" + prettyBool b2
     | Negation(b1)  -> "!" + prettyBool b1 
     | Equals(b1,b2)  -> prettyA b1 + "=" + prettyA b2
     | NOTEquals(b1,b2) -> prettyA b1 + "!=" + prettyA b2
     | LargerThan(b1,b2) -> prettyA b1 + ">" + prettyA b2
     | LargerOREqual(b1,b2) -> prettyA b1 + ">=" + prettyA b2
     | LessThan(b1,b2) -> prettyA b1 + "<" + prettyA b2
     | LessOREqual(b1,b2) -> prettyA b1 + "<=" + prettyA b2
     | ParsB(b1)  -> "(" + prettyBool b1 + ")"
     | _ -> failwith "ERROR with BooleanExpr"
and prettyGC G : string = 
    match G with 
     | IfThen(b,C) -> (prettyBool b) + "->" + (prettyPrint C) 
     | ElseIf(GC1,GC2) -> (prettyGC GC1) + "\n" + "[]" + "\n" + (prettyGC GC2)
     | _ -> failwith "ERROR with GuardedCommand"
and prettyPrint ast =
//    TODO: start here
   match ast with
     | Skip -> "skip"
     | Sequence(c1,c2) -> prettyPrint c1 + ";\n" + prettyPrint c2
     | GC(c1) -> (prettyGC c1)
     | Assign(c1,c2) -> c1 + ":=" + prettyA c2
     | AssignArray(c1,c2,c3) -> c1 + "[" + prettyA c2 + "]" + ":=" + prettyA c3
     | IfFi(c1) -> "if " + prettyGC c1 + "\n" + "fi"
     | DoOd(c1) -> "do " + prettyGC c1 + "\n" + "od"
     | _ -> failwith "ERROR with Command"
//    failwith "GCL parser not yet implemented"



let analysis (src: string) : string =
    match parse Parser.startGCL (src) with
        | Ok ast ->
            Console.Error.WriteLine("> {0}", ast)
            prettyPrint ast
        | Error e -> "Parse error: {0}"