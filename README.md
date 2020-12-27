# Unity Utilities

Some Unity3D utility scripts used in my closed source project Panic At The Picnic 2 published under MIT License.

Currently contains:
- singleton base class
- object pooling system
- attributes simplifying getting components 
- Utility class with some methods used for randomization

Keep in mind that using these attributes will slightly slow down object initialization. 

However you can use object pooling to move this cost into scene loading and if your game doesn't change scenes often it'll become unnoticeable.

You can also use them for long living objects, for example user interface elements.

Also keep in mind that Panic At The Picnic 2 is a single scene game. The polled objects are infinitely reused. If your game is different you might have to make some adjustments, especially look out for static variables.
