using System.Windows.Forms;

namespace Labtt.DrawArea {

	internal class ToolFourLine : ToolObject
	{
		protected GraphicsFourLine graphicsFourLine;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsFourLine = new GraphicsFourLine();
			return graphicsFourLine;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished)
			{
				switch (graphicsFourLine.LineCount)
				{
					case 0:
						graphicsFourLine.LineCount++;
						graphicsFourLine.MoveHandleTo(e.Location, 1);
						break;
					case 1:
						graphicsFourLine.LineCount++;
						graphicsFourLine.MoveHandleTo(e.Location, 2);
						break;
					case 2:
						graphicsFourLine.LineCount++;
						graphicsFourLine.MoveHandleTo(e.Location, 3);
						break;
					case 3:
						graphicsFourLine.LineCount++;
						graphicsFourLine.MoveHandleTo(e.Location, 4);
						base.Finished = true;
						break;
				}
			}
		}
	}
}
