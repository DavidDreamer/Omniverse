using System.Runtime.CompilerServices;
using UnityEngine;

namespace Dreambox.Math
{
	public static class Bresenham
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Line<T>(int x0, int y0, int x1, int y1, T handler) where T: struct, IBresenhamLineHandler
		{
			if (handler.HandlePoint(x0, y0))
			{
				return;
			}

			int dx = x1 - x0;
			int dy = y1 - y0;

			int incx = System.Math.Sign(dx);
			int incy = System.Math.Sign(dy);

			dx = Mathf.Abs(dx);
			dy = Mathf.Abs(dy);

			int pdx, pdy;
			int es, el;

			if (dx > dy)
			{
				pdx = incx;
				pdy = 0;
				es = dy;
				el = dx;
			}
			else
			{
				pdx = 0;
				pdy = incy;
				es = dx;
				el = dy;
			}

			int x = x0;
			int y = y0;

			int error = el / 2;

			for (int t = 0; t < el; ++t)
			{
				error -= es;
				if (error < 0)
				{
					error += el;
					x += incx;
					y += incy;
				}
				else
				{
					x += pdx;
					y += pdy;
				}

				if (handler.HandlePoint(x, y))
				{
					return;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Circle<T>(int x0, int y0, int radius, T handler) where T: struct, IBresenhamCircleHandler
		{
			int x = radius;
			int y = 0;
			int radiusError = 1 - x;

			while (x >= y)
			{
				handler.HandlePoint(x0 + x, y0 + y);
				handler.HandlePoint(x0 + x, y0 - y);
				handler.HandlePoint(x0 - x, y0 + y);
				handler.HandlePoint(x0 - x, y0 - y);
				handler.HandlePoint(x0 + y, y0 + x);
				handler.HandlePoint(x0 + y, y0 - x);
				handler.HandlePoint(x0 - y, y0 + x);
				handler.HandlePoint(x0 - y, y0 - x);

				if (true)
				{
					handler.HandlePoint(x0 + x + 1, y0 + y);
					handler.HandlePoint(x0 + x + 1, y0 - y);
					handler.HandlePoint(x0 - x + 1, y0 + y);
					handler.HandlePoint(x0 - x + 1, y0 - y);
					handler.HandlePoint(x0 + y + 1, y0 + x);
					handler.HandlePoint(x0 + y + 1, y0 - x);
					handler.HandlePoint(x0 - y + 1, y0 + x);
					handler.HandlePoint(x0 - y + 1, y0 - x);
				}

				y++;

				if (radiusError < 0)
				{
					radiusError += 2 * y + 1;
				}
				else
				{
					x--;
					radiusError += 2 * (y - x + 1);
				}
			}
		}
	}
}
