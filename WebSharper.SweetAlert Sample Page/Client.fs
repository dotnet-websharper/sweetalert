namespace WebSharper.SweetAlert_Sample_Page

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Templating
open WebSharper.SweetAlert

[<JavaScript>]
module Client =


    [<SPAEntryPoint>]
    let Main () =
        let Alert0= 
            SweetAlert.Options(
                TitleText = "Information",
                Text = "It works!",
                Type = "info",
                AllowOutsideClick = true,
                ShowCancelButton = true
            )
        let Alert1 =
            SweetAlert.Options(
                TitleText = "Click",
                Text = "You have clicked on the button!",
                Type = "success",
                ConfirmButtonText = "Cool",
                ConfirmButtonColor = "#000000"
            )

        let Alert2 =
            SweetAlert.Options(
                TitleText = "Input",
                Text = "Hello! Please say something",
                Type = "info",
                Input = "text"
            )
        WebSharper.SweetAlert.SweetAlert(Alert0)|>ignore
        let btn1 = 
            Doc.Button "Click me!" [] (fun () ->
                SweetAlert Alert1 |> ignore
            )
        let btn2 =
            Doc.Button "Input" [] (fun () ->
                SweetAlert Alert2 |> ignore
            )
        
        Doc.Concat[
            btn1
            btn2
        ]
        |> Doc.RunById "main"

        let asd a b c = a + b + c
        asd 1 2 3

