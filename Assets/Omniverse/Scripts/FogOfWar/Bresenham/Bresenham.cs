using System.Runtime.CompilerServices;
using UnityEngine;

namespace Dreambox.Math
{
	public static class Bresenham
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Line<T>(int x0, int y0, int x1, int y1, T handler) where T : struct, IBresenhamLineHandler
		{
			if (handler.HandlePoint(x0, y0))
			{
				return;
			}

			int dx = x1 - x0;
			int dy = y1 - y0;

			int sx = System.Math.Sign(dx);
			int sy = System.Math.Sign(dy);

			dx = Mathf.Abs(dx);
			dy = Mathf.Abs(dy);

			int height;
			int length;
			int offsetx;
			int offsety;

			if (dx > dy)
			{
				length = dx;
				height = dy;
				offsetx = sx;
				offsety = 0;
			}
			else
			{
				length = dy;
				height = dx;
				offsetx = 0;
				offsety = sy;
			}

			int lenght2 = length * 2;
			int height2 = height * 2;
			
			int x = x0;
			int y = y0;

			int error = length - height2;

			for (int step = 0; step < length; ++step)
			{
				if (error < 0)
				{
					error += lenght2 - height2;
					x += sx;
					y += sy;
				}
				else
				{
					error -= height2;
					x += offsetx;
					y += offsety;
				}

				if (handler.HandlePoint(x, y))
				{
					return;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Circle<T>(int x0, int y0, int radius, T handler) where T : struct, IBresenhamCircleHandler
		{
			int x = radius;
			int y = 0;
			int radiusError = -x;

			handler.HandlePoint(x0 + x, y0);
			handler.HandlePoint(x0 - x, y0);
			handler.HandlePoint(x0, y0 + x);
			handler.HandlePoint(x0, y0 - x);

			y++;

			while (y <= x)
			{
				handler.HandlePoint(x0 + x, y0 + y);
				handler.HandlePoint(x0 + x, y0 - y);
				handler.HandlePoint(x0 - x, y0 + y);
				handler.HandlePoint(x0 - x, y0 - y);
				handler.HandlePoint(x0 + y, y0 + x);
				handler.HandlePoint(x0 + y, y0 - x);
				handler.HandlePoint(x0 - y, y0 + x);
				handler.HandlePoint(x0 - y, y0 - x);

				if (radiusError < 0)
				{
					radiusError += 2 * y;
				}
				else
				{
					x--;

					handler.HandlePoint(x0 + x, y0 + y);
					handler.HandlePoint(x0 + x, y0 - y);
					handler.HandlePoint(x0 - x, y0 + y);
					handler.HandlePoint(x0 - x, y0 - y);
					handler.HandlePoint(x0 + y, y0 + x);
					handler.HandlePoint(x0 + y, y0 - x);
					handler.HandlePoint(x0 - y, y0 + x);
					handler.HandlePoint(x0 - y, y0 - x);

					radiusError += 2 * (y - x) + 1;
				}

				y++;
			}
		}
	}
}
