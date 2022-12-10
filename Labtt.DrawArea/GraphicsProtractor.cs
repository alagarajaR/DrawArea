using System.Drawing;

namespace Labtt.DrawArea
{
	public class GraphicsProtractor : GraphicsObject
	{
		private PointF vertexPoint;

		private PointF endPoint1;

		private PointF endPoint2;

		private float angle;

		public PointF Vertex => vertexPoint;

		public PointF EndPoint1 => endPoint1;

		public PointF EndPoint2 => endPoint2;

		public float Angle => angle;

		public GraphicsProtractor()
			: this(0f, 0f, 0f, 0f, 0f, 0f)
		{
		}

		public GraphicsProtractor(float x1, float y1, float x2, float y2, float x3, float y3)
		{
			vertexPoint.X = x1;
			vertexPoint.Y = y1;
			endPoint1.X = x2;
			endPoint1.Y = y2;
			endPoint2.X = x3;
			endPoint2.Y = y3;
			UpdateAngle();
		}

		private void UpdateAngle()
		{
			angle = (float)FunctionSet.GetAngleOfThreePoint(vertexPoint, endPoint1, endPoint2);
		}

		public override GraphicsObject Clone()
		{
			GraphicsProtractor graphicsProtractor = new GraphicsProtractor();
			graphicsProtractor.vertexPoint = vertexPoint;
			graphicsProtractor.endPoint1 = endPoint1;
			graphicsProtractor.endPoint2 = endPoint2;
			graphicsProtractor.angle = angle;
			FillDrawGraphicsFields(graphicsProtractor);
			return graphicsProtractor;
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			if (!(vertexPoint == endPoint1))
			{
				Brush brush = new SolidBrush(base.Color);
				g.FillEllipse(brush, vertexPoint.X - 3f - rectangle.X, vertexPoint.Y - 3f - rectangle.Y, 6f, 6f);
				g.FillEllipse(brush, endPoint1.X - 3f - rectangle.X, endPoint1.Y - 3f - rectangle.Y, 6f, 6f);
				Pen pen = new Pen(base.Color, base.PenWidth);
				g.DrawLine(pen, vertexPoint.X - rectangle.X, vertexPoint.Y - rectangle.Y, endPoint1.X - rectangle.X, endPoint1.Y - rectangle.Y);
				if (!(vertexPoint == endPoint2))
				{
					g.FillEllipse(brush, endPoint2.X - 3f - rectangle.X, endPoint2.Y - 3f - rectangle.Y, 6f, 6f);
					g.DrawLine(pen, vertexPoint.X - rectangle.X, vertexPoint.Y - rectangle.Y, endPoint2.X - rectangle.X, endPoint2.Y - rectangle.Y);
					g.DrawString(angle.ToString("F1") + "°", new Font("Courier", 15f, FontStyle.Regular), new SolidBrush(base.Color), vertexPoint.X - rectangle.X, vertexPoint.Y - rectangle.Y);
				}
			}
		}

		internal override void Mapping(RectangleF reference, RectangleF target)
		{
			vertexPoint.X -= reference.X;
			vertexPoint.Y -= reference.Y;
			endPoint1.X -= reference.X;
			endPoint1.Y -= reference.Y;
			endPoint2.X -= reference.X;
			endPoint2.Y -= reference.Y;
			float num = reference.Width / target.Width;
			float num2 = reference.Height / target.Height;
			vertexPoint.X /= num;
			vertexPoint.Y /= num2;
			endPoint1.X /= num;
			endPoint1.Y /= num2;
			endPoint2.X /= num;
			endPoint2.Y /= num2;
			vertexPoint.X += target.X;
			vertexPoint.Y += target.Y;
			endPoint1.X += target.X;
			endPoint1.Y += target.Y;
			endPoint2.X += target.X;
			endPoint2.Y += target.Y;
		}

		internal override int HitTest(PointF point)
		{
			if (!base.Visible)
			{
				return -1;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, vertexPoint) < (double)base.HitRadius)
			{
				return 1;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, endPoint1) < (double)base.HitRadius)
			{
				return 2;
			}
			if (FunctionSet.DistanceOfTwoPoints(point, endPoint2) < (double)base.HitRadius)
			{
				return 3;
			}
			return -1;
		}

		public override void Move(float deltaX, float deltaY)
		{
			vertexPoint.X += deltaX;
			vertexPoint.Y += deltaY;
			endPoint1.X += deltaX;
			endPoint1.Y += deltaY;
			endPoint2.X += deltaX;
			endPoint2.Y += deltaY;
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
					vertexPoint.X += deltaX;
					vertexPoint.Y += deltaY;
					break;
				case 2:
					endPoint1.X += deltaX;
					endPoint1.Y += deltaY;
					break;
				case 3:
					endPoint2.X += deltaX;
					endPoint2.Y += deltaY;
					break;
				default:
					return;
			}
			if (handleNumber != 0)
			{
				UpdateAngle();
			}
			base.MoveHandle(deltaX, deltaY, handleNumber);
		}

		public override void MoveHandleTo(PointF point, int handleNumber)
		{
			switch (handleNumber)
			{
				case 1:
					vertexPoint = point;
					break;
				case 2:
					endPoint1 = point;
					break;
				case 3:
					endPoint2 = point;
					break;
				default:
					return;
			}
			UpdateAngle();
			base.MoveHandleTo(point, handleNumber);
		}
	}
}
