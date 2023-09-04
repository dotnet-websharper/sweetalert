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

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =
    
    let DismissReason = Pattern.EnumStrings "DismissReason" [
        "cancel"
        "backdrop"
        "close"
        "esc"
        "timer"
    ]

    let SweetAlertResult = Generic - fun t ->
        Pattern.Config "SweetAlertResult" {
            Required = [
                "isConfirmed", T<bool>
                "isDenied", T<bool>
                "isDismissed", T<bool>
            ]
            Optional = [
                "value", t.Type
                "dismiss", DismissReason.Type // todo Swal.DismissReason
            ]
        }
    let SweetAlertIcon = Pattern.EnumStrings "SweetAlertIcon" [
        "success"
        "error"
        "warning"
        "info"
    ]
        
    let SweetAlertInput = Pattern.EnumStrings "SweetAlertInput" [
        "text"; "email"; "password"; "number"; "tel"; "range";
        "textarea"; "select"; "radio"; "checkbox"; "file"; "url";
    ]
    
    let SweetAlertPosition = Pattern.EnumStrings "SweetAlertPosition" [
        "top"; "top-start"; "top-end"; "top-left"; "top-right"
        "center"; "center-start"; "center-end"; "center-left"; "center-right"
        "bottom"; "bottom-start"; "bottom-end"; "bottom-left"; "bottom-right"
    ]
    
    
    let SweetAlertCustomClass = Pattern.Config "SweetAlertCustomClass" {
        Required = []
        Optional = []
    }


    let SweetAlertHideShowClass = Pattern.Config "SweetAlertHideShowClass" {
        Required = []
        Optional = [
            "backdrop", T<string> + T<string[]>
            "icon", T<string> + T<string[]>
            "popup", T<string> + T<string[]>
        ]
    }
    let ValueOrThunk (t:Type.Type) = t + (T<unit> ^-> t)
    let SyncOrAsync (t:Type.Type) = t + T<Promise<_>>[t]

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
        "didClose", T<unit> ^-> T<unit>
        "didDestroy", T<unit> ^-> T<unit>
        "footer", T<string> + T<HTMLElement>
        "hideClass", SweetAlertHideShowClass.Type
        "html", T<string> + T<HTMLElement>
        "icon", SweetAlertIcon.Type
        "iconColor", T<string>
        "imageAlt", T<string>
        "imageHeight", T<int> + T<string>
        "imageUrl", T<string>
        "imageWidth", T<int> + T<string>
        "preConfirm", T<obj>?inputValue ^-> SyncOrAsync T<obj>
        "preDeny", T<obj>?value ^-> SyncOrAsync (T<obj> + T<unit>)
        "progressSteps", Type.ArrayOf T<string>
        "reverseButtons", T<bool>
        "showCancelButton", T<bool>
        "showCloseButton", T<bool>
        "showConfirmButton", T<bool>
        "showDenyButton", T<bool>
        "text", T<string>
        "title", T<string> + T<HTMLElement>
        "titleText", T<string>
        "willClose", T<HTMLElement>?popup ^-> T<unit>
    ]
    
    let SweetAlertUpdatableParameters = Pattern.Config "SweetAlertUpdatableParameters" {
        Required = []
        Optional =  updatableParameters
    }
    


    let SweetAlertGrow = (Pattern.EnumStrings "SweetAlertGrow" [
        "row"; "column"; "fullscreen"; false.ToString()
    ])

    let inputType = T<string> + T<int> + T<File> + T<FileList>
    let SweetAlertOptions =
        Pattern.Config "SweetAlertOptions" {
            Required = []
            Optional = [
                "iconHtml", T<string>
                "template", T<string> + T<HTMLTemplateElement> // is this good
                "backdrop", T<bool> + T<string>
                "toast", T<bool>
                "target", T<string> + T<HTMLElement>
                "input", SweetAlertInput.Type
                "width", T<double> + T<string>
                "padding", T<double> + T<string>
                "color", T<string>
                "position", SweetAlertPosition.Type
                // "grow", SweetAlertGrow.Type
                "showClass", SweetAlertHideShowClass.Type
                "timer", T<int>
                "timerProgressBar", T<bool>
                "heightAuto", T<bool>
                "allowEnterKey", SyncOrAsync T<obj> // TODO generic SyncOrAsync
                "stopKeydownPropagation", T<bool>
                "keydownListenerCapture", T<bool>
                "focusConfirm", T<bool>
                "focusDeny", T<bool>
                "focusCancel", T<bool>
                "returnFocus", T<bool>
                "loaderHtml", T<string>
                "showLoaderOnConfirm", T<bool>
                "showLoaderOnDeny", T<bool>
                "inputLabel", T<string>
                "inputPlaceholder", T<string>
                
                "inputValue", SyncOrAsync inputType // TODO: wrap in SyncOrAsync
                "inputOptions", T<obj> // TODO SyncOrAsync<readonlymap<str,str> | record<str,any>>
                "inputAutoFocus", T<bool>
                "inputAutoTrim", T<bool>
                "inputAttributes", T<obj> // Record<str,str>
                "inputValidator", T<string*string> ^-> (T<string> + T<unit>) // TODO string,string -> string | null | void
                "returnInputValueOnDeny", T<bool>
                "validationMessage", T<string>
                "progressStepsDistance", (!? T<int>) + T<string> // default undef
                "willOpen", T<HTMLElement> ^-> T<unit>
                "didOpen", T<HTMLElement> ^-> T<unit>
                "didRender", T<HTMLElement> ^-> T<unit>
                "scrollbarPadding", T<bool>

                yield! updatableParameters
            
            ] 
        }

    let Swal = 
        Class "Swal"
        |+> Static [
            //Generic - fun t ->
            //    Method "fire" (SweetAlertOptions.Type ^-> t)

            Generic - fun t ->
                "fire" =>  SweetAlertOptions.Type ^-> T<Promise<_>>[SweetAlertResult[t]] // TODO: class as input
            Generic - fun t -> 
                "fire" => !? T<string>?title * !? T<string>?html * !? SweetAlertIcon.Type?icon ^-> T<Promise<_>>[SweetAlertResult[t]]

            "mixin" => SweetAlertOptions.Type?options ^-> TSelf
            "isVisible" => T<unit> ^-> T<bool>
            "update" => SweetAlertUpdatableParameters.Type?options ^-> T<unit>
            Generic - fun t ->
                "close" => SweetAlertResult[t] ^-> T<unit>
            
            let get (name:string) (t:Type.Type) =
                $"get{name}" => T<unit> ^-> t

            for n in [ 
                "Container";"Popup";"Title";"ProgressSteps";"HtmlContainer";"Image"
                "CloseButton";"Icon";"IconContent";"ConfirmButton";"DenyButton";"CancelButton"
                "Actions";"Footer";"TimerProgressBar";"ValidationMessage"
            ] do 
                (get n !? T<HTMLElement>) :> CodeModel.IClassMember

            get"FocusableElements" !| T<HTMLElement>

            for n in [
                "hideLoading"
                "isLoading"
                "clickConfirm"
                "clickDeny"
                "clickCancel"
                "disableInput"
                "enableInput"
                "resetValidationMessage"
            ] do 
                (n => T<unit> ^-> T<unit>) :> CodeModel.IClassMember

            get"Input" !? T<HTMLInputElement>

            for n in [
                "getTimerLeft"
                "stopTimer"
                "resumeTimer"
                "toggleTimer"
            ] do 
                (n => T<unit> ^-> !| T<int>) :> CodeModel.IClassMember

            "increaseTimer" => T<int>?n ^-> !| T<int>
            "isTimerRunning" => T<unit> ^-> !| T<bool>
            
            "version" =? T<string>

        ] |> ImportDefault "sweetalert2"

    
 

    let Assembly =
        Assembly [
            Namespace "WebSharper.SweetAlert.Resources" [
                // Resource "Css" "https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/6.6.6/sweetalert2.min.css"
                // |> AssemblyWide
                //Resource "Js" "sweetalert2"
                //|> AssemblyWide
            ]
            
            Namespace "WebSharper.SweetAlert" [
                SweetAlertIcon
                SweetAlertInput
                SweetAlertPosition
                SweetAlertCustomClass
                SweetAlertHideShowClass
                SweetAlertUpdatableParameters
                DismissReason
                SweetAlertResult
                SweetAlertOptions

                Swal
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
[<assembly: System.Reflection.AssemblyDescription("7.0.0.564-beta2")>]
[<assembly: System.Reflection.AssemblyTitle("7.0.0.564-beta2")>]
do ()
