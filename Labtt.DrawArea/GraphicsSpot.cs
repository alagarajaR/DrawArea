using System.Drawing;

namespace Labtt.DrawArea
{

	public class GraphicsSpot : GraphicsObject
	{
		protected PointF location;

		protected float radius;

		public PointF Location => location;

		public float Radius => radius;

		public GraphicsSpot()
			: this(0f, 0f, 0f)
		{
		}

		public GraphicsSpot(float x, float y, float radius)
		{
			location = new PointF(x, y);
			this.radius = radius;
		}

		public override GraphicsObject Clone()
		{
			GraphicsSpot graphicsSpot = new GraphicsSpot();
			graphicsSpot.location = location;
			graphicsSpot.radius = radius;
			FillDrawGraphicsFields(graphicsSpot);
			return graphicsSpot;
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			if (!(radius <= 0f))
			{
				Brush brush = new SolidBrush(base.Color);
				g.FillEllipse(brush, location.X - radius - rectangle.X, location.Y - radius - rectangle.Y, radius * 2f, radius * 2f);
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
			if (FunctionSet.DistanceOfTwoPoints(point, location) < (double)base.HitRadius)
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
					radius = (float)FunctionSet.DistanceOfTwoPoints(location, point);
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
