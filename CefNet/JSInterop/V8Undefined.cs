using System;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using System.Collections;

namespace CefNet.JSInterop
{
	[TypeConverter(typeof(V8UndefinedConverter)) , DebuggerDisplay("undefined"), DebuggerStepThrough]
	public struct V8Undefined : IConvertible
	{
		public static V8Undefined Value = default;

		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.Empty;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return false;
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return 0;
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return 0;
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return 0;
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return 0;
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return 0;
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return 0;
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return 0;
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return 0;
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return "undefined";
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return conversionType.IsValueType ? Activator.CreateInstance(conversionType) : null;
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return 0;
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return 0;
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return 0;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static explicit operator string(V8Undefined value)
		{
			return "undefined";
		}
	}


	public class V8UndefinedConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return true;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return destinationType.IsPrimitive ? Activator.CreateInstance(destinationType) : null;
		}

		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return V8Undefined.Value;
		}

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}

}
