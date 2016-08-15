using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms
{
    public class WaterEffect
    {
        private System.Timers.Timer effectTimer;
        private System.Timers.Timer autoEffectTimer;
        //private System.ComponentModel.IContainer components;

        private Bitmap _bmp;
        private short[,,] _waves;
        private int _waveWidth;
        private int _waveHeight;
        private int _activeBuffer = 0;
        private bool _weHaveWaves;
        private int _bmpHeight, _bmpWidth;
        private byte[] _bmpBytes;
        private BitmapData _bmpBitmapData;
        private int _scale = 1;
        private Control control;

        public WaterEffect(Control control)
        {
            //if (control.Created)
            //{
            this.control = control;
            this.ImageBitmap = new Bitmap(control.BackgroundImage);
            this.control.Paint += WaterEffectControl_Paint;
            this.control.MouseMove += WaterEffectControl_MouseMove;
            effectTimer = new Timers.Timer();
            autoEffectTimer = new Timers.Timer();

            effectTimer.Elapsed += effectTimer_Tick;
            effectTimer.Enabled = true;
            effectTimer.Interval = 10;
            effectTimer.Start();

            autoEffectTimer.Elapsed += autoEffectTimer_Tick;
            autoEffectTimer.Enabled = true;
            autoEffectTimer.Interval = 500;
            autoEffectTimer.Start();
            // }
        }

        public void WaterEffectControl_Paint(object sender, PaintEventArgs e)
        {
            if (_bmp == null) return;
            using (Bitmap tmp = (Bitmap)_bmp.Clone())
            {

                int xOffset, yOffset;
                byte alpha;

                if (_weHaveWaves)
                {
                    BitmapData tmpData = tmp.LockBits(new Rectangle(0, 0, _bmpWidth, _bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    byte[] tmpBytes = new Byte[_bmpWidth * _bmpHeight * 4];

                    Marshal.Copy(tmpData.Scan0, tmpBytes, 0, _bmpWidth * _bmpHeight * 4);

                    for (int x = 1; x < _bmpWidth - 1; x++)
                    {
                        for (int y = 1; y < _bmpHeight - 1; y++)
                        {
                            int waveX = (int)x >> _scale;
                            int waveY = (int)y >> _scale;

                            //check bounds
                            if (waveX <= 0) waveX = 1;
                            if (waveY <= 0) waveY = 1;
                            if (waveX >= _waveWidth - 1) waveX = _waveWidth - 2;
                            if (waveY >= _waveHeight - 1) waveY = _waveHeight - 2;

                            //this gives us the effect of water breaking the light
                            xOffset = (_waves[waveX - 1, waveY, _activeBuffer] - _waves[waveX + 1, waveY, _activeBuffer]) >> 3;
                            yOffset = (_waves[waveX, waveY - 1, _activeBuffer] - _waves[waveX, waveY + 1, _activeBuffer]) >> 3;

                            if ((xOffset != 0) || (yOffset != 0))
                            {
                                //check bounds
                                if (x + xOffset >= _bmpWidth - 1) xOffset = _bmpWidth - x - 1;
                                if (y + yOffset >= _bmpHeight - 1) yOffset = _bmpHeight - y - 1;
                                if (x + xOffset < 0) xOffset = -x;
                                if (y + yOffset < 0) yOffset = -y;

                                //generate alpha
                                alpha = (byte)(200 - xOffset);
                                if (alpha < 0) alpha = 0;
                                if (alpha > 255) alpha = 254;

                                //set colors
                                tmpBytes[4 * (x + y * _bmpWidth)] = _bmpBytes[4 * (x + xOffset + (y + yOffset) * _bmpWidth)];
                                tmpBytes[4 * (x + y * _bmpWidth) + 1] = _bmpBytes[4 * (x + xOffset + (y + yOffset) * _bmpWidth) + 1];
                                tmpBytes[4 * (x + y * _bmpWidth) + 2] = _bmpBytes[4 * (x + xOffset + (y + yOffset) * _bmpWidth) + 2];
                                tmpBytes[4 * (x + y * _bmpWidth) + 3] = alpha;

                            }

                        }
                    }

                    //copy data back
                    Marshal.Copy(tmpBytes, 0, tmpData.Scan0, _bmpWidth * _bmpHeight * 4);
                    tmp.UnlockBits(tmpData);

                }

                e.Graphics.DrawImage(tmp, 0, 0, control.ClientRectangle.Width, control.ClientRectangle.Height);

            }

        }

        private void ProcessWaves()
        {
            int newBuffer = (_activeBuffer == 0) ? 1 : 0;
            bool wavesFound = false;

            for (int x = 1; x < _waveWidth - 1; x++)
            {
                for (int y = 1; y < _waveHeight - 1; y++)
                {
                    _waves[x, y, newBuffer] = (short)(
                                            ((_waves[x - 1, y - 1, _activeBuffer] +
                                            _waves[x, y - 1, _activeBuffer] +
                                            _waves[x + 1, y - 1, _activeBuffer] +
                                            _waves[x - 1, y, _activeBuffer] +
                                            _waves[x + 1, y, _activeBuffer] +
                                            _waves[x - 1, y + 1, _activeBuffer] +
                                            _waves[x, y + 1, _activeBuffer] +
                                            _waves[x + 1, y + 1, _activeBuffer]) >> 2) - _waves[x, y, newBuffer]);

                    //damping
                    if (_waves[x, y, newBuffer] != 0)
                    {
                        _waves[x, y, newBuffer] -= (short)(_waves[x, y, newBuffer] >> 4);
                        wavesFound = true;
                    }


                }
            }

            _weHaveWaves = wavesFound;
            _activeBuffer = newBuffer;

        }

        private void PutDrop(int x, int y, short height)
        {
            _weHaveWaves = true;
            int radius = 20;
            double dist;

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (((x + i >= 0) && (x + i < _waveWidth - 1)) && ((y + j >= 0) && (y + j < _waveHeight - 1)))
                    {
                        dist = Math.Sqrt(i * i + j * j);
                        if (dist < radius)
                            _waves[x + i, y + j, _activeBuffer] = (short)(Math.Cos(dist * Math.PI / radius) * height);
                    }
                }
            }
        }

        private void WaterEffectControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int realX = (int)((e.X / (double)control.ClientRectangle.Width) * _waveWidth);
                int realY = (int)((e.Y / (double)control.ClientRectangle.Height) * _waveHeight);
                PutDrop(realX, realY, 200);
            }
        }
        private Bitmap ImageBitmap
        {
            get { return _bmp; }
            set
            {
                _bmp = value;
                _bmpHeight = _bmp.Height;
                _bmpWidth = _bmp.Width;

                _waveWidth = _bmpWidth >> _scale;
                _waveHeight = _bmpHeight >> _scale;
                _waves = new Int16[_waveWidth, _waveHeight, 2];

                _bmpBytes = new Byte[_bmpWidth * _bmpHeight * 4];
                _bmpBitmapData = _bmp.LockBits(new Rectangle(0, 0, _bmpWidth, _bmpHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                Marshal.Copy(_bmpBitmapData.Scan0, _bmpBytes, 0, _bmpWidth * _bmpHeight * 4);
            }
        }
        public int Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        private void effectTimer_Tick(object sender, System.EventArgs e)
        {

            if (_weHaveWaves)
            {
                control.Invalidate();
                ProcessWaves();
            }
        }
        private void autoEffectTimer_Tick(object sender, EventArgs e)
        {
            Random rd = new Random();
            for (int i = 0; i < 10; i++)         //随机产生五个波源
                PutDrop(rd.Next(control.Width - 1), rd.Next(control.Height - 1), (short)rd.Next(100, 200));
        }
    }
}
