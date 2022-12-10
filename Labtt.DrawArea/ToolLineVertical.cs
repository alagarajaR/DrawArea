using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolLineVertical : ToolLineBySlope
	{
		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished)
			{
				graphicsLineBySlope.MoveHandleTo(e.Location, 0);
				graphicsLineBySlope.MoveHandleTo(new PointF(float.PositiveInfinity, 0f), 1);
				base.Finished = true;
			}
		}
	}
}
