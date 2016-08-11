#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.CaliburnMicro
// 
// Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

#endregion

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
	/// <summary>
	///     Extension method to support Icon conversion
	///     TODO: check what to do when the DPI of the screen is set to something about 96
	/// </summary>
	public static class HelperExtensions
	{
		[DllImport("user32.dll")]
		private static extern bool DestroyIcon(IntPtr handle);

		/// <summary>
		///     Render the frameworkElement to an Icon
		/// </summary>
		/// <param name="frameworkElement">FrameworkElement</param>
		/// <param name="size">Size, using the bound as size by default</param>
		/// <param name="dpiX">Horizontal DPI settings</param>
		/// <param name="dpiY">Vertical DPI settings</param>
		/// <returns>Icon</returns>
		public static Icon ToIcon(this FrameworkElement frameworkElement, Size? size = null, double dpiX = 96.0, double dpiY = 96.0)
		{
			if (frameworkElement == null)
			{
				throw new ArgumentNullException(nameof(frameworkElement));
			}
			var bitmapSource = frameworkElement.ToBitmapSource(size, dpiX, dpiY);
			return bitmapSource.ToIcon();
		}

		/// <summary>
		///     Render the frameworkElement to a BitmapSource
		/// </summary>
		/// <param name="frameworkElement">FrameworkElement</param>
		/// <param name="size">Size, using the bound as size by default</param>
		/// <param name="dpiX">Horizontal DPI settings</param>
		/// <param name="dpiY">Vertical DPI settings</param>
		/// <returns>BitmapSource</returns>
		public static BitmapSource ToBitmapSource(this FrameworkElement frameworkElement, Size? size = null, double dpiX = 96.0, double dpiY = 96.0)
		{
			if (frameworkElement == null)
			{
				throw new ArgumentNullException(nameof(frameworkElement));
			}
			// Make sure we have a size
			if (!size.HasValue)
			{
				var bounds = VisualTreeHelper.GetDescendantBounds(frameworkElement);
				size = bounds != Rect.Empty ? bounds.Size : new Size(16, 16);
			}

			// Create a viewbox to render the frameworkElement in the correct size
			var viewbox = new Viewbox
			{
				//frameworkElement to render
				Child = frameworkElement
			};
			viewbox.Measure(size.Value);
			viewbox.Arrange(new Rect(new Point(), size.Value));
			viewbox.UpdateLayout();

			var renderTargetBitmap = new RenderTargetBitmap((int) (size.Value.Width*dpiX/96.0),
				(int) (size.Value.Height*dpiY/96.0),
				dpiX,
				dpiY,
				PixelFormats.Pbgra32);
			var drawingVisual = new DrawingVisual();
			using (var drawingContext = drawingVisual.RenderOpen())
			{
				var visualBrush = new VisualBrush(viewbox);
				drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(), size.Value));
			}
			renderTargetBitmap.Render(drawingVisual);
			return renderTargetBitmap;
		}

		/// <summary>
		///     Convert the BitmapSource to an Icon
		/// </summary>
		/// <param name="bitmapImage">BitmapSource</param>
		/// <returns>Icon</returns>
		public static Icon ToIcon(this BitmapSource bitmapImage)
		{
			if (bitmapImage == null)
			{
				return null;
			}

			// No need to dispse the memorystream as the created tmpBitmap "owns" the stream and already disposes it.
			var memoryStream = new MemoryStream();
			var encoder = new PngBitmapEncoder(); // With this we also respect transparency.
			encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
			encoder.Save(memoryStream);
			// Make sure the stream is read from the beginning
			memoryStream.Seek(0, SeekOrigin.Begin);
			using (var tmpBitmap = new Bitmap(memoryStream))
			{
				var iconHandle = tmpBitmap.GetHicon();
				try
				{
					// Create a clone, otherwise a dispose on the returned icon will NOT dispose the resources correctly!
					return (Icon) Icon.FromHandle(iconHandle).Clone();
				}
				finally
				{
					// Dispose of the icon resource that were created
					DestroyIcon(iconHandle);
				}
			}
		}
	}
}