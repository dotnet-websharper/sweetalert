#r "nuget: FAKE.Core"
#r "nuget: Fake.Core.Target"
#r "nuget: Fake.IO.FileSystem"
#r "nuget: Fake.Tools.Git"
#r "nuget: Fake.DotNet.Cli"
#r "nuget: Fake.DotNet.AssemblyInfoFile"
#r "nuget: Fake.DotNet.Paket"
#r "nuget: Paket.Core"

open Fake.Core
let execContext = Context.FakeExecutionContext.Create false "build.fsx" []
Context.setExecutionContext (Context.RuntimeContext.Fake execContext)

#load "paket-files/wsbuild/github.com/dotnet-websharper/build-script/WebSharper.Fake.fsx"
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open WebSharper.Fake

let targets =
    WSTargets.Default (fun () -> GetSemVerOf "WebSharper" |> ComputeVersion)
    |> fun args ->
        { args with
            Attributes =
                    [
                        AssemblyInfo.Company "IntelliFactory"
                        AssemblyInfo.Copyright "(c) IntelliFactory 2023"
                        AssemblyInfo.Title "https://github.com/dotnet-websharper/sweetalert"
                        AssemblyInfo.Product "WebSharper SweetAlert 11.7+"
                    ]
        }    
    |> MakeTargets

Target.runOrDefault "Build"
