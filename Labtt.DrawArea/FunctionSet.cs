using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Labtt.DrawArea
{

	public class FunctionSet
	{
		public enum TwoDDirection
		{
			Top,
			TopRight,
			Right,
			BottomRight,
			Bottom,
			BottomLeft,
			Left,
			TopLeft
		}

		private FunctionSet()
		{
		}

		public static List<PointF> GetIntersectionOfLineAndRectangle(PointF location, float slope, RectangleF rectangle)
		{
			PointF start = location;
			PointF empty = PointF.Empty;
			return GetIntersectionOfLineAndRectangle(end: (slope == 0f) ? new PointF(start.X + 1f, start.Y) : ((!float.IsInfinity(slope)) ? new PointF(location.X + 10f, location.Y + slope * 10f) : new PointF(start.X, start.Y + 1f)), start: start, rectangle: rectangle);
		}

		public static List<PointF> GetIntersectionOfLineAndRectangle(PointF start, PointF end, RectangleF rectangle)
		{
			List<PointF> list = new List<PointF>();
			if (start == end)
			{
				return list;
			}
			if (start.X == end.X)
			{
				if (start.X > rectangle.X && start.X < rectangle.X + rectangle.Width)
				{
					list.Add(new PointF(start.X, rectangle.Y));
					list.Add(new PointF(start.X, rectangle.Y + rectangle.Height));
				}
			}
			else if (start.Y == end.Y)
			{
				if (start.Y > rectangle.Y && start.Y < rectangle.Y + rectangle.Height)
				{
					list.Add(new PointF(rectangle.X, start.Y));
					list.Add(new PointF(rectangle.X + rectangle.Width, start.Y));
				}
			}
			else
			{
				double num = (end.Y - start.Y) / (end.X - start.X);
				double num2 = start.Y - start.X * (end.Y - start.Y) / (end.X - start.X);
				float num3 = (float)(num * (double)rectangle.X + num2);
				if (num3 >= rectangle.Y && num3 < rectangle.Y + rectangle.Height)
				{
					list.Add(new PointF(rectangle.X, num3));
				}
				float num4 = (float)(num * (double)(rectangle.X + rectangle.Width) + num2);
				if (num4 > rectangle.Y && num4 <= rectangle.Y + rectangle.Height)
				{
					list.Add(new PointF(rectangle.X + rectangle.Width, num4));
				}
				float num5 = (float)(((double)rectangle.Y - num2) / num);
				if (num5 > rectangle.X && num5 <= rectangle.X + rectangle.Width)
				{
					list.Add(new PointF(num5, rectangle.Y));
				}
				float num6 = (float)(((double)(rectangle.Y + rectangle.Height) - num2) / num);
				if (num6 >= rectangle.X && num6 < rectangle.X + rectangle.Width)
				{
					list.Add(new PointF(num6, rectangle.Y + rectangle.Height));
				}
			}
			return list;
		}

		public static double DistanceOfTwoPoints(PointF p1, PointF p2)
		{
			return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}

		public static Point GetIntersectionOfTwoLines(Point start1, Point end1, Point start2, Point end2)
		{
			try
			{
				int x = start1.X;
				int y = start1.Y;
				int x2 = end1.X;
				int y2 = end1.Y;
				int x3 = start2.X;
				int y3 = start2.Y;
				int x4 = end2.X;
				int y4 = end2.Y;
				int x5 = (y3 * x4 * x2 - y4 * x3 * x2 - y3 * x4 * x + y4 * x3 * x - y * x2 * x4 + y2 * x * x4 + y * x2 * x3 - y2 * x * x3) / (x4 * y2 - x4 * y - x3 * y2 + x3 * y - x2 * y4 + x2 * y3 + x * y4 - x * y3);
				int y5 = (y4 * x3 * y2 - y3 * x4 * y2 + y3 * x4 * y - y4 * x3 * y + y * x2 * y4 - y * x2 * y3 - y2 * x * y4 + y2 * x * y3) / (y4 * x2 - y4 * x - y3 * x2 + x * y3 - y2 * x4 + y2 * x3 + y * x4 - y * x3);
				return new Point(x5, y5);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static bool GetDiamondPoints(ref Point[] cornerPoints, out int left, out int right, out int top, out int bottom)
		{
			left = 0;
			int x = cornerPoints[0].X;
			for (int i = 1; i < cornerPoints.Length; i++)
			{
				if (cornerPoints[i].X < x)
				{
					x = cornerPoints[i].X;
					left = i;
				}
			}
			right = 0;
			x = cornerPoints[0].X;
			for (int i = 1; i < cornerPoints.Length; i++)
			{
				if (cornerPoints[i].X > x)
				{
					x = cornerPoints[i].X;
					right = i;
				}
			}
			top = 0;
			x = cornerPoints[0].Y;
			for (int i = 1; i < cornerPoints.Length; i++)
			{
				if (cornerPoints[i].Y < x)
				{
					x = cornerPoints[i].Y;
					top = i;
				}
			}
			bottom = 0;
			x = cornerPoints[0].Y;
			for (int i = 1; i < cornerPoints.Length; i++)
			{
				if (cornerPoints[i].Y > x)
				{
					x = cornerPoints[i].Y;
					bottom = i;
				}
			}
			if (left == top || left == bottom || right == top || right == bottom)
			{
				return false;
			}
			return true;
		}

		public static bool GetSlopeOfTwoPoint(out double Slope, PointF StartPoint, PointF EndPoint)
		{
			if (StartPoint.Equals(EndPoint) || StartPoint.X == EndPoint.X)
			{
				Slope = 0.0;
				return false;
			}
			if (StartPoint.Y == EndPoint.Y)
			{
				Slope = 0.0;
				return true;
			}
			Slope = (double)(EndPoint.Y - StartPoint.Y) / (double)(EndPoint.X - StartPoint.X);
			return true;
		}

		public static void GetCircular(PointF P1, PointF P2, PointF P3, ref float R, ref PointF PCenter)
		{
			if (!P1.Equals(P2) && !P1.Equals(P3) && !P2.Equals(P3))
			{
				double Slope = 0.0;
				GetSlopeOfTwoPoint(out Slope, P1, P2);
				double Slope2 = 0.0;
				GetSlopeOfTwoPoint(out Slope2, P1, P3);
				if (Math.Abs(Slope) != Math.Abs(Slope2))
				{
					float num = 2f * (P2.X - P1.X);
					float num2 = 2f * (P2.Y - P1.Y);
					float num3 = P2.X * P2.X + P2.Y * P2.Y - P1.X * P1.X - P1.Y * P1.Y;
					float num4 = 2f * (P3.X - P2.X);
					float num5 = 2f * (P3.Y - P2.Y);
					float num6 = P3.X * P3.X + P3.Y * P3.Y - P2.X * P2.X - P2.Y * P2.Y;
					float num7 = (num2 * num6 - num5 * num3) / (num2 * num4 - num5 * num);
					float num8 = (num4 * num3 - num * num6) / (num2 * num4 - num5 * num);
					float num9 = (R = (float)Math.Sqrt((num7 - P1.X) * (num7 - P1.X) + (num8 - P1.Y) * (num8 - P1.Y)));
					PointF pointF = (PCenter = new PointF(num7, num8));
				}
			}
		}

		public static void GetCircular(PointF P1, PointF P2, PointF P3, ref Point PCenter, ref float R)
		{
			if (!P1.Equals(P2) && !P1.Equals(P3) && !P2.Equals(P3))
			{
				double Slope = 0.0;
				GetSlopeOfTwoPoint(out Slope, P1, P2);
				double Slope2 = 0.0;
				GetSlopeOfTwoPoint(out Slope2, P1, P3);
				if (Math.Abs(Slope) != Math.Abs(Slope2))
				{
					float num = 2f * (P2.X - P1.X);
					float num2 = 2f * (P2.Y - P1.Y);
					float num3 = P2.X * P2.X + P2.Y * P2.Y - P1.X * P1.X - P1.Y * P1.Y;
					float num4 = 2f * (P3.X - P2.X);
					float num5 = 2f * (P3.Y - P2.Y);
					float num6 = P3.X * P3.X + P3.Y * P3.Y - P2.X * P2.X - P2.Y * P2.Y;
					float num7 = (num2 * num6 - num5 * num3) / (num2 * num4 - num5 * num);
					float num8 = (num4 * num3 - num * num6) / (num2 * num4 - num5 * num);
					float num9 = (R = (float)Math.Sqrt((num7 - P1.X) * (num7 - P1.X) + (num8 - P1.Y) * (num8 - P1.Y)));
					Point point = (PCenter = new Point((int)num7, (int)num8));
				}
			}
		}

		public static TwoDDirection GetTwoPointDirection(PointF FirstPoint, PointF SecondPoint)
		{
			if (SecondPoint.X == FirstPoint.X && SecondPoint.Y < FirstPoint.Y)
			{
				return TwoDDirection.Top;
			}
			if (SecondPoint.X > FirstPoint.X && SecondPoint.Y < FirstPoint.Y)
			{
				return TwoDDirection.TopRight;
			}
			if (SecondPoint.X > FirstPoint.X && SecondPoint.Y == FirstPoint.Y)
			{
				return TwoDDirection.Right;
			}
			if (SecondPoint.X > FirstPoint.X && SecondPoint.Y > FirstPoint.Y)
			{
				return TwoDDirection.BottomRight;
			}
			if (SecondPoint.X == FirstPoint.X && SecondPoint.Y > FirstPoint.Y)
			{
				return TwoDDirection.Bottom;
			}
			if (SecondPoint.X < FirstPoint.X && SecondPoint.Y > FirstPoint.Y)
			{
				return TwoDDirection.BottomLeft;
			}
			if (SecondPoint.X < FirstPoint.X && SecondPoint.Y == FirstPoint.Y)
			{
				return TwoDDirection.Left;
			}
			return TwoDDirection.TopLeft;
		}

		public static float GetAngleLineWithHorizontalLine(PointF LineStartPoint, PointF LineEndPoint)
		{
			if (LineStartPoint.Y == LineEndPoint.Y)
			{
				return 0f;
			}
			if (LineStartPoint.X == LineEndPoint.X)
			{
				return 90f;
			}
			double value = (double)(LineStartPoint.Y - LineEndPoint.Y) / (double)(LineStartPoint.X - LineEndPoint.X);
			double num = Math.Atan(Math.Abs(value));
			num = 180.0 * (num / Math.PI);
			return (float)num;
		}

		public static double GetAngleOfThreePoint(PointF FirstPoint, PointF SecondPoint, PointF ThirdPoint)
		{
			double Slope = 0.0;
			double Slope2 = 0.0;
			GetSlopeOfTwoPoint(out Slope, FirstPoint, SecondPoint);
			GetSlopeOfTwoPoint(out Slope2, FirstPoint, ThirdPoint);
			double num = GetAngleLineWithHorizontalLine(FirstPoint, SecondPoint);
			double num2 = GetAngleLineWithHorizontalLine(FirstPoint, ThirdPoint);
			double num3 = 0.0;
			if ((FirstPoint.X == SecondPoint.X && FirstPoint.Y == ThirdPoint.Y) || (FirstPoint.X == ThirdPoint.X && FirstPoint.Y == SecondPoint.Y))
			{
				return 90.0;
			}
			if (FirstPoint.Equals(SecondPoint) || FirstPoint.Equals(ThirdPoint) || SecondPoint.Equals(ThirdPoint))
			{
				return 0.0;
			}
			if (FirstPoint.Y == SecondPoint.Y && SecondPoint.Y == ThirdPoint.Y)
			{
				if ((FirstPoint.X > SecondPoint.X && FirstPoint.X < ThirdPoint.X) || (FirstPoint.X > ThirdPoint.X && FirstPoint.X < SecondPoint.X))
				{
					return 180.0;
				}
				return 0.0;
			}
			if (FirstPoint.X == SecondPoint.X && SecondPoint.X == ThirdPoint.X)
			{
				if ((FirstPoint.Y > SecondPoint.Y && FirstPoint.Y < ThirdPoint.Y) || (FirstPoint.Y > ThirdPoint.Y && FirstPoint.Y < SecondPoint.Y))
				{
					return 180.0;
				}
				return 0.0;
			}
			if (Math.Abs(Slope) == Math.Abs(Slope2))
			{
				if ((FirstPoint.X > SecondPoint.X && FirstPoint.X < ThirdPoint.X) || (FirstPoint.X > ThirdPoint.X && FirstPoint.X < SecondPoint.X))
				{
					return 180.0;
				}
				return 0.0;
			}
			TwoDDirection twoDDirection = TwoDDirection.Top;
			TwoDDirection twoDDirection2 = TwoDDirection.Top;
			twoDDirection = GetTwoPointDirection(FirstPoint, SecondPoint);
			twoDDirection2 = GetTwoPointDirection(FirstPoint, ThirdPoint);
			switch (twoDDirection)
			{
				case TwoDDirection.Top:
					return twoDDirection2 switch
					{
						TwoDDirection.Top => 0.0,
						TwoDDirection.TopRight => num - num2,
						TwoDDirection.Right => 90.0,
						TwoDDirection.BottomRight => num + num2,
						TwoDDirection.Bottom => 180.0,
						TwoDDirection.BottomLeft => num + num2,
						TwoDDirection.Left => 90.0,
						TwoDDirection.TopLeft => num - num2,
						_ => -1.0,
					};
				case TwoDDirection.TopRight:
					switch (twoDDirection2)
					{
						case TwoDDirection.Top:
							return num2 - num;
						case TwoDDirection.TopRight:
							return Math.Abs(num2 - num);
						case TwoDDirection.Right:
							return num;
						case TwoDDirection.BottomRight:
							return num + num2;
						case TwoDDirection.Bottom:
							return 90.0 + num;
						case TwoDDirection.BottomLeft:
							num3 = 180.0 - num2 + num;
							if (num3 > 180.0)
							{
								return 360.0 - num3;
							}
							return num3;
						case TwoDDirection.Left:
							return 180.0 - num;
						case TwoDDirection.TopLeft:
							return 180.0 - num - num2;
						default:
							return -1.0;
					}
				case TwoDDirection.Right:
					return twoDDirection2 switch
					{
						TwoDDirection.Top => 90.0,
						TwoDDirection.TopRight => num2,
						TwoDDirection.Right => 0.0,
						TwoDDirection.BottomRight => num2,
						TwoDDirection.Bottom => 90.0,
						TwoDDirection.BottomLeft => 180.0 - num2,
						TwoDDirection.Left => 180.0,
						TwoDDirection.TopLeft => 180.0 - num2,
						_ => -1.0,
					};
				case TwoDDirection.BottomRight:
					switch (twoDDirection2)
					{
						case TwoDDirection.Top:
							return 90.0 + num;
						case TwoDDirection.TopRight:
							return num + num2;
						case TwoDDirection.Right:
							return num;
						case TwoDDirection.BottomRight:
							return Math.Abs(num - num2);
						case TwoDDirection.Bottom:
							return 90.0 - num;
						case TwoDDirection.BottomLeft:
							return 180.0 - num - num2;
						case TwoDDirection.Left:
							return 180.0 - num;
						case TwoDDirection.TopLeft:
							num3 = 180.0 - num2 + num;
							if (num3 > 180.0)
							{
								return 360.0 - num3;
							}
							return num3;
						default:
							return -1.0;
					}
				case TwoDDirection.Bottom:
					return twoDDirection2 switch
					{
						TwoDDirection.Top => 180.0,
						TwoDDirection.TopRight => 90.0 + num2,
						TwoDDirection.Right => 90.0,
						TwoDDirection.BottomRight => 90.0 - num2,
						TwoDDirection.Bottom => 0.0,
						TwoDDirection.BottomLeft => 90.0 - num2,
						TwoDDirection.Left => 90.0,
						TwoDDirection.TopLeft => 90.0 + num2,
						_ => -1.0,
					};
				case TwoDDirection.BottomLeft:
					switch (twoDDirection2)
					{
						case TwoDDirection.Top:
							return 90.0 + num;
						case TwoDDirection.TopRight:
							num3 = 180.0 - num2 + num;
							if (num3 > 180.0)
							{
								return 360.0 - num3;
							}
							return num3;
						case TwoDDirection.Right:
							return 180.0 - num;
						case TwoDDirection.BottomRight:
							return 180.0 - num - num2;
						case TwoDDirection.Bottom:
							return 90.0 - num;
						case TwoDDirection.BottomLeft:
							return Math.Abs(num - num2);
						case TwoDDirection.Left:
							return num;
						case TwoDDirection.TopLeft:
							return num + num2;
						default:
							return -1.0;
					}
				case TwoDDirection.Left:
					return twoDDirection2 switch
					{
						TwoDDirection.Top => 90.0,
						TwoDDirection.TopRight => 180.0 - num2,
						TwoDDirection.Right => 180.0,
						TwoDDirection.BottomRight => 180.0 - num2,
						TwoDDirection.Bottom => 90.0,
						TwoDDirection.BottomLeft => num2,
						TwoDDirection.Left => 0.0,
						TwoDDirection.TopLeft => num2,
						_ => -1.0,
					};
				default:
					switch (twoDDirection2)
					{
						case TwoDDirection.Top:
							return 90.0 - num;
						case TwoDDirection.TopRight:
							return 180.0 - num - num2;
						case TwoDDirection.Right:
							return 180.0 - num;
						case TwoDDirection.BottomRight:
							num3 = 180.0 - num2 + num;
							if (num3 > 180.0)
							{
								return 360.0 - num3;
							}
							return num3;
						case TwoDDirection.Bottom:
							return 90.0 + num;
						case TwoDDirection.BottomLeft:
							return num + num2;
						case TwoDDirection.Left:
							return num;
						case TwoDDirection.TopLeft:
							return Math.Abs(num - num2);
						default:
							return -1.0;
					}
			}
		}

		public static Bitmap ConversionResolution(Bitmap bmp, Size size)
		{
			try
			{
				Bitmap bitmap = new Bitmap(size.Width, size.Height);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(bmp, new Rectangle(0, 0, size.Width, size.Height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
				graphics.Dispose();
				return bitmap;
			}
			catch (Exception innerException)
			{
				throw new Exception("转换分辨率失败！", innerException);
			}
		}

		public static Bitmap RgbToGrayScale(Bitmap original)
		{
			if (original != null)
			{
				Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
				BitmapData bitmapData = original.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
				int width = bitmapData.Width;
				int height = bitmapData.Height;
				int stride = bitmapData.Stride;
				int num = stride - width * 3;
				IntPtr scan = bitmapData.Scan0;
				int num2 = stride * height;
				int num3 = 0;
				int num4 = 0;
				byte[] array = new byte[num2];
				Marshal.Copy(scan, array, 0, num2);
				byte[] array2 = new byte[width * height];
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						byte b = array[num3];
						byte b2 = array[num3 + 1];
						byte b3 = array[num3 + 2];
						array2[num4] = (byte)((b + b2 + b3) / 3);
						num3 += 3;
						num4++;
					}
					num3 += num;
				}
				Marshal.Copy(array, 0, scan, num2);
				original.UnlockBits(bitmapData);
				return BuiltGrayBitmap(array2, width, height);
			}
			return null;
		}

		private static Bitmap BuiltGrayBitmap(byte[] rawValues, int width, int height)
		{
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			int num = bitmapData.Stride - bitmapData.Width;
			IntPtr scan = bitmapData.Scan0;
			int num2 = bitmapData.Stride * bitmapData.Height;
			byte[] array = new byte[num2];
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					array[num4++] = rawValues[num3++];
				}
				num4 += num;
			}
			Marshal.Copy(array, 0, scan, num2);
			bitmap.UnlockBits(bitmapData);
			ColorPalette palette;
			using (Bitmap bitmap2 = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
			{
				palette = bitmap2.Palette;
			}
			for (int i = 0; i < 256; i++)
			{
				ref Color reference = ref palette.Entries[i];
				reference = Color.FromArgb(i, i, i);
			}
			bitmap.Palette = palette;
			return bitmap;
		}
	}
}