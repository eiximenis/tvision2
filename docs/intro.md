# What is TVision2?

TVision2 is a library for building console based applications ([TUI](https://en.wikipedia.org/wiki/Text-based_user_interface)-based applications or console games). It is built based on concepts taken from Unity3D, React and Redux among others.

It is built on top the netcore `System.Console` and it is a `netstandard` library, so it should be cross platform, **but right now all tests are being performed in Windows**.

Its name is a tribute to [Turbo Vision](https://en.wikipedia.org/wiki/Turbo_Vision) an awesome library created by Borland in the DOS era. Despite its name TVision2 is not a new version of Turbo Vision, nor a revision.

## What I can do with TVision2

**Any netcore console-based application**. It doesn't matter if is an application (i. e. a new clon of [FarManager](https://www.farmanager.com/)) or a console-based game (i. e. a competitor of the amazing [Dwarf Fortress](https://en.wikipedia.org/wiki/Dwarf_Fortress)). TVision2 can help you to make it.

## What limitations it have?

As TVision2 is built up on the `System.Console` inherits of all their limitations. The most important one is the **lack of mouse support**. To overcome this limitation (and some others) I started the [TvConsole project](https://github.com/eiximenis/tvconsole). TvConsole reimplements all `System.Console` APIs from the scratch using native OS calls and includes mouse support. Unfortunately TvConsole is Windows only and I don't want to make TVision2 Windows only. I am not sure about what to do at this point (the best solution would be make TvConsole cross-platform, of course).

## How it looks like?

In a TVision2 application yor UI is a set of components. Every component has a set of behaviors, a set of drawers and a property bag. TVision2 runs like a game engine, trying to update your UI at a fixed rate of 30 fps. At every frame:

1. All behaviors of every component could be invoked giving them the opportunity to create a new property bag for the control
2. Drawers of every component are invoked only if the property bag has been changed by one of the behaviors.

Drawers don't draw directly to the console: instead they receive a _Viewport_ and draw onto it. Then the TVision2 engine combines all Viewports outputs to a virtual console. Finally the status of the Virtual Console is compared against the real console displayed on the screen and only the needed characters are updated.

TVision2 promotes composition over inheritance: components are customized by adding/removing behaviors and drawers instead of inheriting from a base class and override specific methods. Berhaviors and drawers are just functions, and should have no state.

Also, TVision2 promotes one-direction-data-flow: All application state is stored in objects called _stores_. When a state change occurs in one _store_ the desired behaviors are invoked, giving them the chance to update the property bags of their components to force a new redraw. Behaviors **can't update the state**, instead they have to publish _actions_. Actions are received by the stores, and every time an store receive an _action_ a set of _reducers_ is invoked. Reducers can generate a new state for the store, and if this happens desired behaviors are invoked, closing the circle.
So, information only goes in one direction: from the stores to the behaviors. Behaviors can only publish actions to stores.

## Ooof... this seems to complex.

It is not! Take a time to be comfortable with all concepts, and you'll see how they fit and play well together. :)