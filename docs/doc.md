# WebSharper.SweetAlert

[SweetAlert](https://limonte.github.io/sweetalert2/) is a JavaScript library to replace JavaScript's popup boxes with better looking ones.

# Configuration
First of all we have to configure the popup boxes we want to use later.

```fsharp
let WarningBox =
    Box = (
        TitleText = "Warning!",
        Text = "This is a warning",
        Type = "warning",
        ShowCancelButton = true,
        ConfirmButtonColor = "#000000"
    )
```
We have to instantiate a `Box` type object, which has several properties. With them we can configure the looks and the behavior of our popup boxes.
For the full list of the options see the [original documentation](https://limonte.github.io/sweetalert2/). The only difference is that in WebSharper every name starts with a capital letter.

If many of your boxes share the same characteristics you can set the default options to suit you well. For this you can use the `SweetAlert.SetDefaults` method.
```fsharp
SweetAlert.SetDefaults Warning
```
It takes a `Box` object as its only argument and returns with a unit.

You can reset defaults to their original values with the `SweetAlert.ResetDefaults` method.

# Showing a box

To show the box you just have to pass the `Box` obejct to `SweetAlert.ShowBox` like this:
```fsharp
//note: SweetAlert takes a Box and returns a Promise object
SweetAlert.ShowBox Warning |> ignore
```

# Input fields in a box

With SweetAlert you can make popup boxes with input fields inside of it. Input fields can be the following: *text*, *email*, *password*, *number*, *tel*, *range*, *textarea*, *select*, *radio*, *checkbox*, *file* and *url*.

```fsharp
let InputBox =
    Box = (
        TitleText = "Input",
        Text = "Write something here:",
        Type = "info",
        Input = "text"
    )
```

In this case it is not enough to simply show the Box, we have to get the value inside of the input field, when the user sends it.
`ShowBox` returns with a `Promise` so we have to use that. WbHsarper's Promise is not entirely same as JavaScript promise so, first we have to write an inline JavaScript function:
```fsharp
[<Inline "function (d) {$f(d)}">]

let F f = X<System.Action>
```
After that we can use the Then function of Promise to do something with the result of the form:
```fsharp
SweetAlert.ShowBox(InputBox).Then(F (fun (result: string) -> Console.Log result))|> ignore
```
First we show the box, after the `Promise` is fulfilled we can do something with the result inside of the `.Then` function (In the place of Console.Log). It returns a `Promise`.

For more information about Promises follow [this link](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise) to the MDN docs.

# SweetAlert methods
SweetAlert has several more methods, but I won't include them in this documentation, since they work exactly the ame as the original methods, so you can get every info about them in the [JavaScript docs](https://limonte.github.io/sweetalert2/). Again the only difference is between the naming conventions: the WebSharper methods start with a capital letter.

# Summary
The extensions works exactly the same as the orginal lib, except to show a box you have to use the `ShowBox` method.