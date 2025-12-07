@echo off
"C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.39f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\SDK\\cmake\\3.22.1\\bin\\cmake.exe" ^
  "-HD:\\3.CreateGame\\PrisonOfTheKing\\Library\\Bee\\Android\\Prj\\IL2CPP\\Gradle\\unityLibrary\\src\\main\\cpp" ^
  "-DCMAKE_SYSTEM_NAME=Android" ^
  "-DCMAKE_EXPORT_COMPILE_COMMANDS=ON" ^
  "-DCMAKE_SYSTEM_VERSION=23" ^
  "-DANDROID_PLATFORM=android-23" ^
  "-DANDROID_ABI=arm64-v8a" ^
  "-DCMAKE_ANDROID_ARCH_ABI=arm64-v8a" ^
  "-DANDROID_NDK=C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.39f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\NDK" ^
  "-DCMAKE_ANDROID_NDK=C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.39f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\NDK" ^
  "-DCMAKE_TOOLCHAIN_FILE=C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.39f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\NDK\\build\\cmake\\android.toolchain.cmake" ^
  "-DCMAKE_MAKE_PROGRAM=C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.39f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\SDK\\cmake\\3.22.1\\bin\\ninja.exe" ^
  "-DCMAKE_LIBRARY_OUTPUT_DIRECTORY=D:\\3.CreateGame\\PrisonOfTheKing\\Library\\Bee\\Android\\Prj\\IL2CPP\\Gradle\\unityLibrary\\build\\intermediates\\cxx\\RelWithDebInfo\\2422481g\\obj\\arm64-v8a" ^
  "-DCMAKE_RUNTIME_OUTPUT_DIRECTORY=D:\\3.CreateGame\\PrisonOfTheKing\\Library\\Bee\\Android\\Prj\\IL2CPP\\Gradle\\unityLibrary\\build\\intermediates\\cxx\\RelWithDebInfo\\2422481g\\obj\\arm64-v8a" ^
  "-DCMAKE_BUILD_TYPE=RelWithDebInfo" ^
  "-DCMAKE_FIND_ROOT_PATH=D:\\3.CreateGame\\PrisonOfTheKing\\.utmp\\RelWithDebInfo\\2422481g\\prefab\\arm64-v8a\\prefab" ^
  "-BD:\\3.CreateGame\\PrisonOfTheKing\\.utmp\\RelWithDebInfo\\2422481g\\arm64-v8a" ^
  -GNinja ^
  "-DANDROID_STL=c++_shared" ^
  "-DANDROID_SUPPORT_FLEXIBLE_PAGE_SIZES=ON"
