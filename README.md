# Adapter Pattern Generator

####Still in early development!

Adapter Pattern  will be a Visual Studio add-in that allows a user to generate adapter patterns for selected assemblies.

## Overview of Potential Features
- Generates interface and glue code for adapters
- Refactors existing code that refers to adapted assemblies
- Adapts Static Methods including Constructor
- Avoids garbage collection issues
- Copies documentation to classes and interfaces

## First thoughts on how it should work

1. User will select the Adapter Pattern Generator Project Item
1. A wizard will allow the user to select assemblies to generate
1. The Add-in will then generate a project for interfaces and a project for classes, the classes assembly will reference the interface assembly
1. Each type in each assembly selected will have an interface and class for static members and an interface and class for non static members generated in the appropriate assemblies.
1. Enums will be copied to the interface assembly
1. Adapted Interfaces will be copied to the interface assembly
1. Fields will be treated as properties
1. Constants will be treated as read only properties
1. Classes that adapt the non static side will handle garbage collection (IDisposable)