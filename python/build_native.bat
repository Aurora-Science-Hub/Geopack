@echo off
REM Builds the NativeAOT shared library for the current platform.
REM
REM   .\python\build_native.bat              auto-detect RID from the host
REM   set RID=win-x64 && .\python\build_native.bat
REM
REM Output: python\out\win-x64\geopack.dll
setlocal

set "SCRIPT_DIR=%~dp0"
set "REPO_ROOT=%SCRIPT_DIR%.."

if "%RID%"=="" (
  REM Use the native architecture even when running under WoW64.
  set "NATIVE_ARCH=%PROCESSOR_ARCHITECTURE%"
  if defined PROCESSOR_ARCHITEW6432 set "NATIVE_ARCH=%PROCESSOR_ARCHITEW6432%"
  set "RID=win-x86"
  if "%NATIVE_ARCH%"=="AMD64" set "RID=win-x64"
  if "%NATIVE_ARCH%"=="ARM64" set "RID=win-arm64"
)

set "OUT_DIR=%SCRIPT_DIR%out\%RID%"
if not exist "%OUT_DIR%" mkdir "%OUT_DIR%"

REM PublishAot/IsAotCompatible come from src\Directory.Build.props,
REM NativeLib=Shared from src\Geopack.Native\Geopack.Native.csproj.
dotnet publish "%REPO_ROOT%src\Geopack.Native\Geopack.Native.csproj" ^
  -c Release ^
  -r "%RID%" ^
  -o "%OUT_DIR%"
if errorlevel 1 exit /b %errorlevel%

echo.
echo Native library (%RID%):
dir /b "%OUT_DIR%"
endlocal
