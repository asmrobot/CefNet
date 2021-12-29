// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefGen
{

	public class TypeSymbolInfo
	{
		private TypeSymbolInfo() { }

		private int PointerCount;

		public ITypeSymbol Type { get; private set; }

		public bool IsPointedType
		{
			get { return PointerCount > 0; }
		}

		public bool IsDoublePointedType
		{
			get { return PointerCount > 1; }
		}

		public string AsClrTypeName()
		{
			string name = Type.Name;
			if (name.EndsWith("_t"))
				name = name.Remove(name.Length - 2).ToUpperCamel();
			return DotnetTypeNameToCSharpTypeName(name);
		}

		public override string ToString()
		{
			if (IsDoublePointedType)
				return DotnetTypeNameToCSharpTypeName(Type.Name) + "**";
			if (IsPointedType)
				return DotnetTypeNameToCSharpTypeName(Type.Name) + "*";
			return DotnetTypeNameToCSharpTypeName(Type.Name);
		}

		private static string DotnetTypeNameToCSharpTypeName(string typeName)
		{
			switch (typeName)
			{
				case "Void":
				case "SByte":
				case "Byte":
				case "Double":
					return typeName.ToLowerInvariant();
				case "Int32":
					return "int";
				case "Int64":
					return "long";
				case "Int16":
					return "short";
				case "UInt32":
					return "uint";
				case "UInt64":
					return "ulong";
				case "UInt16":
					return "ushort";
				case "Single":
					return "float";
				case "CefString":
				case "CefStringUserfree":
					return "string";
				case "UIntPtr":
					return "long";
				case "Char":
				case "char":
					return "char16";
			}
			return typeName;
		}

		public static TypeSymbolInfo FromSymbol(ITypeSymbol symbol)
		{
			var info = new TypeSymbolInfo();
			info.PointerCount = 0;
			while (symbol is IPointerTypeSymbol pointerType)
			{
				info.PointerCount++;
				symbol = pointerType.PointedAtType;
			}
			info.Type = symbol;
			return info;
		}
	}

}
