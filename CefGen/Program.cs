// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CppAst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CefGen
{
	partial class Program
	{
		private static readonly HashSet<string> IgnoreClasses = new HashSet<string>
		{
			"CefStringTraitsWide",
			"CefStringTraitsUTF8",
			"CefStringTraitsUTF16",
			"CefStringBase",
			"_cef_main_args_t",
			"_cef_window_info_t",
			"_XEvent",
			"_XDisplay",

		};

		static void Main(string[] args)
		{
			string cefPath = null;
			string outDirPath = null;
			string projectPath = GetProjectPath();

			bool onlyStdCall = false;
			foreach (string arg in args)
			{
				if (arg.StartsWith("--"))
				{
					if (arg.StartsWith("--out="))
						outDirPath = arg.Substring(6);
					if (arg == "--stdcall")
						onlyStdCall = true;
					continue;
				}
				else if (cefPath == null)
				{
					cefPath = arg;
				}
			}

			if (string.IsNullOrWhiteSpace(cefPath))
				cefPath = Path.Combine(projectPath, "..", "cef");
			else
				cefPath = cefPath.Trim();
			if (!Directory.Exists(cefPath))
			{
				Console.WriteLine("Could not find the CEF directory.");
				Environment.Exit(-1);
				return;
			}

			if (string.IsNullOrWhiteSpace(outDirPath))
				outDirPath = Path.Combine(projectPath, "..", "CefNet", "Generated");
			else
				outDirPath = outDirPath.Trim();
			if (!Directory.Exists(Path.GetDirectoryName(outDirPath)))
			{
				Console.WriteLine("Could not create the output directory.");
				Environment.Exit(-1);
				return;
			}

			Clean(outDirPath);

			Console.WriteLine("Generate unsafe types...");
			GenerateFromCHeaders(cefPath, outDirPath, onlyStdCall);

			Console.WriteLine("Compile unsafe types...");
			var nativeTypes = new NativeCefApiTypes(outDirPath);
			nativeTypes.Build();

			Console.WriteLine("Generate wrappers...");
			var managedApiBuilder = new ManagedCefApiBuilder(outDirPath);
			managedApiBuilder.Imports.Add("CefNet.CApi");
			managedApiBuilder.Imports.Add("CefNet.Internal");
			managedApiBuilder.EnableCallbackOverrideCheck = true;
			managedApiBuilder.GenerateFrom(nativeTypes);

			Console.WriteLine("Generate glue classes...");
			var cefnetGen = new CefNetCodeGen(outDirPath);
			cefnetGen.Generate();

			Console.WriteLine("Compile wrappers...");
			var managedTypes = new ManagedCefApiTypes(outDirPath);
			managedTypes.Build();

			Console.WriteLine("Generate MSIL for wrappers...");
			var msilGen = new ManagedCefApiMsilCodeGen(outDirPath);
			msilGen.GenerateFrom(managedTypes);

			Console.WriteLine("Complete.");
		}

		private static string GetProjectPath()
		{
			string projectPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
			string rootPath = Path.GetPathRoot(projectPath);
			while (Path.GetFileName(projectPath) != "CefGen")
			{
				if (projectPath == rootPath)
					throw new DirectoryNotFoundException("Could not find the project directory.");
				projectPath = Path.GetDirectoryName(projectPath);
			}
			return projectPath;
		}

		private static string GetApiHash(string cefPath)
		{
			foreach (string line in File.ReadAllLines(Path.Combine(cefPath, "include", "cef_api_hash.h")))
			{
				if (line.StartsWith("#define CEF_API_HASH_UNIVERSAL"))
				{
					return Regex.Match(line, "\"[a-z0-9]+\"").Value.Trim('"');
				}
			}
			Console.WriteLine("API hash not found.");
			Environment.Exit(-1);
			return null;
		}

		private static string ApplyHotPatch(string basePath, List<string> files)
		{
			string temp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
			if (Directory.Exists(temp))
			{
				Directory.Delete(temp, true);
				Thread.Sleep(1000);
			}
			Directory.CreateDirectory(temp);
			Directory.CreateDirectory(Path.Combine(temp, "include"));
			Directory.CreateDirectory(Path.Combine(temp, "include", "internal"));

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				string path = Path.Combine(basePath, "include", "internal", "cef_types_linux.h");
				if (File.Exists(path))
				{
					string content = File.ReadAllText(path, Encoding.UTF8);
					content = content.Replace("#define cef_cursor_handle_t unsigned long", "typedef unsigned long HCURSOR;\n#define cef_cursor_handle_t HCURSOR");
					content = content.Replace("#define cef_window_handle_t unsigned long", "typedef unsigned long HWND;\n#define cef_window_handle_t HWND");
					content = content.Replace("#define cef_event_handle_t XEvent*", "typedef XEvent* CefEventHandle;\n#define cef_event_handle_t CefEventHandle");
					File.WriteAllText(Path.Combine(temp, "include", "internal", "cef_types_linux.h"), content, Encoding.UTF8);
				}
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				string path = Path.Combine(basePath, "include", "internal", "cef_types_win.h");
				if (File.Exists(path))
				{
					string content = File.ReadAllText(path, Encoding.UTF8);
					content = content.Replace("#define cef_event_handle_t MSG*", "typedef MSG* CefEventHandle;\n#define cef_event_handle_t CefEventHandle");
					File.WriteAllText(Path.Combine(temp, "include", "internal", "cef_types_win.h"), content, Encoding.UTF8);
				}
				FixSTL1300(temp, files);
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				string path = Path.Combine(basePath, "include", "internal", "cef_types_mac.h");
				if (File.Exists(path))
				{
					string content = File.ReadAllText(path, Encoding.UTF8);
					content = content.Replace("#define cef_cursor_handle_t void*", "typedef void* HCURSOR;\n#define cef_cursor_handle_t HCURSOR");
					content = content.Replace("#define cef_window_handle_t void*", "typedef void* HWND;\n#define cef_window_handle_t HWND");
					content = content.Replace("#define cef_event_handle_t void*", "typedef void* CefEventHandle;\n#define cef_event_handle_t CefEventHandle");
					File.WriteAllText(Path.Combine(temp, "include", "internal", "cef_types_mac.h"), content, Encoding.UTF8);
				}
			}
			return temp;
		}

		private static void GenerateFromCHeaders(string basePath, string outDirPath, bool onlyStdCall)
		{
			var options = new CppParserOptions();
			var files = new List<string>(Directory.GetFiles(Path.Combine(basePath, "include", "capi"), "*.h"));
			files.Add(Path.Combine(basePath, "include", "cef_version.h"));
			files.Add(Path.Combine(basePath, "include", "cef_api_hash.h"));
			options.IncludeFolders.Add(ApplyHotPatch(basePath, files));
			options.IncludeFolders.Add(basePath);

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				options.IncludeFolders.Add("/usr/include/clang/8/include");
				options.TargetAbi = "gnu";
				options.TargetCpu = CppTargetCpu.X86_64;
				options.TargetSystem = "linux";
				options.TargetVendor = "pc";
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				//options.Defines.Add("_ALLOW_COMPILER_AND_STL_VERSION_MISMATCH");
			}
			else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				//options.TargetCpu = CppTargetCpu.X86_64;
				options.TargetSystem = "darwin";
				options.TargetVendor = "apple";
				options.SystemIncludeFolders.Add("/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/include");
				options.SystemIncludeFolders.Add("/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/include/c++/v1");
				options.SystemIncludeFolders.Add("/Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX.sdk/usr/include");
				options.SystemIncludeFolders.Add("/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/lib/clang/11.0.0/include");
				options.AdditionalArguments.Add("-stdlib=libc++");
			}

			var nativeBuild = new NativeCefApiBuilder(onlyStdCall)
			{
				Namespace = "CefNet.CApi",
				BaseDirectory = basePath
			};

			var enumBuild = new NativeCefApiBuilder(onlyStdCall)
			{
				Namespace = "CefNet",
				BaseDirectory = basePath
			};

			CppCompilation compilation = CppParser.ParseFiles(files, options);
			if (compilation.HasErrors)
			{
				foreach(var msg in compilation.Diagnostics.Messages)
					Console.WriteLine(msg);

				Environment.Exit(-1);
				return;
			}
			var aliasResolver = new AliasResolver(compilation);
			nativeBuild.ResolveCefTypeDef += aliasResolver.HandleResolveEvent;
			enumBuild.ResolveCefTypeDef += aliasResolver.HandleResolveEvent;
			Func<string, string> resolveType = aliasResolver.ResolveNonFail;

			foreach (CppClass @class in compilation.Classes)
			{
				string source = @class.Span.Start.File;
				if (!source.Contains("capi") && !source.Contains("internal"))
					continue;

				if (IgnoreClasses.Contains(@class.Name))
					continue;

				string fileName = aliasResolver.ResolveNonFail(@class.Name);
				using (var csfile = new StreamWriter(Path.Combine(outDirPath, "Native", "Types", fileName + ".cs"), false, Encoding.UTF8))
				//using (var ilfile = new StreamWriter(Path.Combine(outDirPath, "Native", "MSIL", fileName + ".il"), false, Encoding.UTF8))
				{
					//nativeBuild.Format(@class, csfile, ilfile);
					nativeBuild.Format(@class, csfile, null);
					csfile.Flush();
					//ilfile.Flush();
				}
			}

			foreach (CppTypedef typedef in compilation.Typedefs)
			{
				string name = typedef.Name;
				if (name.StartsWith("cef_string_")
					|| name == "cef_color_t"
					//|| name == "cef_platform_thread_id_t"
					|| name == "cef_platform_thread_handle_t")
				{
					var sb = new StringBuilder();
					try
					{
						var w = new StringWriter(sb);
						nativeBuild.Format(typedef, w);
						w.Flush();
						w.Dispose();
					}
					catch (InvalidOperationException)
					{
						continue;
					}
					string fileName = aliasResolver.ResolveNonFail(typedef.Name);
					File.WriteAllText(Path.Combine(outDirPath, "Native", "Typedefs", fileName + ".cs"), sb.ToString(), Encoding.UTF8);
				}
			}

			foreach (CppEnum @enum in compilation.Enums)
			{
				string fileName = aliasResolver.ResolveNonFail(@enum.Name);
				using (var csfile = new StreamWriter(Path.Combine(outDirPath, "Managed", "Enums", fileName + ".cs"), false, Encoding.UTF8))
				{
					enumBuild.Format(@enum, csfile);
					csfile.Flush();
				}
			}

			FixSTL1300RemoveIncludesAfterParse(compilation.Functions);

			var api = new CefApiClass("CefNativeApi")
			{
				Functions = compilation.Functions,
				ApiHash = GetApiHash(basePath)
			};

			using (var csfile = new StreamWriter(Path.Combine(outDirPath, "Native", "CefNativeApi.cs"), false, Encoding.UTF8))
			{
				nativeBuild.Format(api, csfile);
				csfile.Flush();
			}

		}

		private static void Clean(string path)
		{
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}
			Directory.CreateDirectory(path);
			Thread.Sleep(1000);
			Directory.CreateDirectory(Path.Combine(path, "Managed"));
			Directory.CreateDirectory(Path.Combine(path, "Managed", "MSIL"));
			Directory.CreateDirectory(Path.Combine(path, "Managed", "Types"));
			Directory.CreateDirectory(Path.Combine(path, "Managed", "Enums"));
			Directory.CreateDirectory(Path.Combine(path, "Managed", "Internal"));
			Directory.CreateDirectory(Path.Combine(path, "Native"));
			Directory.CreateDirectory(Path.Combine(path, "Native", "MSIL"));
			Directory.CreateDirectory(Path.Combine(path, "Native", "Types"));
			Directory.CreateDirectory(Path.Combine(path, "Native", "Typedefs"));
		}

		static partial void FixSTL1300(string tempIncludePath, List<string> files);
		static partial void FixSTL1300RemoveIncludesAfterParse(CppContainerList<CppFunction> functions);

	}




}
