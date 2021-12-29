using CefNet;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RCWTest
{
	unsafe class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			IntPtr key = Test1();
			Console.WriteLine("collect");
			GC.Collect();
			GC.WaitForPendingFinalizers();
			Console.WriteLine("release");
			Test2(key);
		
			for (int i = 0; i < 5; i++)
			{
				Console.WriteLine("collect");
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
			Console.ReadKey();
		}

		public static IntPtr Test1()
		{
			var test = new TestClass();
			var app = new CefAppImpl();
			IntPtr key = (IntPtr)app.GetNativeInstance();
			app = null;
			test = null;
			return key;
		}

		public static void Test2(IntPtr key)
		{
			CefAppImpl app = (CefAppImpl)CefAppImpl.GetInstance(key);
			app.Release();
		}

	}

	sealed class TestClass
	{
		~TestClass()
		{
			Console.WriteLine("finalize TestClass");
		}
	}

	sealed class CefAppImpl : CefApp
	{
		~CefAppImpl()
		{
			Console.WriteLine("finalize CefAppImpl");
		}
	}
}
