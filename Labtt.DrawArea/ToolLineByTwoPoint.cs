namespace Labtt.DrawArea
{

	internal class ToolLineByTwoPoint : ToolSegment
	{
		protected GraphicsLineByTwoPoint graphicsLine;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsLine = new GraphicsLineByTwoPoint();
			graphicsSegment = graphicsLine;
			return graphicsLine;
		}
	}
}
