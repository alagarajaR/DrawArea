using System.Collections.Generic;
using System.Drawing;

namespace Labtt.DrawArea
{

	public class GraphicsLineByTwoPoint : GraphicsSegment
	{
		public GraphicsLineByTwoPoint()
			: base(0f, 0f, 0f, 0f)
		{
		}

		public GraphicsLineByTwoPoint(float x1, float y1, float x2, float y2)
			: base(x1, y1, x2, y2)
		{
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			if (!(startPoint == endPoint))
			{
				Brush brush = new SolidBrush(base.Color);
				Pen pen = new Pen(base.Color, base.PenWidth);
				g.FillEllipse(brush, startPoint.X - 3f - rectangle.X, startPoint.Y - 3f - rectangle.Y, 6f, 6f);
				g.FillEllipse(brush, endPoint.X - 3f - rectangle.X, endPoint.Y - 3f - rectangle.Y, 6f, 6f);
				List<PointF> intersectionOfLineAndRectangle = FunctionSet.GetIntersectionOfLineAndRectangle(startPoint, endPoint, rectangle);
				if (intersectionOfLineAndRectangle.Count >= 2)
				{
					g.DrawLine(pen, intersectionOfLineAndRectangle[0].X - rectangle.X, intersectionOfLineAndRectangle[0].Y - rectangle.Y, intersectionOfLineAndRectangle[1].X - rectangle.X, intersectionOfLineAndRectangle[1].Y - rectangle.Y);
				}
			}
		}
	}
}
