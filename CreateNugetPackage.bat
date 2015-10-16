.nuget\nuget.exe restore Semiodesk.TinyVirtuoso.sln
MSBuild.exe /p:Configuration=Release /t:Rebuild "/p:Platform=Any CPU" Semiodesk.TinyVirtuoso.sln

mkdir NuGet\TinyVirtuoso\lib\Net35\
copy /y Build\Release\TinyVirtuoso.dll NuGet\TinyVirtuoso\lib\Net35\TinyVirtuoso.dll


.nuget\nuget.exe pack NuGet\TinyVirtuoso\TinyVirtuoso.nuspec
.nuget\nuget.exe pack NuGet\TinyVirtuoso.win\TinyVirtuoso.win.nuspec
.nuget\nuget.exe pack NuGet\TinyVirtuoso.osx\TinyVirtuoso.osx.nuspec
