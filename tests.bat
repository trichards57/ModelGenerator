mkdir CoverageReport\History

packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"packages\xunit.runner.console.2.1.0\tools\xunit.console.exe" -targetargs:"ModelGenerator.Tests\bin\Debug\ModelGenerator.Tests.dll -noshadow" -filter:"+[ModelGenerator]*" -output:coverage.xml -coverbytest:"ModelGenerator.Tests\bin\Debug\*.tests.dll" -skipautoprops

if "%CI%" == "True" packages\coveralls.io.1.3.4\tools\coveralls.net.exe --opencover coverage.xml
if "%CI%" == "" packages\ReportGenerator.2.5.2\tools\ReportGenerator.exe -reports:coverage.xml -targetdir:CoverageReport -historydir:CoverageReport\History 