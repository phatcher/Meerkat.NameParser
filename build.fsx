/// FAKE Build script

#r "packages/build/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile
open Fake.Git
open Fake.NuGetHelper
open Fake.RestorePackageHelper
open Fake.ReleaseNotesHelper

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

let nunitPath = toolsDir @@ "NUnit-2.6.3/bin"

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir;]
)

Target "PackageRestore" (fun _ ->
    RestorePackages()
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
    !! (buildDir + "/*.Test.dll")
    |> NUnit (fun p ->
       {p with
          ToolPath = nunitPath
          DisableShadowCopy = true
          OutputFile = buildDir @@ "TestResults.xml"})
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