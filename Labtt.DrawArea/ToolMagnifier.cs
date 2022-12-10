using System;
using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal class ToolMagnifier : Tool
	{
		private const string RECTANGLE_IDENTIFIER = "RECTANGLE_IDENTIFIER";

		private GraphicsRectangle graphicsRectangle;

		public override void Initialize(ImageBox imageBox)
		{
			base.Initialize(imageBox);
			owner.Cursor = Cursors.Cross;
		}

		public override void Close()
		{
			Dispose();
			base.Close();
		}

		public override void Dispose()
		{
			base.Dispose();
			owner.Cursor = Cursors.Default;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (owner.Image != null && e.Button == MouseButtons.Left)
			{
				graphicsRectangle = new GraphicsRectangle();
				graphicsRectangle.Identifier = "RECTANGLE_IDENTIFIER";
				graphicsRectangle.Color = Color.OldLace;
				owner.GraphicsList.Add(graphicsRectangle);
				graphicsRectangle.MoveHandleTo(e.Location, 4);
				graphicsRectangle.MoveHandleTo(e.Location, 1);
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (owner.Image != null && graphicsRectangle != null)
			{
				graphicsRectangle.MoveHandleTo(e.Location, 4);
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (owner.Image != null && e.Button == MouseButtons.Left && graphicsRectangle != null)
			{
				PointF location = new PointF(Math.Min(graphicsRectangle.Start.X, graphicsRectangle.End.X), Math.Min(graphicsRectangle.Start.Y, graphicsRectangle.End.Y));
				PointF pointF = new PointF(Math.Max(graphicsRectangle.Start.X, graphicsRectangle.End.X), Math.Max(graphicsRectangle.Start.Y, graphicsRectangle.End.Y));
				RectangleF zoomArea = new RectangleF(location, new SizeF(pointF.X - location.X, pointF.Y - location.Y));
				owner.ZoomIn(zoomArea);
				owner.GraphicsList.RemoveByIdentifier("RECTANGLE_IDENTIFIER");
			}
		}
	}
}
