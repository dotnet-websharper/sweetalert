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
namespace WebSharper.SweetAlert

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    /// Reasons that can explain a dismissal. Also available at runtime as
    /// `Swal.DismissReason`, which is what these values represent.
    let DismissReason = Pattern.EnumStrings "DismissReason" [
        "cancel"
        "backdrop"
        "close"
        "esc"
        "timer"
    ]

    /// The icons built into SweetAlert2.
    let SweetAlertIcon = Pattern.EnumStrings "SweetAlertIcon" [
        "success"
        "error"
        "warning"
        "info"
        "question"
    ]

    /// The input field types supported by the `input` option.
    let SweetAlertInput =
        Pattern.EnumInlines "SweetAlertInput" [
            "Text", "'text'"
            "Email", "'email'"
            "Password", "'password'"
            "Number", "'number'"
            "Tel", "'tel'"
            "Search", "'search'"
            "Range", "'range'"
            "Textarea", "'textarea'"
            "Select", "'select'"
            "Radio", "'radio'"
            "Checkbox", "'checkbox'"
            "File", "'file'"
            "Url", "'url'"
            "Date", "'date'"
            "DateTimeLocal", "'datetime-local'"
            "Time", "'time'"
            "Week", "'week'"
            "Month", "'month'"
        ]

    /// The position of the popup on the screen.
    let SweetAlertPosition =
        Pattern.EnumInlines "SweetAlertPosition" [
            "Top", "'top'"
            "TopStart", "'top-start'"
            "TopEnd", "'top-end'"
            "TopLeft", "'top-left'"
            "TopRight", "'top-right'"
            "Center", "'center'"
            "CenterStart", "'center-start'"
            "CenterEnd", "'center-end'"
            "CenterLeft", "'center-left'"
            "CenterRight", "'center-right'"
            "Bottom", "'bottom'"
            "BottomStart", "'bottom-start'"
            "BottomEnd", "'bottom-end'"
            "BottomLeft", "'bottom-left'"
            "BottomRight", "'bottom-right'"
        ]

    /// The theme of the popup. The `Light`, `Dark` and `Auto` themes are
    /// included in the core stylesheet; the named themes need the matching
    /// `sweetalert2/themes/<theme>.css` stylesheet to be loaded.
    let SweetAlertTheme =
        Pattern.EnumInlines "SweetAlertTheme" [
            "Light", "'light'"
            "Dark", "'dark'"
            "Auto", "'auto'"
            "Minimal", "'minimal'"
            "Borderless", "'borderless'"
            "Bootstrap4", "'bootstrap-4'"
            "Bootstrap4Light", "'bootstrap-4-light'"
            "Bootstrap4Dark", "'bootstrap-4-dark'"
            "Bootstrap5", "'bootstrap-5'"
            "Bootstrap5Light", "'bootstrap-5-light'"
            "Bootstrap5Dark", "'bootstrap-5-dark'"
            "MaterialUi", "'material-ui'"
            "MaterialUiLight", "'material-ui-light'"
            "MaterialUiDark", "'material-ui-dark'"
            "EmbedIframe", "'embed-iframe'"
            "Bulma", "'bulma'"
            "BulmaLight", "'bulma-light'"
            "BulmaDark", "'bulma-dark'"
        ]

    /// The grow direction of the popup, or `False` to not grow.
    let SweetAlertGrow =
        Pattern.EnumInlines "SweetAlertGrow" [
            "Row", "'row'"
            "Column", "'column'"
            "Fullscreen", "'fullscreen'"
            "False", "false"
        ]

    /// The lifecycle events that can be subscribed to with `Swal.On`,
    /// `Swal.Once` and `Swal.Off`.
    let SweetAlertEventName = Pattern.EnumStrings "SweetAlertEventName" [
        "didRender"
        "willOpen"
        "didOpen"
        "willClose"
        "didClose"
        "didDestroy"
    ]

    /// CSS classes applied to the parts of the popup.
    let SweetAlertCustomClass = Pattern.Config "SweetAlertCustomClass" {
        Required = []
        Optional = [
            "container", T<string> + T<string[]>
            "popup", T<string> + T<string[]>
            "title", T<string> + T<string[]>
            "closeButton", T<string> + T<string[]>
            "icon", T<string> + T<string[]>
            "image", T<string> + T<string[]>
            "htmlContainer", T<string> + T<string[]>
            "input", T<string> + T<string[]>
            "inputLabel", T<string> + T<string[]>
            "validationMessage", T<string> + T<string[]>
            "actions", T<string> + T<string[]>
            "confirmButton", T<string> + T<string[]>
            "denyButton", T<string> + T<string[]>
            "cancelButton", T<string> + T<string[]>
            "loader", T<string> + T<string[]>
            "footer", T<string> + T<string[]>
            "timerProgressBar", T<string> + T<string[]>
        ]
    }

    /// The CSS classes used for the show/hide animations.
    let SweetAlertHideShowClass = Pattern.Config "SweetAlertHideShowClass" {
        Required = []
        Optional = [
            "backdrop", T<string> + T<string[]>
            "icon", T<string> + T<string[]>
            "popup", T<string> + T<string[]>
        ]
    }

    /// The value resolved by `Swal.Fire`.
    let SweetAlertResult = Generic - fun t ->
        Pattern.Config "SweetAlertResult" {
            Required = [
                "isConfirmed", T<bool>
                "isDenied", T<bool>
                "isDismissed", T<bool>
            ]
            Optional = [
                "value", t.Type
                "dismiss", DismissReason.Type
            ]
        }

    /// The partial result accepted by `Swal.Close`.
    let SweetAlertPartialResult = Generic - fun t ->
        Pattern.Config "SweetAlertPartialResult" {
            Required = []
            Optional = [
                "isConfirmed", T<bool>
                "isDenied", T<bool>
                "isDismissed", T<bool>
                "value", t.Type
                "dismiss", DismissReason.Type
            ]
        }

    /// A value that can also be computed lazily on access.
    let ValueOrThunk (t: Type.Type) = t + (T<unit> ^-> t)
    /// A value or a promise of it.
    let SyncOrAsync (t: Type.Type) = t + T<Promise<_>>[t]

    /// The type of the callbacks attached to lifecycle events.
    let EventHandler = T<unit> ^-> T<unit>

    let inputType = T<string> + T<int> + T<File> + T<FileList>

    let updatableParameters = [
        "allowEscapeKey", ValueOrThunk T<bool>
        "allowOutsideClick", ValueOrThunk T<bool>
        "background", T<string>
        "buttonsStyling", T<bool>
        "cancelButtonAriaLabel", T<string>
        "cancelButtonColor", T<string>
        "cancelButtonText", T<string>
        "closeButtonAriaLabel", T<string>
        "closeButtonHtml", T<string>
        "confirmButtonAriaLabel", T<string>
        "confirmButtonColor", T<string>
        "confirmButtonText", T<string>
        "currentProgressStep", !? T<int>
        "customClass", SweetAlertCustomClass.Type + T<string>
        "denyButtonAriaLabel", T<string>
        "denyButtonColor", T<string>
        "denyButtonText", T<string>
        "didClose", EventHandler
        "didDestroy", EventHandler
        "footer", T<string> + T<HTMLElement>
        "hideClass", SweetAlertHideShowClass.Type
        "html", T<string> + T<HTMLElement>
        "icon", SweetAlertIcon.Type
        "iconColor", T<string>
        "imageAlt", T<string>
        "imageHeight", T<int> + T<string>
        "imageUrl", T<string>
        "imageWidth", T<int> + T<string>
        "preConfirm", T<obj> ^-> T<obj>
        "preDeny", T<obj> ^-> T<obj>
        "progressSteps", T<string[]>
        "reverseButtons", T<bool>
        "showCancelButton", T<bool>
        "showCloseButton", T<bool>
        "showConfirmButton", T<bool>
        "showDenyButton", T<bool>
        "text", T<string>
        "theme", SweetAlertTheme.Type
        "title", T<string> + T<HTMLElement>
        "titleText", T<string>
        "willClose", T<HTMLElement> ^-> T<unit>
    ]

    /// The options that can be passed to `Swal.Update`.
    let SweetAlertUpdatableParameters = Pattern.Config "SweetAlertUpdatableParameters" {
        Required = []
        Optional = updatableParameters
    }

    /// All the options accepted by `Swal.Fire`.
    let SweetAlertOptions =
        Pattern.ConfigObs "SweetAlertOptions" {
            Required = []
            Optional = [
                // Content
                "iconHtml", T<string>
                "template", T<string> + T<HTMLTemplateElement>

                // Layout
                "position", SweetAlertPosition.Type
                "grow", SweetAlertGrow.Type
                "width", T<double> + T<string>
                "padding", T<double> + T<string>
                "color", T<string>
                "backdrop", T<bool> + T<string>
                "target", T<string> + T<HTMLElement>
                "heightAuto", T<bool>
                "scrollbarPadding", T<bool>
                "topLayer", T<bool>

                // Behaviour
                "toast", T<bool>
                "draggable", T<bool>
                "animation", T<bool>
                "timer", T<int>
                "timerProgressBar", T<bool>
                "stopKeydownPropagation", T<bool>
                "keydownListenerCapture", T<bool>

                // Buttons
                "focusConfirm", T<bool>
                "focusDeny", T<bool>
                "focusCancel", T<bool>
                "returnFocus", T<bool>
                "loaderHtml", T<string>
                "showLoaderOnConfirm", T<bool>
                "showLoaderOnDeny", T<bool>

                // Input
                "input", SweetAlertInput.Type
                "inputLabel", T<string>
                "inputPlaceholder", T<string>
                "inputValue", SyncOrAsync inputType
                "inputOptions", SyncOrAsync T<obj>
                "inputAutoFocus", T<bool>
                "inputAutoTrim", T<bool>
                "inputAttributes", T<obj>
                "inputValidator", T<obj> ^-> T<obj>
                "returnInputValueOnDeny", T<bool>
                "validationMessage", T<string>

                // Progress steps
                "progressStepsDistance", T<int> + T<string>

                // Lifecycle hooks
                "willOpen", T<HTMLElement> ^-> T<unit>
                "didOpen", T<HTMLElement> ^-> T<unit>
                "didRender", T<HTMLElement> ^-> T<unit>

                // Styling
                "showClass", SweetAlertHideShowClass.Type

                // Everything that `Swal.Update` can also change.
                yield! updatableParameters
            ]
            Obsolete = [
                "allowEnterKey", ValueOrThunk T<bool>
            ]
        }

    /// The members of the `Swal` API. They are static on the imported default
    /// export and instance members on the object returned by `Swal.Mixin`.
    /// `mixinReturn` is the type that `mixin` resolves to (`TSelf` for the
    /// instance type, `SweetAlertInstance` for the static one).
    let swalApi (mixinReturn: Type.Type) : list<CodeModel.IClassMember> = [
        Generic - fun t ->
            "fire" => SweetAlertOptions.Type ^-> T<Promise<_>>[SweetAlertResult[t]]
        Generic - fun t ->
            "fire" => T<string>?title ^-> T<Promise<_>>[SweetAlertResult[t]]
        Generic - fun t ->
            "fire" => T<string>?title * T<string>?html ^-> T<Promise<_>>[SweetAlertResult[t]]
        Generic - fun t ->
            "fire" => T<string>?title * T<string>?html * SweetAlertIcon.Type?icon ^-> T<Promise<_>>[SweetAlertResult[t]]
            |> WithComment "Shows a popup with the given title, HTML body and icon."

        "mixin" => SweetAlertOptions.Type ^-> mixinReturn
        |> WithComment "Creates a reusable `Swal` instance with the given default options."

        "isVisible" => T<unit> ^-> T<bool>
        "update" => SweetAlertUpdatableParameters.Type ^-> T<unit>
        "close" => T<unit> ^-> T<unit>
        Generic - fun t ->
            "close" => SweetAlertPartialResult[t] ^-> T<unit>
            |> WithComment "Closes the popup, resolving its promise with the given partial result."

        let get (name: string) (t: Type.Type) =
            $"get{name}" => T<unit> ^-> t

        // These getters return `HTMLElement | null`: a plain type that is
        // `null` while no popup is open, so check it with `isNull`.
        for n in [
            "Container"; "Popup"; "Title"; "ProgressSteps"; "HtmlContainer"; "Image"
            "Icon"; "IconContent"; "Actions"; "Footer"; "TimerProgressBar"; "ValidationMessage"
        ] do
            (get n T<HTMLElement>) :> CodeModel.IClassMember

        (get "CloseButton" T<HTMLButtonElement>) :> CodeModel.IClassMember
        (get "ConfirmButton" T<HTMLButtonElement>) :> CodeModel.IClassMember
        (get "DenyButton" T<HTMLButtonElement>) :> CodeModel.IClassMember
        (get "CancelButton" T<HTMLButtonElement>) :> CodeModel.IClassMember
        (get "Input" T<HTMLInputElement>) :> CodeModel.IClassMember

        get "FocusableElements" !| T<HTMLElement>

        for n in [
            "enableButtons"
            "disableButtons"
            "hideLoading"
            "clickConfirm"
            "clickDeny"
            "clickCancel"
            "disableInput"
            "enableInput"
            "resetValidationMessage"
        ] do
            (n => T<unit> ^-> T<unit>) :> CodeModel.IClassMember

        "isLoading" => T<unit> ^-> T<bool>
        "showLoading" => T<unit> ^-> T<unit>
        "showLoading" => T<HTMLButtonElement> ^-> T<unit>
        |> WithComment "Shows the loader, replacing the given button (the confirm button by default)."

        "showValidationMessage" => T<string> ^-> T<unit>

        for n in [
            "getTimerLeft"
            "stopTimer"
            "resumeTimer"
            "toggleTimer"
        ] do
            (n => T<unit> ^-> !? T<int>) :> CodeModel.IClassMember

        "increaseTimer" => T<int>?n ^-> !? T<int>
        "isTimerRunning" => T<unit> ^-> !? T<bool>

        "bindClickHandler" => T<unit> ^-> T<unit>
        "bindClickHandler" => T<string> ^-> T<unit>
        |> WithComment "Allows triggering popups declaratively with `data-swal-template` attributes."

        "isValidParameter" => T<string> ^-> T<bool>
        "isUpdatableParameter" => T<string> ^-> T<bool>

        "argsToParams" => ((!| T<string>) + SweetAlertOptions.Type) ^-> SweetAlertOptions.Type
        |> WithComment "Normalizes the arguments of `Swal.Fire` into an options object."

        "on" => SweetAlertEventName.Type * EventHandler ^-> T<unit>
        |> WithComment "Attaches a handler to a lifecycle event."
        "once" => SweetAlertEventName.Type * EventHandler ^-> T<unit>
        |> WithComment "Attaches a handler to a lifecycle event, running it at most once."
        "off" => T<unit> ^-> T<unit>
        "off" => SweetAlertEventName.Type ^-> T<unit>
        "off" => SweetAlertEventName.Type * EventHandler ^-> T<unit>
        |> WithComment "Removes lifecycle event handlers."

        "version" =? T<string>
    ]

    /// An instance created by `Swal.Mixin`: the same API with default options
    /// baked in.
    let SweetAlertInstance : CodeModel.Class =
        Class "SweetAlertInstance"
        |+> Instance (swalApi TSelf)

    /// The default export of `sweetalert2`.
    let Swal : CodeModel.Class =
        Class "Swal"
        |+> Static (swalApi SweetAlertInstance.Type)
        |> ImportDefault "sweetalert2"

    let Assembly =
        Assembly [
            Namespace "WebSharper.SweetAlert" [
                DismissReason
                SweetAlertIcon
                SweetAlertInput
                SweetAlertPosition
                SweetAlertTheme
                SweetAlertGrow
                SweetAlertEventName
                SweetAlertCustomClass
                SweetAlertHideShowClass
                SweetAlertResult
                SweetAlertPartialResult
                SweetAlertUpdatableParameters
                SweetAlertOptions
                SweetAlertInstance
                Swal
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()