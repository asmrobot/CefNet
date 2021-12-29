using CefNet;
using CefNet.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsCoreApp
{
	static class ScriptableObjectTests
	{
		public static void SendTestScriptableObjectToRenderer(CefFrame frame)
		{
			frame.SendProcessMessage(CefProcessId.Renderer, new CefProcessMessage("test scriptableobject"));
		}

		public static void SendGCCollectToRenderer(CefFrame frame)
		{
			frame.SendProcessMessage(CefProcessId.Renderer, new CefProcessMessage("call GC.Collect()"));
		}

		public static async void HandleScriptableObjectTestMessage(object sender, CefProcessMessageReceivedEventArgs e)
		{
			if (e.Name == "test scriptableobject")
			{
				await ScriptableObjectTestAsync(e.Frame);
			}
			else if (e.Name == "call GC.Collect()")
			{
				GC.Collect();
			}
		}

		public static async Task ScriptableObjectTestAsync(CefFrame frame)
		{
			dynamic scriptableObject1 = await frame.GetScriptableObjectAsync(CancellationToken.None)
				.ConfigureAwait(true);
			dynamic scriptableObject2 = await frame.GetScriptableObjectAsync(CancellationToken.None)
				.ConfigureAwait(false);
			dynamic window = scriptableObject1.window;
			dynamic document = window.document;

			document.title = "document title";

			window.alert(string.Format("Equals: {0}", scriptableObject1 == scriptableObject2 && document == window.document));

			window.alert(document.querySelectorAll("div")[0]);

			MessageBox.Show("window.hello = " + Convert.ToString(window.hello));
		}
	}
}
