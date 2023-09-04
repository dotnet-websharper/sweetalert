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
open WebSharper.SweetAlert

[<JavaScript>]
module Client =
    module Helpers =
        let (|IsConfirmed|IsDenied|IsDismissed|) (res: SweetAlertResult<'T>) =
            if res.IsConfirmed then IsConfirmed(res.Value)
            else if res.IsDismissed then IsDismissed(res.Dismiss)
            else if res.IsDenied then IsDenied
            else failwith "SweetAlert JS failure: Result not set as confirmed, dimissed or denied"

    open Helpers

    [<SPAEntryPoint>]
    let Main () =
        let Alert0 () =
            Swal.Fire<unit>(SweetAlertOptions(
                TitleText = "Information",
                Text = "It works!",
                Icon = SweetAlertIcon.Info,
                AllowOutsideClick = true,
                ShowCancelButton = true
            ))
        let Alert1 () =
            Swal.Fire<unit>(SweetAlertOptions(
                TitleText = "Click",
                Text = "You have clicked on the button!",
                Icon = SweetAlertIcon.Success,
                ConfirmButtonText = "Cool",
                ConfirmButtonColor = "#000000"
            ))
        let InputAlert () =
            Swal.Fire<string>(SweetAlertOptions(
                TitleText = "Input",
                Text = "Hello! Please say something",
                Icon = SweetAlertIcon.Info,
                Input = SweetAlertInput.Text
            ))

        Alert0() |> ignore
        let btn1 = 
            Doc.Button "Click me!" [] (fun () ->
                Alert1() |> ignore
            )
        let rResult = Var.Create ""


        let btn2 =
            
            Doc.Button "Input" [] (fun () ->
                
                promise {
                    let! result = InputAlert()
                    Console.Log result

                    return! 
                        match result with 
                        | IsConfirmed v ->
                                rResult.Set v
                                ("Result", $"You have wrote: {v}")
                        | IsDismissed reason -> 
                                ("Dismissed", $"Dismissed with reason: {reason}")
                        | IsDenied -> 
                                ("Denied", "Dialog denied.")
                        |> Swal.Fire
                } |> ignore
                ()

            )

        Doc.Concat[
            btn1
            br [] []
            btn2
            h2 [] [text "The last input was: "]
            p [] [textView rResult.View]
        ]
        |> Doc.RunById "main"

