using System;
using ElmSharp;

namespace Microsoft.Maui.Handlers
{
	public partial class WindowHandler : ElementHandler<IWindow, Window>
	{
		public static void MapTitle(WindowHandler handler, IWindow window) { }

		public static void MapContent(WindowHandler handler, IWindow window)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			var nativeContent = window.Content.ToContainerView(handler.MauiContext);

			nativeContent.SetAlignment(-1, -1);
			nativeContent.SetWeight(1, 1);
			nativeContent.Show();
			handler.MauiContext.GetModalStack().Reset();
			handler.MauiContext.GetModalStack().Push(nativeContent);

			if (window.VisualDiagnosticsOverlay != null)
				window.VisualDiagnosticsOverlay.Initialize();
		}
	}
}