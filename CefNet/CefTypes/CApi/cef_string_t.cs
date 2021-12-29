#if BIT64
#undef BIT64
#endif

using System;
using System.Runtime.InteropServices;

namespace CefNet.CApi
{
	/// <summary>
	/// Represents CEF string.
	/// </summary>
	public unsafe partial struct cef_string_t
	{
		/// <summary>
		/// Gets and sets the pointer to allocated memory for the current string.
		/// </summary>
		public char* Str
		{
			get { return Base.str; }
			set { Base.str = value; }
		}

		/// <summary>
		/// Gets and sets the size of the current CEF string.
		/// </summary>
		public int Length
		{
			get { return (int)Base.length; }
			set { Base.length = unchecked((UIntPtr)value); }
		}

		/// <summary>
		/// Returns the hash code for this string.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			// source: https://github.com/dotnet/corefx/blob/a10890f4ffe0fadf090c922578ba0e606ebdd16c/src/Common/src/System/Text/StringOrCharArray.cs#L140

			char* s = this.Base.str;
			int count = (s == null) ? 0 : this.Length;

			int hash1 = (5381 << 16) + 5381;
			int hash2 = hash1;

			for (int i = 0; i < count; ++i)
			{
				int c = *s++;
				hash1 = unchecked((hash1 << 5) + hash1) ^ c;

				if (++i >= count)
					break;

				c = *s++;
				hash2 = unchecked((hash2 << 5) + hash2) ^ c;
			}

			return unchecked(hash1 + (hash2 * 1566083941));
		}

		/// <summary>
		/// Compares this CEF string with the specified <see cref="String"/> object by evaluating
		/// the numeric values of the corresponding <see cref="Char"/> objects in each string.
		/// </summary>
		/// <param name="s">A string to compare.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
		public unsafe int CompareOrdinal(string s)
		{
			fixed (cef_string_utf16_t* self = &Base)
			{
				return CompareOrdinal((cef_string_t*)self, s);
			}
		}

		/// <summary>
		/// Compares the specified CEF string with the specified <see cref="String"/> object by evaluating
		/// the numeric values of the corresponding <see cref="Char"/> objects in each string.
		/// </summary>
		/// <param name="a">The pointer to a CEF string to compare.</param>
		/// <param name="b">The string to compare.</param>
		/// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
		public static unsafe int CompareOrdinal(cef_string_t* a, string b)
		{
			if (a == null)
			{
				return (b == null) ? 0 : -1;
			}

			if (b == null)
				return 1;

			if (a->Length != b.Length)
				return a->Length - b.Length;

			if (*(a->Str) - b[0] != 0)
			{
				return *(a->Str) - b[0];
			}
			return IntPtr.Size == 8 ? CompareOrdinalHelper64(a, b) : CompareOrdinalHelper32(a, b);
		}

