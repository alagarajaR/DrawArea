using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea {

	internal class ToolLineHorizontal : ToolLineBySlope
	{
		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished)
			{
				graphicsLineBySlope.MoveHandleTo(e.Location, 0);
				graphicsLineBySlope.MoveHandleTo(new Point(0, 0), 1);
				base.Finished = true;
			}
		}
	}
	}
