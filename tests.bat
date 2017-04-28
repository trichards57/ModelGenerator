
mkdir tools
mkdir reports\coverage\historydir

nuget install OpenCover -ExcludeVersion -OutputDirectory tools
nuget install xunit.runner.console -ExcludeVersion -OutputDirectory tools
nuget install ReportGenerator -ExcludeVersion -OutputDirectory tools


dotnet build 

tools\OpenCover\tools\OpenCover.Console.exe -target:"tools\xunit.runner.console\tools\xunit.console.x86.exe" -targetargs:"ModelGenerator.Tests\bin\Debug\net46\ModelGenerator.Tests.dll -noshadow" -register:user -output:"reports\coverage\coverage.xml" -skipautoprops -filter:"+[ModelGenerator]*"  -excludebyattribute:*.ExcludeFromCodeCoverage* -mergebyhash -returntargetcode -coverbytest:*.Tests.*
tools\ReportGenerator\tools\ReportGenerator.exe -reports:reports\coverage\coverage.xml -targetdir:reports\coverage -historydir:reports\coverage\history
