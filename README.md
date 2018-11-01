[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg?style=flat)](https://gitter.im/Fody/Fody)
[![NuGet Status](http://img.shields.io/nuget/v/JetBrainsAnnotations.Fody.svg?style=flat)](https://www.nuget.org/packages/JetBrainsAnnotations.Fody/)
[![Build status](https://ci.appveyor.com/api/projects/status/da0rw28krhjubnf7?svg=true)](https://ci.appveyor.com/project/tom-englert/jetbrainsannotations-fody)

## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Icon](https://raw.github.com/Fody/JetBrainsAnnotations/master/package_icon.png)

Converts all JetBrains ReSharper code annotation attributes to [External Annotations](https://www.jetbrains.com/help/resharper/Code_Analysis__External_Annotations.html), 
so you can provide R# annotations to 3rd parties but don't need to deploy JetBrainsAnnotations.dll. 


## NuGet installation

Install the [JetBrainsAnnotations.Fody NuGet package](https://nuget.org/packages/JetBrainsAnnotations.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```
PM> Install-Package JetBrainsAnnotations.Fody
PM> Update-Package Fody
```

The `Update-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


## What are JetBrains Annotations

The assembly JetBrainsAnnotations.dll is shipped as a [nuget package](https://www.nuget.org/packages/JetBrains.Annotations/).
It provides standard JetBrains ReSharper code annotation attribute implementations. 
This allows you to better leverage the ReSharper intellisense.

To provide the annotations to 3rd parties you must define [`JETBRAINS_ANNOTATIONS`](https://www.jetbrains.com/help/resharper/Code_Analysis__Annotations_in_Source_Code.html) to include the attributes in your assembly.
However now you have a reference and would need to ship the JetBrainsAnnotations.dll with your product. 

This Fody plugin adds the define [`JETBRAINS_ANNOTATIONS`](https://www.jetbrains.com/help/resharper/Code_Analysis__Annotations_in_Source_Code.html) 
to your existing defines, then converts all attributes to an external annotations XML file on the fly, and then removes the reference to 
`JetBrains.Annotations` again so you can ship just the text file with your assembly and don't need to reference JetBrainsAnnotations.

For more information 

 * http://www.jetbrains.com/resharper/webhelp/Code_Analysis__External_Annotations.html 
 * http://www.jetbrains.com/resharper/features/code_analysis.html#Annotated_Framework


## What it actually does to your assembly

 * For each attribute defined in JetBrainsAnnotations.dll it adds an entry to your
   assemblies external annotations XML file and removes the usage of the attribute from your assembly.
 * Removes the reference to JetBrainsAnnotations.dll

## What it actually does to your project

* Updates `<project name>.ExternalAnnotations.xml` every time you compile.

  NOTE: To make your annotations available to 3rd parties, you must ship this file along with the assembly.

* If your project is set to generate an XML documentation file, the documentation is extended with 
  the annotation attributes. 
  

## What you may need to change manually

* Add the `<project name>.ExternalAnnotations.xml` to your project manually and mark it as content + copy to output.
* Mark the reference to `Jetbrains.Annotations.dll` as `Copy Local => False`, so it won't get copied to your 
  target directory and eventually get picked up by installers.
* If you deploy your project as a NuGet package, add `developmentDependency="true"` to the 
  JetBrains.Annotations package entry in your projects `packages.config` files, else NuGet will list JetBrains.Annotations 
  as a dependency of your package:
    ```xml
    <package id="JetBrains.Annotations" version="11.0.0" targetFramework="net452" developmentDependency="true" />
    ```
  or if you are using a `PackageReference` in your project files, mark it as private assests:
    ```xml
    <PackageReference Include="JetBrains.Annotations" Version="*">
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    ```


## Icon

<a href="http://thenounproject.com/noun/fighter-jet/#icon-No9259" target="_blank">Fighter Jet</a> designed by <a href="http://thenounproject.com/lukefirth" target="_blank">Luke Anthony Firth</a> from The Noun Project.
