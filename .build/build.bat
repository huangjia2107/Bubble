@echo off

:: utf-8
Chcp 65001>nul

dotnet build -c Release ..\Bubble.sln

@IF %ERRORLEVEL% NEQ 0 pause