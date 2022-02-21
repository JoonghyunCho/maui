#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Graphics.Platform.PlatformGraphicsView;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Graphics.Platform.PlatformGraphicsView;
#elif WINDOWS
using PlatformView = Microsoft.Maui.Graphics.Win2D.W2DGraphicsView;
#elif TIZEN
using PlatformView = Microsoft.Maui.Graphics.Skia.Views.SkiaGraphicsView;
#elif NETSTANDARD || (NET6_0 && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IGraphicsViewHandler : IViewHandler
	{
		new IGraphicsView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}