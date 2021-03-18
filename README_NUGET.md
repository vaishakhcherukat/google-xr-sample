# Nuget Packages

## Setup

Nuget Packages were installed by adapting these steps to work with `Visual Studio for Mac`:
https://www.what-could-possibly-go-wrong.com/unity-and-nuget/

### .NET Preparation

- The article has steps to configure Unity to use a different version of the .NET framework. This step doesn't seem necessary in Unity_2019 as it uses `.Net standard 2.0`
`nuget.config`

- The article suggests using `".\Assets\packages"` as the nuget repository path in `nuget.config`. That didn't seem to work as NuGet installed several versions of the same DLL name. Instead we used `<add key="repositoryPath" value="NugetPackages" />`

## Adding a package

- Open Visual Studio
- Project->Manage NuGet Packages`

This should install the package and its dependencies to `./NugetPackages` in the project root. 

From the project root:
- `find NugetPackages -name '*dll' | grep 'netstandard2.0'`
- Copy the new DLLs to `Assets/NugetPackages` 
- Some packages don't have a DLL for netstandard2.0. In that case try finding the previous version number closest to netstandard2.0

 Clean Unity's cache (from project root):
- Close Unity.
- `rm -rf Library`
- Restart Unity.

Connect the package to the assembly definitions that will use that code. Example: to use Moq.dll:
  - select Assets/_tests/_tests.asmdef from project root
  - Under `Assembly References` click `+` to add the DLL
  - Choose the DLL from dropdown list (moq.dll)
  - Remember to hit `Apply` at the bottom of the Inspector tab.

