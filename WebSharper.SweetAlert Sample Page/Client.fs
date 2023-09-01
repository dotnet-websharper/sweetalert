// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace WebSharper.SweetAlert_Sample_Page

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation
open WebSharper.UI.Templating
open WebSharper.SweetAlert

[<JavaScript>]
module Client =

    [<SPAEntryPoint>]
    let Main () =
        let Alert0= 
            SweetAlert.Box(
                TitleText = "Information",
                Text = "It works!",
                Type = "info",
                AllowOutsideClick = true,
                ShowCancelButton = true
            )
        let Alert1 =
            SweetAlert.Box(
                TitleText = "Click",
                Text = "You have clicked on the button!",
                Type = "success",
                ConfirmButtonText = "Cool",
                ConfirmButtonColor = "#000000"
            )

        let Alert2 =
            SweetAlert.Box(
                TitleText = "Input",
                Text = "Hello! Please say something",
                Type = "info",
                Input = "text"
            )
        SweetAlert.SetDefaults Alert0
        SweetAlert.ShowBox Alert0 |> ignore
        let btn1 = 
            Doc.Button "Click me!" [] (fun () ->
                SweetAlert.ShowBox Alert1 |> ignore
            )
        let rResult = Var.Create ""


        let btn2 =
            Doc.Button "Input" [] (fun () ->
  //              (SweetAlert.ShowBox Alert2).Then(fun r -> 
  //                  let Alert = Box(Text = "You have wrote: "+string(r), TitleText = "Result")
  //                  Console.Log r
  //                  SweetAlert.ShowBox(Alert)|>ignore) |> ignore
   
                SweetAlert.ShowBox(Alert2).Then(fun x -> rResult := x)|> ignore
  //              SweetAlert.Close()
  //              SweetAlert.ShowBox(Alert2).State() |> Console.Log
  //              SweetAlert.ShowBox(Alert2).Then (fun result -> Console.Log result) |> ignore
  //              SweetAlert.Then(SweetAlert(Alert2)) |> Console.Log
                
  //              rValue.Value <- SweetAlert.Then(SweetAlert.ShowBox(Alert2))
  //              SweetAlert.GetInput |> Console.Log


            )

        Doc.Concat[
            btn1
            br[][]
            btn2
            h2[][text "The last input was: "]
            p[][textView rResult.View]
        ]
        |> Doc.RunById "main"

