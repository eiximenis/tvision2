# TVision2 - A Console component library

TVision2 is a library to create console applications based on components. It is a mid-level library, but depending on your needs it could be all that you need. If you want to have the concept of "control" (like buttons, checkboxes and so on) then take a look to _TvControls_. 

## Component Tree

All components of your application are stored in the _component tree_. Despite its current name is more a list than a tree (currently no panel/layout support exists on TVision2). The component tree manages two lists of components:

* One list with all the components
* The responders chain

The responders chain are a ordered-list of components that will receive all events (keyboard and mouse). Only responders receive events. When a responder receives an event it can:

1. Process it and do not event go to the next responder
2. Process it and let event go to the next responder
3. Ignore it (event will go to the next responder)







