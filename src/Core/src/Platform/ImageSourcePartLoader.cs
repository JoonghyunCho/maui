using System;
using System.Threading.Tasks;
using Microsoft.Maui.Handlers;

#if __IOS__ || MACCATALYST
using PlatformImage = UIKit.UIImage;
using PlatformView = UIKit.UIView;
#elif MONOANDROID
using PlatformImage = Android.Graphics.Drawables.Drawable;
using PlatformView = Android.Views.View;
#elif WINDOWS
using PlatformImage = Microsoft.UI.Xaml.Media.ImageSource;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif TIZEN
using PlatformImage = Tizen.UIExtensions.ElmSharp.Image;
using PlatformView = Tizen.UIExtensions.ElmSharp.Image;
#elif NETSTANDARD || (NET6_0 && !IOS && !ANDROID && !TIZEN)
using PlatformImage = System.Object;
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Platform
{
	public partial class ImageSourcePartLoader
	{
		IImageSourceServiceProvider? _imageSourceServiceProvider;
		IImageSourceServiceProvider ImageSourceServiceProvider =>
			_imageSourceServiceProvider ??= Handler.GetRequiredService<IImageSourceServiceProvider>();

		readonly Func<IImageSourcePart?> _imageSourcePart;
		Action<PlatformImage?>? SetImage { get; }
		PlatformView? PlatformView => Handler.PlatformView as PlatformView;

		internal ImageSourceServiceResultManager SourceManager { get; } = new ImageSourceServiceResultManager();

		IElementHandler Handler { get; }

		public ImageSourcePartLoader(
			IElementHandler handler,
			Func<IImageSourcePart?> imageSourcePart,
			Action<PlatformImage?> setImage)
		{
			Handler = handler;
			_imageSourcePart = imageSourcePart;
			SetImage = setImage;
		}

		public void Reset()
		{
			SourceManager.Reset();
		}

		public async Task UpdateImageSourceAsync()
		{
			if (PlatformView != null)
			{
				var token = this.SourceManager.BeginLoad();
				var imageSource = _imageSourcePart();

				if (imageSource != null)
				{
<<<<<<< HEAD
#if __IOS__ || __ANDROID__ || WINDOWS
					var result = await imageSource.UpdateSourceAsync(PlatformView, ImageSourceServiceProvider, SetImage!, token)
=======
#if __IOS__ || __ANDROID__ || WINDOWS || TIZEN
					var result = await imageSource.UpdateSourceAsync(PlatformView, ImageSourceServiceProvider, SetImage!, token)
>>>>>>> fea972f23 (Bump to latest (rc1))
						.ConfigureAwait(false);

					SourceManager.CompleteLoad(result);
#else
					await Task.CompletedTask;
#endif
				}
				else
				{
					SetImage?.Invoke(null);
					SourceManager.CompleteLoad(null);
				}
			}
		}
	}
}
