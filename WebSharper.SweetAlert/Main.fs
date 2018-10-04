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
namespace WebSharper.SweetAlert

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator
open WebSharper.JQuery

module Definition =
    let SweetAlert = Class "sweetAlert"

    let SweetAlertProm =
        Class "SweetAlertPromise"
        |=> Inherits T<Promise>
        |+> Instance [
            "then" => T<string -> unit> ^-> T<unit>
        ]

    let Box =
        Pattern.Config "Box"{
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
                "preConfirm", T<string> ^-> SweetAlertProm
                "imageUrl", T<string>
                "imageWidth", T<int>
                "imageHeight", T<int>
                "imageClass", T<string>
                "inputPlaceholder", T<string>
                "inputValue", T<string>
                "inputOptions", T<string[]>
                "inputAutoTrim", T<bool>
                "inputAttributes", T<System.Collections.Generic.Dictionary<_,_>>.[T<string>, T<string>]
                "inputValidator", T<string> ^-> SweetAlertProm
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
            "showBox" => Box?box ^-> SweetAlertProm
            |> WithInline ("return Sweetalert2($box);")
            "isVisible" => T<unit> ^-> T<bool>
            "setDefaults" => Box ^-> T<unit>
            "resetDefaults" => T<unit> ^-> T<unit>
            "close" => T<unit> ^-> T<unit>
            "enableButtons" => T<unit> ^-> T<unit>
            "getTitle" =? T<string>
            "getContent" =? T<string>
            "getImage" =? T<string>
            "getConfirmButton" =? T<JavaScript.Dom.Node>
            "getCancelButton" =? T<JavaScript.Dom.Node>
            "disableButtons" => T<unit> ^-> T<unit>
            "enableConfirmButton" => T<unit> ^-> T<unit>
            "disableConfirmButton" => T<unit> ^-> T<unit>
            "showLoading" => T<unit> ^-> T<unit>
            "hideLoading" => T<unit> ^-> T<unit>
            "clickConfirm" => T<unit> ^-> T<unit>
            "clickCancel" => T<unit> ^-> T<unit>
            "showValidationErrorMessage" => T<string> ^-> T<unit>
            "resetValidationError" => T<unit> ^-> T<unit>
            "getInput" =? T<JavaScript.Dom.Node>
            "disableInput" => T<unit> ^-> T<unit>
            "enableInput" => T<unit> ^-> T<unit>
            "queue" => Type.ArrayOf Box ^-> T<unit>
            "getQueueStep" =? T<int>
            "insertQueueStep" => (Box * !? T<int>) ^-> T<unit>
            "deleteQueueStep" => T<int> ^-> T<unit>
            "getProgressSteps" =? T<int>
            "setProgressSteps" => T<int> ^-> T<unit>
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
                Box
                SweetAlert
                SweetAlertProm

            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
