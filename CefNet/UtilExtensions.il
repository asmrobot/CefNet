.class auto ansi abstract sealed beforefieldinit CefNet.UtilsExtensions
{
	.method public hidebysig static void InitBlock(native int startAddress, uint8 'value', int32 size)
		cil managed aggressiveinlining 
	{
		.maxstack 3

		ldarg.0
		ldarg.1
		ldarg.2
		unaligned. 1
		initblk
		ret
	}

	.method public hidebysig static !!TTo& AsRef<TFrom, TTo>([in] !!TFrom& source) cil managed aggressiveinlining
	{
		.param [1]
		.custom instance void [mscorlib]System.Runtime.CompilerServices.IsReadOnlyAttribute::.ctor() = ( 01 00 00 00 )
		.maxstack 1
		ldarg.0
		ret
	}

	.method public hidebysig static !!T As<class T>(object o) cil managed aggressiveinlining
	{
		.maxstack 1
		ldarg.0
		ret
	}

}