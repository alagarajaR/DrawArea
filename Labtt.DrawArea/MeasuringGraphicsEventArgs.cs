using System;

namespace Labtt.DrawArea
{

	public class MeasuringGraphicsEventArgs : EventArgs
	{
		private string identifier;

		private float xImagePixel;

		private float yImagePixel;

		private float xRealDistance;

		private float yRealDistance;

		public string Identifier
		{
			get
			{
				return identifier;
			}
			internal set
			{
				identifier = value;
			}
		}

		public float XImagePixel
		{
			get
			{
				return xImagePixel;
			}
			internal set
			{
				xImagePixel = value;
			}
		}

		public float YImagePixel
		{
			get
			{
				return yImagePixel;
			}
			internal set
			{
				yImagePixel = value;
			}
		}

		public float XPhysicalDistance
		{
			get
			{
				return xRealDistance;
			}
			internal set
			{
				xRealDistance = value;
			}
		}

		public float YPhysicalDistance
		{
			get
			{
				return yRealDistance;
			}
			internal set
			{
				yRealDistance = value;
			}
		}
	}
}
