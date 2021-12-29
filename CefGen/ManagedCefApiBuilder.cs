// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefGen.CodeDom;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CefGen
{


	sealed class ManagedCefApiBuilder : ApiBuilderBase
	{
		private string _basePath;

		public ManagedCefApiBuilder(string basePath)
		{
			_basePath = basePath;
		}

		public bool EnableCallbackOverrideCheck { get; set; }

		private INamedTypeSymbol TypeSymbol { get; set; }
		private CefTypeKind TypeSymbolKind { get; set; }
		private NativeCefApiTypes NativeTypes { get; set; }

		private CodeType Class { get; set; }

		internal void GenerateFrom(NativeCefApiTypes nativeTypes)
		{
			NativeTypes = nativeTypes;

			foreach (INamedTypeSymbol symbol in nativeTypes.RefCounted.Values)
			{
				TypeSymbol = symbol;
				TypeSymbolKind = CefTypeKind.RefCounted;
				GenerateClass(symbol, "CefBaseRefCounted<" + symbol.Name + ">");
			}

			foreach (INamedTypeSymbol symbol in nativeTypes.Scoped.Values)
			{
				TypeSymbol = symbol;
				TypeSymbolKind = CefTypeKind.Scoped;
				GenerateClass(symbol, "CefBaseScoped<" + symbol.Name + ">");
			}

			foreach (INamedTypeSymbol symbol in nativeTypes.Sized.Values)
			{
				if (symbol.Name == "cef_base_ref_counted_t")
					continue;
				if (symbol.Name == "cef_base_scoped_t")
					continue;

				TypeSymbol = symbol;
				TypeSymbolKind = CefTypeKind.Sized;

				GenerateSimpleClass(symbol, null);
			}

			foreach (INamedTypeSymbol symbol in nativeTypes.Simple.Values)
			{
				if (symbol.Name.StartsWith("cef_string_"))
					continue;
				if (symbol.Name == "MSG")
					continue;
				if (symbol.Name == "cef_color_t")
					continue;
				if (symbol.Name == "cef_main_args_t")
					continue;
				if (symbol.Name == "cef_window_info_t")
					continue;
				if (symbol.Name == "CefEventHandle")
					continue;

				TypeSymbol = symbol;
				TypeSymbolKind = CefTypeKind.Simple;

				GenerateSimpleStruct(symbol, null);
			}
		}

		private static void WriteToFile(CodeFile fileDecl, string file)
		{
			using (var csfile = new StreamWriter(file, false, Encoding.UTF8))
			{
				var csharp = new CSharpCodeGen();
				csharp.Directives.Add("#pragma warning disable 0169, 1591, 1573");
				csharp.GenerateCode(fileDecl, csfile);
				csfile.Flush();
			}
		}

		private void GenerateClass(INamedTypeSymbol symbol, string baseType)
		{
			List<string> constructorLines;

			string filePath = symbol.Locations[0].SourceTree.FilePath;
			CodeFile fileDecl = CreateCodeFile(filePath.Substring(filePath.IndexOf("Generated")).Replace('\\', '/'));
			var ns = new CodeNamespace("CefNet");
			fileDecl.Namespaces.Add(ns);

			List<CodeMethod> avoids = null;


			Class = new CodeClass(symbol.Name.Remove(symbol.Name.Length - 2).ToUpperCamel());
			Class.Attributes = CodeAttributes.Public | CodeAttributes.Unsafe | CodeAttributes.Partial;
			Class.BaseType = baseType;
			Class.Comments.AddSymbolComment(symbol);
			if (symbol.IsImplementation())
			{
				GenerateProxy();
				Class.Comments.AddVSDocComment("Role: Proxy", "remarks");
				AddConstructors(null);
			}
			else
			{
				Class.BaseType += ", I" + Class.Name + "Private";
				avoids = new List<CodeMethod>();
				constructorLines = GenerateHandler(avoids);
				Class.Comments.AddVSDocComment("Role: Handler", "remarks");
				AddConstructors(constructorLines);
			}

			ns.Types.Add(Class);

			WriteToFile(fileDecl, Path.Combine(_basePath, "Managed", "Types", Class.Name + ".cs"));
		}

		private void GenerateSimpleClass(INamedTypeSymbol symbol, string baseType)
		{
			string filePath = symbol.Locations[0].SourceTree.FilePath;
			CodeFile fileDecl = CreateCodeFile(filePath.Substring(filePath.IndexOf("Generated")).Replace('\\', '/'));
			var ns = new CodeNamespace("CefNet");
			fileDecl.Namespaces.Add(ns);

			Class = new CodeClass(symbol.Name.Remove(symbol.Name.Length - 2).ToUpperCamel());
			Class.Attributes = CodeAttributes.Public | CodeAttributes.Unsafe | CodeAttributes.Partial;
			Class.Comments.AddSymbolComment(symbol);

			Class.Comments.AddVSDocComment("Role: Proxy", "remarks");

			var varDecl = new CodeField(symbol.Name + "*", "_instance");
			varDecl.Attributes = CodeAttributes.Private;
			Class.Members.Add(varDecl);

			varDecl = new CodeField("bool", "_disposable");
			varDecl.Attributes = CodeAttributes.Private | CodeAttributes.ReadOnly;
			Class.Members.Add(varDecl);

			var ctorDecl = new CodeConstructor(Class.Name);
			ctorDecl.Attributes = CodeAttributes.Public;
			ctorDecl.Body = string.Format("_disposable = true;\r\n_instance = ({0}*)CefStructure.Allocate(sizeof({0}));\r\n_instance->size = new UIntPtr((uint)sizeof({0}));", symbol.Name);
			Class.Members.Add(ctorDecl);

			AddConstructors(null);



			var propDecl = new CodeProperty("NativeInstance");
			propDecl.Type = new CodeMethodParameter("value") { Type = symbol.Name + "*" };
			propDecl.Attributes = CodeAttributes.Public;
			propDecl.GetterBody = "return _instance;";
			Class.Members.Add(propDecl);

			var methodDecl = new CodeMethod("GetNativeInstance");
			methodDecl.RetVal = new CodeMethodParameter(null) { Type = symbol.Name + "*" };
			methodDecl.Attributes = CodeAttributes.Public;
			methodDecl.Body = "return _instance;";
			Class.Members.Add(methodDecl);

			foreach (IFieldSymbol field in symbol.GetMembers().OfType<IFieldSymbol>())
			{
				GenerateSimpleProperty(field);
			}

			CodeProperty[] disposable = Class.Members.OfType<CodeProperty>().Where(prop => prop.Type.Type == "string").ToArray();
			if (disposable.Length > 0)
			{
				Class.BaseType = "IDisposable";

				var dispose = new CodeMethod("Dispose");
				dispose.Attributes = CodeAttributes.Public;
				dispose.RetVal = new CodeMethodParameter(null) { Type = "void" };
				dispose.Body = "Dispose(true);\r\nGC.SuppressFinalize(this);";
				Class.Members.Add(dispose);

				dispose = new CodeMethod("Dispose");
				dispose.Attributes = CodeAttributes.Protected | CodeAttributes.Virtual;
				dispose.RetVal = new CodeMethodParameter(null) { Type = "void" };
				dispose.Parameters.Add(new CodeMethodParameter("disposing") { Type = "bool" });
				dispose.Body = "if (_disposable && _instance != null)\r\n{\r\n\t"
					+ string.Join("\r\n\t", disposable.Select(prop => prop.Name + " = null;"))
					+ "\r\n\tMarshal.FreeHGlobal((IntPtr)_instance);\r\n\t_instance = null;"
					+ "}";
				Class.Members.Add(dispose);

				var dtor = new CodeFinalizer(Class.Name);
				dtor.Body = "Dispose(false);";
				Class.Members.Add(dtor);
			}

			ns.Types.Add(Class);

			WriteToFile(fileDecl, Path.Combine(_basePath, "Managed", "Types", Class.Name + ".cs"));
		}

		private void GenerateSimpleStruct(INamedTypeSymbol symbol, string baseType)
		{
			string filePath = symbol.Locations[0].SourceTree.FilePath;
			CodeFile fileDecl = CreateCodeFile(filePath.Substring(filePath.IndexOf("Generated")).Replace('\\', '/'));
			var ns = new CodeNamespace("CefNet");
			fileDecl.Namespaces.Add(ns);

			Class = new CodeStruct(symbol.Name.Remove(symbol.Name.Length - 2).ToUpperCamel());
			Class.Attributes = CodeAttributes.Public | CodeAttributes.Unsafe | CodeAttributes.Partial;
			Class.Comments.AddSymbolComment(symbol);

			Class.Comments.AddVSDocComment("Role: Proxy", "remarks");

			var instanceDecl = new CodeField(symbol.Name, "_instance");
			instanceDecl.Attributes = CodeAttributes.Private;
			Class.Members.Add(instanceDecl);

			foreach (IFieldSymbol field in symbol.GetMembers().OfType<IFieldSymbol>())
			{
				GenerateSimpleProperty(field);
			}

			CodeProperty[] disposable = Class.Members.OfType<CodeProperty>().Where(prop => prop.Type.Type == "string").ToArray();
			if (disposable.Length > 0)
			{
				Class.BaseType = "IDisposable";
				var dispose = new CodeMethod("Dispose");
				dispose.Attributes = CodeAttributes.Public;
				dispose.RetVal = new CodeMethodParameter(null) { Type = "void" };
				dispose.Body = string.Join("\r\n", disposable.Select(prop => prop.Name + " = null;"));
				Class.Members.Add(dispose);
			}

			var convOp = new CodeOperator(Class.Name);
			convOp.Attributes = CodeAttributes.Public | CodeAttributes.Static;
			convOp.RetVal = new CodeMethodParameter(null) { Type = "implicit" };
			convOp.Parameters.Add(new CodeMethodParameter("instance") { Type = symbol.Name });
			convOp.Body = "return new " + Class.Name + " { _instance = instance };";
			Class.Members.Add(convOp);

			convOp = new CodeOperator(symbol.Name);
			convOp.Attributes = CodeAttributes.Public | CodeAttributes.Static;
			convOp.RetVal = new CodeMethodParameter(null) { Type = "implicit" };
			convOp.Parameters.Add(new CodeMethodParameter("instance") { Type = Class.Name });
			convOp.Body = "return instance._instance;";
			Class.Members.Add(convOp);

			ns.Types.Add(Class);

			WriteToFile(fileDecl, Path.Combine(_basePath, "Managed", "Types", Class.Name + ".cs"));
		}

		private void GenerateSimpleProperty(IFieldSymbol field)
		{
			var prop = new CodeProperty(field.Name.ToUpperCamel().EscapeName());
			prop.Attributes = CodeAttributes.Public;
			prop.Comments.AddSymbolComment(field);

			TypeSymbolInfo symbolInfo = TypeSymbolInfo.FromSymbol(field.Type);
			CefTypeKind typeKind = NativeTypes.GetTypeKind(symbolInfo.Type);
			if (symbolInfo.IsDoublePointedType)
				throw new NotImplementedException();
			if (symbolInfo.IsPointedType)
			{
				if (field.Type.ToString() != "void*")
					throw new NotImplementedException();
				prop.Type = new CodeMethodParameter("value") { Type = "IntPtr" };
			}
			else
			{
				prop.Type = new CodeMethodParameter("value") { Type = field.IsBool() ? "bool" : symbolInfo.AsClrTypeName() };
			}

			string accessOp = (TypeSymbolKind != CefTypeKind.Simple) ? "->" : ".";

			string fieldName = field.Name.EscapeName();
			if (prop.Type.Type == "bool")
			{
				prop.GetterBody = string.Format("return _instance{0}{1} != 0;", accessOp, fieldName);
				prop.SetterBody = "_instance" + accessOp + fieldName + " = value ? 1 : 0;";
			}
			else if (field.Type.Name == "cef_string_t")
			{
				if (accessOp == ".")
				{
					prop.GetterBody = "fixed (cef_string_t* s = &_instance." + fieldName + ")\r\n{\r\n\treturn CefString.Read(s);\r\n}";
					prop.SetterBody = "fixed (cef_string_t* s = &_instance." + fieldName + ")\r\n{\r\n\tCefString.Replace(s, value);\r\n}";
				}
				else
				{
					prop.GetterBody = "return CefString.Read(&_instance->" + fieldName + ");";
					prop.SetterBody = "CefString.Replace(&_instance->" + fieldName + ", value);";
				}
			}
			else if (field.Type.ToString() == "void*")
			{
				prop.GetterBody = "return new IntPtr(_instance" + accessOp + fieldName + ");";
				prop.SetterBody = string.Format("_instance{0}{1} = (void*)value;", accessOp, fieldName);
			}
			else if (field.Type.Name == "UIntPtr")
			{
				prop.GetterBody = "return (long)(_instance" + accessOp + fieldName + ");";
				prop.SetterBody = string.Format("_instance{0}{1} = new UIntPtr((ulong)value);", accessOp, fieldName);
			}
			else
			{
				prop.GetterBody = "return _instance" + accessOp + fieldName + ";";
				prop.SetterBody = "_instance" + accessOp + fieldName + " = value;";
			}

			Class.Members.Add(prop);
		}

		private void AddConstructors(List<string> constructorLines)
		{
			int index = 0;

			foreach (CodeTypeMember member in Class.Members)
			{
				if (!(member is CodeField || member is CodeConstructor))
					break;
				index++;
			}

			var createMethod = new CodeMethod("Create");
			createMethod.RetVal = new CodeMethodParameter(null) { Type = Class.Name };
			createMethod.Attributes = CodeAttributes.Internal | CodeAttributes.Static | CodeAttributes.Unsafe;
			createMethod.Parameters.Add(new CodeMethodParameter("instance") { Type = "IntPtr" });
			createMethod.Body = string.Format("return new {0}(({1}*)instance);", Class.Name, TypeSymbol.Name);

			CodeConstructor ctorDecl;

			ctorDecl = new CodeConstructor(Class.Name);
			ctorDecl.Attributes = CodeAttributes.Public;
			ctorDecl.Parameters.Add(new CodeMethodParameter("instance") { Type = TypeSymbol.Name + "*" });
			if (TypeSymbolKind == CefTypeKind.RefCounted)
			{
				ctorDecl.BaseArgs.Add("(cef_base_ref_counted_t*)instance");
			}
			else if (TypeSymbolKind == CefTypeKind.Scoped)
			{
				ctorDecl.BaseArgs.Add("(cef_base_scoped_t*)instance");
			}
			else if (TypeSymbolKind == CefTypeKind.Sized)
			{
				ctorDecl.Body = "_instance = instance;";
			}
			Class.Members.Insert(index, ctorDecl);


			var nativeRoots = new List<CodeField>();
			if (//!(typeKind == CefTypeKind.RefCounted || typeKind == CefTypeKind.Scoped) || 
				constructorLines != null)
			{
				foreach (CodeDelegate decl in Class.Members.Where(m => m is CodeDelegate))
				{
					string fnname = decl.Name.Remove(decl.Name.Length - 8);
					var root = new CodeField(decl.Name, "fn" + fnname);
					root.Attributes = CodeAttributes.Private | CodeAttributes.Static | CodeAttributes.ReadOnly;
					root.Value = fnname + "Impl";
					root.LegacyDefine = "NET_LESS_5_0";
					nativeRoots.Add(root);
				}

				ctorDecl = new CodeConstructor(Class.Name);
				ctorDecl.Attributes = CodeAttributes.Public;
				ctorDecl.Body = string.Join("\n", constructorLines);
				Class.Members.Insert(index, ctorDecl);
			}

			Class.Members.Insert(index, createMethod);

			for (int i = nativeRoots.Count - 1; i >= 0; i--)
			{
				Class.Members.Insert(0, nativeRoots[i]);
			}



			if (TypeSymbolKind == CefTypeKind.Sized)
			{
				var wrapMethod = new CodeMethod("Wrap");
				wrapMethod.RetVal = new CodeMethodParameter(null) { Type = Class.Name };
				wrapMethod.Attributes = CodeAttributes.Internal | CodeAttributes.Static | CodeAttributes.Unsafe;
				wrapMethod.Parameters.Add(new CodeMethodParameter("instance") { Type = "IntPtr" });
				wrapMethod.Body = string.Format("return new {0}(({1}*)instance);", Class.Name, TypeSymbol.Name);
			}
		}

		private List<string> GenerateHandler(List<CodeMethod> avoids)
		{
			var constructorLines = new List<string>();
			constructorLines.Add(TypeSymbol.Name + "* self = this.NativeInstance;");

			var legacyCode = new List<string>();
			var modernCode = new List<string>();

			foreach (ISymbol symbol in TypeSymbol.GetMembers())
			{
				if (symbol is IFieldSymbol field)
				{
					IMethodSymbol method = GetAssociatedMethod(field);
					if (method != null)
					{
						CodeMethod avoid = GenerateAvoidCallback(method);
						avoids.Add(avoid);
						List<CefParameterInfo> args = GenerateManagedCallback(method);
						GenerateNativeCallbackDelegate(method);
						GenerateNativeCallback(field, method, args, avoid != null);

						legacyCode.Add(string.Format("self->{0} = (void*)Marshal.GetFunctionPointerForDelegate(fn{1});", field.Name, method.Name));

						var argTypes = new List<string>();
						argTypes.Add(TypeSymbol.Name + "*");
						argTypes.AddRange(method.Parameters.Select(arg => arg.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
						argTypes.Add(method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
						string delegateType = "delegate* unmanaged[Stdcall]<" + string.Join(", ", argTypes) + ">";
						modernCode.Add(string.Format("self->{0} = ({2})&{1}Impl;", field.Name, method.Name, delegateType));
						
					}
					else if (field.Type.Name != "cef_base_ref_counted_t")
					{
						throw new NotImplementedException();
					}
				}
			}
			if ((modernCode.Count | legacyCode.Count) != 0)
			{
				constructorLines.Add("#if NET_LESS_5_0");
				constructorLines.AddRange(legacyCode);
				constructorLines.Add("#else");
				constructorLines.AddRange(modernCode);
				constructorLines.Add("#endif");
			}
			return constructorLines;
		}

		private void GenerateProxy()
		{
			ICollection<IMethodSymbol> methods;
			ICollection<CefProperty> properties;
			DetectProxyMethods(out methods, out properties);

			foreach (CefProperty property in properties)
			{
				GenerateProxyProperty(property);
			}
			foreach (IMethodSymbol method in methods)
			{
				GenerateProxyMethod(method);
			}
		}

		private void DetectProxyMethods(out ICollection<IMethodSymbol> methods, out ICollection<CefProperty> properties)
		{
			string name;
			CefProperty property;
			methods = new List<IMethodSymbol>();
			var props = new Dictionary<string, CefProperty>();
			properties = props.Values;
			foreach (ISymbol symbol in TypeSymbol.GetMembers())
			{
				if (symbol is IFieldSymbol field)
				{
					IMethodSymbol method = GetAssociatedMethod(field);
					if (method != null)
					{
						if (method.ReturnsVoid)
						{
							if (method.Parameters.Length == 1
								&& method.Name.StartsWith("Set"))
							{
								name = method.Name.Substring(3);
								if (!props.TryGetValue(name, out property))
								{
									property = new CefProperty();
									props.Add(name, property);
								}
								property.Setter = method;
								continue;
							}
						}
						else if (method.Parameters.Length == 0
							&& method.Name.StartsWith("Get", "Has", "Is", "Can"))
						{
							if (TypeSymbol.Name.EndsWith("value_t") && method.Name.StartsWith("Get")
								&& method.Name != "GetSize" && !method.Name.EndsWith("Type"))
							{
								methods.Add(method);
								continue;
							}
							if (TypeSymbol.Name == "cef_v8context_t" && method.Name == "GetGlobal")
							{
								methods.Add(method);
								continue;
							}
							int startPos = method.Name.IndexOf(char.IsUpper, 1);
							name = method.Name.Substring(Math.Max(startPos, 0));
							if (!props.TryGetValue(name, out property))
							{
								property = new CefProperty();
								props.Add(name, property);
							}
							property.Getter = method;
							continue;
						}
						methods.Add(method);
					}
					else if (field.Type.Name != "cef_base_ref_counted_t"
						&& field.Type.Name != "cef_base_scoped_t")
					{
						throw new NotImplementedException();
					}
				}
			}
			foreach (string key in props.Keys.ToArray())
			{
				property = props[key];
				if (property.Getter == null)
				{
					props.Remove(key);
					methods.Add(property.Setter);
				}
				else if (property.Setter == null)
				{
					name = "Set" + key;
					foreach (IMethodSymbol method in methods)
					{
						if (method.Name == name)
						{
							props.Remove(key);
							methods.Add(property.Getter);
							break;
						}
					}
				}
				else if (!SymbolEqualityComparer.Default.Equals(property.SetterType, property.GetterType))
				{

					if (property.GetterType.Name == "cef_string_userfree_t"
						&& property.SetterType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) == "cef_string_t*")
					{
						continue;
					}
					props.Remove(key);
					methods.Add(property.Getter);
					methods.Add(property.Setter);
				}
			}
		}

		private IMethodSymbol GetAssociatedMethod(IFieldSymbol field)
		{
			foreach (ISymbol symbol in TypeSymbol.GetMembers())
			{
				if (symbol is IMethodSymbol method)
				{
					AttributeData attribute = method.GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == "NativeNameAttribute");
					if (attribute == null)
						continue;
					string nativeName = (string)attribute.ConstructorArguments.First().Value;
					if (nativeName == field.Name)
						return method;
				}
			}

			return null;
		}

		private void GenerateNativeCallbackDelegate(IMethodSymbol method)
		{
			var delegateDecl = new CodeDelegate(method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), method.Name + "Delegate");
			delegateDecl.Attributes = CodeAttributes.Private | CodeAttributes.Unsafe;
			delegateDecl.CustomAttributes.AddUnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Winapi);
			delegateDecl.Parameters.Add(new CodeMethodParameter("self") { Type = TypeSymbol.Name + "*" });
			delegateDecl.ReturnTypeName = method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			delegateDecl.LegacyDefine = "NET_LESS_5_0";

			foreach (IParameterSymbol parameter in method.Parameters)
			{
				var paramDecl = new CodeMethodParameter(parameter.Name.EscapeName());
				paramDecl.Type = parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
				delegateDecl.Parameters.Add(paramDecl);
			}
			Class.Members.Add(delegateDecl);
		}

		private void GenerateNativeCallback(IFieldSymbol field, IMethodSymbol method, IList<CefParameterInfo> managedArgs, bool hasAvoid)
		{
			var callback = new CodeMethod(method.Name + "Impl");
			callback.Attributes = CodeAttributes.Private | CodeAttributes.Static | CodeAttributes.Unsafe;
			callback.CustomAttributes.AddUnmanagedCallesOnlyAttribute();
			callback.HasThisArg = true;
			callback.RetVal = new CodeMethodParameter(null) { Type = method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) };
			callback.Parameters.Add(new CodeMethodParameter("self") { Type = TypeSymbol.Name + "*" });

			callback.Comments.AddComment(field.GetComment());
			foreach (IParameterSymbol parameter in method.Parameters)
			{
				var paramDecl = new CodeMethodParameter(parameter.Name.EscapeName());
				paramDecl.Type = parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
				callback.Parameters.Add(paramDecl);
			}
			var body = new StringBuilder()
				.Append("var instance = GetInstance((IntPtr)self) as ").Append(Class.Name).AppendLine(";")
				.Append("if (instance == null");
			if (hasAvoid)
			{
				body.Append(" || ((I").Append(Class.Name).Append("Private)instance).Avoid").Append(method.Name).Append("()");
			}
			body.AppendLine(")").AppendLine("{");

			CefTypeKind typeKind;
			TypeSymbolInfo typeInfo;

			int index = 0;
			var wrappers = new StringBuilder();
			var disposers = new StringBuilder();
			var args = new List<string>(managedArgs.Count);
			foreach (IParameterSymbol arg in method.Parameters)
			{
				index++;
				CefParameterInfo managedInfo = managedArgs.First(cpi => SymbolEqualityComparer.Default.Equals(cpi.Symbol, arg));
				if (managedInfo.IsArraySize)
					continue;

				typeInfo = TypeSymbolInfo.FromSymbol(arg.Type);
				typeKind = NativeTypes.GetTypeKind(typeInfo.Type);

				if (typeInfo.Type.Name == "cef_string_t")
				{
					if (managedInfo.Parameter.Direction == CodeMethodParameterDirection.Ref)
					{
						wrappers.AppendFormat("string s_{0} = CefString.Read({1});\r\n", arg.Name, arg.Name.EscapeName())
							.AppendFormat("string s_orig_{0} = s_{0};\r\n", arg.Name);
						disposers.AppendFormat("if (s_{0} != s_orig_{0})\r\n\tCefString.Replace({1}, s_{0});\r\n", arg.Name, arg.Name.EscapeName());
						args.Add("ref s_" + arg.Name);
					}
					else
					{
						args.Add("CefString.Read(" + arg.Name.EscapeName() + ")");
					}
				}
				else if (typeInfo.Type.Name == "cef_string_list_t")
				{
					args.Add("CefStringList.Wrap(" + arg.Name.EscapeName() + ")");
				}
				else if (typeInfo.Type.Name == "cef_window_info_t")
				{
					args.Add("CefWindowInfo.Wrap(" + arg.Name.EscapeName() + ")");
				}
				else if (typeKind == CefTypeKind.RefCounted)
				{
					if (typeInfo.IsDoublePointedType)
					{
						if (!managedInfo.IsArray)
						{
							wrappers.AppendFormat("{2} obj_{0} = {2}.Wrap({2}.Create, *{1});\r\n", arg.Name, arg.Name.EscapeName(), managedInfo.Parameter.Type);
							disposers.AppendFormat("*{1} = (obj_{0} != null) ? obj_{0}.GetNativeInstance() : null;\r\n", arg.Name, arg.Name.EscapeName());
							args.Add("ref obj_" + arg.Name);
						}
						else
						{
							string arrayBaseType = managedInfo.Parameter.Type.Replace("[]", string.Empty);
							wrappers.AppendFormat("{2} obj_{0} = new {3}[(int){1}Count];\r\n", arg.Name, arg.Name.EscapeName(), managedInfo.Parameter.Type, arrayBaseType)
								.AppendFormat("for (int i = 0; i < obj_{0}.Length; i++)\r\n", arg.Name)
								.Append("{\r\n\t")
								.AppendFormat("var item = *({0} + i);\r\n\t", arg.Name)
								.Append("item->@base.AddRef();\r\n\t")
								.AppendFormat("obj_{0}[i] = {1}.Wrap({1}.Create, item);\r\n", arg.Name, arrayBaseType)
								.AppendLine("}");

							args.Add("obj_" + arg.Name);
						}
					}
					else
					{
						args.Add(string.Format("{0}.Wrap({0}.Create, {1})", managedInfo.Parameter.Type, arg.Name.EscapeName()));
						body.Append("\tReleaseIfNonNull((cef_base_ref_counted_t*)").Append(arg.Name.EscapeName()).AppendLine(");");
					}
				}
				else if (typeKind == CefTypeKind.Scoped)
				{
					if (typeInfo.IsDoublePointedType)
					{
						if (!managedInfo.IsArray)
						{
							wrappers.AppendFormat("{2} obj_{0} = {2}.Wrap({2}.Create, *{1});\r\n", arg.Name, arg.Name.EscapeName(), managedInfo.Parameter.Type);
							disposers.AppendFormat("*{1} = (obj_{0} != null) ? obj_{0}.GetNativeInstance() : null;\r\n", arg.Name, arg.Name.EscapeName());
							args.Add("ref obj_" + arg.Name);
						}
						else
						{
							string arrayBaseType = managedInfo.Parameter.Type.Replace("[]", string.Empty);
							wrappers.AppendFormat("{2} obj_{0} = new {3}[(int){1}Count];\r\n", arg.Name, arg.Name.EscapeName(), managedInfo.Parameter.Type, arrayBaseType)
								.AppendFormat("for (int i = 0; i < obj_{0}.Length; i++)\r\n", arg.Name)
								.Append("{\r\n\t")
								.AppendFormat("obj_{0}[i] = {1}.Wrap({1}.Create, *({2} + i));\r\n", arg.Name, arrayBaseType, arg.Name.EscapeName())
								.AppendLine("}");

							args.Add("obj_" + arg.Name);
						}
					}
					else
					{
						args.Add(string.Format("{0}.Wrap({0}.Create, {1})", managedInfo.Parameter.Type, arg.Name.EscapeName()));
					}
				}
				else if (typeKind == CefTypeKind.Sized) // (arg.IsCefStruct)
				{
					if (typeInfo.Type.Name == "cef_base_ref_counted_t")
					{
						args.Add(string.Format("UnknownRefCounted.Wrap(UnknownRefCounted.Create, {0})", arg.Name.EscapeName()));
					}
					else
					{
						args.Add(string.Format("new {0}({1})", managedInfo.Parameter.Type, arg.Name.EscapeName()));
					}
				}
				else if (typeKind == CefTypeKind.Simple && typeInfo.IsPointedType)//arg.IsSimpleStructPtr)
				{
					if (managedInfo.IsArray)
					{
						if (managedInfo.Parameter.Direction != CodeMethodParameterDirection.Ref)
						{
							string arrayBaseType = managedInfo.Parameter.Type.Replace("[]", string.Empty);
							wrappers.AppendFormat("{2} obj_{0} = new {3}[(int){1}Count];\r\n", arg.Name, arg.Name.EscapeName(), managedInfo.Parameter.Type, arrayBaseType)
								.AppendFormat("for (int i = 0; i < obj_{0}.Length; i++)\r\n", arg.Name)
								.Append("{\r\n\t")
								.AppendFormat("obj_{0}[i] = *({1}*)({2} + i);\r\n", arg.Name, arrayBaseType, arg.Name.EscapeName())
								.AppendLine("}");

							args.Add("obj_" + arg.Name);
						}
						else
						{
							throw new NotImplementedException();
						}
					}
					else
					{
						if (managedInfo.Parameter.Direction != CodeMethodParameterDirection.Ref)
						{
							args.Add(string.Format("*({0}*){1}", managedInfo.Parameter.Type, arg.Name.EscapeName()));
						}
						else
						{
							args.Add(string.Format("ref *({0}*){1}", managedInfo.Parameter.Type, arg.Name.EscapeName()));
						}
					}
				}
				else if (arg.Type.ToString() == "void*")
				{
					args.Add("unchecked((IntPtr)" + arg.Name.EscapeName() + ")");
				}
				else if (managedInfo.Parameter.Direction == CodeMethodParameterDirection.Ref)
				{
					if (arg.Type.ToString() == "void**")
					{
						args.Add(string.Format("ref *({1}*){0}", arg.Name.EscapeName(), managedInfo.Parameter.Type));
					}
					else if (arg.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) == "UIntPtr*")
					{
						wrappers.AppendFormat("long c{0} = (long)(*{1});\r\n", index, arg.Name.EscapeName());
						disposers.AppendFormat("*{1} = new UIntPtr((ulong)c{0});\r\n", index, arg.Name.EscapeName());
						args.Add(string.Format("ref c{0}", index));
					}
					else if (arg.Type.ToString() == "cef_color_t*")
					{
						args.Add("ref *(CefColor*)" + arg.Name.EscapeName());
					}
					else
					{
						args.Add("ref *" + arg.Name.EscapeName());
					}
				}
				else if (typeInfo.IsDoublePointedType)
				{
					args.Add("(IntPtr)" + arg.Name.EscapeName());
				}
				else if (managedInfo.Parameter.Type == "bool")
				{
					args.Add(arg.Name.EscapeName() + " != 0");
				}
				else if (managedInfo.Parameter.Type == "long" && arg.Type.Name == "UIntPtr")
				{
					args.Add("(long)" + arg.Name.EscapeName());
				}
				else
				{
					args.Add(arg.Name.EscapeName());
				}
			}

			string callCode = string.Format("instance.{0}({1})", method.Name, string.Join(", ", args));


			if (method.ReturnsVoid)
			{
				body.AppendLine("\treturn;")
					.AppendLine("}")
					.Append(wrappers.ToString())
					.Append(callCode)
					.Append(";");
			}
			else
			{
				body.AppendLine("\treturn default;")
					.AppendLine("}");

				typeInfo = TypeSymbolInfo.FromSymbol(method.ReturnType);
				typeKind = NativeTypes.GetTypeKind(method.ReturnType);

				CodeMethodParameter retVal = managedArgs.Last().Parameter;
				if (method.ReturnType.ToString().EndsWith("_t*"))//sharedMethod.RetVal.Type.EndsWith("_t*"))
				{
					body.Append(wrappers.ToString())
						.AppendFormat("{0} rv = {1};\r\n", retVal.Type, callCode)
						.AppendLine("if (rv == null)")
						.AppendLine("\treturn null;")
						.Append("return (rv != null) ? rv.GetNativeInstance() : null;");
				}
				else
				{
					body.Append(wrappers.ToString());

					if (disposers.Length == 0)
					{
						if (retVal.Type == "bool")
							body.AppendFormat("return {0} ? 1 : 0;", callCode);
						else if (retVal.Type == "long" && method.ReturnType.Name == "UIntPtr")
							body.AppendFormat("return new UIntPtr((ulong){0});", callCode);
						else
							body.AppendFormat("return {0};", callCode);
					}
					else
					{
						if (retVal.Type == "bool")
							body.AppendFormat("int rv = {0} ? 1 : 0;\r\n", callCode);
						else if (retVal.Type == "long" && method.ReturnType.Name == "UIntPtr")
							body.AppendFormat("UIntPtr rv = new UIntPtr((ulong){0});\r\n", callCode);
						else
							body.AppendFormat("{0} rv = {1};\r\n", retVal.Type, callCode);
						body.Append(disposers.ToString())
							.Append("return rv;");
					}
				}

			}


			callback.Body = body.ToString();
			Class.Members.Add(callback);
		}

		private List<CefParameterInfo> GenerateManagedCallback(IMethodSymbol method)
		{
			var callback = new CodeMethod(method.Name);
			callback.Attributes = CodeAttributes.Protected | CodeAttributes.Internal | CodeAttributes.Virtual | CodeAttributes.Unsafe;
			callback.HasThisArg = true;

			callback.Comments.AddSymbolComment(method);

			TypeSymbolInfo returnSymbolInfo = TypeSymbolInfo.FromSymbol(method.ReturnType);
			CefTypeKind typeKind = NativeTypes.GetTypeKind(returnSymbolInfo.Type);
			if (returnSymbolInfo.IsDoublePointedType)
				throw new NotImplementedException();
			if (returnSymbolInfo.IsPointedType)
			{
				if (typeKind != CefTypeKind.RefCounted && typeKind != CefTypeKind.Scoped)
					throw new NotImplementedException();
			}
			callback.RetVal = new CodeMethodParameter(null) { Type = method.IsBool() ? "bool" : returnSymbolInfo.AsClrTypeName() };

			var args = new List<CefParameterInfo>();
			IParameterSymbol prev = null;
			foreach (IParameterSymbol parameter in method.Parameters)
			{
				TypeSymbolInfo symbolInfo = TypeSymbolInfo.FromSymbol(parameter.Type);
				var paramDecl = new CodeMethodParameter(parameter.Name.ToLowerCamel().EscapeName());
				paramDecl.Type = symbolInfo.AsClrTypeName();
				if (symbolInfo.IsDoublePointedType)
				{
					paramDecl.Direction = CodeMethodParameterDirection.Ref;

					if (paramDecl.Type == "void")
					{
						paramDecl.Type = "IntPtr";
					}
					else if (paramDecl.Type == "float")
					{
						paramDecl.Direction = CodeMethodParameterDirection.Default;
						paramDecl.Type = "IntPtr";
					}
				}
				else if (symbolInfo.IsPointedType)
				{
					typeKind = NativeTypes.GetTypeKind(symbolInfo.Type);
					if (paramDecl.Type == "void")
					{
						paramDecl.Type = "IntPtr";
					}
					else if (typeKind != CefTypeKind.RefCounted && typeKind != CefTypeKind.Scoped
						&& typeKind != CefTypeKind.Sized && !parameter.IsImmutable()
						&&  paramDecl.Type != "CefWindowInfo")
					{
						paramDecl.Direction = CodeMethodParameterDirection.Ref;
					}

				}
				if (parameter.IsBool())
				{
					paramDecl.Type = "bool";
				}

				var pi = new CefParameterInfo(parameter);
				pi.Parameter = paramDecl;


				if (symbolInfo.IsPointedType && prev != null
					&& prev.Name.EndsWith("count", StringComparison.OrdinalIgnoreCase)
					&& prev.Type.Name == "UIntPtr")
				{
					paramDecl.Type += "[]";
					callback.Parameters.RemoveAt(callback.Parameters.Count - 1);
					args[args.Count - 1].IsArraySize = true;
					pi.IsArray = true;
					if (parameter.IsImmutable())
					{
						paramDecl.Direction = CodeMethodParameterDirection.In;
					}
				}
				callback.Parameters.Add(paramDecl);
				args.Add(pi);
				prev = parameter;
			}
			if (method.ReturnsVoid)
			{
				callback.Body = null;
			}
			else
			{
				callback.Body = "return default;";
				args.Add(new CefParameterInfo(method.ReturnType) { Parameter = callback.RetVal });
			}
			Class.Members.Add(callback);
			return args;
		}

		private CodeMethod GenerateAvoidCallback(IMethodSymbol method)
		{
			if (method.Parameters.Length == 0)
				return null;
			var decl = new CodeMethod("I" + Class.Name + "Private.Avoid" + method.Name);
			decl.Attributes = CodeAttributes.External;
			decl.CustomAttributes.AddMethodImplForwardRefAttribute();
			decl.RetVal = new CodeMethodParameter(null) { Type = "bool" };
			Class.Members.Add(decl);
			return decl;
		}

		private void GenerateProxyProperty(CefProperty property)
		{
			var decl = new CodeProperty(property.Name);
			decl.Attributes = CodeAttributes.Public | CodeAttributes.Unsafe | CodeAttributes.Virtual;
			string comment = property.Getter.GetComment();
			if (string.IsNullOrWhiteSpace(comment))
				comment = property.Setter.GetComment();

			if (comment.StartsWith("True if "))
				comment = "Returns true (1) if " + comment.Substring(8);

			if (comment.StartsWith("Returns true (1) if "))
			{
				if (property.Setter == null)
					comment = "Gets a value indicating whether " + comment.Substring(20);
				else
					comment = "Gets or sets a value indicating whether " + comment.Substring(20);
			}
			else if(comment.StartsWith("Returns "))
			{
				if (property.Setter == null)
					comment = "Gets " + comment.Substring(8);
				else
					comment = "Gets or sets " + comment.Substring(8);
			}
			else if (comment.StartsWith("Get "))
			{
				if (property.Setter != null)
					comment = "Gets and sets " + comment.Substring(4);
			}
			comment = comment.Replace("his function", "his property").Replace("true (1)", "true").Replace("false (0)", "false");
			decl.Comments.AddVSDocComment(System.Net.WebUtility.HtmlDecode(comment), "summary");


			TypeSymbolInfo returnSymbolInfo = TypeSymbolInfo.FromSymbol(property.Getter.ReturnType);
			CefTypeKind typeKind = NativeTypes.GetTypeKind(returnSymbolInfo.Type);
			if (returnSymbolInfo.IsDoublePointedType)
				throw new NotImplementedException();
			if (returnSymbolInfo.IsPointedType)
			{
				if (typeKind != CefTypeKind.RefCounted && typeKind != CefTypeKind.Scoped
					&& returnSymbolInfo.AsClrTypeName() != "CefBaseRefCounted")
				{
					throw new NotImplementedException();
				}
			}
			decl.Type = new CodeMethodParameter("value") { Type = property.Getter.IsBool() ? "bool" : returnSymbolInfo.AsClrTypeName() };

			CefParameterInfo parameterInfo;
			var parameters = new List<CefParameterInfo>(1);
			if (property.Getter != null)
			{
				parameterInfo = new CefParameterInfo(property.Getter.ReturnType);
				parameterInfo.Parameter = decl.Type;
				parameters.Add(parameterInfo);
				decl.GetterBody = GenerateProxyMethodBody(property.Getter, parameters);
			}
			if (property.Setter != null)
			{
				parameterInfo = new CefParameterInfo(property.Setter.Parameters[0]);
				parameterInfo.Parameter = decl.Type;
				parameters.Add(parameterInfo);
				decl.SetterBody = GenerateProxyMethodBody(property.Setter, parameters);
			}


			Class.Members.Add(decl);
		}

		private void GenerateProxyMethod(IMethodSymbol method)
		{
			var callback = new CodeMethod(method.Name);
			callback.Attributes = CodeAttributes.Public | CodeAttributes.Virtual | CodeAttributes.Unsafe;
			callback.HasThisArg = true;

			callback.Comments.AddSymbolComment(method);

			TypeSymbolInfo returnSymbolInfo = TypeSymbolInfo.FromSymbol(method.ReturnType);
			CefTypeKind typeKind = NativeTypes.GetTypeKind(returnSymbolInfo.Type);
			if (returnSymbolInfo.IsDoublePointedType)
				throw new NotImplementedException();
			if (returnSymbolInfo.IsPointedType)
			{
				if (typeKind != CefTypeKind.RefCounted && typeKind != CefTypeKind.Scoped
					&& returnSymbolInfo.AsClrTypeName() != "CefBaseRefCounted")
				{
					throw new NotImplementedException();
				}
			}
			callback.RetVal = new CodeMethodParameter(null) { Type = method.IsBool() ? "bool" : returnSymbolInfo.AsClrTypeName() };

			var args = new List<CefParameterInfo>();
			IParameterSymbol prev = null;
			foreach (IParameterSymbol parameter in method.Parameters)
			{
				TypeSymbolInfo symbolInfo = TypeSymbolInfo.FromSymbol(parameter.Type);
				var paramDecl = new CodeMethodParameter(parameter.Name.ToLowerCamel().EscapeName());
				paramDecl.Type = symbolInfo.AsClrTypeName();
				if (symbolInfo.IsDoublePointedType)
				{
					paramDecl.Direction = CodeMethodParameterDirection.Ref;
					if (paramDecl.Type == "void")
					{
						paramDecl.Type = "IntPtr";
					}
					else if (paramDecl.Type == "byte" && parameter.IsImmutable())
					{
						paramDecl.Direction = CodeMethodParameterDirection.In;
						paramDecl.Type = "IntPtr";
					}
				}
				else if (symbolInfo.IsPointedType)
				{
					typeKind = NativeTypes.GetTypeKind(symbolInfo.Type);
					if (paramDecl.Type == "void")
					{
						paramDecl.Type = "IntPtr";
					}
					else if (typeKind != CefTypeKind.RefCounted && typeKind != CefTypeKind.Scoped
						&& typeKind != CefTypeKind.Sized && !parameter.IsImmutable())
					{
						paramDecl.Direction = CodeMethodParameterDirection.Ref;
					}

				}
				if (parameter.IsBool())
				{
					paramDecl.Type = "bool";
				}

				var pi = new CefParameterInfo(parameter);
				pi.Parameter = paramDecl;


				if (symbolInfo.IsPointedType && prev != null
					&& prev.Name.EndsWith("count", StringComparison.OrdinalIgnoreCase)
					&& prev.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).StartsWith("UIntPtr"))
				{
					paramDecl.Type += "[]";
					if (args[args.Count - 1].Parameter.Direction != CodeMethodParameterDirection.Ref)
					{
						callback.Parameters.RemoveAt(callback.Parameters.Count - 1);
					}
					args[args.Count - 1].IsArraySize = true;
					pi.IsArray = true;
					if (parameter.IsImmutable())
					{
						paramDecl.Direction = CodeMethodParameterDirection.In;
					}
				}
				callback.Parameters.Add(paramDecl);
				args.Add(pi);
				prev = parameter;
			}
			if (!method.ReturnsVoid)
			{
				args.Add(new CefParameterInfo(method.ReturnType) { Parameter = callback.RetVal });
			}

			callback.Body = GenerateProxyMethodBody(method, args);

			Class.Members.Add(callback);
		}


		private string GenerateProxyMethodBody(IMethodSymbol method, List<CefParameterInfo> parameters)
		{
			CefParameterInfo returnsArg = null;
			if (!method.ReturnsVoid)
			{
				returnsArg = parameters[parameters.Count - 1];
				parameters.RemoveAt(parameters.Count - 1);
			}

			var body = new StringBuilder();

			var usings = new StringBuilder();
			var wrappers = new StringBuilder();
			var disposers = new StringBuilder();
			var args = new List<string>(method.Parameters.Length);
			int index = -1;
			CefParameterInfo prev = null;
			foreach (CefParameterInfo arg in parameters)
			{
				index++;

				if (arg.IsArraySize)
				{
					if (arg.Parameter.Direction == CodeMethodParameterDirection.Ref)
					{
						disposers.AppendFormat("{0} = (long)c{1};\r\n", arg.Parameter.Name, index + 1);
					}
					prev = arg;
					continue;
				}

				TypeSymbolInfo symbolInfo = TypeSymbolInfo.FromSymbol(((IParameterSymbol)arg.Symbol).Type);
				CefTypeKind typeKind = NativeTypes.GetTypeKind(symbolInfo.Type);

				if (arg.Parameter.Type == "string")
				{
					usings.AppendFormat("fixed (char* s{0} = {1})\r\n", index, arg.Parameter.Name);
					wrappers.AppendFormat("var cstr{0} = new cef_string_t {{ Str = s{0}, Length = {1} != null ? {1}.Length : 0 }};\r\n", index, arg.Parameter.Name);
					args.Add("&cstr" + index);

					if (arg.Parameter.Direction == CodeMethodParameterDirection.Ref)
					{
						disposers.AppendFormat("{1} = CefString.ReadAndFree(&cstr{0});\r\n", index, arg.Parameter.Name);
					}
				}
				else if (arg.Parameter.Type == "CefWindowInfo")
				{
					args.Add(arg.Parameter.Name + ".GetNativeInstance()");
				}
				else if (arg.Parameter.Type == "CefStringList")
				{
					args.Add(arg.Parameter.Name + ".GetNativeInstance()");
				}
				else if (typeKind == CefTypeKind.RefCounted)
				{
					if (symbolInfo.IsDoublePointedType)
					{
						if (arg.IsArray)
						{
							if (arg.Parameter.Direction == CodeMethodParameterDirection.Ref)
							{
								wrappers.AppendFormat("var c{0} = new UIntPtr((uint){1}.Length);\r\n", index, arg.Parameter.Name);
								wrappers.AppendFormat("{3} arr{0} = ({3})Marshal.AllocHGlobal(sizeof({2}) * {1}.Length);\r\n", index, arg.Parameter.Name, arg.NativeTypeName.Replace("**", "*"), arg.NativeTypeName);
								wrappers.AppendFormat("for (int i = 0; i < {0}.Length; i++)\r\n{{\r\n", arg.Parameter.Name);
								wrappers.AppendFormat("\tvar e{0} = {1}[i];\r\n", index, arg.Parameter.Name);
								wrappers.AppendFormat("\t*(arr{0} + i) = e{0} != null ? e{0}.GetNativeInstance() : null;\r\n}}\r\n", index);

								disposers.AppendFormat("for (int i = (int)c{0}; i >= 0; i--)\r\n{{\r\n", index);
								disposers.AppendFormat("\t{1}[i] = {2}.Wrap({2}.Create, *(arr{0} + i)); \r\n}}\r\n", index, arg.Parameter.Name, arg.Parameter.Type.Replace("[]", ""));
								disposers.AppendFormat("Marshal.FreeHGlobal((IntPtr)arr{0});\r\n", index);
								args.Add(string.Format("&c{0}, arr{0}", index));
							}
							else
							{
								wrappers.AppendFormat("{3} arr{0} = ({3})Marshal.AllocHGlobal(sizeof({2}) * {1}.Length);\r\n", index, arg.Parameter.Name, arg.NativeTypeName.Replace("**", "*"), arg.NativeTypeName);
								wrappers.AppendFormat("for (int i = 0; i < {0}.Length; i++)\r\n{{\r\n", arg.Parameter.Name);
								wrappers.AppendFormat("\tvar e{0} = {1}[i];\r\n", index, arg.Parameter.Name);
								wrappers.AppendFormat("\t*(arr{0} + i) = e{0} != null ? e{0}.GetNativeInstance() : null;\r\n}}\r\n", index);
								disposers.AppendFormat("Marshal.FreeHGlobal((IntPtr)arr{0});\r\n", index);
								args.Add(string.Format("new UIntPtr((uint){1}.Length), arr{0}", index, arg.Parameter.Name));
							}
						}
						else
						{
							wrappers.AppendFormat("{2} p{0} = ({1} != null) ? {1}.GetNativeInstance() : null;\r\n", index, arg.Parameter.Name, arg.NativeTypeName.Replace("**", "*"));
							wrappers.AppendFormat("{2} pp{0} = &p{0};\r\n", index, arg.Parameter.Name, arg.NativeTypeName);
							disposers.AppendFormat("{1} = {2}.Wrap({2}.Create, p{0});\r\n", index, arg.Parameter.Name, arg.Parameter.Type);
							args.Add("pp" + index.ToString());
						}
					}
					else
					{
						args.Add(string.Format("({0} != null) ? {0}.GetNativeInstance() : null", arg.Parameter.Name));
					}
				}
				else if (typeKind == CefTypeKind.Scoped)
				{
					if (symbolInfo.IsDoublePointedType)
					{
						if (!arg.IsArray)
						{
							args.Add(arg.Parameter.Name);
						}
						else
						{
							args.Add(arg.Parameter.Name);
						}
					}
					else
					{
						args.Add(string.Format("({0} != 0) ? {0}.GetNativeInstance() : null", arg.Parameter.Name));
					}
				}
				else if (typeKind == CefTypeKind.Sized)
				{
					args.Add(arg.Parameter.Name + ".GetNativeInstance()");
				}
				else if (typeKind == CefTypeKind.Simple && symbolInfo.IsPointedType)
				{
					if (arg.IsArray)
					{
						if (arg.Parameter.Direction == CodeMethodParameterDirection.Ref)
						{
							usings.AppendFormat("fixed ({2}* p{0} = {1})\r\n", index, arg.Parameter.Name, arg.Parameter.Type.Replace("[]", ""));
							wrappers.AppendFormat("UIntPtr c{0} = new UIntPtr((uint){1}.Length);\r\n", index, arg.Parameter.Name);
							args.Add(string.Format("&c{0}, ({1})p{0}", index, arg.NativeTypeName));
							disposers.AppendFormat("Array.Resize(ref {1}, (int)c{0});", index, arg.Parameter.Name);
						}
						else
						{
							usings.AppendFormat("fixed ({2}* p{0} = {1})\r\n", index, arg.Parameter.Name, arg.Parameter.Type.Replace("[]", ""));
							args.Add(string.Format("new UIntPtr((uint){1}.Length), ({2})p{0}", index, arg.Parameter.Name, arg.NativeTypeName));
						}
					}
					else if (arg.Parameter.Direction != CodeMethodParameterDirection.Ref)
					{
						args.Add(string.Format("({0})&{1}", arg.NativeTypeName, arg.Parameter.Name));
					}
					else
					{
						usings.AppendFormat("fixed ({2}* p{0} = &{1})\r\n", index, arg.Parameter.Name, arg.Parameter.Type);
						args.Add(string.Format("({1})p{0}", index, arg.NativeTypeName));
					}
				}
				else if (arg.NativeTypeName == "void*")
				{
					args.Add("(void*)" + arg.Parameter.Name);
				}
				else if (arg.Parameter.Direction == CodeMethodParameterDirection.Ref)
				{
					if (arg.NativeTypeName == "void**")
					{
						usings.AppendFormat("fixed ({2}* p{0} = &{1})\r\n", index, arg.Parameter.Name, arg.Parameter.Type);
						args.Add("(void**)p" + index.ToString());
					}
					else if (arg.NativeTypeName == "UIntPtr*")
					{
						wrappers.AppendFormat("UIntPtr c{0} = new UIntPtr((ulong){1});\r\n", index, arg.Parameter.Name);
						disposers.AppendFormat("{1} = (long)c{0};\r\n", index, arg.Parameter.Name);
						args.Add("&c" + index.ToString());
					}
					else if (arg.NativeTypeName == "cef_color_t*")
					{
						usings.AppendFormat("fixed ({2}* p{0} = &{1})\r\n", index, arg.Parameter.Name, arg.Parameter.Type);
						args.Add("(cef_color_t*)p" + index.ToString());
					}
					else if (arg.IsArray)
					{
						usings.AppendFormat("fixed ({2}* p{0} = {1})\r\n", index, arg.Parameter.Name, arg.Parameter.Type.Replace("[]", ""));
						wrappers.AppendFormat("UIntPtr c{0} = new UIntPtr((uint){1}.Length);\r\n", index, arg.Parameter.Name);
						args.Add(string.Format("&c{0}, p{0}", index));
						disposers.AppendFormat("Array.Resize(ref {1}, (int)c{0});\r\n", index, arg.Parameter.Name);
					}
					else
					{
						usings.AppendFormat("fixed ({2}* p{0} = &{1})\r\n", index, arg.Parameter.Name, arg.Parameter.Type);
						args.Add("p" + index.ToString());
					}
				}
				else if (symbolInfo.IsDoublePointedType)
				{
					if (arg.NativeTypeName == "byte**") //"char8**"
					{
						args.Add(string.Format("(byte**){0}", arg.Parameter.Name));
					}
					else
					{
						args.Add(string.Format("({1}){0}", arg.Parameter.Name, arg.NativeTypeName));
					}
				}
				else if (arg.Parameter.Type == "bool" && arg.NativeTypeName == "int")
				{
					args.Add(arg.Parameter.Name + " ? 1 : 0");
				}
				else if (arg.Parameter.Type == "long" && arg.NativeTypeName == "UIntPtr")
				{
					args.Add(string.Format("new UIntPtr((ulong){0})", arg.Parameter.Name));
				}
				else
				{
					args.Add(arg.Parameter.Name);
				}
				prev = arg;
			}

			string indent = string.Empty;

			if (usings.Length != 0)
			{
				WriteMultilineText(body, usings.ToString(), indent, false);
				body.AppendLine();
				body.Append("{");
				indent = "\t";
			}

			WriteMultilineText(body, wrappers.ToString(), indent, true);

			if (!method.ReturnsVoid)
			{
				if (disposers.Length == 0)
				{
					if (usings.Length == 0)
					{
						body.Append("return ");
					}
					else
					{
						body.AppendLine().Append(indent).Append("return ");
					}
				}
				else
				{
					body.AppendLine().Append(indent).Append("var rv = ");
				}
			}
			else
			{
				if (usings.Length != 0 || (body.Length > 0 && body[body.Length - 1] == '}'))
					body.AppendLine();
				body.Append(indent);
			}

			string wrappingformat = "{0};";

			TypeSymbolInfo returnsSymbolInfo = TypeSymbolInfo.FromSymbol(method.ReturnType);
			CefTypeKind returnsTypeKind = NativeTypes.GetTypeKind(returnsSymbolInfo.Type);

			if (method.ReturnType.Name == "cef_string_userfree_t")
			{
				wrappingformat = "CefString.ReadAndFree({0});";
			}
			else if (returnsTypeKind == CefTypeKind.RefCounted)
			{
				if (returnsSymbolInfo.IsDoublePointedType)
				{
					throw new NotImplementedException();
				}
				else
				{
					wrappingformat = string.Format("{0}.Wrap({0}.Create, {{0}});", returnsArg.Parameter.Type);
				}
			}
			else if (returnsTypeKind == CefTypeKind.Sized)
			{
				if (returnsArg != null && returnsArg.Parameter.Type == "CefBaseRefCounted")
				{
					wrappingformat = "UnknownRefCounted.Wrap(UnknownRefCounted.Create, {0});";
				}
			}
			else if (method.ReturnType.Name == "Int32" && (returnsArg != null && returnsArg.Parameter.Type == "bool"))
			{
				wrappingformat = "{0} != 0;";
			}
			else if (method.ReturnType.Name == "UIntPtr" && (returnsArg != null && returnsArg.Parameter.Type == "long"))
			{
				wrappingformat = "(long){0};";
			}

			if (disposers.Length == 0 && !method.ReturnsVoid)
			{
				if(wrappingformat.EndsWith(';'))
					wrappingformat = "SafeCall(" + wrappingformat.Substring(0, wrappingformat.Length - 1) + ");";
				else
					wrappingformat = "SafeCall(" + wrappingformat + ")";
			}

			body.AppendFormat(wrappingformat, string.Format("NativeInstance->{0}({1})", method.Name, string.Join(", ", args)));

			if (disposers.Length != 0)
			{
				WriteMultilineText(body, disposers.ToString(), indent, true);

				if (!method.ReturnsVoid)
				{
					body.AppendLine();
					if (usings.Length != 0)
					{
						body.Append("\tGC.KeepAlive(this);\r\n\t");
					}
					else
					{
						body.Append("GC.KeepAlive(this);\r\n");
					}
					body.Append("return rv;");
				}
			}
			if (usings.Length != 0)
			{
				body.AppendLine("\r\n}");
			}
			body.TrimEnd();
			if (method.ReturnsVoid)
			{
				body.Append("\r\nGC.KeepAlive(this);");
			}
			return body.ToString();
		}

		private static void WriteMultilineText(StringBuilder sb, string text, string indent, bool startCR)
		{
			if (text.Length == 0)
				return;

			text = text.Trim(new[] { '\r', '\n', '\t' });
			string[] lines = text.Split('\n');

			if (lines.Length > 1 || !string.IsNullOrWhiteSpace(lines[0]))
			{
				foreach (string line in lines)
				{
					if (startCR) sb.AppendLine();
					sb.Append(indent).Append(line.Trim('\r'));
					startCR = true;
				}
			}
		}

		
	}

}
