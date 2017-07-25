# WebSharper.SweetAlert

[SweetAlert](https://limonte.github.io/sweetalert2/) is a JavaScript library to replace JavaScript's popup boxes to better looking ones.

# Configuration
First of all we have to configure the popup boxes we want to use later.

```fsharp
let Warning =
    Box = (
        TitleText = "Warning!"
        Text = "This is a warning"
        Type = "warning"
	ShowCancelButton = true
	ConfirmButtonColor = "#000000"
    )
```
We have to instantiate a Box type object, which contains several fields. With them we can configure the looks and the behavior of our popup boxes.
For the full list of the options see the [original documentation](https://limonte.github.io/sweetalert2/). The only difference between the original options and the extensions, are that here in WebSharper all of the option names start with a capital letter, as you can see in the example.

If many of your boxes share the same characteristics you can set the default options to suit you well. For this you can use the SweetAlert.SetDefaults method.
```fsharp
SweetAlert.SetDefaults Warning // Box -> unit
```
It takes a Box object as its only argument and returns with a unit.

You can reset defaults to their original values with the SweetAlert.ResetDefaults method.

# Showing the box

To show the box you just have to pass the Box obejct to SweetAlert like this:
```fsharp
//note: SweetAlert returns with itself so you have to ignore the result if you just want to show the box
SweetAlert.ShowBox Warning |> ignore
