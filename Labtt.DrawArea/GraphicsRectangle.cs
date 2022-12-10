using System;
using System.Drawing;

namespace Labtt.DrawArea
{

	internal class GraphicsRectangle : GraphicsObject
	{
		private PointF startPoint;

		private PointF endPoint;

		public PointF Start => startPoint;

		public PointF End => endPoint;

		public GraphicsRectangle()
			: this(0f, 0f, 0f, 0f)
		{
		}

		public GraphicsRectangle(float x1, float y1, float x2, float y2)
		{
			startPoint = new PointF(x1, y1);
			endPoint = new PointF(x2, y2);
		}

		public override GraphicsObject Clone()
		{
			GraphicsRectangle graphicsRectangle = new GraphicsRectangle();
			graphicsRectangle.startPoint = startPoint;
			graphicsRectangle.endPoint = endPoint;
			FillDrawGraphicsFields(graphicsRectangle);
			return graphicsRectangle;
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			if (!(startPoint == endPoint) && startPoint.X != endPoint.X && startPoint.Y != endPoint.Y)
			{
				Brush brush = new SolidBrush(base.Color);
				g.FillEllipse(brush, startPoint.X - 3f - rectangle.X, startPoint.Y - 3f - rectangle.Y, 6f, 6f);
				g.FillEllipse(brush, endPoint.X - 3f - rectangle.X, endPoint.Y - 3f - rectangle.Y, 6f, 6f);
				g.FillEllipse(brush, startPoint.X - 3f - rectangle.X, endPoint.Y - 3f - rectangle.Y, 6f, 6f);
				g.FillEllipse(brush, endPoint.X - 3f - rectangle.X, startPoint.Y - 3f - rectangle.Y, 6f, 6f);
				Pen pen = new Pen(base.Color, base.PenWidth);
				g.DrawRectangle(pen, Math.Min(startPoint.X, endPoint.X) - rectangle.X, Math.Min(startPoint.Y, endPoint.Y) - rectangle.Y, Math.Abs(startPoint.X - endPoint.X) - rectangle.X, Math.Abs(startPoint.Y - endPoint.Y) - rectangle.Y);
			}
		}

		internal override void Mapping(RectangleF reference, RectangleF target)
		{
			startPoint.X -= reference.X;
			startPoint.Y -= reference.Y;
			endPoint.X -= reference.X;
			endPoint.Y -= reference.Y;
			float num = reference.Width / target.Width;
			float num2 = reference.Height / target.Height;
			startPoint.X /= num;
			startPoint.Y /= num2;
			endPoint.X /= num;
			endPoint.Y /= num2;
			startPoint.X += target.X;
			startPoint.Y += target.Y;
			endPoint.X += target.X;
			endPoint.Y += target.Y;
		}

		internal override int HitTest(PointF point)
		{
			if (!base.Visible)
			{
				return -1;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, startPoint) < (double)base.HitRadius)
			{
				return 1;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, new PointF(endPoint.X, startPoint.Y)) < (double)base.HitRadius)
			{
				return 2;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, new PointF(startPoint.X, endPoint.Y)) < (double)base.HitRadius)
			{
				return 3;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, endPoint) < (double)base.HitRadius)
			{
				return 4;
			}
			return -1;
		}

		public override void Move(float deltaX, float deltaY)
		{
			startPoint.X += deltaX;
			startPoint.Y += deltaY;
			endPoint.X += deltaX;
			endPoint.Y += deltaY;
			base.Move(deltaX, deltaY);
		}

		public override void MoveHandle(float deltaX, float deltaY, int handleNumber)
		{
			switch (handleNumber)
			{
				case 0:
					Move(deltaX, deltaY);
					break;
				case 1:
					startPoint.X += deltaX;
					startPoint.Y += deltaY;
					break;
				case 2:
					endPoint.X += deltaX;
					startPoint.Y += deltaY;
					break;
				case 3:
					startPoint.X += deltaX;
					endPoint.Y += deltaY;
					break;
				case 4:
					endPoint.X += deltaX;
					endPoint.Y += deltaY;
					break;
				default:
					return;
			}
			base.MoveHandle(deltaX, deltaY, handleNumber);
		}

		public override void MoveHandleTo(PointF point, int handleNumber)
		{
			switch (handleNumber)
			{
				case 1:
					startPoint = point;
					break;
				case 2:
					endPoint.X = point.X;
					startPoint.Y = point.Y;
					break;
				case 3:
					startPoint.X = point.X;
					endPoint.Y = point.Y;
					break;
				case 4:
					endPoint.X = point.X;
					endPoint.Y = point.Y;
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
