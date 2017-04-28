del duplicates-report.txt

cd ModelGenerator

..\tools\simian\simian-2.4.0 -excludes=**/*.g.* **/*.cs  >> ..\duplicates-report.txt


cd ..\ModelGenerator.Tests

..\tools\simian\simian-2.4.0 -excludes=**/*.g.* **/*.cs  >> ..\duplicates-report.txt

cd ..