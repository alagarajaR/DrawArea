using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolSpot : ToolObject
	{
		protected GraphicsSpot graphicsSpot;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsSpot = new GraphicsSpot();
			return graphicsSpot;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished)
			{
				graphicsSpot.MoveHandleTo(e.Location, 0);
				graphicsSpot.MoveHandleTo(new Point(e.X + 3, e.Y), 1);
				base.Finished = true;
			}
		}
	}
}
