call "c:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat"


MSbuild /target:%1 /property:teamcity_build_checkoutDir=.. /property:teamcity_dotnet_nunitlauncher_msbuild_task="notthere" /property:BUILD_NUMBER="0.1.345.abcd" /property:Minor="1" build.win.proj
PAUSE

REM #/verbosity:detailed