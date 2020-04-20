# TvControls - A controls library built upon TVision2

TvControls is a "classic controls" library built upon TVision2. It provides the "control" concept. A "control" is:

* A Component
* With mutable state
* That can invoke commands
* And with specified set of drawers and behaviors

TvControls provide a set of pre-created controls and all needed infrastructure to create new controls.

## Focus

Like most control libraries, TvControls uses the concept of "focused" control to indicate what control will receive mouse and keyboard events. Only one control can have the focus at the same time. Focus is a **new concept introduced in TvControls**.

In _Tvision.Core_ all components are stored in a flatten list, with no specific order. Core do not care about component ordering, as the idea of child components do not exist. But controls are a bit more complex because:

1. A control can be made up of more controls (i. e. dropdown is a label + list)
2. A control can "capture" the focus, and make the focus available only to its childs

So, in TvControls controls have a **parent-child relationship**. This relationship is mantained by TvControls in the `ControlsTree` class. Focus is transferred first to child controls and then to parent ones in the order they're added to the control tree. However a control can update this behavior by:

* "Capturing" the focus. Once the focus is captured, the focus can only go to the control and their descendants.
* Not allowing the focus. If a control do not allow the focus, the control itself **and all their descendants cannot have the focus**.
* Being unfocusable: If a control is unfocusable it can't have focus, but their descendants can.