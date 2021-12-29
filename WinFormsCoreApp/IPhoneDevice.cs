using CefNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsCoreApp
{
	public enum IPhone
	{
		Model4 = 40,
		Model4s = 41,
		Model5 = 50,
		Model5s = 51,
		Model5c = 52,
		ModelSE = 53,
		Model6 = 60,
		Model6Plus = 61,
		Model6s = 62,
		Model6sPlus = 63,
		Model7 = 70,
		Model7s = 71,
		Model7Plus = 72,
		Model8 = 80,
		Model8Plus = 81,
		ModelX = 100,
		ModelXR = 101,
		ModelXS = 102,
		ModelXSMax = 103,
		Model11 = 110,
		Model11Pro = 111,
		Model11ProMax = 112,

	}

	public sealed class IPhoneDevice : VirtualDevice
	{
		private bool _landscapeMode;

		public IPhoneDevice(IPhone model, int screenWidth, int screenHeight, float devicePixelRatio)
			: base(new CefScreenInfo
			{
				Rect = new CefRect(0, 0, screenWidth, screenHeight),
				AvailableRect = new CefRect(0, 0, screenWidth, screenHeight),
				Depth = 32,
				DepthPerComponent = 32,
				DeviceScaleFactor = devicePixelRatio
			})
		{
			if (model < IPhone.ModelX)
			{
				this.NormalTopPanel = 70;
				this.CollapsedTopPanel = 39.5f;
				this.NormalBottomPanel = 44;
			}
			else
			{
				this.NormalTopPanel = 94;
				this.CollapsedTopPanel = 62.8f;
				this.NormalBottomPanel = 83;
			}
			ShowControlTools(false);
		}

		public static IPhoneDevice Create(IPhone model)
		{
			switch (model)
			{
				case IPhone.Model4:
				case IPhone.Model4s:
					return new IPhoneDevice(model, 320, 480, 2);
				case IPhone.Model5:
				case IPhone.Model5c:
				case IPhone.Model5s:
				case IPhone.ModelSE:
					return new IPhoneDevice(model, 320, 568, 2);
				case IPhone.Model6:
				case IPhone.Model6s:
				case IPhone.Model7:
				case IPhone.Model8:
					return new IPhoneDevice(model, 375, 667, 2);
				case IPhone.Model6Plus:
				case IPhone.Model6sPlus:
					return new IPhoneDevice(model, 375, 667, 3);
				case IPhone.Model7Plus:
				case IPhone.Model8Plus:
					return new IPhoneDevice(model, 414, 736, 3);
				case IPhone.ModelX:
				case IPhone.ModelXS:
				case IPhone.Model11Pro:
					return new IPhoneDevice(model, 375, 812, 3);
				case IPhone.ModelXR:
				case IPhone.Model11:
					return new IPhoneDevice(model, 414, 896, 2);
				case IPhone.ModelXSMax:
				case IPhone.Model11ProMax:
					return new IPhoneDevice(model, 414, 896, 3);
			}
			throw new NotSupportedException();
		}

		public IPhone Model { get; private set; }

		public override CefRect ViewportRect
		{
			get
			{
				CefRect r = ScreenInfo.Rect;
				if (_landscapeMode)
				{
					r = new CefRect(0, 0, r.Height, r.Width);
				}
				if (UseDeviceWidth)
					return new CefRect(0, 0, r.Width, (int)(r.Height - TopPanel - BottomPanel));
				return new CefRect(0, 0, 980, (int)((r.Height - TopPanel - BottomPanel) / r.Width * 980.0f));
			}
		}

		public bool UseDeviceWidth { get; set; }

		private float CollapsedTopPanel { get; }

		private float NormalTopPanel { get; }

		private float NormalBottomPanel { get; }

		public float TopPanel { get; private set; }

		public float BottomPanel { get; private set; }

		public void ShowControlTools(bool show)
		{
			if (show)
			{
				TopPanel = NormalTopPanel;
				BottomPanel = NormalBottomPanel;
			}
			else
			{
				TopPanel = CollapsedTopPanel;
				BottomPanel = 0;
			}
		}

		public override void Rotate()
		{
			_landscapeMode = !_landscapeMode;
		}
	}
}
