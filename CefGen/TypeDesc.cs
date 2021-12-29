// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CppAst;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefGen
{
	public class TypeDesc
	{
		private string _clrType;

		public TypeDesc(string name, CppType typeRef)
		{
			Name = name;
			_clrType = name;
			TypeRef = typeRef;
		}

		public CppType TypeRef { get; }
		public CppFunctionType FunctionTypeRef { get; set; }

		public string Name { get; }

		public string ClrType
		{
			get
			{
				return IsCallable ? "void*" : _clrType;
			}
			set
			{
				_clrType = value;
			}
		}

		public bool IsCallable
		{
			get { return FunctionTypeRef != null; }
		}

		public bool IsUnsafe
		{
			get { return TypeRef is CppPointerType; }
		}

		public override string ToString()
		{
			return ClrType;
		}
	}
}
