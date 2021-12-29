// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CppAst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefGen
{
	sealed class CefApiClass : CppTypeDeclaration, ICppDeclarationContainer, ICppMember, ICppElement, ICppContainer
	{
		public CefApiClass(string name)
			: base(CppTypeKind.StructOrClass)
		{
			this.Name = name;
			this.Fields = new CppContainerList<CppField>(this);
			this.Functions = new CppContainerList<CppFunction>(this);
			this.Enums = new CppContainerList<CppEnum>(this);
			this.Classes = new CppContainerList<CppClass>(this);
			this.Typedefs = new CppContainerList<CppTypedef>(this);
			this.Attributes = new CppContainerList<CppAttribute>(this);
		}

		public string ApiHash { get; set; }

		public override int SizeOf { get; set; }

		public CppContainerList<CppField> Fields { get; set; }

		public CppContainerList<CppFunction> Functions { get; set; }

		public CppContainerList<CppEnum> Enums { get; set; }

		public CppContainerList<CppClass> Classes { get; set; }

		public CppContainerList<CppTypedef> Typedefs { get; set; }

		public CppContainerList<CppAttribute> Attributes { get; set; }

		public string Name { get; set; }

		public override CppType GetCanonicalType()
		{
			return this;
		}

		public override IEnumerable<ICppDeclaration> Children()
		{
			foreach (CppEnum @enum in Enums)
				yield return @enum;

			foreach (CppClass @class in Classes)
				yield return @class;

			foreach (CppTypedef typedef in Typedefs)
				yield return typedef;

			foreach (CppField field in Fields)
				yield return field;

			foreach (CppFunction function in Functions.OrderBy(f => f.Name))
				yield return function;
		}
	}
}
