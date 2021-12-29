// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using CppAst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace CefGen
{

	sealed class NativeCefApiBuilder : CefApiBuilderBase
	{
		private readonly bool _onlyStdCall;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="onlyStdCall">Use STDCALL on linux</param>
		public NativeCefApiBuilder(bool onlyStdCall)
		{
			_onlyStdCall = onlyStdCall;
		}

		protected override MsilCodeGenBase CreateMsilCodeGen()
		{
			return new NativeCefApiMsilCodeGen(_onlyStdCall);
		}

		protected override void BuildTypedef(CodeNamespace ns, CppTypedef typedef)
		{
			TypeDesc fieldType = GetTypeDesc(typedef.ElementType);
			string baseTypeName = ResolveCefType(fieldType.ToString());
			string aliasTypeName = GetClassName(typedef.Name);
			if (aliasTypeName == baseTypeName)
				throw new InvalidOperationException();

			var decl = new CodeStruct(aliasTypeName);
			decl.Attributes = CodeAttributes.Public | CodeAttributes.Unsafe | CodeAttributes.Partial;
			decl.Comments.AddVSDocComment(typedef.Comment, "summary");

			var attr = new CustomCodeAttribute(typeof(StructLayoutAttribute));
			attr.AddParameter(LayoutKind.Sequential);
			decl.CustomAttributes.Add(attr);


			var field = new CodeField(baseTypeName, "Base");
			field.Attributes = CodeAttributes.Public;
			//field.CustomAttributes.Add(new CustomCodeAttribute(typeof(FieldOffsetAttribute)) { Parameters = { "0" } });
			decl.Members.Add(field);

			ns.Types.Add(decl);
		}

		protected override void BuildEnum(CodeNamespace ns, CppEnum @enum)
		{
			bool flags = false;
			var decl = new CodeEnum(GetClassName(@enum.Name));
			decl.Type = @enum.IntegerType.GetDisplayName();
			if (decl.Type != "int" && decl.Type != "unsigned int")
			{
				Console.WriteLine(decl.Type);
				throw new NotImplementedException();
			}
			decl.Attributes = CodeAttributes.Public;
			decl.Comments.AddVSDocComment(@enum.Comment, "summary");

			//bool first = true;
			//string pattern = @enum.Items.FirstOrDefault()?.Name ?? string.Empty;
			//foreach (CppEnumItem item in @enum.Items)
			//{
			//	while (!item.Name.StartsWith(pattern) && pattern.Length > 0)
			//	{
			//		pattern = pattern.Remove(pattern.Length - 1);
			//	}
			//	if (!first && char.IsDigit(item.Name[pattern.Length]))
			//	{
			//		pattern = null;
			//		break;
			//	}
			//	first = false;
			//}

			bool first = true;
			string pattern = @enum.Items.FirstOrDefault()?.Name ?? string.Empty;
			foreach (CppEnumItem item in @enum.Items)
			{
				while (pattern != null && !item.Name.StartsWith(pattern) && pattern.Length > 0)
				{
					//pattern = pattern.Remove(pattern.Length - 1);
					int split = pattern.TrimEnd('_').LastIndexOf('_') + 1;
					if (split == 0)
					{
						pattern = null;
						break;
					}
					pattern = pattern.Remove(split);
				}
				if (pattern != null && !first && char.IsDigit(item.Name[pattern.Length]))
				{
					pattern = null;
					break;
				}
				first = false;
			}

			foreach (CppEnumItem item in @enum.Items)
			{
				string name = pattern != null ? item.Name.Substring(pattern.Length).ToLowerInvariant().ToUpperCamel() : item.Name;
				string value = item.ValueExpression?.ToString();
				value = (string.IsNullOrEmpty(value) || value.StartsWith("(")) ? item.Value.ToString() : value;
				if (pattern != null)
				{
					string[] combined = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					if (combined.Length > 1)
					{
						for (int i = 0; i < combined.Length; i++)
						{
							string subVal = combined[i];
							if(subVal.StartsWith(pattern))
								combined[i] = subVal.Substring(pattern.Length).ToLowerInvariant().ToUpperCamel();
						}
						value = string.Join(' ', combined);
					}
					else if (value.StartsWith(pattern))
					{
						value = value.Substring(pattern.Length).ToLowerInvariant().ToUpperCamel();
					}
				}
				if (Regex.IsMatch(value, @"\d<<\d"))
					value = value.Replace("<<", " << ");
				flags |= value.Contains("<<");
				var itemDecl = new CodeEnumItem(name, value);
				itemDecl.Comments.AddVSDocComment(item.Comment, "summary");
				decl.Members.Add(itemDecl);
			}
			if (flags)
			{
				decl.CustomAttributes.Add(new CustomCodeAttribute(typeof(FlagsAttribute)));
			}
			ns.Types.Add(decl);
		}

		protected override void BuildClass(CodeNamespace ns, CppClass @class)
		{
			if (@class.TypeKind != CppTypeKind.StructOrClass
				|| @class.ClassKind != CppClassKind.Struct)
			{
				if (@class.Name == "CefStringBase")
					return;
				//if (@class.ClassKind == CppClassKind.Class)
				//	return;
				Console.WriteLine(@class.Name);
				throw new NotImplementedException();
			}

			string className = GetClassName(@class.Name);

			string sourceFile = @class.GetSourceFile();

			if (sourceFile.EndsWith("_capi.h"))
			{
				string fname = Path.GetFileName(sourceFile);
				fname = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(sourceFile)), fname.Remove(fname.Length - 7) + ".h");
				if (File.Exists(fname))
				{
					string cppClassName = className.EndsWith("_t") ? className.Remove(className.Length - 2).ToUpperCamel() : className.ToUpperCamel();
					string prevLine = null;
					foreach (string line in File.ReadLines(fname))
					{
						if (line.IndexOf("class " + cppClassName + " ", StringComparison.OrdinalIgnoreCase) != -1)
						{
							if (prevLine.Contains("source=library"))
							{
								Extensions.StructTypes.Add(className, CefSourceKind.Library);
							}
							else if (prevLine.Contains("source=client"))
							{
								Extensions.StructTypes.Add(className, CefSourceKind.Client);
							}
							break;
						}
						prevLine = line;
					}
				}
			}

			var decl = new CodeStruct(className);
			decl.Attributes = CodeAttributes.Public | CodeAttributes.Unsafe | CodeAttributes.Partial;
			decl.Comments.AddVSDocComment(@class.Comment, "summary");

			var attr = new CustomCodeAttribute(typeof(StructLayoutAttribute));
			attr.AddParameter(LayoutKind.Sequential);
			decl.CustomAttributes.Add(attr);


			//CLSCompliant

			if (@class.Functions.Count > 0)
			{
				if (@class.Fields.Count > 0)
					throw new NotSupportedException();

				foreach (CppFunction fn in @class.Functions)
				{
					DefineFunction(decl, fn);
				}
			}

			foreach (CppField field in @class.Fields)
			{
				DefineField(decl, field);
			}

			ns.Types.Add(decl);
		}

		protected override void BuildCefApi(CodeNamespace ns, CefApiClass @class)
		{
			var decl = new CodeClass(GetClassName(@class.Name));
			decl.Attributes = CodeAttributes.Public | CodeAttributes.Static | CodeAttributes.Partial;
			decl.Comments.AddVSDocComment(@class.Comment, "summary");
			decl.Members.Add(new CodeField("string", "ApiHash") { Value = "\"" + @class.ApiHash + "\"", Attributes = CodeAttributes.Public | CodeAttributes.Static | CodeAttributes.ReadOnly });

			foreach (CppFunction fn in @class.Functions.OrderBy(f => f.Name))
			{
				if (fn.Name == "cef_get_xdisplay")
					continue;
				if (fn.Name == "cef_get_current_platform_thread_handle")
					continue;
				DefineFunction(decl, fn);
			}

			ns.Types.Add(decl);
		}

		private void DefineFunction(CodeType typeDecl, CppFunction func)
		{
			if (func.Name == "ArraySizeHelper")
				return;
			
			if (func.LinkageKind != CppLinkageKind.External)
				throw new NotImplementedException();

			TypeDesc retType = GetTypeDesc(func.ReturnType);
			var fn = new CodeMethod(func.Name);
			fn.RetVal = new CodeMethodParameter(null) { Type = ResolveCefType(retType.ToString()) };
			fn.Attributes = CodeAttributes.Public | CodeAttributes.External | CodeAttributes.Unsafe | CodeAttributes.Static;
			if (func.CallingConvention == CppCallingConvention.C)
				fn.CustomAttributes.AddDllImportfAttribute(CallingConvention.Cdecl);
			else if (func.CallingConvention == CppCallingConvention.X86StdCall)
				fn.CustomAttributes.AddDllImportfAttribute(CallingConvention.StdCall);
			else
				throw new NotImplementedException();

			string filename = func.Span.Start.File;
			filename = Path.GetRelativePath(BaseDirectory, filename).Replace('\\', '/');
			fn.Comments.AddVSDocComment(func.Comment, "summary");
			fn.Comments.AddVSDocComment(string.Format("Defined in {0} as\n{1}", filename, func.ToString()), "remarks");

			CppContainerList<CppParameter> @params = func.Parameters;
			for (int i = 0; i < @params.Count; i++)
			{
				CppParameter arg = @params[i];

				var param = new CodeMethodParameter(arg.Name.EscapeName());
				TypeDesc paramType = GetTypeDesc(arg.Type);

				string argType = paramType.ToString();
				while (argType.StartsWith("const "))
				{
					argType = argType.Substring(6);
					param.Direction = CodeMethodParameterDirection.In;
				}

				param.Type = ResolveCefType(argType);
				fn.Parameters.Add(param);
			}

			typeDecl.Members.Add(fn);
		}

		private void DefineField(CodeType typeDecl, CppField field)
		{
			CodeMethod caller = null;

			TypeDesc fieldType = GetTypeDesc(field.Type);
			var fld = new CodeField(ResolveCefType(fieldType.ToString()), field.Name.EscapeName());
			if (fieldType.IsCallable)
			{
				fld.Attributes = CodeAttributes.Public;// | CodeAttributes.ReadOnly;
				fld.Comments.AddVSDocComment(fieldType.Name, "summary");

				CppFunctionType fnType = fieldType.FunctionTypeRef;
				TypeDesc retType = GetTypeDesc(fnType.ReturnType);
				caller = new CodeMethod(field.Name.ToUpperCamel(fnType.Parameters.Count).EscapeName());
				var rvtype = new CodeMethodParameter(null);
				rvtype.Type = ResolveCefType(retType.ToString());
				if (retType.Name == "char16" || retType.Name == "wchar")
				{
					rvtype.CustomAttributes.Add(new CustomCodeAttribute("return: MarshalAs(UnmanagedType.U2)"));
					throw new NotImplementedException(); // TODO: check it
				}
				caller.RetVal = rvtype;
				caller.Attributes = CodeAttributes.Public| CodeAttributes.Unsafe;
				//caller.CustomAttributes.AddMethodImplForwardRefAttribute();
				caller.CustomAttributes.Add(new CustomCodeAttribute("NativeName") { Parameters = { "\"" + field.Name + "\"" } });
				caller.Comments.AddVSDocComment(field.Comment, "summary");
				caller.Callee = fld;
				CppContainerList<CppParameter> @params = fnType.Parameters;
				for (int i = 0; i < @params.Count; i++)
				{
					CppParameter arg = @params[i];
					if (i == 0 && arg.Name == "self")
					{
						string argTypeName = ResolveCefType(arg.Type.GetDisplayName());
						if (argTypeName == typeDecl.Name + "*")
						{
							caller.HasThisArg = true;
							continue;
						}
					}
					var param = new CodeMethodParameter(arg.Name.EscapeName());
					TypeDesc paramType = GetTypeDesc(arg.Type);

					string argType = paramType.ToString();
					while (argType.StartsWith("const "))
					{
						argType = argType.Substring(6);
						param.Direction = CodeMethodParameterDirection.In;
					}
					if (param.Direction == CodeMethodParameterDirection.In)
					{
						param.CustomAttributes.Add(new CustomCodeAttribute("Immutable"));
					}
					if (argType == "char16" || argType == "wchar")
					{
						param.CustomAttributes.Add(new CustomCodeAttribute("MarshalAs(UnmanagedType.U2)"));
						throw new NotImplementedException(); // TODO: check it
					}
					param.Type = ResolveCefType(argType);
					caller.Parameters.Add(param);
				}

				//fld.TypeName = GetNativeDelegate(caller, typeDecl);
				caller.Body = GetNativeCallMehtodBody(caller, fld, typeDecl);
			}
			else
			{
				fld.Comments.AddVSDocComment(field.Comment, "summary");
				fld.Attributes = CodeAttributes.Public;
				if (fieldType.Name == "char16" || fieldType.Name == "wchar")
				{
					fld.CustomAttributes.Add(new CustomCodeAttribute("MarshalAs(UnmanagedType.U2)"));
				}
			}
			typeDecl.Members.Add(fld);
			if (caller != null)
			{
				typeDecl.Members.Add(caller);
			}
		}

		private string GetNativeDelegate(CodeMethod caller, CodeType typeDecl)
		{
			var sb = new StringBuilder();
			sb.Append("delegate* unmanaged[Stdcall]<");
			var args = new List<string>();
			if (caller.HasThisArg)
				args.Add(typeDecl.Name + "*");
			args.AddRange(caller.Parameters.Select(arg => CSharpCodeGen.GetClrTypeName(arg.Type)));
			args.Add(CSharpCodeGen.GetClrTypeName(caller.RetVal.Type));
			sb.Append(string.Join(", ", args));
			sb.Append(">");
			return sb.ToString();
		}

		private string GetNativeCallMehtodBody(CodeMethod caller, CodeField field, CodeType typeDecl)
		{
			var sb = new StringBuilder();
			sb.Append("fixed (").Append(typeDecl.Name).Append("* self = &this)\n{\n\t");
			if (caller.RetVal.Type != "void")
			{
				sb.Append("return ");
			}

			var argTypes = new List<string>();
			if (caller.HasThisArg)
				argTypes.Add(typeDecl.Name + "*");
			argTypes.AddRange(caller.Parameters.Select(arg => CSharpCodeGen.GetClrTypeName(arg.Type)));
			argTypes.Add(CSharpCodeGen.GetClrTypeName(caller.RetVal.Type));

			sb.Append("((delegate* unmanaged[Stdcall]<").Append(string.Join(", ", argTypes)).Append(">)");
			sb.Append(field.Name).Append(")(");
			var args = new List<string>();
			if (caller.HasThisArg)
				args.Add("self");
			args.AddRange(caller.Parameters.Select(arg => arg.Name));
			sb.Append(string.Join(", ", args));
			sb.Append(");");
			sb.Append("\n}");

			return sb.ToString();
		}

	}

}
