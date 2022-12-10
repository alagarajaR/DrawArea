namespace Labtt.DrawArea
{

	internal class ToolRangeFinder : ToolSegment
	{
		protected GraphicsRangeFinder graphicsRangeFinder;

		protected override GraphicsObject LoadGraphics()
		{
			graphicsRangeFinder = new GraphicsRangeFinder();
			graphicsSegment = graphicsRangeFinder;
			return graphicsRangeFinder;
		}
	}
}
