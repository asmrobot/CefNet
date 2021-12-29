using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CppAst;

namespace CefGen
{
	partial class Program
	{
		/// <summary>
		/// Workaround for <see href="https://github.com/microsoft/STL/issues/1300"/>.
		/// </summary>
		/// <param name="tempIncludePath"></param>
		static partial void FixSTL1300(string tempIncludePath, List<string> files)
		{
			string path = FindVS168IncludePath();
			if (path == null)
				return;

			path = Path.Combine(path, "intrin0.h");
			if (!File.Exists(path))
				return;

			string content = File.ReadAllText(path, Encoding.UTF8);

			const string a = @"#ifdef __clang__
// This looks like a circular include but it is not because clang overrides <intrin.h> with their specific version.
// See further discussion in LLVM-47099.
#include <intrin.h>
#else /* ^^^ __clang__ // !__clang__ vvv */";

			const string b = "#endif /* ^^^ !__clang__ */";

			if (content.Contains(a) && content.Contains(b))
			{
				content = content.Replace(a, string.Empty).Replace(b, string.Empty);

				const string c = @"__MACHINEX86_X64(unsigned int _tzcnt_u32(unsigned int))
__MACHINEX64(unsigned __int64 _tzcnt_u64(unsigned __int64))";

				content = content.Replace(c, "#ifndef __clang__\n" + c + "\n#endif // __clang__");

				path = Path.Combine(tempIncludePath, "intrin0.h");
				File.WriteAllText(path, content, Encoding.UTF8);

				files.Insert(0, path);
			}
		}

		static partial void FixSTL1300RemoveIncludesAfterParse(CppContainerList<CppFunction> functions)
		{
			for (int i = functions.Count - 1; i >= 0; i--)
			{
				if (Path.GetFileName(functions[i].SourceFile) == "intrin0.h")
				{
					functions.RemoveAt(i);
				}
			}
		}

		private static string FindVS168IncludePath()
		{
			string vspath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019";
			if (!Directory.Exists(vspath))
			{
				vspath = @"C:\Program Files\Microsoft Visual Studio\2019";
				if (!Directory.Exists(vspath))
					return null;
			}

			foreach (string vsedition in new[] { "Enterprise", "Professional", "Community", "Preview" })
			{
				string includePath = Path.Combine(vspath, vsedition, @"VC\Tools\MSVC\14.28.29333\include");
				if (Directory.Exists(includePath))
					return includePath;
			}
			return null;
		}
	}
}
