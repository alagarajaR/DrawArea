using System;
using System.Drawing;

namespace Labtt.DrawArea
{

	public abstract class GraphicsObject
	{
		private Color color = Color.Blue;

		private int penWidth = 1;

		private string identifier;

		private float hitRadius = 10f;

		private bool selectable;

		private bool visible = true;

		private int selectedHandle;

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public int PenWidth
		{
			get
			{
				return penWidth;
			}
			set
			{
				penWidth = value;
			}
		}

		public string Identifier
		{
			get
			{
				return identifier;
			}
			set
			{
				identifier = value;
			}
		}

		public float HitRadius
		{
			get
			{
				return hitRadius;
			}
			set
			{
				hitRadius = value;
			}
		}

		public bool Selectable
		{
			get
			{
				return selectable;
			}
			set
			{
				selectable = value;
			}
		}

		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				visible = value;
			}
		}

		public int SelectedHandle
		{
			get
			{
				return selectedHandle;
			}
			set
			{
				selectedHandle = value;
			}
		}

		public event EventHandler OnGraphicsChanged;

		public GraphicsObject()
		{
			selectable = true;
			selectedHandle = -1;
		}

		public abstract GraphicsObject Clone();

		internal abstract void Draw(RectangleF rectangle, Graphics g);

		internal abstract void Mapping(RectangleF reference, RectangleF target);

		internal abstract int HitTest(PointF point);

		public virtual void Move(float deltaX, float deltaY)
		{
			if (this.OnGraphicsChanged != null)
			{
				this.OnGraphicsChanged(this, null);
			}
		}

		public virtual void MoveHandle(float deltaX, float deltaY, int handleNumber)
		{
			if (handleNumber > 0 && this.OnGraphicsChanged != null)
			{
				this.OnGraphicsChanged(this, null);
			}
		}

		public virtual void MoveHandleTo(PointF point, int handleNumber)
		{
			if (this.OnGraphicsChanged != null)
			{
				this.OnGraphicsChanged(this, null);
			}
		}

		public virtual object Test()
		{
			return null;
		}

		protected void FillDrawGraphicsFields(GraphicsObject graphics)
		{
			graphics.Color = Color;
			graphics.PenWidth = PenWidth;
			graphics.Identifier = Identifier;
			graphics.HitRadius = HitRadius;
			graphics.Selectable = Selectable;
			graphics.Visible = Visible;
			graphics.SelectedHandle = SelectedHandle;
		}

		public virtual void Refresh()
		{
			if (this.OnGraphicsChanged != null)
			{
				this.OnGraphicsChanged(this, null);
			}
		}
	}
}
