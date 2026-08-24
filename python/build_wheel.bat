@echo off
REM Builds the native library, bundles it into the Python package, and produces a
REM platform-specific wheel in python\dist\.
REM
REM   .\python\build_wheel.bat
REM   pip install python\dist\geopack_2008-*.whl
REM
REM Requires: .NET SDK 10, a C toolchain (for NativeAOT), and Python 3.8+.
setlocal

set "SCRIPT_DIR=%~dp0"
set "PY_DIR=%SCRIPT_DIR%"
set "PKG_DIR=%PY_DIR%geopack"
set "DIST_DIR=%PY_DIR%dist"

REM 1. Native library ---------------------------------------------------------
call "%PY_DIR%build_native.bat"
if errorlevel 1 exit /b %errorlevel%

REM build_native.bat picks its own default RID; mirror the same detection here so
REM we can find the artifact.
if "%RID%"=="" (
  set "NATIVE_ARCH=%PROCESSOR_ARCHITECTURE%"
  if defined PROCESSOR_ARCHITEW6432 set "NATIVE_ARCH=%PROCESSOR_ARCHITEW6432%"
  set "RID=win-x86"
  if "%NATIVE_ARCH%"=="AMD64" set "RID=win-x64"
  if "%NATIVE_ARCH%"=="ARM64" set "RID=win-arm64"
)

set "OUT_DIR=%PY_DIR%out\%RID%"
if not exist "%OUT_DIR%\geopack.dll" (
  echo No native library found in %OUT_DIR% (expected geopack.dll) 1>&2
  exit /b 1
)

REM 2. Bundle into the package ------------------------------------------------
copy /y "%OUT_DIR%\geopack.dll" "%PKG_DIR%\" >nul

REM 3. Build the wheel --------------------------------------------------------
rmdir /s /q "%PY_DIR%build" 2>nul
rmdir /s /q "%DIST_DIR%" 2>nul
for /d %%d in ("%PY_DIR%*.egg-info") do rmdir /s /q "%%d" 2>nul
mkdir "%DIST_DIR%"

python -m build --version >nul 2>&1
if errorlevel 1 goto pipwheel
python -m build --wheel --outdir "%DIST_DIR%" "%PY_DIR%"
goto done

:pipwheel
python -m pip wheel --no-deps --wheel-dir "%DIST_DIR%" "%PY_DIR%"

:done
if errorlevel 1 exit /b %errorlevel%

echo.
echo Wheel:
dir /b "%DIST_DIR%\*.whl"
endlocal
