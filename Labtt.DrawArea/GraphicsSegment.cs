using System.Drawing;

namespace Labtt.DrawArea
{

	public class GraphicsSegment : GraphicsObject
	{
		protected PointF startPoint;

		protected PointF endPoint;

		public PointF StartPoint => startPoint;

		public PointF EndPoint => endPoint;

		public GraphicsSegment()
			: this(0f, 0f, 0f, 0f)
		{
		}

		public GraphicsSegment(float x1, float y1, float x2, float y2)
		{
			startPoint.X = x1;
			startPoint.Y = y1;
			endPoint.X = x2;
			endPoint.Y = y2;
		}

		public override GraphicsObject Clone()
		{
			GraphicsSegment graphicsSegment = new GraphicsSegment();
			graphicsSegment.startPoint = startPoint;
			graphicsSegment.endPoint = endPoint;
			FillDrawGraphicsFields(graphicsSegment);
			return graphicsSegment;
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			if (!(startPoint == endPoint))
			{
				Brush brush = new SolidBrush(base.Color);
				g.FillEllipse(brush, startPoint.X - 3f - rectangle.X, startPoint.Y - 3f - rectangle.Y, 6f, 6f);
				g.FillEllipse(brush, endPoint.X - 3f - rectangle.X, endPoint.Y - 3f - rectangle.Y, 6f, 6f);
				Pen pen = new Pen(base.Color, base.PenWidth);
				g.DrawLine(pen, startPoint.X - rectangle.X, startPoint.Y - rectangle.Y, endPoint.X - rectangle.X, endPoint.Y - rectangle.Y);
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
			if (FunctionSet.DistanceOfTwoPoints(point, endPoint) < (double)base.HitRadius)
			{
				return 2;
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
					endPoint = point;
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
