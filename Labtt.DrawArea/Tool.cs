using System;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	internal abstract class Tool
	{
		protected ImageBox owner;

		public virtual void Initialize(ImageBox imageBox)
		{
			owner = imageBox;
		}

		public virtual void Close()
		{
		}

		public virtual void Dispose()
		{
		}

		public virtual void OnMouseDown(MouseEventArgs e)
		{
		}

		public virtual void OnMouseMove(MouseEventArgs e)
		{
		}

		public virtual void OnMouseUp(MouseEventArgs e)
		{
		}

		public virtual void OnMouseEnter(EventArgs e)
		{
		}

		public virtual void OnMouseLeave(EventArgs e)
		{
		}
	}
}