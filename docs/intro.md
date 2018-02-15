# What is TVision2?

TVision2 is a library for building console based applications ([TUI](https://en.wikipedia.org/wiki/Text-based_user_interface)-based applications or console games). It is built based on concepts taken from Unity3D, React and Redux among others.

Its name is a tribute to [Turbo Vision](https://en.wikipedia.org/wiki/Turbo_Vision) an awesome library created by Borland in the DOS era. Despite its name TVision2 is not a new version of Turbo Vision, nor a revision.

## What I can do with TVision2

**Any netcore console-based application**. It doesn't matter if is an application (i. e. a new clon of [FarManager](https://www.farmanager.com/)) or a console-based game (i. e. a competitor of the amazing [Dwarf Fortress](https://en.wikipedia.org/wiki/Dwarf_Fortress)). TVision2 can help you to make it.

> Tvision2 is inteneded to help you creating console apps that behave like "full screen apps". For typical console apps, Tvision2 do not offer any value.

## What limitations it have?

As TVision2 uses [TvConsole](https://github.com/eiximenis/tvconsole) project it shares the same limitations: currently **no Linux support**. Once TvConsole gets Linux support, TVision2 will get it too.

## Programming model

There are three different libraries:

* `Tvision2.Core`: Main library containing the core of the TVision2. Depending on the application this library is all you need.
* `Tvision2.Controls`: A suit of controls created based on the foundations provided by _Tvision2.Core_.
* `Tvision2.Statex`: A state management library that seamessly integrates with _Tvision2.Core_.

## Tvision2.Core

This library provides the foundational items of TVision2. Application GUI is envisioned as a set of _components_. Each component has its own behavior and knows how to draw itself. **There is no way for one component to interact with any other component**. Components are, deliberatly, isolated.

Behavior of a component is specified by adding one or more _behavior_ objects to it. Tvision2 works like a game engine: continuously is rendering your current screen, so per each frame, all behaviors of your component will be invoked. A behavior has, basically, two options:

1. Change the state of the component
2. Do not change the state of the component

If any of the behaviors change the state of the component, the component will be redrawn on the screen.

So, a typical frame iteration in Tvision2 looks like:

1. Events are peeked
2. All behaviors are invoked
3. For any component that any of its behaviors changed its state, the component is redrawn

To draw a component, Tvision2 relies on the _drawers_ objects: any component have one or more drawers. If the component has to be redrawn, all drawers are invoked. Drawers do not directly draw to the console. Instead every drawer  interact with its _viewport_. Then all viewports are projected and combined in a virtual console. Then contents of the virtual console are compared with the real console generating a set of differences that must be applied to real console to have the same content that the virtual console. Finally those differences are applied to real console.

