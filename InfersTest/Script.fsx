#load "Scripts/load-references.fsx"

open PerfUtil
open Infers.Toys.Pretty

type IShowTester =
    inherit ITestable
    abstract Run : unit -> unit

type R<'a> = { S: string; A: 'a list }
let r = { S = "foo"; A = [[| Some 1; None |]] }

let printfTest = 
    {
        new IShowTester with
            member __.Run() = 
                for _ in 1..500 do sprintf "%A" r |> ignore
            member __.Fini() = ()
            member __.Init() = ()
            member __.Name = "printf %A"
    }

let infersTest = 
    {
        new IShowTester with
            member __.Run() = for _ in 1..500 do show r |> ignore
            member __.Fini() = ()
            member __.Init() = ()
            member __.Name = "Infers"
    }

let tester = ImplementationComparer<IShowTester>(infersTest, [printfTest], verbose = true, warmup = true)
tester.Run (repeat 10 (fun x -> x.Run()), id = "test")
