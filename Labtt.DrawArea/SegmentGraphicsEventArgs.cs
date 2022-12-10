using System;
using System.Drawing;

namespace Labtt.DrawArea
{

	public class SegmentGraphicsEventArgs : EventArgs
	{
		private string identifier;

		private float slope;

		private float xImagePixel;

		private float yImagePixel;

		private float xRealDistance;

		private float yRealDistance;

		private PointF imageStart;

		private PointF physicalStart;

		private PointF imageEnd;

		private PointF physicalEnd;

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

		public float Slope
		{
			get
			{
				return slope;
			}
			internal set
			{
				slope = value;
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

		public PointF ImageStart
		{
			get
			{
				return imageStart;
			}
			internal set
			{
				imageStart = value;
			}
		}

		public PointF PhysicalStart
		{
			get
			{
				return physicalStart;
			}
			internal set
			{
				physicalStart = value;
			}
		}

		public PointF ImageEnd
		{
			get
			{
				return imageEnd;
			}
			internal set
			{
				imageEnd = value;
			}
		}

		public PointF PhysicalEnd
		{
			get
			{
				return physicalEnd;
			}
			internal set
			{
				physicalEnd = value;
			}
		}
	}
}
