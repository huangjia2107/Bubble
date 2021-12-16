@echo off

:: utf-8
Chcp 65001>nul

:: 发布目录
set publish_path=..\publish

:: 发布目录下 App 目录
set publish_app_path=%publish_path%\App
:: 发布目录下 Package 目录
set publish_pack_path=%publish_path%\Package

:: 1.删除原发布目录
if exist %publish_path% (
    del /s/q %publish_path%\*.*
    rd /s/q %publish_path%
)

:: 2.发布 Demo
dotnet publish -c Release -f netcoreapp3.1 --no-self-contained -o %publish_app_path%\ ..\Bubble.sln

:: 3.发布 Package
dotnet pack -c Release -o %publish_pack_path%\ ..\src\Bubble\Bubble.csproj

@IF %ERRORLEVEL% NEQ 0 pause