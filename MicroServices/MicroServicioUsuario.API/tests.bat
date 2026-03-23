@echo off

dotnet sonarscanner begin ^
/k:"mi_proyecto" ^
/d:sonar.host.url="http://localhost:9000" ^
/d:sonar.token="sqa_daee41845a8d83be4a4fce1654deccdc19e19c55" ^
/d:sonar.cs.opencover.reportsPaths="MicroServicioUser.Tests/TestResults/coverage/coverage.opencover.xml"

dotnet build MicroServicioUsuario.API.sln

dotnet test MicroServicioUser.Tests/MicroServicioUser.Tests.csproj ^
/p:CollectCoverage=true ^
/p:CoverletOutput=./TestResults/coverage/ ^
/p:CoverletOutputFormat=opencover

dotnet sonarscanner end /d:sonar.token="sqa_daee41845a8d83be4a4fce1654deccdc19e19c55