

namespace simpleSumNameSpace
open System.IO
open System
open System.Text.RegularExpressions

module simpleSumModule =
    let simpleSumDomain (filepath)  =
        // Add simplesum domain to the summationDomain.vpr file
        use writer: StreamWriter = File.AppendText(filepath)

        if not (domainExists ("domain simpleSums") (filepath)) then
            writer.WriteLine("
            domain simpleSums {
            // uninterpreted function
            function simplesum(i: Int, N: Int): Int

            axiom emptySum{
                simplesum(0,0) == 0
            }
            
            axiom iGreaterThanN {
                forall i:Int, N:Int::
                    i > N ==> simplesum(i,N) == 0
            }

            axiom rightRecursion {
                forall i:Int, N: Int :: 
                    0 <= i < N ==> simplesum(i,N) == simplesum(i,N-1) + N
            }

            axiom leftRecursion {
                forall i:Int, N: Int :: 
                    0 <= i < N ==> simplesum(i,N) == i + simplesum(i+1,N)
            }
            

            }")