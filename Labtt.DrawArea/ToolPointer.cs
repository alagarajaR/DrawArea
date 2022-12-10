using System.Drawing;
using System.Windows.Forms;

namespace Labtt.DrawArea { 

internal class ToolPointer : Tool
{
	private Color colorBefore;

	private GraphicsObject selectedGraphics;

	private int handleNumber;

	public override void OnMouseDown(MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left)
		{
			return;
		}
		foreach (GraphicsObject item in owner.GraphicsList.Selectable)
		{
			int num = item.HitTest(e.Location);
			if (num >= 0)
			{
				selectedGraphics = item;
				colorBefore = selectedGraphics.Color;
				selectedGraphics.Color = Color.DarkOrange;
				selectedGraphics.SelectedHandle = num;
				handleNumber = num;
				break;
			}
		}
	}

	public override void OnMouseMove(MouseEventArgs e)
	{
		if (selectedGraphics != null && handleNumber >= 0 && e.Button == MouseButtons.Left)
		{
			selectedGraphics.MoveHandleTo(e.Location, handleNumber);
		}
	}

	public override void OnMouseUp(MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			handleNumber = -1;
			if (selectedGraphics != null)
			{
				selectedGraphics.Color = colorBefore;
				selectedGraphics.SelectedHandle = -1;
			}
			selectedGraphics = null;
		}
	}
}
}
