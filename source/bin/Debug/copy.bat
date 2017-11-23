@echo off
setlocal
set PATH="..\..\..\Assemblies\"
cd /d %~dp0
copy /y *.dll %PATH%
endlocal