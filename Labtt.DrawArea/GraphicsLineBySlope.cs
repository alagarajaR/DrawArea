using System;
using System.Collections.Generic;
using System.Drawing;

namespace Labtt.DrawArea
{

	public class GraphicsLineBySlope : GraphicsObject
	{
		protected PointF location;

		protected float slope;

		public PointF Location => location;

		public float Slope => slope;

		public GraphicsLineBySlope()
			: this(0f, 0f, 0f)
		{
		}

		public GraphicsLineBySlope(float x, float y, float slope)
		{
			location = new PointF(x, y);
			this.slope = slope;
		}

		public override GraphicsObject Clone()
		{
			GraphicsLineBySlope graphicsLineBySlope = new GraphicsLineBySlope();
			graphicsLineBySlope.location = location;
			graphicsLineBySlope.slope = slope;
			FillDrawGraphicsFields(graphicsLineBySlope);
			return graphicsLineBySlope;
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			if (location == PointF.Empty)
			{
				return;
			}
			Brush brush = new SolidBrush(base.Color);
			Pen pen = new Pen(base.Color, base.PenWidth);
			if (slope == 0f)
			{
				g.FillEllipse(brush, location.X - 3f - rectangle.X, location.Y - 3f - rectangle.Y, 6f, 6f);
				g.DrawLine(pen, 0f, location.Y - rectangle.Y, rectangle.Width, location.Y - rectangle.Y);
				return;
			}
			if (float.IsInfinity(slope))
			{
				g.FillEllipse(brush, location.X - 3f - rectangle.X, location.Y - 3f - rectangle.Y, 6f, 6f);
				g.DrawLine(pen, location.X - rectangle.X, 0f, location.X - rectangle.X, rectangle.Height);
				return;
			}
			List<PointF> intersectionOfLineAndRectangle = FunctionSet.GetIntersectionOfLineAndRectangle(location, slope, rectangle);
			if (intersectionOfLineAndRectangle.Count >= 2)
			{
				g.FillEllipse(brush, location.X - 3f - rectangle.X, location.Y - 3f - rectangle.Y, 6f, 6f);
				g.DrawLine(pen, intersectionOfLineAndRectangle[0].X - rectangle.X, intersectionOfLineAndRectangle[0].Y - rectangle.Y, intersectionOfLineAndRectangle[1].X - rectangle.X, intersectionOfLineAndRectangle[1].Y - rectangle.Y);
			}
		}

		internal override void Mapping(RectangleF reference, RectangleF target)
		{
			location.X -= reference.X;
			location.Y -= reference.Y;
			float num = reference.Width / target.Width;
			float num2 = reference.Height / target.Height;
			location.X /= num;
			location.Y /= num2;
			location.X += target.X;
			location.Y += target.Y;
		}

		internal override int HitTest(PointF point)
		{
			if (!base.Visible)
			{
				return -1;
			}
			float num = Math.Abs((slope * point.X - point.Y + Location.Y - slope * location.X) / (float)Math.Sqrt(slope * slope + 1f));
			if ((slope == 0f && Math.Abs(point.Y - location.Y) < base.HitRadius) || (float.IsInfinity(slope) && Math.Abs(point.X - location.X) < base.HitRadius) || num < base.HitRadius)
			{
				return 0;
			}
			return -1;
		}

		public override void Move(float deltaX, float deltaY)
		{
			location.X += deltaX;
			location.Y += deltaY;
			base.Move(deltaX, deltaY);
		}

		public override void MoveHandle(float deltaX, float deltaY, int handleNumber)
		{
			if (handleNumber == 0)
			{
				Move(deltaX, deltaY);
				base.MoveHandle(deltaX, deltaY, handleNumber);
			}
		}

		public override void MoveHandleTo(PointF point, int handleNumber)
		{
			switch (handleNumber)
			{
				case 0:
					location.X = point.X;
					location.Y = point.Y;
					break;
				case 1:
					if (slope == point.X)
					{
						return;
					}
					slope = point.X;
					break;
				default:
					return;
			}
			base.MoveHandleTo(point, handleNumber);
		}

		public override object Test()
		{
			return base.Test();
		}
	}
}
