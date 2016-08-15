using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ChurSkins.DrawTools
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>
	public class DrawEllipse : DrawTools.DrawRectangle
	{
		public DrawEllipse()
		{
            SetRectangle(0, 0, 1, 1);
            Initialize();
		}

        public DrawEllipse(int x, int y, int width, int height)
        {
            Rectangle = new Rectangle(x, y, width, height);
            Initialize();
        }

        public override void Draw(Graphics g,DashStyle ds)
        {
            Pen pen = new Pen(Color, PenWidth);
            pen.DashStyle = ds;
            g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(Rectangle));

            pen.Dispose();
        }


	}
}
