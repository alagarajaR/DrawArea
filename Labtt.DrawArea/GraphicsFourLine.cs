using System;
using System.Drawing;

namespace Labtt.DrawArea
{

	public class GraphicsFourLine : GraphicsObject
	{
		private float left;

		private float right;

		private float top;

		private float bottom;

		private int lineCount;

		public float Left => left;

		public float Right => right;

		public float Top => top;

		public float Bottom => bottom;

		public int LineCount
		{
			get
			{
				return lineCount;
			}
			set
			{
				lineCount = value;
			}
		}

		public GraphicsFourLine()
			: this(0f, 0f, 0f, 0f, 0)
		{
		}

		public GraphicsFourLine(float left, float right, float top, float bottom, int lineCount = 4)
		{
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
			this.lineCount = lineCount;
		}

		public override GraphicsObject Clone()
		{
			GraphicsFourLine graphicsFourLine = new GraphicsFourLine();
			graphicsFourLine.left = left;
			graphicsFourLine.right = right;
			graphicsFourLine.top = top;
			graphicsFourLine.bottom = bottom;
			graphicsFourLine.lineCount = lineCount;
			FillDrawGraphicsFields(graphicsFourLine);
			return graphicsFourLine;
		}

		internal override void Draw(RectangleF rectangle, Graphics g)
		{
			Pen pen = new Pen(base.Color, base.PenWidth);
			switch (lineCount)
			{
				default:
					return;
				case 4:
					g.DrawLine(pen, new PointF(0f, bottom - rectangle.Y), new PointF(rectangle.Width, bottom - rectangle.Y));
					goto case 3;
				case 3:
					g.DrawLine(pen, new PointF(0f, top - rectangle.Y), new PointF(rectangle.Width, top - rectangle.Y));
					goto case 2;
				case 2:
					g.DrawLine(pen, new PointF(right - rectangle.X, 0f), new PointF(right - rectangle.X, rectangle.Height));
					break;
				case 1:
					break;
			}
			g.DrawLine(pen, new PointF(left - rectangle.X, 0f), new PointF(left - rectangle.X, rectangle.Height));
		}

		internal override void Mapping(RectangleF reference, RectangleF target)
		{
			left -= reference.X;
			right -= reference.X;
			top -= reference.Y;
			bottom -= reference.Y;
			float num = reference.Width / target.Width;
			float num2 = reference.Height / target.Height;
			left /= num;
			right /= num;
			top /= num2;
			bottom /= num2;
			left += target.X;
			right += target.X;
			top += target.Y;
			bottom += target.Y;
		}

		internal override int HitTest(PointF point)
		{
			if (!base.Visible)
			{
				return -1;
			}
			if (Math.Abs(point.X - left) < base.HitRadius)
			{
				return 1;
			}
			if (Math.Abs(point.X - right) < base.HitRadius)
			{
				return 2;
			}
			if (Math.Abs(point.Y - top) < base.HitRadius)
			{
				return 3;
			}
			if (Math.Abs(point.Y - bottom) < base.HitRadius)
			{
				return 4;
			}
			return -1;
		}

		public override void Move(float deltaX, float deltaY)
		{
			left += deltaX;
			right += deltaX;
			top += deltaY;
			bottom += deltaY;
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
					left += deltaX;
					break;
				case 2:
					right += deltaX;
					break;
				case 3:
					top += deltaY;
					break;
				case 4:
					bottom += deltaY;
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
					left = point.X;
					break;
				case 2:
					right = point.X;
					break;
				case 3:
					top = point.Y;
					break;
				case 4:
					bottom = point.Y;
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
