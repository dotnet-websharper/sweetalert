namespace WebSharper.SweetAlert

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator
open WebSharper.JQuery

module Definition =
    let SweetAlert = Class "sweetAlert"
    let Options =
        Pattern.Config "Options"{
            Required = []
            Optional =
            [
                "title", T<string>
                "titleText", T<string>
                "text", T<string>
                "html", T<string>
                "type", T<string>
                "target", T<string>
                "input", T<string>
                "width", T<string>
                "padding", T<int>
                "background", T<string>
                "customClass", T<string>
                "timer", T<int>
                "animation", T<bool>
                "allowOutsideClick", T<bool>
                "allowEscapeKey", T<bool>
                "allowEnterKey", T<bool>
                "showConfirmButton", T<bool>
                "showCancelButton", T<bool>
                "confirmButtonText", T<string>
                "cancelButtonText", T<string>
                "confirmButtonColor", T<string>
                "cancleButtonColor", T<string>
                "confirmButtonClass", T<string>
                "cancelButtonClass", T<string>
                "buttonsStyling", T<bool>
                "reverseButtons", T<bool>
                "focusCancel", T<bool>
                "showCloseButton", T<bool>
                "showLoaderOnConfirm", T<bool>
                "preConfirm", T<string> ^-> T<Promise>
                "imageUrl", T<string>
                "imageWidth", T<int>
                "imageHeight", T<int>
                "imageClass", T<string>
                "inputPlaceholder", T<string>
                "inputValue", T<string>
                "inputOptions", T<string[]>
                "inputAutoTrim", T<bool>
                "inputAttributes", T<System.Collections.Generic.Dictionary<_,_>>.[T<string>, T<string>]
                "inputValidator", T<string> ^-> T<Promise>
                "inputClass", T<string>
                "progressSteps", T<int[]>
                "currentProgressStep", T<unit> ^-> T<int>
                "progressStepDistance", T<string>
                "onOpen", T<JavaScript.Dom.Node> ^-> T<unit>
                "onClose", T<JavaScript.Dom.Node> ^-> T<unit>
                "useRejections", T<bool>
            ]
        }

    SweetAlert
        |+> Static[
            Constructor(Options)
            "isVisible" => T<unit> ^-> T<bool>
            "setDefaults" => Options ^-> T<unit>
            "resetDefaults" => T<unit> ^-> T<unit>
            "close" => T<unit> ^-> T<unit>
            "enableButtons" => T<unit> ^-> T<unit>
            "getTitle" => T<unit> ^-> T<string>
            "getContent" => T<unit> ^-> T<string>
            "getImage" => T<unit> ^-> T<string>
            "getConfirmButton" => T<unit> ^-> T<JavaScript.Dom.Node>
            "getCancelButton" => T<unit> ^-> T<JavaScript.Dom.Node>
            "disableButtons" => T<unit> ^-> T<unit>
            "enableConfirmButton" => T<unit> ^-> T<unit>
            "disableConfirmButton" => T<unit> ^-> T<unit>
            "showLoading" => T<unit> ^-> T<unit>
            "hideLoading" => T<unit> ^-> T<unit>
            "clickConfirm" => T<unit> ^-> T<unit>
            "clickCancel" => T<unit> ^-> T<unit>
            "showValidationErrorMessage" => T<string> ^-> T<unit>
            "resetValidationError" => T<unit> ^-> T<unit>
            "getInput" => T<unit> ^-> T<JavaScript.Dom.Node>
            "disableInput" => T<unit> ^-> T<unit>
            "enableInput" => T<unit> ^-> T<unit>
            "queue" => Type.ArrayOf Options ^-> T<unit>
            "getQueueStep" => T<unit> ^-> T<int>
            "insertQueueStep" => (Options * !? T<int>) ^-> T<unit>
            "deleteQueueStep" => T<int> ^-> T<unit>
            "getProgressSteps" =? T<int>
            "setProgressSteps" =! T<int>
            "showProgressSteps" => T<unit> ^-> T<unit>
            "hideProgressSteps" => T<unit> ^-> T<unit>
        ]|>ignore
 

    let Assembly =
        Assembly [
            Namespace "WebSharper.SweetAlert.Resources" [
                Resource "Css" "https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/6.6.6/sweetalert2.min.css"
                |> AssemblyWide
                Resource "Js" "https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/6.6.6/sweetalert2.min.js"
                |> AssemblyWide
            ]
            Namespace "WebSharper.SweetAlert"[
                Options
                SweetAlert

            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
