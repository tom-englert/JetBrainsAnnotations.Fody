[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg?style=flat)](https://gitter.im/Fody/Fody)
[![NuGet Status](http://img.shields.io/nuget/v/JetBrainsAnnotations.Fody.svg?style=flat)](https://www.nuget.org/packages/JetBrainsAnnotations.Fody/)
![Badge](https://tom-englert.visualstudio.com/_apis/public/build/definitions/75bf84d2-d359-404a-a712-07c9f693f635/14/badge)

## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Icon](https://raw.github.com/Fody/JetBrainsAnnotations/master/Icons/package_icon.png)

Converts all JetBrains ReSharper code annotation attributes to [External Annotations](https://www.jetbrains.com/help/resharper/Code_Analysis__External_Annotations.html), 
so you can provide R# annotations to 3rd parties but don't need to deploy JetBrainsAnnotations.dll. 


## The nuget package

https://nuget.org/packages/JetBrainsAnnotations.Fody/

    PM> Install-Package JetBrainsAnnotations.Fody


## What are JetBrains Annotations

The assembly JetBrainsAnnotations.dll is shipped as a [nuget package](https://www.nuget.org/packages/JetBrains.Annotations/).
It provides standard JetBrains ReSharper code annotation attribute implementations. 
This allows you to better leverage the ReSharper intellisense.

To provide the annotations to 3rd parties you must define [`JETBRAINS_ANNOTATIONS`](https://www.jetbrains.com/help/resharper/Code_Analysis__Annotations_in_Source_Code.html) to include the attributes in your assembly.
However now you have a reference and need to ship the JetBrainsAnnotations.dll with your product. 

This Fody plugin converts all attributes to an external annotations XML file on the fly, so you 
can ship just the text file with your assembly and don't need to reference JetBrainsAnnotations.

For more information 

 * http://www.jetbrains.com/resharper/webhelp/Code_Analysis__External_Annotations.html 
 * http://www.jetbrains.com/resharper/features/code_analysis.html#Annotated_Framework


## What it actually does to your assembly

 * For each attribute defined in JetBrainsAnnotations.dll it adds an entry to your
   assemblies external annotations XML file and removes the usage of the attribute from your assembly.
 * Removes the reference to JetBrainsAnnotations.dll

## What it actually does to your project

* Adds the &lt;project name&gt;.ExternalAnnotations.xml file to your project, and marks it as CopyToOutput.
* Updates this file every time you compile.

To make your annotations available to 3rd parties, you must ship this file along with the assembly.


## Icon

<a href="http://thenounproject.com/noun/fighter-jet/#icon-No9259" target="_blank">Fighter Jet</a> designed by <a href="http://thenounproject.com/lukefirth" target="_blank">Luke Anthony Firth</a> from The Noun Project.
