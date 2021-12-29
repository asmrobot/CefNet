# CefNet

CefNet is a .NET CLR binding for the Chromium Embedded Framework (CEF).

## Getting Started

Since CefNet is a wrapper, you need to have the Chromium Embedded Framework
somewhere on your development system (and redistribute it with your application).

(1) Download the Chromium Embedded Framework from:
	https://cef-builds.spotifycdn.com/index.html

*Note: The major and minor version numbers of CEF and CefNet must match.*

(2) Create an instance of the CefNetApplication and initialize it with your settings:
```C#
var settings = new CefSettings();
settings.NoSandbox = true;
settings.MultiThreadedMessageLoop = false; // or true
settings.WindowlessRenderingEnabled = true;
settings.LocalesDirPath = "path_to_cef/locales";
settings.ResourcesDirPath = "path_to_cef";

var app = new CefNetApplication();
app.Initialize("path_to_cef", settings);
```

(3) Add a WebView control to the form of your application.

(4) Run event loop, for example:
```C#
CefNetApplication.Run();
```
*Note: You can use the event loop of the UI-framework you are using.*

(5) You need to explicitly call `CefNetApplication.Shutdown()` from the main
thread before you exit your application:
```C#
app.Shutdown();
app.Dispose();
```

For more information, see the sample applications.

## Features

* Cross-platform
* Full managed code

## Warning
The API of this project is not frozen and is subject to change.

## Develop

1. Install [.NET Core SDK](https://www.microsoft.com/net/download)
2. Install the [DotAsm](https://www.nuget.org/packages/DotAsm/) tool: `dotnet tool install -g DotAsm`
3. Run `git clone https://github.com/CefNet/CefNet.git`
4. Download a [CEF package](https://cef-builds.spotifycdn.com/index.html). See [Directory.Build.props](Directory.Build.props) for the required CEF version.
5. Extract all files into the cef/ directory.
6. Copy cef/Resources/icudtl.dat into cef/Release/.

## Migration to other CEF build
1. Download a [CEF package (standard or minimal)](https://cef-builds.spotifycdn.com/index.html).
2. Extract all header files into the cef/include directory.
3. Build and run CefGen.sln in debug mode to generate the generated files. Watch the output for errors.
4. Build CefNet.sln
5. If the build fails, make the necessary changes.

## Similar projects and links

* [CefGlue](https://gitlab.com/xiliumhq/chromiumembedded/cefglue): An alternative .NET CEF wrapper built using P/Invoke.
* [CefSharp](https://github.com/cefsharp/CefSharp): Another .NET CEF wrapper built using C++/CLI.
* [CEF Bitbucket Project](https://bitbucket.org/chromiumembedded/cef/overview): The official CEF issue tracker
* The official CEF Forum: http://magpcss.org/ceforum/
* CEF API Docs: http://magpcss.org/ceforum/apidocs3/index-all.html
