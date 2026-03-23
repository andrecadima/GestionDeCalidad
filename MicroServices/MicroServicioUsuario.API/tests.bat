@echo off

dotnet sonarscanner begin ^
/k:"mi_proyecto" ^
/d:sonar.host.url="http://localhost:9000" ^
/d:sonar.token="sqa_57d254a558e41314c5db5c1431e5c6ef8c8348fd" ^
/d:sonar.cs.opencover.reportsPaths="MicroServicioUser.Tests/TestResults/coverage/coverage.opencover.xml"

dotnet build MicroServicioUsuario.API.sln

dotnet test MicroServicioUser.Tests/MicroServicioUser.Tests.csproj ^
/p:CollectCoverage=true ^
/p:CoverletOutput=./TestResults/coverage/ ^
/p:CoverletOutputFormat=opencover

dotnet sonarscanner end /d:sonar.token="sqa_57d254a558e41314c5db5c1431e5c6ef8c8348fd"