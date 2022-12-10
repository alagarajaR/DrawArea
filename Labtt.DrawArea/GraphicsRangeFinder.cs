using System;
using System.Drawing;

namespace Labtt.DrawArea
{

	public class GraphicsRangeFinder : GraphicsSegment
	{
		public float RealDistance { get; set; }

		public GraphicsRangeFinder()
			: this(0f, 0f, 0f, 0f)
		{
		}

		public GraphicsRangeFinder(float x1, float y1, float x2, float y2)
			: base(x1, y1, x2, y2)
		{
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			base.Draw(rectangle, g);
			if (startPoint == endPoint)
			{
				return;
			}
			Pen pen = new Pen(base.Color, base.PenWidth);
			PointF empty = PointF.Empty;
			PointF empty2 = PointF.Empty;
			PointF empty3 = PointF.Empty;
			PointF empty4 = PointF.Empty;
			int num = 20;
			if (startPoint.X == endPoint.X)
			{
				empty = new PointF(startPoint.X - (float)num, startPoint.Y);
				empty2 = new PointF(startPoint.X + (float)num, startPoint.Y);
				empty3 = new PointF(endPoint.X - (float)num, endPoint.Y);
				empty4 = new PointF(endPoint.X + (float)num, endPoint.Y);
			}
			else if (startPoint.Y == endPoint.Y)
			{
				empty = new PointF(startPoint.X, startPoint.Y - (float)num);
				empty2 = new PointF(startPoint.X, startPoint.Y + (float)num);
				empty3 = new PointF(endPoint.X, endPoint.Y - (float)num);
				empty4 = new PointF(endPoint.X, endPoint.Y + (float)num);
			}
			else
			{
				float num2 = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
				double num3 = Math.Atan(num2);
				float num4 = (float)(180.0 * (num3 / Math.PI));
				float num5 = Math.Abs(num4);
				int num6 = (int)((double)num * Math.Sin((double)num5 / 180.0 * Math.PI));
				int num7 = (int)((double)num * Math.Cos((double)num5 / 180.0 * Math.PI));
				if (num4 > 0f)
				{
					empty = new PointF(startPoint.X + (float)num6, startPoint.Y - (float)num7);
					empty2 = new PointF(startPoint.X - (float)num6, startPoint.Y + (float)num7);
					empty3 = new PointF(endPoint.X + (float)num6, endPoint.Y - (float)num7);
					empty4 = new PointF(endPoint.X - (float)num6, endPoint.Y + (float)num7);
				}
				else
				{
					empty = new PointF(startPoint.X + (float)num6, startPoint.Y + (float)num7);
					empty2 = new PointF(startPoint.X - (float)num6, startPoint.Y - (float)num7);
					empty3 = new PointF(endPoint.X + (float)num6, endPoint.Y + (float)num7);
					empty4 = new PointF(endPoint.X - (float)num6, endPoint.Y - (float)num7);
				}
			}
			g.DrawLine(pen, empty.X - rectangle.X, empty.Y - rectangle.Y, empty2.X - rectangle.X, empty2.Y - rectangle.Y);
			g.DrawLine(pen, empty3.X - rectangle.X, empty3.Y - rectangle.Y, empty4.X - rectangle.X, empty4.Y - rectangle.Y);
			PointF pointF = new PointF((startPoint.X + endPoint.X) / 2f, (startPoint.Y + endPoint.Y) / 2f);
			g.DrawString(RealDistance.ToString("F2") + "um", new Font("Courier", 15f, FontStyle.Regular), new SolidBrush(base.Color), pointF.X - rectangle.X, pointF.Y - rectangle.Y);
		}
	}
}
