
mkdir tools
mkdir reports\coverage\historydir

nuget install OpenCover -ExcludeVersion -OutputDirectory tools
nuget install xunit.runner.console -ExcludeVersion -OutputDirectory tools
nuget install ReportGenerator -ExcludeVersion -OutputDirectory tools
nuget install SpecFlow -ExcludeVersion -OutputDirectory tools
nuget install Pickles.CommandLine -ExcludeVersion -OutputDirectory tools

cd ModelGenerator.Test

..\tools\SpecFlow\tools\specflow.exe generateall ModelGenerator.Test.csproj

cd ..

dotnet build 

tools\OpenCover\tools\OpenCover.Console.exe -target:"tools\xunit.runner.console\tools\xunit.console.x86.exe" -targetargs:"ModelGenerator.Test\bin\Debug\ModelGenerator.Test.dll -noshadow" -register:user -output:"reports\coverage\coverage.xml" -skipautoprops -filter:"+[ModelGenerator]*"  -excludebyattribute:*.ExcludeFromCodeCoverage* -mergebyhash -returntargetcode -coverbytest:*.Test.dll
tools\ReportGenerator\tools\ReportGenerator.exe -reports:reports\coverage\coverage.xml -targetdir:reports\coverage -historydir:reports\coverage\history
tools\Pickles.CommandLine\tools\pickles.exe --feature-directory=.\ModelGenerator.Test --output-directory=.\docs