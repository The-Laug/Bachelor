module Graph

open Parse
open Types
open FSharp.Text.Lexing
open System
open AST

(*
    This defines the input and output for graphs. Please do not
    change the definitions below as they are needed for the validation and
    evaluation tools!
*)

type Input = { determinism: Determinism }

type Output = { dot: string }

type State = string

type Operation = Command

type Edge = { qstart : State ; op :Operation ; qend :State }


let edges (ast, qS , qF) :Edge List=
    match ast with 
    | Skip -> [{ qstart =qS ; op = ast ; qend =qF }]

let ast2pg ast =
    edges(ast,"qS","qF")


let op2dot (op:Operation) = 
    "[label=" + string (op) + "]"

let rec edge2dot (e : Edge) : string =
// 98 -> q1 [label = "skip");
    e.qstart + " -> " + e.qend + " " + op2dot(e.op) + ";"

let rec edges2dot (pg : Edge List) : string =
    match pg with
    | [] -> ""
    | e::pg -> edge2dot(e) + edges2dot (pg)

let pg2dot (pg:Edge List) =
    "digraph program_graph {rankdir=LR; " + edges2dot(pg) + "}"



let analysis (src: string) (input: Input) : Output =
    match parse Parser.startGCL (src) with
        | Ok ast ->
            // Console.Error.WriteLine("> {0}", ast)
            let pg = ast2pg(ast)
            let dotstring = pg2dot(pg)
            { dot = dotstring}
        | Error e -> {dot = ""}
    // failwith "Graph analysis not yet implemented" // TODO: start here
