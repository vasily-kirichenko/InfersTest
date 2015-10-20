#load "Scripts/load-references.fsx"

open PerfUtil
open Infers.Toys.Pretty

type IShowTester =
    inherit ITestable
    abstract Run : unit -> unit

let printfTest = 
    {
        new IShowTester with
            member x.Run() = sprintf "%A" (Some 1) |> ignore
            member x.Fini() = ()
            member x.Init() = ()
            member __.Name = "printf"
    }

let infersTest = 
    {
        new IShowTester with
            member x.Run() = show (Some 1) |> ignore
            member x.Fini() = ()
            member x.Init() = ()
            member __.Name = "infers"
    }

let tester = ImplementationComparer<IShowTester>(printfTest, [infersTest])
tester.Run (repeat 100 (fun x -> x.Run()), id = "test 0")