		private unsafe static int CompareOrdinalHelper32(cef_string_t* strA, string strB)
		{
			// Source: https://github.com/dotnet/runtime/blob/1e3e7a9c368e04764b27de401c98b848d38febf8/src/libraries/System.Private.CoreLib/src/System/String.Comparison.cs#L59

			//// NOTE: This may be subject to change if eliminating the check
			//// in the callers makes them small enough to be inlined by the JIT
			//Contract.Assert(strA.m_firstChar == strB.m_firstChar,
			//	"For performance reasons, callers of this method should " +
			//	"check/short-circuit beforehand if the first char is the same.");

			int length = Math.Min(strA->Length, strB.Length);

			//fixed (char* ap = strA->Base.str)
			fixed (char* bp = strB)
			{
				//char* a = ap;
				char* b = bp;
				char* a = strA->Base.str;
				//char* b = strB->Base.str;

				// Check if the second chars are different here
				// The reason we check if m_firstChar is different is because
				// it's the most common case and allows us to avoid a method call
				// to here.
				// The reason we check if the second char is different is because
				// if the first two chars the same we can increment by 4 bytes,
				// leaving us word-aligned on both 32-bit (12 bytes into the string)
				// and 64-bit (16 bytes) platforms.

				// For empty strings, the second char will be null due to padding.
				// The start of the string (not including sync block pointer)
				// is the method table pointer + string length, which takes up
				// 8 bytes on 32-bit, 12 on x64. For empty strings the null
				// terminator immediately follows, leaving us with an object
				// 10/14 bytes in size. Since everything needs to be a multiple
				// of 4/8, this will get padded and zeroed out.

				// For one-char strings the second char will be the null terminator.

				// NOTE: If in the future there is a way to read the second char
				// without pinning the string (e.g. System.Runtime.CompilerServices.Unsafe
				// is exposed to mscorlib, or a future version of C# allows inline IL),
				// then do that and short-circuit before the fixed.

				if (*(a + 1) != *(b + 1)) goto DiffOffset1;

				// Since we know that the first two chars are the same,
				// we can increment by 2 here and skip 4 bytes.
				// This leaves us 8-byte aligned, which results
				// on better perf for 64-bit platforms.
				length -= 2; a += 2; b += 2;

				// unroll the loop
#if BIT64
				while (length >= 12)
				{
					if (*(long*)a != *(long*)b) goto DiffOffset0;
					if (*(long*)(a + 4) != *(long*)(b + 4)) goto DiffOffset4;
					if (*(long*)(a + 8) != *(long*)(b + 8)) goto DiffOffset8;
					length -= 12; a += 12; b += 12;
				}
#else // BIT64
				while (length >= 10)
				{
					if (*(int*)a != *(int*)b) goto DiffOffset0;
					if (*(int*)(a + 2) != *(int*)(b + 2)) goto DiffOffset2;
					if (*(int*)(a + 4) != *(int*)(b + 4)) goto DiffOffset4;
					if (*(int*)(a + 6) != *(int*)(b + 6)) goto DiffOffset6;
					if (*(int*)(a + 8) != *(int*)(b + 8)) goto DiffOffset8;
					length -= 10; a += 10; b += 10;
				}
#endif // BIT64

				// Fallback loop:
				// go back to slower code path and do comparison on 4 bytes at a time.
				// This depends on the fact that the String objects are
				// always zero terminated and that the terminating zero is not included
				// in the length. For odd string sizes, the last compare will include
				// the zero terminator.
				while (length > 0)
				{
					if (*(int*)a != *(int*)b) goto DiffNextInt;
					length -= 2;
					a += 2;
					b += 2;
				}

				// At this point, we have compared all the characters in at least one string.
				// The longer string will be larger.
				return strA->Length - strB.Length;

#if BIT64
				DiffOffset8: a += 4; b += 4;
				DiffOffset4: a += 4; b += 4;
#else // BIT64
				// Use jumps instead of falling through, since
				// otherwise going to DiffOffset8 will involve
				// 8 add instructions before getting to DiffNextInt
				DiffOffset8: a += 8; b += 8; goto DiffOffset0;
				DiffOffset6: a += 6; b += 6; goto DiffOffset0;
				DiffOffset4: a += 2; b += 2;
				DiffOffset2: a += 2; b += 2;
#endif // BIT64

				DiffOffset0:
				// If we reached here, we already see a difference in the unrolled loop above
#if BIT64
				if (*(int*)a == *(int*)b)
				{
					a += 2; b += 2;
				}
#endif // BIT64

				DiffNextInt:
				if (*a != *b) return *a - *b;

				DiffOffset1:
				return *(a + 1) - *(b + 1);
			}
		}

		private unsafe static int CompareOrdinalHelper64(cef_string_t* strA, string strB)
		{
			// See the CompareOrdinalHelper32 for details.

			int length = Math.Min(strA->Length, strB.Length);

			fixed (char* bp = strB)
			{
				char* b = bp;
				char* a = strA->Base.str;
				if (*(a + 1) != *(b + 1)) goto DiffOffset1;
				length -= 2; a += 2; b += 2;

				while (length >= 12)
				{
					if (*(long*)a != *(long*)b) goto DiffOffset0;
					if (*(long*)(a + 4) != *(long*)(b + 4)) goto DiffOffset4;
					if (*(long*)(a + 8) != *(long*)(b + 8)) goto DiffOffset8;
					length -= 12; a += 12; b += 12;
				}

				while (length > 0)
				{
					if (*(int*)a != *(int*)b) goto DiffNextInt;
					length -= 2;
					a += 2;
					b += 2;
				}

				return strA->Length - strB.Length;

				DiffOffset8: a += 4; b += 4;
				DiffOffset4: a += 4; b += 4;


				DiffOffset0:
				if (*(int*)a == *(int*)b)
				{
					a += 2; b += 2;
				}

				DiffNextInt:
				if (*a != *b) return *a - *b;

				DiffOffset1:
				return *(a + 1) - *(b + 1);
			}
		}


	}
}
