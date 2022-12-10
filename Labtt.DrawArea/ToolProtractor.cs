using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolProtractor : ToolObject
	{
		protected GraphicsProtractor graphicsProtractor;

		private int drawingNumber;

		public override void Initialize(ImageBox imageBox)
		{
			base.Initialize(imageBox);
			drawingNumber = 0;
		}

		protected override GraphicsObject LoadGraphics()
		{
			graphicsProtractor = new GraphicsProtractor();
			return graphicsProtractor;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Finished && !(graphicsProtractor.Vertex != graphicsProtractor.EndPoint1) && !(graphicsProtractor.Vertex != graphicsProtractor.EndPoint2))
			{
				drawingNumber = 1;
				graphicsProtractor.MoveHandleTo(e.Location, 1);
				graphicsProtractor.MoveHandleTo(e.Location, 2);
				graphicsProtractor.MoveHandleTo(e.Location, 3);
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Finished && !(graphicsProtractor.Vertex == Point.Empty))
			{
				if (drawingNumber == 1)
				{
					graphicsProtractor.MoveHandleTo(e.Location, 2);
				}
				else if (drawingNumber == 2)
				{
					graphicsProtractor.MoveHandleTo(e.Location, 3);
				}
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (base.Finished)
			{
				return;
			}
			if (drawingNumber == 1)
			{
				if (!(graphicsProtractor.EndPoint1 == graphicsProtractor.Vertex))
				{
					graphicsProtractor.MoveHandleTo(e.Location, 2);
					drawingNumber++;
				}
			}
			else if (drawingNumber == 2 && !(graphicsProtractor.EndPoint2 == graphicsProtractor.Vertex))
			{
				graphicsProtractor.MoveHandleTo(e.Location, 3);
				drawingNumber++;
				base.Finished = true;
			}
		}
	}
}
