using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolLineBySlope : ToolObject
	{
		protected GraphicsLineBySlope graphicsLineBySlope;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsLineBySlope = new GraphicsLineBySlope();
			return graphicsLineBySlope;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished && !(graphicsLineBySlope.Location != Point.Empty) && !(graphicsLineBySlope.Location == e.Location))
			{
				graphicsLineBySlope.MoveHandleTo(e.Location, 0);
				graphicsLineBySlope.MoveHandleTo(new Point(0, 0), 1);
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Finished && !(graphicsLineBySlope.Location == Point.Empty))
			{
				float x = ((float)e.Y - graphicsLineBySlope.Location.Y) / ((float)e.X - graphicsLineBySlope.Location.X);
				graphicsLineBySlope.MoveHandleTo(new PointF(x, 0f), 1);
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!base.Finished && !(graphicsLineBySlope.Location == e.Location))
			{
				float x = ((float)e.Y - graphicsLineBySlope.Location.Y) / ((float)e.X - graphicsLineBySlope.Location.X);
				graphicsLineBySlope.MoveHandleTo(new PointF(x, 0f), 1);
				base.Finished = true;
			}
		}
	}
}
