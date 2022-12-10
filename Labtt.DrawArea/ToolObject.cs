using System;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal abstract class ToolObject : Tool
	{
		private bool drawing = false;

		private bool finished = false;

		private GraphicsObject graphics;

		protected bool Finished
		{
			get
			{
				return finished;
			}
			set
			{
				finished = value;
				if (value)
				{
					owner.ActiveTool = DrawToolType.Pointer;
				}
			}
		}

		internal string Identifer
		{
			get
			{
				if (graphics == null)
				{
					return null;
				}
				return graphics.Identifier;
			}
			set
			{
				if (graphics != null)
				{
					graphics.Identifier = value;
				}
			}
		}

		public GraphicsObject Graphics => graphics;

		public override void Initialize(ImageBox imageBox)
		{
			base.Initialize(imageBox);
			graphics = LoadGraphics();
			AddNewObject(graphics);
			drawing = true;
			finished = false;
		}

		protected abstract GraphicsObject LoadGraphics();

		public override void Close()
		{
			base.Close();
			if (drawing && finished)
			{
				drawing = false;
			}
			Dispose();
		}

		public override void Dispose()
		{
			if (drawing)
			{
				owner.GraphicsList.RemoveAt(0);
			}
			drawing = false;
			finished = false;
			graphics = null;
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!drawing)
			{
				throw new Exception("工具未初始化！");
			}
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!drawing)
			{
				throw new Exception("工具未初始化！");
			}
		}

		public override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!drawing)
			{
				throw new Exception("工具未初始化！");
			}
		}

		protected void AddNewObject(GraphicsObject graphics)
		{
			owner.GraphicsList.Add(graphics);
			owner.Refresh();
		}
	}
}
