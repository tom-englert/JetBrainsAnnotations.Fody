## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

Modifies an assembly so you can leverage JetBrains Annotations but don't need to deploy JetBrainsAnnotations.dll

[Introduction to Fody](http://github.com/Fodyp/Fody/wiki/SampleUsage)

## Nuget 

Nuget package http://nuget.org/packages/JetBrainsAnnotations.Fody 

To Install from the Nuget Package Manager Console 
    
    PM> Install-Package JetBrainsAnnotations.Fody

## What are JetBrains Annotations

The assembly JetBrainsAnnotations.dll comes as part of the nuget http://nuget.org/packages/JetBrains.Annotations.
It provides standard JetBrains ReSharper code annotation attribute implementations. This allows you to better leverage the ReSharper intellisense.

For more information 

 * http://www.jetbrains.com/resharper/webhelp/Code_Analysis__External_Annotations.html 
 * https://github.com/artikh/JetBrains.Annotations
 * http://www.jetbrains.com/resharper/features/code_analysis.html#Annotated_Framework

## What it actually does to your assembly

 * For each attribute defined in JetBrainsAnnotations.dll remove usages of those attributes from your assembly.
 * Removes the reference to JetBrainsAnnotations.dll
