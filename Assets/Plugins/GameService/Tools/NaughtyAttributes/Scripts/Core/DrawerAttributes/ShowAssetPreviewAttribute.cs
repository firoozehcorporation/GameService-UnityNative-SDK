using System;

namespace Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ShowAssetPreviewAttribute : DrawerAttribute
	{
		public int Width { get; private set; }
		public int Height { get; private set; }

		public ShowAssetPreviewAttribute(int width = 64, int height = 64)
		{
			Width = width;
			Height = height;
		}
	}
}
