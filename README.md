## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Icon](https://raw.github.com/Fody/JetBrainsAnnotations/master/Icons/package_icon.png)

Modifies an assembly so you can leverage JetBrains Annotations but don't need to deploy JetBrainsAnnotations.dll

[Introduction to Fody](http://github.com/Fodyp/Fody/wiki/SampleUsage)

## Nuget 

Nuget package http://nuget.org/packages/JetBrainsAnnotations.Fody 

To Install from the Nuget Package Manager Console 
    
    PM> Install-Package JetBrainsAnnotations.Fody

## What are JetBrains Annotations

The assembly JetBrainsAnnotations.dll is shipped as part of ReSharper and is included with this nuget.
It provides standard JetBrains ReSharper code annotation attribute implementations. This allows you to better leverage the ReSharper intellisense.

For more information 

 * http://www.jetbrains.com/resharper/webhelp/Code_Analysis__External_Annotations.html 
 * http://www.jetbrains.com/resharper/features/code_analysis.html#Annotated_Framework

## What it actually does to your assembly

 * For each attribute defined in JetBrainsAnnotations.dll remove usages of those attributes from your assembly.
 * Removes the reference to JetBrainsAnnotations.dll

## Icon

<a href="http://thenounproject.com/noun/fighter-jet/#icon-No9259" target="_blank">Fighter Jet</a> designed by <a href="http://thenounproject.com/lukefirth" target="_blank">Luke Anthony Firth</a> from The Noun Project