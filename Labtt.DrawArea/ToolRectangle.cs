using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolRectangle : ToolObject
	{
		protected GraphicsRectangle graphicsRectangle;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsRectangle = new GraphicsRectangle();
			return graphicsRectangle;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished && !(graphicsRectangle.Start != graphicsRectangle.End))
			{
				graphicsRectangle.MoveHandleTo(e.Location, 4);
				graphicsRectangle.MoveHandleTo(e.Location, 1);
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Finished && !(graphicsRectangle.Start == Point.Empty))
			{
				graphicsRectangle.MoveHandleTo(e.Location, 4);
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!base.Finished && !(graphicsRectangle.Start == graphicsRectangle.End) && graphicsRectangle.Start.X != graphicsRectangle.End.X && graphicsRectangle.Start.Y != graphicsRectangle.End.Y)
			{
				graphicsRectangle.MoveHandleTo(e.Location, 4);
				base.Finished = true;
			}
		}
	}
}
