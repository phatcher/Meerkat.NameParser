/// FAKE Build script

#r "packages/build/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.Git
open Fake.NuGetHelper
open Fake.RestorePackageHelper
open Fake.ReleaseNotesHelper
open Fake.Testing

// Version info
let projectName = "Meerkat.NameParser"
let projectSummary = ""
let projectDescription = "Parses personal names into their constituent parts"
let authors = ["Paul Hatcher"]

let release = LoadReleaseNotes "RELEASE_NOTES.md"

// Properties
let buildDir = "./build"
let toolsDir = getBuildParamOrDefault "tools" "./tools"
let nugetDir = "./nuget"
let solutionFile = "Meerkat.NameParser.sln"

let nunitPath = "./packages/build/NUnit.ConsoleRunner/tools/nunit3-console.exe"

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir;]
)

Target "PackageRestore" (fun _ ->
    !! solutionFile
    |> MSBuildRelease buildDir "Restore"
    |> Log "AppBuild-Output: "
)

Target "SetVersion" (fun _ ->
    let commitHash = Information.getCurrentHash()
    let infoVersion = String.concat " " [release.AssemblyVersion; commitHash]
    CreateCSharpAssemblyInfo "./code/SolutionInfo.cs"
        [Attribute.Version release.AssemblyVersion
         Attribute.FileVersion release.AssemblyVersion
         Attribute.InformationalVersion infoVersion]
)

Target "Build" (fun _ ->
    !! solutionFile
    |> MSBuildRelease buildDir "Build"
    |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    // Exclude the package integrated version as it will find the wrong version in the build directory
    !! (buildDir + "/Meerkat.NameParser.Test.dll")
    |> NUnit3 (fun p ->
       {p with
          ToolPath = nunitPath
          })
)

Target "Pack" (fun _ ->
    let nugetParams p = 
      { p with 
          Authors = authors
          Version = release.AssemblyVersion
          ReleaseNotes = release.Notes |> toLines
          OutputPath = buildDir 
          AccessKey = getBuildParamOrDefault "nugetkey" ""
          Publish = hasBuildParam "nugetkey" }

    NuGet nugetParams "nuget/Meerkat.NameParser.nuspec"
)

Target "Release" (fun _ ->
    let tag = String.concat "" ["v"; release.AssemblyVersion] 
    Branches.tag "" tag
    Branches.pushTag "" "origin" tag
)

Target "Default" DoNothing

// Dependencies
"Clean"
    ==> "SetVersion"
    ==> "PackageRestore"
    ==> "Build"
    ==> "Test"
    ==> "Default"
    ==> "Pack"
    ==> "Release"

RunTargetOrDefault "Default"