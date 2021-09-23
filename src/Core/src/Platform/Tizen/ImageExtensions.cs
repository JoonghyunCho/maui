﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Tizen.UIExtensions.ElmSharp;

namespace Microsoft.Maui
{
	public static class ImageExtensions
	{
		public static void Clear(this Image nativeImage)
		{
		}

		public static void UpdateAspect(this Image nativeImage, IImage image)
		{
			nativeImage.Aspect = image.Aspect.ToNative();
		}

		public static void UpdateIsAnimationPlaying(this Image nativeImage, IImageSourcePart image)
		{
			nativeImage.IsAnimated = image.IsAnimationPlaying;
			nativeImage.IsAnimationPlaying = image.IsAnimationPlaying;
		}

		public static async Task<IImageSourceServiceResult<Image>?> UpdateSourceAsync(this Image nativeImage, IImageSourcePart image, IImageSourceServiceProvider services, CancellationToken cancellationToken = default)
		{
			nativeImage.Clear();

			image.UpdateIsLoading(false);

			var imageSource = image.Source;
			if (imageSource == null)
				return null;

			var events = image as IImageSourcePartEvents;

			events?.LoadingStarted();
			image.UpdateIsLoading(true);

			try
			{
				var service = services.GetRequiredImageSourceService(imageSource);
				var result = await service.GetImageAsync(imageSource, nativeImage, cancellationToken);
				var isLoaded = result != null;

				var applied = !cancellationToken.IsCancellationRequested && isLoaded && imageSource == image.Source;

				// only set the image if we are still on the same one
				if (applied)
				{
					nativeImage.UpdateIsAnimationPlaying(image);
				}

				events?.LoadingCompleted(applied);

				return result;
			}
			catch (OperationCanceledException)
			{
				// no-op
				events?.LoadingCompleted(false);
			}
			catch (Exception ex)
			{
				events?.LoadingFailed(ex);
			}
			finally
			{
				// only mark as finished if we are still working on the same image
				if (imageSource == image.Source)
				{
					image.UpdateIsLoading(false);
				}
			}

			return null;
		}

		public static async Task<IImageSourceServiceResult<Image>?> UpdateSourceAsync(this IImageSourcePart image, Image destinationContext, IImageSourceServiceProvider services, Action<Image?> setImage, CancellationToken cancellationToken = default)
		{
			image.UpdateIsLoading(false);

			var imageSource = image.Source;
			if (imageSource == null)
				return null;

			var events = image as IImageSourcePartEvents;

			events?.LoadingStarted();
			image.UpdateIsLoading(true);

			try
			{
				var service = services.GetRequiredImageSourceService(imageSource);
				var result = await service.GetImageAsync(imageSource, destinationContext, cancellationToken);
				var tImage = result?.Value;

				var applied = !cancellationToken.IsCancellationRequested && tImage != null && imageSource == image.Source;

				// only set the image if we are still on the same one
				if (applied)
				{
					setImage.Invoke(tImage);
					destinationContext.UpdateIsAnimationPlaying(image);
				}

				events?.LoadingCompleted(applied);

				return result;
			}
			catch (OperationCanceledException)
			{
				// no-op
				events?.LoadingCompleted(false);
			}
			catch (Exception ex)
			{
				events?.LoadingFailed(ex);
			}
			finally
			{
				// only mark as finished if we are still working on the same image
				if (imageSource == image.Source)
				{
					image.UpdateIsLoading(false);
				}
			}

			return null;
		}
	}
}
