using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolCircleBy3Point : ToolObject
	{
		private const string CIRCLE_POINTS_IDENTIFIER = "CIRCLE_POINTS_IDENTIFIER";

		protected GraphicsCircle graphicsCircle;

		private GraphicsSpot[] points;

		private int drawingNumber;

		public override void Initialize(ImageBox imageBox)
		{
			base.Initialize(imageBox);
			points = new GraphicsSpot[3]
			{
			new GraphicsSpot(),
			new GraphicsSpot(),
			new GraphicsSpot()
			};
			points[0].Identifier = "CIRCLE_POINTS_IDENTIFIER";
			points[1].Identifier = "CIRCLE_POINTS_IDENTIFIER";
			points[2].Identifier = "CIRCLE_POINTS_IDENTIFIER";
			owner.GraphicsList.Add(points[0]);
			owner.GraphicsList.Add(points[1]);
			owner.GraphicsList.Add(points[2]);
			drawingNumber = 0;
		}

		protected override GraphicsObject LoadGraphics()
		{
			graphicsCircle = new GraphicsCircle();
			return graphicsCircle;
		}

		public override void Dispose()
		{
			owner.GraphicsList.RemoveByIdentifier("CIRCLE_POINTS_IDENTIFIER");
			base.Dispose();
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (base.Finished || (drawingNumber == 1 && points[0].Location == e.Location) || (drawingNumber == 2 && points[0].Location == e.Location) || (drawingNumber == 2 && points[1].Location == e.Location))
			{
				return;
			}
			points[drawingNumber].MoveHandleTo(e.Location, 0);
			points[drawingNumber].MoveHandleTo(new Point(e.X + 2, e.Y), 1);
			drawingNumber++;
			if (drawingNumber == points.Length)
			{
				Point PCenter = Point.Empty;
				float R = 0f;
				FunctionSet.GetCircular(points[0].Location, points[1].Location, e.Location, ref PCenter, ref R);
				if (R != 0f)
				{
					graphicsCircle.MoveHandleTo(PCenter, 0);
					graphicsCircle.MoveHandleTo(new Point(PCenter.X + (int)((double)R + 0.5), PCenter.Y), 1);
					base.Finished = true;
				}
			}
		}
	}
}
