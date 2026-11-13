// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2026 IntelliFactory
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
namespace WebSharper.SweetAlert.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.SweetAlert

[<JavaScript>]
module Client =

    // The template is the whole document body, loaded from the DOM: editing
    // index.html and refreshing the browser is enough.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

    // ------------------------------------------------------------------
    // State bound into the template
    // ------------------------------------------------------------------

    let themeStorageKey = "websharper-sweetalert-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"

    let status = Var.Create "Pick a demo — every option of the binding is wired up."
    let timerInfo = Var.Create "No popup timer is running."
    let apiInfo = Var.Create "Fire a popup, then inspect the live DOM."
    let eventLog = Var.Create "Lifecycle events will appear here."

    // The popup follows the page theme, which also demonstrates the `theme` option.
    let popupTheme () =
        if theme.Value = "dark" then SweetAlertTheme.Dark else SweetAlertTheme.Light

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <- if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    let log (line: string) =
        if eventLog.Value = "" then eventLog.Value <- line
        else eventLog.Value <- eventLog.Value + "\n" + line

    let clearLog () = eventLog.Value <- ""

    let eventName (e: SweetAlertEventName) = As<string> e

    /// Describes a `SweetAlertResult` in the status line.
    let describeResult (r: SweetAlertResult<'T>) =
        if r.IsConfirmed then "confirmed"
        elif r.IsDenied then "denied"
        elif r.IsDismissed then
            let reason =
                if (r.Dismiss :> obj) ===. JS.Undefined then "unknown"
                else As<string> r.Dismiss
            "dismissed (" + reason + ")"
        else "unknown"

    // The DOM getters return `null` while no popup is open.
    let describeEl (el: HTMLElement) =
        if isNull el then "null" else "yes (" + el.ClassName + ")"

    let describeButton (el: HTMLButtonElement) =
        if isNull el then "null" else "yes"

    let describeInput (el: HTMLInputElement) =
        if isNull el then "null" else "yes (value: " + el.Value + ")"

    let describeInt (o: Optional<int>) =
        match Optional.toOption o with
        | Some n -> string n
        | None -> "undefined"

    let describeBool (o: Optional<bool>) =
        match Optional.toOption o with
        | Some b -> string b
        | None -> "undefined"

    // ------------------------------------------------------------------
    // Basics
    // ------------------------------------------------------------------

    let simpleAlert () =
        promise {
            let! result =
                Swal.Fire<unit>(SweetAlertOptions(
                    TitleText = "Information",
                    Text = "It works!",
                    Icon = SweetAlertIcon.Info,
                    ShowCancelButton = true,
                    AllowOutsideClick = true
                ))
            status.Value <- "Simple alert: " + describeResult result
        }
        |> ignore

    let confirmDialog () =
        promise {
            let! result =
                Swal.Fire<unit>(SweetAlertOptions(
                    TitleText = "Save changes?",
                    Html = "You can <b>confirm</b>, <b>deny</b> or cancel this dialog.",
                    Icon = SweetAlertIcon.Question,
                    ShowDenyButton = true,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Save",
                    DenyButtonText = "Don't save",
                    CancelButtonText = "Cancel",
                    ReverseButtons = true,
                    Footer = "A footer rendered by SweetAlert2.",
                    CustomClass = SweetAlertCustomClass(
                        Popup = "demo-popup",
                        HtmlContainer = "demo-html-container"
                    )
                ))
            status.Value <- "Confirm dialog: " + describeResult result
        }
        |> ignore

    let inputDialog () =
        promise {
            let! result =
                Swal.Fire<string>(SweetAlertOptions(
                    TitleText = "Your name",
                    Text = "Hello! Please say something.",
                    Icon = SweetAlertIcon.Info,
                    Input = SweetAlertInput.Text,
                    InputPlaceholder = "At least 3 characters",
                    ShowCancelButton = true,
                    // Return a validation message, or an empty string to accept
                    // the value. Returning `false` (or a promise of it) also
                    // keeps the popup open.
                    InputValidator = (fun (value: obj) ->
                        let text = As<string> value
                        if isNull text || text.Trim().Length < 3 then
                            box "Please type at least 3 characters"
                        else
                            box "")
                ))
            match result with
            | r when r.IsConfirmed ->
                status.Value <- "Input: you typed \"" + r.Value + "\""
            | r when r.IsDismissed ->
                status.Value <- "Input: " + describeResult r
            | r ->
                status.Value <- "Input: " + describeResult r
        }
        |> ignore

    let shortForm () =
        promise {
            let! result = Swal.Fire<unit>("Short form", "Title, body and icon in one call.", SweetAlertIcon.Success)
            status.Value <- "Short form: " + describeResult result
        }
        |> ignore

    // ------------------------------------------------------------------
    // Toasts, timers and loaders
    // ------------------------------------------------------------------

    /// A reusable `Swal` instance with toast defaults baked in.
    let toast =
        Swal.Mixin(SweetAlertOptions(
            Toast = true,
            Position = SweetAlertPosition.TopEnd,
            Timer = 3000,
            TimerProgressBar = true,
            ShowConfirmButton = false
        ))

    let showToast () =
        toast.Fire<unit>(SweetAlertOptions(
            TitleText = "Saved",
            Text = "This toast came from a mixin instance.",
            Icon = SweetAlertIcon.Success
        ))
        |> ignore
        status.Value <- "Toast fired from the `Swal.Mixin` instance."

    let asyncLoader () =
        promise {
            let! result =
                Swal.Fire<unit>(SweetAlertOptions(
                    TitleText = "Async confirm",
                    Text = "Confirming runs a 1.5 second async `preConfirm`.",
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,
                    ShowLoaderOnConfirm = true,
                    PreConfirm = (fun (_: obj) ->
                        promise {
                            Swal.ShowLoading()
                            do! Async.Sleep 1500
                            Swal.HideLoading()
                            return box "loaded"
                        })
                ))
            status.Value <- "Async loader: " + describeResult result
        }
        |> ignore

    let stopTimer () =
        Swal.StopTimer() |> ignore
        timerInfo.Value <- "Timer stopped; " + describeInt (Swal.GetTimerLeft()) + " ms left."

    let resumeTimer () =
        Swal.ResumeTimer() |> ignore
        timerInfo.Value <- "Timer resumed; " + describeInt (Swal.GetTimerLeft()) + " ms left."

    let toggleTimer () =
        Swal.ToggleTimer() |> ignore
        timerInfo.Value <- "Timer toggled; " + describeInt (Swal.GetTimerLeft()) + " ms left."

    let increaseTimer () =
        Swal.IncreaseTimer(2000) |> ignore
        timerInfo.Value <- "Timer increased by 2s; " + describeInt (Swal.GetTimerLeft()) + " ms left."

    let startTimer () =
        Swal.Fire<unit>(SweetAlertOptions(
            TitleText = "Timer",
            Text = "This popup closes in 8 seconds.",
            Icon = SweetAlertIcon.Info,
            Timer = 8000,
            TimerProgressBar = true,
            ShowConfirmButton = false,
            // The backdrop is disabled so the timer controls above stay clickable
            // while the popup is open.
            Backdrop = false,
            Theme = popupTheme()
        ))
        |> ignore
        timerInfo.Value <- "Timer started; " + describeInt (Swal.GetTimerLeft()) + " ms left."

    // ------------------------------------------------------------------
    // Layout, themes and validation
    // ------------------------------------------------------------------

    let draggable () =
        Swal.Fire<unit>(SweetAlertOptions(
            TitleText = "Draggable",
            Text = "Drag this popup around by its body.",
            Icon = SweetAlertIcon.Info,
            Draggable = true,
            Grow = SweetAlertGrow.Row,
            Width = 34.0,
            Position = SweetAlertPosition.Center,
            Theme = popupTheme()
        ))
        |> ignore
        status.Value <- "Draggable popup fired."

    let themed () =
        Swal.Fire<unit>(SweetAlertOptions(
            TitleText = "Themed",
            Text = "This popup uses the `theme` option and custom CSS classes.",
            Icon = SweetAlertIcon.Warning,
            Theme = popupTheme(),
            CustomClass = SweetAlertCustomClass(
                Popup = "demo-popup",
                ConfirmButton = "btn btn-primary",
                CancelButton = "btn"
            ),
            ShowCancelButton = true
        ))
        |> ignore
        status.Value <- "Themed popup fired."

    let progressSteps () =
        Swal.Fire<string>(SweetAlertOptions(
            TitleText = "Progress steps",
            Text = "Step 2 of 3. Type a value to continue.",
            Icon = SweetAlertIcon.Info,
            Input = SweetAlertInput.Text,
            ProgressSteps = [| "Start"; "Details"; "Done" |],
            CurrentProgressStep = 1,
            ProgressStepsDistance = "2em",
            ShowCancelButton = true,
            Theme = popupTheme()
        ))
        |> ignore
        status.Value <- "Progress steps popup fired."

    let validation () =
        promise {
            let! result =
                Swal.Fire<string>(SweetAlertOptions(
                    TitleText = "Email",
                    Input = SweetAlertInput.Email,
                    InputPlaceholder = "name@example.com",
                    ValidationMessage = "That does not look like an e-mail address.",
                    InputAutoTrim = true,
                    ShowCancelButton = true
                ))
            status.Value <-
                if result.IsConfirmed then "Email: " + result.Value
                else "Email: " + describeResult result
        }
        |> ignore

    /// `inputOptions` and `inputAttributes` are typed as `obj`, so plain JS
    /// objects are built with small inlines.
    [<Inline "({ alpha: 'Alpha', beta: 'Beta', gamma: 'Gamma' })">]
    let selectOptions () : obj = X<obj>

    [<Inline "({ maxlength: '24' })">]
    let selectAttributes () : obj = X<obj>

    let imageDemo () =
        Swal.Fire<unit>(SweetAlertOptions(
            TitleText = "Image + top layer",
            Text = "An image, explicit show/hide classes and the top layer.",
            ImageUrl = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 160 100'%3E%3Crect width='160' height='100' rx='12' fill='%237066e0'/%3E%3Ccircle cx='80' cy='50' r='26' fill='white'/%3E%3C/svg%3E",
            ImageWidth = 160,
            ImageHeight = 100,
            ImageAlt = "A violet badge",
            TopLayer = true,
            Animation = true,
            ShowCloseButton = true,
            ShowClass = SweetAlertHideShowClass(
                Popup = "swal2-show",
                Backdrop = "swal2-backdrop-show",
                Icon = "swal2-icon-show"
            ),
            HideClass = SweetAlertHideShowClass(
                Popup = "swal2-hide",
                Backdrop = "swal2-backdrop-hide",
                Icon = "swal2-icon-hide"
            )
        ))
        |> ignore
        status.Value <- "Image + top layer popup fired."

    let selectDemo () =
        promise {
            let! result =
                Swal.Fire<string>(SweetAlertOptions(
                    TitleText = "Pick one",
                    Input = SweetAlertInput.Select,
                    InputOptions = selectOptions (),
                    InputAttributes = selectAttributes (),
                    ShowDenyButton = true,
                    ShowCancelButton = true,
                    ReturnInputValueOnDeny = true,
                    PreDeny = (fun (value: obj) ->
                        log ("preDeny: " + As<string> value)
                        box value)
                ))
            status.Value <-
                "Select: " + describeResult result
                + (if result.IsConfirmed || result.IsDenied then " (" + result.Value + ")" else "")
        }
        |> ignore

    // ------------------------------------------------------------------
    // Reading and mutating the live popup
    // ------------------------------------------------------------------

    let updatePopup () =
        Swal.Fire<unit>(SweetAlertOptions(
            TitleText = "Initial title",
            Text = "This popup is updated right after it opens.",
            Icon = SweetAlertIcon.Info,
            ShowCancelButton = true,
            // No backdrop, so the "Close" button above remains clickable.
            Backdrop = false,
            DidRender = fun _ -> log "didRender (before update)"
        ))
        |> ignore

        Swal.Update(SweetAlertUpdatableParameters(
            TitleText = "Updated title",
            Text = "The title, text and icon were changed with `Swal.Update`.",
            Icon = SweetAlertIcon.Success,
            ShowCancelButton = false
        ))

        let title = Swal.GetTitle()
        apiInfo.Value <-
            "isVisible=" + string (Swal.IsVisible())
            + " · title=" + (if isNull title then "null" else title.InnerText)

    let getters () =
        Swal.Fire<unit>(SweetAlertOptions(
            TitleText = "Inspect the DOM",
            Text = "The getters read the live popup.",
            Icon = SweetAlertIcon.Question,
            Input = SweetAlertInput.Text,
            InputValue = "sample value",
            ShowCancelButton = true,
            ShowCloseButton = true,
            DidOpen = fun _ ->
                let focusable = Swal.GetFocusableElements()
                apiInfo.Value <-
                    "container=" + describeEl (Swal.GetContainer())
                    + " · popup=" + describeEl (Swal.GetPopup())
                    + " · title=" + describeEl (Swal.GetTitle())
                    + " · html=" + describeEl (Swal.GetHtmlContainer())
                    + " · icon=" + describeEl (Swal.GetIcon())
                    + " · iconContent=" + describeEl (Swal.GetIconContent())
                    + " · actions=" + describeEl (Swal.GetActions())
                    + " · footer=" + describeEl (Swal.GetFooter())
                    + " · timerBar=" + describeEl (Swal.GetTimerProgressBar())
                    + " · progressSteps=" + describeEl (Swal.GetProgressSteps())
                    + " · image=" + describeEl (Swal.GetImage())
                    + " · validation=" + describeEl (Swal.GetValidationMessage())
                    + " · close=" + describeButton (Swal.GetCloseButton())
                    + " · confirm=" + describeButton (Swal.GetConfirmButton())
                    + " · deny=" + describeButton (Swal.GetDenyButton())
                    + " · cancel=" + describeButton (Swal.GetCancelButton())
                    + " · input=" + describeInput (Swal.GetInput())
                    + " · focusable=" + string focusable.Length
                    + " · loading=" + string (Swal.IsLoading())
                    + " · timer=" + describeInt (Swal.GetTimerLeft())
                    + " · timerRunning=" + describeBool (Swal.IsTimerRunning())
        ))
        |> ignore
        status.Value <- "Getters ran while the popup was open."

    let toggleInput () =
        Swal.Fire<string>(SweetAlertOptions(
            TitleText = "Toggle the input",
            Input = SweetAlertInput.Text,
            InputValue = "editable",
            ShowCancelButton = true
        ))
        |> ignore
        Swal.DisableInput()
        apiInfo.Value <- "Input disabled; it is enabled again in 2 seconds."
        JS.SetTimeout (fun () ->
            Swal.EnableInput()
            Swal.ShowValidationMessage("The input is enabled again.")
            apiInfo.Value <- "Input enabled; a validation message is shown."
        ) 2000
        |> ignore

    let clickButtons () =
        Swal.Fire<unit>(SweetAlertOptions(
            TitleText = "Programmatic clicks",
            Text = "The confirm button is clicked after one second.",
            Icon = SweetAlertIcon.Info,
            ShowDenyButton = true,
            ShowCancelButton = true,
            DidOpen = fun _ ->
                JS.SetTimeout (fun () -> Swal.ClickConfirm()) 1000 |> ignore
        ))
        |> ignore
        status.Value <- "clickConfirm() is scheduled in 1 second."

    let closePopup () =
        Swal.Close()
        status.Value <- "Swal.Close() called."

    // ------------------------------------------------------------------
    // Declarative templates and parameter queries
    // ------------------------------------------------------------------

    /// Creates an HTML5 `<template>` element at runtime. Template elements are
    /// created here rather than in index.html because WebSharper.UI uses
    /// `template[id]` elements as nested templates while preparing the page.
    let createTemplate (id: string) (html: string) =
        let t = JS.Document.CreateElement("template") :?> HTMLTemplateElement
        t.Id <- id
        t.InnerHTML <- html
        JS.Document.Body.AppendChild(t) |> ignore

    let templateOption () =
        Swal.Fire<unit>(SweetAlertOptions(Template = "#option-template"))
        |> ignore
        status.Value <- "Popup rendered from the `template` option."

    let parameterQueries () =
        let parsed = Swal.ArgsToParams([| "Parsed title"; "Parsed body"; "success" |])
        apiInfo.Value <-
            "isValidParameter('theme')=" + string (Swal.IsValidParameter("theme"))
            + " · isValidParameter('nope')=" + string (Swal.IsValidParameter("nope"))
            + " · isUpdatableParameter('icon')=" + string (Swal.IsUpdatableParameter("icon"))
            + " · isUpdatableParameter('timer')=" + string (Swal.IsUpdatableParameter("timer"))
            + " · argsToParams title=" + As<string> parsed.Title
            + " · argsToParams icon=" + As<string> parsed.Icon

    // ------------------------------------------------------------------
    // Lifecycle events
    // ------------------------------------------------------------------

    let lifecycleEvents = [
        SweetAlertEventName.WillOpen
        SweetAlertEventName.DidOpen
        SweetAlertEventName.DidRender
        SweetAlertEventName.WillClose
        SweetAlertEventName.DidClose
        SweetAlertEventName.DidDestroy
    ]

    let lifecycleOn () =
        clearLog ()
        for ev in lifecycleEvents do
            let name = eventName ev
            Swal.On(ev, fun () -> log ("on " + name))
        log "Subscribed to all lifecycle events."

    let lifecycleOnce () =
        Swal.Once(SweetAlertEventName.DidOpen, fun () -> log "once didOpen")
        log "Subscribed once to didOpen."

    let lifecycleOff () =
        Swal.Off()
        log "Removed all lifecycle event handlers."

    // ------------------------------------------------------------------
    // Entry point
    // ------------------------------------------------------------------

    [<SPAEntryPoint>]
    let Main () =
        let savedTheme =
            try JS.Window.LocalStorage.GetItem(themeStorageKey) with _ -> null
        if savedTheme = "dark" || savedTheme = "light" then
            theme.Value <- savedTheme
        applyTheme ()

        // `.Bind()` renders the whole document body, so the elements used by
        // the demos only exist after this call.
        IndexTemplate()
            .themeLabel(themeLabel.V)
            .status(status.V)
            .timerInfo(timerInfo.V)
            .apiInfo(apiInfo.V)
            .eventLog(eventLog.V)
            .version(Swal.Version)
            .ToggleTheme(fun _ -> toggleTheme ())
            .SimpleAlert(fun _ -> simpleAlert ())
            .ConfirmDialog(fun _ -> confirmDialog ())
            .InputDialog(fun _ -> inputDialog ())
            .ShortForm(fun _ -> shortForm ())
            .Toast(fun _ -> showToast ())
            .AsyncLoader(fun _ -> asyncLoader ())
            .TimerStart(fun _ -> startTimer ())
            .TimerStop(fun _ -> stopTimer ())
            .TimerResume(fun _ -> resumeTimer ())
            .TimerToggle(fun _ -> toggleTimer ())
            .TimerIncrease(fun _ -> increaseTimer ())
            .Draggable(fun _ -> draggable ())
            .Themed(fun _ -> themed ())
            .ProgressSteps(fun _ -> progressSteps ())
            .Validation(fun _ -> validation ())
            .ImageDemo(fun _ -> imageDemo ())
            .SelectDemo(fun _ -> selectDemo ())
            .UpdatePopup(fun _ -> updatePopup ())
            .Getters(fun _ -> getters ())
            .ToggleInput(fun _ -> toggleInput ())
            .ClickButtons(fun _ -> clickButtons ())
            .ClosePopup(fun _ -> closePopup ())
            .TemplateOption(fun _ -> templateOption ())
            .ParameterQueries(fun _ -> parameterQueries ())
            .LifecycleOn(fun _ -> lifecycleOn ())
            .LifecycleOnce(fun _ -> lifecycleOnce ())
            .LifecycleOff(fun _ -> lifecycleOff ())
            .ClearLog(fun _ -> clearLog ())
            .Bind()

        // Template elements used by the declarative demos are created at
        // runtime, after the page template has been bound.
        createTemplate "declarative-template"
            """<swal-title>Declarative</swal-title>
<swal-html>This popup comes from a <code>&lt;template&gt;</code> element.</swal-html>
<swal-icon type="success"></swal-icon>
<swal-button type="confirm">Nice</swal-button>
<swal-button type="cancel">No, thanks</swal-button>
<swal-param name="showCancelButton" value="true"></swal-param>
<swal-footer>Rendered by SweetAlert2 from the markup.</swal-footer>"""
        createTemplate "option-template"
            """<swal-title>From the <code>template</code> option</swal-title>
<swal-html>Passed to <code>Swal.Fire</code> as <code>Template = "#option-template"</code>.</swal-html>
<swal-icon type="info"></swal-icon>
<swal-button type="confirm">Got it</swal-button>"""

        // The declarative trigger lives in the bound template.
        Swal.BindClickHandler()