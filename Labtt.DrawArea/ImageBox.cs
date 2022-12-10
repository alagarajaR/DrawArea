using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Labtt.DrawArea
{

	public class ImageBox : PictureBox
	{
		private bool showMouseCrossLine = true;

		private bool showCenterCrossLine = false;

		private Point mousePosition = Point.Empty;

		private Image _originalImage;

		private PointF _cutPosition;

		private DrawToolType activeTool;

		private Tool[] tools;

		private GraphicsList graphicsList;

		private float _movingXPixelLength = -1f;

		private float _movingYPixelLength = -1f;

		private float _measureXPixelLength = -1f;

		private float _measureYPixelLength = -1f;

		private float _physicalX = 0f;

		private float _physicalY = 0f;

		private int imageWidth = 0;

		private int imageHeight = 0;

		[DefaultValue(true)]
		public bool ShowMouseCrossLine
		{
			get
			{
				return showMouseCrossLine;
			}
			set
			{
				mousePosition = Point.Empty;
				showMouseCrossLine = value;
			}
		}

		[DefaultValue(false)]
		public bool ShowCenterCrossLine
		{
			get
			{
				return showCenterCrossLine;
			}
			set
			{
				showCenterCrossLine = value;
			}
		}

		[DefaultValue(DrawToolType.Pointer)]
		public DrawToolType ActiveTool
		{
			get
			{
				return activeTool;
			}
			set
			{
				tools[(int)activeTool].Close();
				tools[(int)value].Initialize(this);
				if (activeTool != value)
				{
					activeTool = value;
					if (this.OnToolChanged != null)
					{
						this.OnToolChanged(activeTool, null);
					}
				}
			}
		}

		public string ActiveToolIdentifier
		{
			get
			{
				if (!(tools[(int)ActiveTool] is ToolObject toolObject))
				{
					return null;
				}
				return toolObject.Identifer;
			}
			set
			{
				if (tools[(int)ActiveTool] is ToolObject toolObject)
				{
					toolObject.Identifer = value;
				}
			}
		}

		public GraphicsList GraphicsList
		{
			get
			{
				return graphicsList;
			}
			set
			{
				graphicsList = value;
			}
		}

		public new Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
				ImageWidth = value?.Width ?? 0;
				ImageHeight = value?.Height ?? 0;
				_originalImage = null;
				_cutPosition = Point.Empty;
			}
		}

		public new string ImageLocation
		{
			get
			{
				return base.ImageLocation;
			}
			set
			{
				base.ImageLocation = value;
				Bitmap bitmap = new Bitmap(value);
				ImageWidth = bitmap?.Width ?? 0;
				ImageHeight = bitmap?.Height ?? 0;
				_originalImage = null;
				_cutPosition = Point.Empty;
			}
		}

		public int ImageWidth
		{
			get
			{
				return imageWidth;
			}
			private set
			{
				imageWidth = value;
			}
		}

		public int ImageHeight
		{
			get
			{
				return imageHeight;
			}
			set
			{
				imageHeight = value;
			}
		}

		public PointF PhysicalLocation
		{
			get
			{
				return new PointF(_physicalX, _physicalY);
			}
			set
			{
				UpdatePhysicalLocation(value);
			}
		}

		public event ToolChangedEventHandler OnToolChanged;

		public event MeasuringGraphicsEventHandler OnFourLineChanged;

		public event MeasuringGraphicsEventHandler OnCircleChanged;

		public event MeasuringGraphicsEventHandler OnRangeFinderChanged;

		public event PointGraphicsEventHandler OnSpotChanged;

		public event PointGraphicsEventHandler OnSlashChanged;

		public event SegmentGraphicsEventHandler OnLineChanged;

		public ImageBox()
		{
			base.MouseDown += ImageBox_MouseDown;
			base.MouseMove += ImageBox_MouseMove;
			base.MouseUp += ImageBox_MouseUp;
			base.MouseEnter += ImageBox_MouseEnter;
			base.MouseLeave += ImageBox_MouseLeave;
			base.Paint += ImageBox_Paint;
			graphicsList = new GraphicsList(this);
			tools = new Tool[Enum.GetValues(typeof(DrawToolType)).Length];
			tools[0] = new ToolPointer();
			tools[1] = new ToolMagnifier();
			tools[2] = new ToolSpot();
			tools[3] = new ToolSegment();
			tools[4] = new ToolLineByTwoPoint();
			tools[5] = new ToolLineHorizontal();
			tools[6] = new ToolLineVertical();
			tools[7] = new ToolLineBySlope();
			tools[8] = new ToolRangeFinder();
			tools[9] = new ToolProtractor();
			tools[10] = new ToolCircleBy3Point();
			tools[11] = new ToolCircle();
			tools[12] = new ToolRectangle();
			tools[13] = new ToolFourLine();
			ActiveTool = DrawToolType.Pointer;
			base.SizeMode = PictureBoxSizeMode.Zoom;
			base.BorderStyle = BorderStyle.None;
			BackColor = Color.Gainsboro;
		}

		private void ImageBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (Image != null)
			{
				Focus();
				tools[(int)ActiveTool].OnMouseDown(e);
				Invalidate();
			}
		}

		private void ImageBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (Image != null)
			{
				if (showMouseCrossLine)
				{
					mousePosition = e.Location;
				}
				tools[(int)ActiveTool].OnMouseMove(e);
				Invalidate();
			}
		}

		private void ImageBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (Image != null)
			{
				tools[(int)ActiveTool].OnMouseUp(e);
			}
		}

		private void ImageBox_MouseEnter(object sender, EventArgs e)
		{
			if (Image != null)
			{
				tools[(int)ActiveTool].OnMouseEnter(e);
			}
		}

		private void ImageBox_MouseLeave(object sender, EventArgs e)
		{
			if (Image != null)
			{
				mousePosition = Point.Empty;
				tools[(int)ActiveTool].OnMouseLeave(e);
				Invalidate();
			}
		}

		private void ImageBox_Paint(object sender, PaintEventArgs e)
		{
			if (Image != null)
			{
				Graphics graphics = e.Graphics;
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Pen pen = new Pen(Color.Red);
				pen.Width = 1f;
				pen.DashStyle = DashStyle.Dash;
				if (mousePosition != Point.Empty)
				{
					graphics.DrawLine(pen, mousePosition.X, 0, mousePosition.X, base.Height);
					graphics.DrawLine(pen, 0, mousePosition.Y, base.Width, mousePosition.Y);
				}
				pen.Width = 1f;
				pen.DashStyle = DashStyle.Dash;
				pen.Color = Color.AntiqueWhite;
				if (showCenterCrossLine)
				{
					graphics.DrawLine(pen, base.Width / 2, 0, base.Width / 2, base.Height);
					graphics.DrawLine(pen, 0, base.Height / 2, base.Width, base.Height / 2);
				}
				if (graphicsList != null)
				{
					graphicsList.Draw(new Rectangle(0, 0, base.Width, base.Height), graphics);
				}
			}
		}

		internal void graphics_GraphicsChanged(object sender, EventArgs e)
		{
			Type type = sender.GetType();
			if (type == typeof(GraphicsFourLine))
			{
				if (this.OnFourLineChanged != null)
				{
					GraphicsFourLine graphicsFourLine = sender as GraphicsFourLine;
					PointF boxPoint = new PointF(graphicsFourLine.Left, graphicsFourLine.Top);
					PointF boxPoint2 = new PointF(graphicsFourLine.Right, graphicsFourLine.Bottom);
					PointF pointF = ConvertBoxPointFToImagePointF(boxPoint);
					PointF pointF2 = ConvertBoxPointFToImagePointF(boxPoint2);
					float num = 0f;
					float num2 = 0f;
					float xPhysicalDistance = 0f;
					float yPhysicalDistance = 0f;
					if (graphicsFourLine.Left > 0f && graphicsFourLine.Left < (float)base.Width && graphicsFourLine.Right > 0f && graphicsFourLine.Right < (float)base.Width)
					{
						num = Math.Abs(pointF2.X - pointF.X);
						xPhysicalDistance = num * _measureXPixelLength;
					}
					if (graphicsFourLine.Top > 0f && graphicsFourLine.Top < (float)base.Width && graphicsFourLine.Bottom > 0f && graphicsFourLine.Bottom < (float)base.Width)
					{
						num2 = Math.Abs(pointF2.Y - pointF.Y);
						yPhysicalDistance = num2 * _measureYPixelLength;
					}
					MeasuringGraphicsEventArgs measuringGraphicsEventArgs = new MeasuringGraphicsEventArgs();
					measuringGraphicsEventArgs.Identifier = graphicsFourLine.Identifier;
					measuringGraphicsEventArgs.XImagePixel = num;
					measuringGraphicsEventArgs.YImagePixel = num2;
					measuringGraphicsEventArgs.XPhysicalDistance = xPhysicalDistance;
					measuringGraphicsEventArgs.YPhysicalDistance = yPhysicalDistance;
					this.OnFourLineChanged(sender, measuringGraphicsEventArgs);
				}
			}
			else if (type == typeof(GraphicsRangeFinder))
			{
				GraphicsRangeFinder graphicsRangeFinder = sender as GraphicsRangeFinder;
				graphicsRangeFinder.RealDistance = PhysicalDistanceBetweenTwoBoxPoints(graphicsRangeFinder.StartPoint, graphicsRangeFinder.EndPoint);
				if (this.OnRangeFinderChanged != null)
				{
					PointF pointF3 = ConvertBoxPointFToImagePointF(graphicsRangeFinder.StartPoint);
					PointF pointF4 = ConvertBoxPointFToImagePointF(graphicsRangeFinder.EndPoint);
					float num = Math.Abs(pointF3.X - pointF4.X);
					float num2 = Math.Abs(pointF3.Y - pointF4.Y);
					float xPhysicalDistance = num * _measureXPixelLength;
					float yPhysicalDistance = num2 * _measureYPixelLength;
					MeasuringGraphicsEventArgs measuringGraphicsEventArgs = new MeasuringGraphicsEventArgs();
					measuringGraphicsEventArgs.Identifier = graphicsRangeFinder.Identifier;
					measuringGraphicsEventArgs.XImagePixel = num;
					measuringGraphicsEventArgs.YImagePixel = num2;
					measuringGraphicsEventArgs.XPhysicalDistance = xPhysicalDistance;
					measuringGraphicsEventArgs.YPhysicalDistance = yPhysicalDistance;
					this.OnRangeFinderChanged(sender, measuringGraphicsEventArgs);
				}
			}
			else if (type == typeof(GraphicsCircle))
			{
				if (this.OnCircleChanged != null)
				{
					GraphicsCircle graphicsCircle = sender as GraphicsCircle;
					PointF boxPoint = new PointF(graphicsCircle.Location.X - graphicsCircle.Radius, graphicsCircle.Location.Y - graphicsCircle.Radius);
					PointF boxPoint2 = new PointF(graphicsCircle.Location.X + graphicsCircle.Radius, graphicsCircle.Location.Y + graphicsCircle.Radius);
					PointF pointF = ConvertBoxPointFToImagePointF(boxPoint);
					PointF pointF2 = ConvertBoxPointFToImagePointF(boxPoint2);
					float num = Math.Abs(pointF2.X - pointF.X);
					float num2 = Math.Abs(pointF2.Y - pointF.Y);
					float xPhysicalDistance = num * _measureXPixelLength;
					float yPhysicalDistance = num2 * _measureYPixelLength;
					MeasuringGraphicsEventArgs measuringGraphicsEventArgs = new MeasuringGraphicsEventArgs();
					measuringGraphicsEventArgs.Identifier = graphicsCircle.Identifier;
					measuringGraphicsEventArgs.XImagePixel = num;
					measuringGraphicsEventArgs.YImagePixel = num2;
					measuringGraphicsEventArgs.XPhysicalDistance = xPhysicalDistance;
					measuringGraphicsEventArgs.YPhysicalDistance = yPhysicalDistance;
					this.OnCircleChanged(sender, measuringGraphicsEventArgs);
				}
			}
			else if (type == typeof(GraphicsLineBySlope))
			{
				if (this.OnSlashChanged != null)
				{
					GraphicsLineBySlope graphicsLineBySlope = sender as GraphicsLineBySlope;
					PointF imagePoint = ConvertBoxPointFToImagePointF(graphicsLineBySlope.Location);
					PointF physicalCoordinate = ConvertImagePointFToPhysicalCoordinate(imagePoint);
					PointGraphicsEventArgs pointGraphicsEventArgs = new PointGraphicsEventArgs();
					pointGraphicsEventArgs.Identifier = graphicsLineBySlope.Identifier;
					pointGraphicsEventArgs.ImagePoint = imagePoint;
					pointGraphicsEventArgs.PhysicalCoordinate = physicalCoordinate;
					this.OnSlashChanged(sender, pointGraphicsEventArgs);
				}
			}
			else if (type == typeof(GraphicsSpot))
			{
				if (this.OnSpotChanged != null)
				{
					GraphicsSpot graphicsSpot = sender as GraphicsSpot;
					PointF imagePoint = ConvertBoxPointFToImagePointF(graphicsSpot.Location);
					PointF physicalCoordinate = ConvertImagePointFToPhysicalCoordinate(imagePoint);
					PointGraphicsEventArgs pointGraphicsEventArgs = new PointGraphicsEventArgs();
					pointGraphicsEventArgs.Identifier = graphicsSpot.Identifier;
					pointGraphicsEventArgs.ImagePoint = imagePoint;
					pointGraphicsEventArgs.PhysicalCoordinate = physicalCoordinate;
					this.OnSpotChanged(sender, pointGraphicsEventArgs);
				}
			}
			else if (type == typeof(GraphicsLineByTwoPoint) && this.OnLineChanged != null)
			{
				GraphicsLineByTwoPoint graphicsLineByTwoPoint = sender as GraphicsLineByTwoPoint;
				PointF pointF3 = ConvertBoxPointFToImagePointF(graphicsLineByTwoPoint.StartPoint);
				PointF pointF4 = ConvertBoxPointFToImagePointF(graphicsLineByTwoPoint.EndPoint);
				PointF physicalStart = ConvertImagePointFToPhysicalCoordinate(pointF3);
				PointF physicalEnd = ConvertImagePointFToPhysicalCoordinate(pointF4);
				SegmentGraphicsEventArgs segmentGraphicsEventArgs = new SegmentGraphicsEventArgs();
				segmentGraphicsEventArgs.Identifier = graphicsLineByTwoPoint.Identifier;
				segmentGraphicsEventArgs.Slope = (graphicsLineByTwoPoint.EndPoint.Y - graphicsLineByTwoPoint.StartPoint.Y) / (graphicsLineByTwoPoint.EndPoint.X - graphicsLineByTwoPoint.StartPoint.X);
				segmentGraphicsEventArgs.XImagePixel = Math.Abs(pointF3.X - pointF4.X);
				segmentGraphicsEventArgs.YImagePixel = Math.Abs(pointF3.Y - pointF4.Y);
				segmentGraphicsEventArgs.XPhysicalDistance = Math.Abs(physicalStart.X - physicalEnd.X);
				segmentGraphicsEventArgs.YPhysicalDistance = Math.Abs(physicalStart.Y - physicalEnd.Y);
				segmentGraphicsEventArgs.ImageStart = pointF3;
				segmentGraphicsEventArgs.ImageEnd = pointF4;
				segmentGraphicsEventArgs.PhysicalStart = physicalStart;
				segmentGraphicsEventArgs.PhysicalEnd = physicalEnd;
				this.OnLineChanged(sender, segmentGraphicsEventArgs);
			}
		}

		public void UpdateMovingCalibrationInfo(float xPixelLength, float yPixelLength)
		{
			if (_movingXPixelLength == xPixelLength && _movingYPixelLength == yPixelLength)
			{
				return;
			}
			if (_movingXPixelLength > 0f && _movingYPixelLength > 0f)
			{
				float num = ImageWidth;
				float num2 = ImageHeight;
				float num3 = num * xPixelLength;
				float num4 = num2 * yPixelLength;
				float num5 = num3 / _movingXPixelLength;
				float num6 = num4 / _movingYPixelLength;
				float num7 = base.Width;
				float num8 = base.Height;
				float num9 = num7 / num;
				float num10 = num8 / num2;
				float num11 = ((num9 > num10) ? num10 : num9);
				float num12 = num5 * num11;
				float num13 = num6 * num11;
				RectangleF target = new RectangleF(0f, 0f, num7, num8);
				if (num9 < num10)
				{
					target.Y = (num8 - num2 * num9) / 2f;
					target.Height -= 2f * target.Y;
				}
				else
				{
					target.X = (num7 - num * num10) / 2f;
					target.Width -= 2f * target.X;
				}
				RectangleF reference = new RectangleF((num7 - num12) / 2f, (num8 - num13) / 2f, num12, num13);
				GraphicsList.MappingGraphics(reference, target);
			}
			_movingXPixelLength = xPixelLength;
			_movingYPixelLength = yPixelLength;
			foreach (GraphicsObject graphics in GraphicsList)
			{
				graphics.Refresh();
			}
			Invalidate();
		}

		public void InitMovingCalibrationInfo(float xPixelLength, float yPixelLength)
		{
			_movingXPixelLength = xPixelLength;
			_movingYPixelLength = yPixelLength;
			Invalidate();
		}

		public void SetMeasureCalibrationInfo(float xPixelLength, float yPixelLength)
		{
			_measureXPixelLength = xPixelLength;
			_measureYPixelLength = yPixelLength;
		}

		public void UpdatePhysicalLocation(float physicalX, float physicalY)
		{
			if (!(_movingXPixelLength <= 0f) && !(_movingYPixelLength <= 0f) && (physicalX != _physicalX || physicalY != _physicalY))
			{
				if (GraphicsList.Count == 0)
				{
					_physicalX = physicalX;
					_physicalY = physicalY;
					return;
				}
				float num = physicalX - _physicalX;
				float num2 = physicalY - _physicalY;
				float num3 = num / _movingXPixelLength;
				float num4 = num2 / _movingYPixelLength;
				float num5 = ImageWidth;
				float num6 = ImageHeight;
				float num7 = base.Width;
				float num8 = base.Height;
				float num9 = num7 / num5;
				float num10 = num8 / num6;
				float num11 = ((num9 > num10) ? num10 : num9);
				float deltaX = 0f - num3 * num11;
				float deltaY = 0f - num4 * num11;
				_physicalX = physicalX;
				_physicalY = physicalY;
				GraphicsList.Move(deltaX, deltaY);
			}
		}

		public void UpdatePhysicalLocation(PointF location)
		{
			UpdatePhysicalLocation(location.X, location.Y);
		}

		public void InitPhysicalLocation(float physicalX, float physicalY)
		{
			_physicalX = physicalX;
			_physicalY = physicalY;
		}

		public void InitPhysicalLocation(PointF location)
		{
			InitPhysicalLocation(location.X, location.Y);
		}

		public void ClearGraphics()
		{
			ActiveTool = DrawToolType.Pointer;
			graphicsList.Clear();
			Invalidate();
		}

		internal bool Contains(PointF pt)
		{
			return pt.X > 0f && pt.X < (float)(base.Width - 1) && pt.Y > 0f && pt.Y < (float)(base.Height - 1);
		}

		public void OpenImage(string filename)
		{
			ImageLocation = filename;
		}

		public bool SaveImage(string filename)
		{
			if (Image == null)
			{
				return false;
			}
			Bitmap bmp = new Bitmap(Image);
			RectangleF imageRectangleInBox = GetImageRectangleInBox(bmp);
			Size size = imageRectangleInBox.Size.ToSize();
			bmp = FunctionSet.ConversionResolution(bmp, size);
			if (graphicsList != null)
			{
				Graphics g = Graphics.FromImage(bmp);
				graphicsList.Draw(imageRectangleInBox, g);
			}
			bmp.Save(filename, ImageFormat.Bmp);
			return true;
		}

		public bool SaveOriginalImage(string filename)
		{
			if (Image == null)
			{
				return false;
			}
			Bitmap original = new Bitmap((_originalImage == null) ? Image : _originalImage);
			Bitmap bitmap = FunctionSet.RgbToGrayScale(original);
			bitmap.Save(filename, ImageFormat.Bmp);
			return true;
		}

		public RectangleF GetImageRectangleInBox(Image image)
		{
			float num = image.Width;
			float num2 = image.Height;
			float num3 = base.Width;
			float num4 = base.Height;
			RectangleF result = new RectangleF(0f, 0f, num3, num4);
			float num5 = num3 / num;
			float num6 = num4 / num2;
			if (num5 < num6)
			{
				result.Y = (num4 - num2 * num5) / 2f;
				result.Height -= 2f * result.Y;
			}
			else
			{
				result.X = (num3 - num * num6) / 2f;
				result.Width -= 2f * result.X;
			}
			return result;
		}

		internal float PhysicalDistanceBetweenTwoBoxPoints(PointF pt1, PointF pt2)
		{
			if (Image == null)
			{
				return 0f;
			}
			if (_measureXPixelLength <= 0f || _measureYPixelLength <= 0f)
			{
				return 0f;
			}
			PointF pointF = ConvertBoxPointFToImagePointF(pt1);
			PointF pointF2 = ConvertBoxPointFToImagePointF(pt2);
			float num = Math.Abs(pointF.X - pointF2.X);
			float num2 = Math.Abs(pointF.Y - pointF2.Y);
			float num3 = num * _measureXPixelLength;
			float num4 = num2 * _measureYPixelLength;
			return (float)Math.Sqrt(num3 * num3 + num4 * num4);
		}

		public PointF ConvertBoxPointFToImagePointF(PointF boxPoint)
		{
			PointF empty = PointF.Empty;
			try
			{
				float num = ImageWidth;
				float num2 = ImageHeight;
				float num3 = base.Width;
				float num4 = base.Height;
				double num5 = num3 / num;
				double num6 = num4 / num2;
				if (num5 < num6)
				{
					float num7 = (float)(((double)num4 - (double)num2 * num5) / 2.0);
					empty.X = (float)((double)boxPoint.X / num5);
					empty.Y = (float)((double)(boxPoint.Y - num7) / num5);
				}
				else
				{
					float num8 = (float)(((double)num3 - (double)num * num6) / 2.0);
					empty.X = (float)((double)(boxPoint.X - num8) / num6);
					empty.Y = (float)((double)boxPoint.Y / num6);
				}
			}
			catch
			{
				empty = PointF.Empty;
			}
			return empty;
		}

		public PointF ConvertImagePointFToBoxPointF(PointF imagePoint)
		{
			PointF empty = PointF.Empty;
			try
			{
				float num = ImageWidth;
				float num2 = ImageHeight;
				float num3 = base.Width;
				float num4 = base.Height;
				double num5 = num3 / num;
				double num6 = num4 / num2;
				if (num5 < num6)
				{
					float num7 = (float)(((double)num4 - (double)num2 * num5) / 2.0);
					empty.X = (float)((double)imagePoint.X * num5);
					empty.Y = (float)((double)imagePoint.Y * num5 + (double)num7);
				}
				else
				{
					float num8 = (float)(((double)num3 - (double)num * num6) / 2.0);
					empty.X = (float)((double)imagePoint.X * num6 + (double)num8);
					empty.Y = (float)((double)imagePoint.Y * num6);
				}
			}
			catch
			{
				empty = PointF.Empty;
			}
			return empty;
		}

		public PointF ConvertImagePointFToPhysicalCoordinate(PointF imagePoint)
		{
			imagePoint.X += _cutPosition.X;
			imagePoint.Y += _cutPosition.Y;
			float num = ImageWidth;
			float num2 = ImageHeight;
			if (_cutPosition != Point.Empty)
			{
				num = _originalImage.Width;
				num2 = _originalImage.Height;
			}
			PointF pointF = new PointF(imagePoint.X - num / 2f, imagePoint.Y - num2 / 2f);
			PointF result = new PointF(pointF.X * _movingXPixelLength, pointF.Y * _movingYPixelLength);
			result.X += _physicalX;
			result.Y += _physicalY;
			return result;
		}

		public PointF ConvertPhysicalCoordinateToImagePointF(PointF physicalCoordinate)
		{
			physicalCoordinate.X -= _physicalX;
			physicalCoordinate.Y -= _physicalY;
			PointF pointF = new PointF(physicalCoordinate.X / _movingXPixelLength, physicalCoordinate.Y / _movingYPixelLength);
			float num = ImageWidth;
			float num2 = ImageHeight;
			if (_cutPosition != Point.Empty)
			{
				num = _originalImage.Width;
				num2 = _originalImage.Height;
			}
			PointF result = new PointF(num / 2f + pointF.X, num2 / 2f + pointF.Y);
			result.X -= _cutPosition.X;
			result.Y -= _cutPosition.Y;
			return result;
		}

		public PointF ConvertBoxPointFToPhysicalCoordinate(PointF boxPoint)
		{
			PointF imagePoint = ConvertBoxPointFToImagePointF(boxPoint);
			return ConvertImagePointFToPhysicalCoordinate(imagePoint);
		}

		public PointF ConvertPhysicalCoordinateToBoxPointF(PointF physicalCoordinate)
		{
			PointF imagePoint = ConvertPhysicalCoordinateToImagePointF(physicalCoordinate);
			return ConvertImagePointFToBoxPointF(imagePoint);
		}

		public void ZoomIn(RectangleF zoomArea)
		{
			int num = base.Width;
			int num2 = base.Height;
			int num3 = ImageWidth;
			int num4 = ImageHeight;
			Image original = Image;
			PointF pointF = ConvertBoxPointFToImagePointF(zoomArea.Location);
			PointF pointF2 = ConvertBoxPointFToImagePointF(new PointF(zoomArea.X + zoomArea.Width, zoomArea.Y + zoomArea.Height));
			Point point = new Point((int)Math.Round(pointF.X), (int)Math.Round(pointF.Y));
			Point point2 = new Point((int)Math.Round(pointF2.X), (int)Math.Round(pointF2.Y));
			Size size = new Size(point2.X - point.X, point2.Y - point.Y);
			if (size.Width < num && size.Height < num2)
			{
				point.X -= (num - size.Width) / 2;
				point.Y -= (num2 - size.Height) / 2;
				size.Width = num;
				size.Height = num2;
			}
			else
			{
				float num5 = (float)size.Width / (float)num;
				float num6 = (float)size.Height / (float)num2;
				if (num5 < num6)
				{
					point.X -= (int)Math.Round((num6 * (float)num - (float)size.Width) / 2f);
					size.Width = (int)Math.Round(num6 * (float)num);
				}
				else
				{
					point.Y -= (int)Math.Round((num5 * (float)num2 - (float)size.Height) / 2f);
					size.Height = (int)Math.Round(num5 * (float)num2);
				}
			}
			if (size.Width <= num3 && size.Height <= num4)
			{
				if (point.X < 0)
				{
					point.X = 0;
				}
				if (point.Y < 0)
				{
					point.Y = 0;
				}
				if (point.X + size.Width > num3)
				{
					point.X = num3 - size.Width;
				}
				if (point.Y + size.Height > num4)
				{
					point.Y = num4 - size.Height;
				}
				_cutPosition.X += point.X;
				_cutPosition.Y += point.Y;
				RectangleF imageRectangleInBox = GetImageRectangleInBox(original);
				RectangleF empty = RectangleF.Empty;
				float num7 = 0f;
				num7 = ((imageRectangleInBox.X != 0f) ? ((float)num2 / (float)size.Height) : ((float)num / (float)size.Width));
				empty.X = 0f - (float)point.X * num7;
				empty.Y = 0f - (float)point.Y * num7;
				empty.Width = (float)num3 * num7;
				empty.Height = (float)num4 * num7;
				GraphicsList.MappingGraphics(imageRectangleInBox, empty);
				if (_originalImage == null)
				{
					_originalImage = Image;
				}
				Bitmap bitmap = (Bitmap)(base.Image = new Bitmap(original).Clone(new RectangleF(point, size), PixelFormat.Undefined));
				ImageWidth = bitmap.Width;
				ImageHeight = bitmap.Height;
			}
		}

		public void ZoomOut()
		{
			if (_originalImage != null)
			{
				Image image = Image;
				int num = image.Width;
				int num2 = image.Height;
				int num3 = base.Width;
				int num4 = base.Height;
				int num5 = _originalImage.Width;
				int num6 = _originalImage.Height;
				RectangleF imageRectangleInBox = GetImageRectangleInBox(_originalImage);
				RectangleF reference = new RectangleF(0f, 0f, num3, num4);
				RectangleF empty = RectangleF.Empty;
				float num7 = 0f;
				num7 = ((imageRectangleInBox.X != 0f) ? ((float)num4 / (float)num6) : ((float)num3 / (float)num5));
				empty.X = _cutPosition.X * num7 + imageRectangleInBox.X;
				empty.Y = _cutPosition.Y * num7 + imageRectangleInBox.Y;
				empty.Width = (float)num * num7;
				empty.Height = (float)num2 * num7;
				GraphicsList.MappingGraphics(reference, empty);
				Image = _originalImage;
			}
		}
	}
}
