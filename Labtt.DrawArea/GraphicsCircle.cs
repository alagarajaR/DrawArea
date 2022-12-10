using System;
using System.Drawing;

namespace Labtt.DrawArea
{
	public class GraphicsCircle : GraphicsSpot
	{
		public GraphicsCircle()
			: base(0f, 0f, 0f)
		{
		}

		public GraphicsCircle(float x, float y, float radius)
			: base(x, y, radius)
		{
		}

		public override GraphicsObject Clone()
		{
			GraphicsCircle graphicsCircle = new GraphicsCircle();
			graphicsCircle.location = base.Location;
			graphicsCircle.radius = base.Radius;
			FillDrawGraphicsFields(graphicsCircle);
			return graphicsCircle;
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			if (!(base.Radius < 1f))
			{
				Brush brush = new SolidBrush(base.Color);
				Pen pen = new Pen(base.Color, base.PenWidth);
				g.FillEllipse(brush, base.Location.X - 3f - rectangle.X, base.Location.Y - 3f - rectangle.Y, 6f, 6f);
				g.DrawEllipse(pen, base.Location.X - base.Radius - rectangle.X, base.Location.Y - base.Radius - rectangle.Y, base.Radius * 2f, base.Radius * 2f);
			}
		}

		internal override int HitTest(PointF point)
		{
			if (!base.Visible)
			{
				return -1;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, base.Location) < (double)base.HitRadius)
			{
				return 0;
			}
			if (Math.Abs(FunctionSet.DistanceOfTwoPoints(point, base.Location) - (double)base.Radius) < (double)base.HitRadius)
			{
				return 1;
			}
			return -1;
		}
	}
}
