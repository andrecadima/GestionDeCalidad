@echo off

dotnet sonarscanner begin ^
/k:"GestionArancelesEstablishmentAPI" ^
/d:sonar.host.url="http://localhost:9000" ^
/d:sonar.token="sqa_57d254a558e41314c5db5c1431e5c6ef8c8348fd" ^
/d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml" ^
/d:sonar.exclusions="**/Establishment.Inf/**"

dotnet build Establishment.API.sln

dotnet test Establishment.API.sln ^
/p:CollectCoverage=true ^
/p:CoverletOutputFormat=opencover

dotnet sonarscanner end ^
/d:sonar.token="sqa_57d254a558e41314c5db5c1431e5c6ef8c8348fd"