// This file implements a module where we define a data type "expr"
// to store represent arithmetic expressions
module AST


type Arithmetic =
    | Var of string
    | ArrayIndex of (string*Arithmetic) // A[a]
    | NumA of float
    | ParsA of (Arithmetic) // (a)
    | Times of (Arithmetic * Arithmetic)
    | Div of (Arithmetic * Arithmetic)
    | Plus of (Arithmetic * Arithmetic)
    | Minus of (Arithmetic * Arithmetic)
    | Pow of (Arithmetic * Arithmetic)
    | UPlus of (Arithmetic)
    | UMinus of (Arithmetic)

type BooleanExpr =
    | True // true
    | False// false 
    | BandB of BooleanExpr*BooleanExpr
    | BorB of BooleanExpr*BooleanExpr
    | BbitandB of BooleanExpr*BooleanExpr
    | BbitorB of BooleanExpr*BooleanExpr
    | Negation of (BooleanExpr) // ! b
    | Equals of (Arithmetic*Arithmetic) // a = a
    | NOTEquals of (Arithmetic*Arithmetic) // a != a
    | LargerThan of (Arithmetic*Arithmetic) // a > a
    | LargerOREqual of (Arithmetic*Arithmetic) // a >= a
    | LessThan of (Arithmetic*Arithmetic) // a < a
    | LessOREqual of (Arithmetic*Arithmetic) // a <= a
    | ParsB of (BooleanExpr) // (b)
    
type Command = 
                | Skip  // skip
                | Sequence of (Command*Command) // C ; 
                | GC of GuardedCommand
                | Assign of (string*Arithmetic)
                | AssignArray of (string*Arithmetic*Arithmetic)
                | IfFi of GuardedCommand
                | DoOd of GuardedCommand
                
and GuardedCommand = 
    | IfThen of (BooleanExpr * Command) // b -> C
    | ElseIf of (GuardedCommand * GuardedCommand) // GC [] GC





type expr =
    | Num of float
    | TimesExpr of (expr * expr)
    | DivExpr of (expr * expr)
    | PlusExpr of (expr * expr)
    | MinusExpr of (expr * expr)
    | PowExpr of (expr * expr)
    | UPlusExpr of (expr)
    | UMinusExpr of (expr)

