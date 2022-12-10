using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolCircle : ToolObject
	{
		protected GraphicsCircle graphicsCircle;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsCircle = new GraphicsCircle();
			return graphicsCircle;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished && !(graphicsCircle.Radius > 0f))
			{
				graphicsCircle.MoveHandleTo(e.Location, 0);
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Finished && !(graphicsCircle.Location == Point.Empty))
			{
				graphicsCircle.MoveHandleTo(e.Location, 1);
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!base.Finished && graphicsCircle.Radius > 0f)
			{
				base.Finished = true;
			}
		}
	}
}
