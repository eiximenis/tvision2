# Tvision2

**IMPORTANT DISCLAIMER** This is **HEAVY WIP** right now. Currently Tvision2 is in pre-pre-alpha (yes, two pre :P) state. 

TVision2 is just another TUI library. Yes, there are a lot of other libraries out there that do the same that TVision2 pretends to do... this is just another option :)

The name is my tribute to the beloved "Turbo Vision" library, the king of all TUI libraries in the golden days of DOS :)

## What makes Tvision2 special

The key points of Tvision2 are:

1. Based in the same principles of game engines. Core library do not undersand about controls nor layouts. Those aspects are provided through separated libraries.
2. If you don't want to use controls or layouts you are free to do so: just grab the libaries you need.
3. On runtime **everything** is a component. A component is just something that have some behaviors and is drawed in a specific way. Tvision2.Core only understand about components.
4. Tvision2 works like a game engine: each frame is calculated and then all components are drawn onto a virtual console that is later pushed to real console buffer.
5. Tvision2.Controls add a basic controls library. A control is basically a component with predefined behaviors and drawers
6. Tvision2.Layouts add a layout system to Tvision2.Core. By default Tvision2.Core works only with absolute positioning.
7. Tvision2.Statex add a state manager following Redux principles to your application.
8. Tvision2.Statex.Controls is a thin-integration layer to allow easy use of Tvision2.Statex and Tvision2.Controls.

## Show me some code

Tvision2 is a netstandard2 library, and it promotes the use of library as a `IHostedService` running on top a netstandard2 `HostBuilder`. On your `async main` you can start your Tvision2 based-app with following code:

```
var builder = new HostBuilder();
builder.UseTvision2(setup =>
{ 
    setup.UseDotNetConsoleDriver()
}).UseConsoleLifetime();
await builder.RunTvisionConsoleApp();
```

This is the minimum code to start Tvision2 and configure it to use one specific _ConsoleDriver_. The _ConsoleDriver_ is how Tvision2 interacts with real Console buffer.

Of course this code do nothing useful: you should see a black screen. This is because we need to create some component. Here is a typical Hello world, that prints "Tvision2 rocks!" on a yellow text over blue background in the position (10,10) of the console:

```
private static async Task Main(string[] args)
{
    var builder = new HostBuilder();
    builder.UseTvision2(setup =>
    {
        setup.UseDotNetConsoleDriver();
        setup.Options.UseStartup((sp, tui) =>
        {
            var helloWorld = new TvComponent<string>("Tvision2 rocks!");
            helloWorld.AddDrawer(ctx =>
            {
                ctx.DrawStringAt(ctx.State, TvPoint.Zero, ConsoleColor.Yellow, ConsoleColor.Blue);
            });
            helloWorld.AddViewport(new Viewport(new TvPoint(10, 10), 30));
            tui.UI.Add(helloWorld);
            return Task.CompletedTask;
        });
    }).UseConsoleLifetime();
    await builder.RunTvisionConsoleApp();
}
```

You may think "Wow! This is a lot of code for a Hello world!". And you are true, but this is only because a hello world is not a good example to see the power and benefits of Tvision2 :) Trust me: As long as your application grow, you will note that Tvision2 worths every line of code!

### What is doing this code?

The `setup.Options.Startup` allows us to add code to run **before** Tvision2 starts its game-loop. It is not the only way to do that, and not the most common one, but for small apps can be enough. Don't bother about the parameters received by the lambda. You'll learn later what they are and how to use them.

In the 1st line we create a `TvComponent<string>` object. That is a `TvComponent` that "holds" one string object as data. Next we use `AddDrawer` to add a drawer. A component must have at least one drawer if we want to display them on the screen. The parameter of the drawer is an object of type `RenderContext<T>` where `T` is the generic type of `TvComponent`. In our case as we are using a `TvComponent<string>` the `ctx` parameter is a `RenderContext<string>`. The `RenderContext<T>` contains methods to draw onto the virtual console and one property (`State`) of type `T` which contains the value hold by the `TvComponent<T>` object. Our drawer implementation is very easy: just draw the string in the (0,0) (upper-left) position.

Every drawing operation is performed into a _viewport_ So this (0,0) is relative to the viewport of the component. A component can have zero, one or even more than one viewport, but at least one is needed to make it visible on the console. So, we use the `AddViewport` method to add a _viewport_ located in the position (10,10) and with a width of 30 characters. This position (10,10) is a absolute position. As in the drawer func we draw the text in the upper-left position and the _viewport_ is located at (10,10), the text will be drawn in the position (10,10) of the console.

Finally we need to add the component to the Tvision2 engine. The parameter `tui` that we received in the lambda is the Tvision2 engine, so we can add items on it using its `UI` property.

And that's all :)

## Current Status of Libraries

* Tvision2.Core: The work on core is, altought not finished, somewhat stabilized. Some work could be needed to support some scenarios needed by some other libraries, but I don't expect dramatic changes to it. Pending work is making `VirtualConsole` aware of console buffer size changes.

* Tvision2.ConsoleDriver: DotNet console driver is almost done, and Win32 console driver is very advanced but some adjustements needs to be done (to support utf8 characters). Linux driver (using ncurses) is in early development stage and won't almost work. A pending work for all drivers is way to setup the initial state of the console buffer. If you wonder why there are Win32 and Linux specific implementations of the ConsoleDriver (instead of relying in the netcore Console API which is cross-platform) is because netcore Console API is limited (i.e. no mouse support).

* Tvision2.Controls: Basic architecture is done, but all controls needs to be finished, and some controls have to be added. But, from architectural point of view, I don't expect any radical change.

* Tvision2.Viewports: Almost nothing done on this library. The goal of this library is to provide a set of hooks to allow all viewports recalculate automatically if the console buffer size changes.

* Tvision2.Layouts: Work just started in this library. From an architectural point of view, I don't expect any dramatic change. Two basic layouts (Grid and vertical stack panel) are provided, but a lot of work is pending here.

* Tvision2.Statex: This library provides a state management utility that works in a similar way that Redux do. Basic work is done and library is usable. I don't expect a lot of changes on it, but need to test it more.

* Tvision2.Statex.Controls: This library provides a way to allow using Statex with TvControls library in a easy way. Basic work is finished, but some adjustements may be needed.

* Tvision2.Controls.Styles.Mc: This is a "demo-library" that provide a set of pre-built styles for use with Tvision2.Controls. The `Mc` came from "Midnight commander" as the styles in this library try to mimic the midnight commander colors.