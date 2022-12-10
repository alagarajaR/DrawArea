using System.Collections.Generic;
using System.Drawing;

namespace Labtt.DrawArea
{

	public class GraphicsList
	{
		private ImageBox owner;

		private List<GraphicsObject> graphicsList;

		public int Count => graphicsList.Count;

		public GraphicsObject this[int index] => graphicsList[index];

		public IEnumerable<GraphicsObject> Selectable
		{
			get
			{
				foreach (GraphicsObject graphics in graphicsList)
				{
					if (graphics.Selectable)
					{
						yield return graphics;
					}
				}
			}
		}

		public IEnumerable<GraphicsObject> Selection
		{
			get
			{
				foreach (GraphicsObject graphics in graphicsList)
				{
					if (graphics.SelectedHandle >= 0)
					{
						yield return graphics;
					}
				}
			}
		}

		public IEnumerable<GraphicsObject> this[string identifier]
		{
			get
			{
				foreach (GraphicsObject graphics in graphicsList)
				{
					if (graphics.Identifier == identifier)
					{
						yield return graphics;
					}
				}
			}
		}

		public GraphicsList(ImageBox owner)
		{
			this.owner = owner;
			graphicsList = new List<GraphicsObject>();
		}

		public List<GraphicsObject>.Enumerator GetEnumerator()
		{
			return graphicsList.GetEnumerator();
		}

		internal void Draw(RectangleF rectangle, Graphics g)
		{
			for (int num = graphicsList.Count - 1; num >= 0; num--)
			{
				if (graphicsList[num].Visible)
				{
					graphicsList[num].Draw(rectangle, g);
				}
			}
		}

		public void MappingGraphics(RectangleF reference, RectangleF target)
		{
			foreach (GraphicsObject graphics in graphicsList)
			{
				graphics.Mapping(reference, target);
			}
		}

		internal void Move(float deltaX, float deltaY)
		{
			foreach (GraphicsObject graphics in graphicsList)
			{
				graphics.Move(deltaX, deltaY);
			}
		}

		public bool Contains(GraphicsObject graphics)
		{
			return graphicsList.Contains(graphics);
		}

		public void Add(GraphicsObject graphics)
		{
			graphicsList.Insert(0, graphics);
			graphics.OnGraphicsChanged += owner.graphics_GraphicsChanged;
			graphics.Refresh();
			owner.Invalidate();
		}

		public void RemoveAt(int index)
		{
			if (graphicsList.Count > index)
			{
				graphicsList.RemoveAt(index);
			}
		}

		public void Remove(GraphicsObject graphicsObject)
		{
			graphicsList.Remove(graphicsObject);
		}

		public void RemoveByIdentifier(string identifier)
		{
			for (int num = graphicsList.Count - 1; num >= 0; num--)
			{
				if (graphicsList[num].Identifier == identifier)
				{
					graphicsList.RemoveAt(num);
				}
			}
			owner.Invalidate();
		}

		public void Clear()
		{
			graphicsList.Clear();
		}
	}
}
