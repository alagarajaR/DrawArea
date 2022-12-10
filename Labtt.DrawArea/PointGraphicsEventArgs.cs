using System;
using System.Drawing;

namespace Labtt.DrawArea
{

	public class PointGraphicsEventArgs : EventArgs
	{
		private string identifier;

		private PointF imagePoint;

		private PointF physicalCoordinate;

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

		public PointF ImagePoint
		{
			get
			{
				return imagePoint;
			}
			internal set
			{
				imagePoint = value;
			}
		}

		public PointF PhysicalCoordinate
		{
			get
			{
				return physicalCoordinate;
			}
			internal set
			{
				physicalCoordinate = value;
			}
		}
	}
}
