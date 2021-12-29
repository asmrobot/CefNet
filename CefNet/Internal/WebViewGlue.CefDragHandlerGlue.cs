using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		public void CreateOrDestroyDragGlue()
		{
			if (AvoidOnDragEnter() && AvoidOnDraggableRegionsChanged())
				this.DragGlue = null;
			else if (this.DragGlue is null)
				this.DragGlue = new CefDragHandlerGlue(this);
		}

		internal bool AvoidOnDragEnter()
		{
			return false;
		}

		internal protected virtual bool OnDragEnter(CefBrowser browser, CefDragData dragData, CefDragOperationsMask mask)
		{
			return false;
		}

		internal bool AvoidOnDraggableRegionsChanged()
		{
			return false;
		}

		internal protected virtual void OnDraggableRegionsChanged(CefBrowser browser, CefFrame frame, CefDraggableRegion[] regions)
		{

		}
	}
}
