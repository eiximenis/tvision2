# TvControls - A controls library built upon TVision2

TvControls is a "classic controls" library built upon TVision2. It provides the "control" concept. A "control" is:

* A Component
* With mutable state
* That can invoke commands
* And with specified set of drawers and behaviors

TvControls provide a set of pre-created controls and all needed infrastructure to create new controls.

## Focus

Like most control libraries, TvControls uses the concept of "focused" control to indicate what control will receive mouse and keyboard events. Only one control can have the focus at the same time. Focus concept is built around the _responder chain_ capability that TVision2 offers.

The _Controls Tree_ is the object that handles all controls and its focused state. Controls Tree interacts with the _Component Tree_ (a foundational object of TVision2) to ensure that a control is added to _responders chain_ when it has the focus, and it is removed from the chain when it has no longer the focus.

By default TvControls installs a Keyboard Hook in TVision2 to handle all "Tab" keystrokes and change the focused control.



