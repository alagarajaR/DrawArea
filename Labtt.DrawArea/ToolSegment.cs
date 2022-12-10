using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolSegment : ToolObject
	{
		protected GraphicsSegment graphicsSegment;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsSegment = new GraphicsSegment();
			return graphicsSegment;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished && !(graphicsSegment.StartPoint != graphicsSegment.EndPoint))
			{
				graphicsSegment.MoveHandleTo(e.Location, 1);
				graphicsSegment.MoveHandleTo(e.Location, 2);
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Finished && !(graphicsSegment.StartPoint == Point.Empty))
			{
				graphicsSegment.MoveHandleTo(e.Location, 2);
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!base.Finished && !(graphicsSegment.EndPoint == graphicsSegment.StartPoint))
			{
				graphicsSegment.MoveHandleTo(e.Location, 2);
				base.Finished = true;
			}
		}
	}
}
