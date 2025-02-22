[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Tag
)

if ($Tag -eq "") {
    throw "Missing tag"
}

git tag $Tag
Get-ChildItem -Path *.nupkg -Recurse | Remove-Item

dotnet pack -c Release

Get-ChildItem -Path *.nupkg -Recurse | Foreach-Object {
    nuget add $_ -Source $Env:LOCAL_NUGET_PATH
}