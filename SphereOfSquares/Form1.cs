using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SphereOfSquares {
    public partial class Form1 : Form {
        private const int NumberOfSquaresInTotal = 400;
        private const int NumberOfSquaresInACircle = 20;
        private Square[] squares = new Square[NumberOfSquaresInTotal];
        private Queue<int> queue = new Queue<int>(NumberOfSquaresInTotal);
        private ManualResetEvent queueEvent = new ManualResetEvent(false);
        private ManualResetEvent doneDrawing = new ManualResetEvent(false);
        private double radius;
        Pen myPen = new Pen(Color.Blue, 2);

        public Form1() {
            InitializeComponent();
            this.Size = new Size(600, 700);
            InitializeSphere();
        }

        private void InitializeSphere() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(QueueEventHandler));
            ThreadPool.QueueUserWorkItem(new WaitCallback(CreateAllSquares));
        }

        private void CreateAllSquares(object state) {
            // First calculate the radius of the sphere
            radius = (Math.Min(Size.Height, Size.Width) - 20) / 2;
            double centerAngle = 360 / NumberOfSquaresInACircle;
            int indexor = 0;
            for (int i = 0; i < NumberOfSquaresInACircle; i++) {
                for (int j = 0; j < NumberOfSquaresInACircle; j++) {
                    squares[indexor] =
                    new Square(
                        radius,
                        0,
                        centerAngle,
                        new Square.XYAngle(i * centerAngle, j * centerAngle),
                        queue,
                        queueEvent,
                        indexor);
                    indexor++;
                }
            }
            //while (indexor < NumberOfSquaresInACircle) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(indexor * centerAngle, 0),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (2 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(0, (indexor - NumberOfSquaresInACircle) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (3 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(90, (indexor - (2 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (4 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(centerAngle, (indexor - (3 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (5 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(-centerAngle, (indexor - (4 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (6 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(2 * centerAngle, (indexor - (5 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (7 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(-2 * centerAngle, (indexor - (6 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (8 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(3 * centerAngle, (indexor - (7 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (9 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(-3 * centerAngle, (indexor - (8 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (10 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(4 * centerAngle, (indexor - (9 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
            //while (indexor < (11 * NumberOfSquaresInACircle)) {
            //    squares[indexor] =
            //        new Square(
            //            radius,
            //            0,
            //            centerAngle,
            //            new Square.XYAngle(-4 * centerAngle, (indexor - (10 * NumberOfSquaresInACircle)) * centerAngle),
            //            queue,
            //            queueEvent,
            //            indexor);
            //    indexor++;
            //}
        }

        protected override void OnPaint(PaintEventArgs e) {
            //if (queue.Count > 0) {
            //    Pen myPen = new Pen(Color.Blue, 1);
            //    Graphics gfx = e.Graphics;
            //    do {
            //        int value = queue.Dequeue();
            //        PointF[] coords = squares[value].Coordinates;
            //        // Draw the coordinates
            //        gfx.DrawLine(myPen, coords[0], coords[1]);
            //        gfx.DrawLine(myPen, coords[1], coords[2]);
            //        gfx.DrawLine(myPen, coords[2], coords[3]);
            //        gfx.DrawLine(myPen, coords[3], coords[0]);
            //    } while (queue.Count > 0);
            //}
            
            Graphics gfx = e.Graphics;
            for (int i = 0; i < NumberOfSquaresInTotal; i++) {
                if (squares[i] != null && squares[i].IsVisible && squares[i].IsDirty) {
                    PointF[] coords = squares[i].Coordinates;
                    // Draw the coordinates
                    gfx.DrawPolygon(myPen, coords);
                    gfx.FillPolygon(squares[i].MyBrush, coords);
                    //gfx.DrawLine(myPen, coords[0], coords[1]);
                    //gfx.DrawLine(myPen, coords[1], coords[2]);
                    //gfx.DrawLine(myPen, coords[2], coords[3]);
                    //gfx.DrawLine(myPen, coords[3], coords[0]);
                    squares[i].IsDirty = false;
                }
            }
            doneDrawing.Set();
            base.OnPaint(e);
        }

        private void QueueEventHandler(object state) {
            while (true) {
                queueEvent.WaitOne();
                doneDrawing.Reset();
                this.Invalidate();
                doneDrawing.WaitOne();
                queueEvent.Reset();
            }
        }

        private Rectangle GetRectangle(PointF[] coords) {
            float xMin = Math.Min(coords[0].X, coords[3].X);
            float xMax = Math.Max(coords[1].X, coords[2].X);
            float yMin = Math.Min(coords[0].Y, coords[1].Y);
            float yMax = Math.Max(coords[2].Y, coords[3].Y);
            return new Rectangle((int)xMin, (int)yMin, (int)(xMax - xMin), (int)(yMax - yMin));

        }
        private bool mouseDown = false;
        private Point mouseDownPoint;
        
        protected override void OnMouseDown(MouseEventArgs e) {
            mouseDown = true;
            mouseDownPoint = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (mouseDown) {
                Point currentLocation = e.Location;
                Point mouseMovement = new Point(currentLocation.X - mouseDownPoint.X, currentLocation.Y - mouseDownPoint.Y);
                ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateAllSquares), mouseMovement);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            mouseDown = false;
            base.OnMouseUp(e);
        }

        private void UpdateAllSquares(object state) {
            Point mouseMovement = (Point)state;
            for (int i = 0; i < NumberOfSquaresInTotal; i++) {
                squares[i].XYAngleValue =
                    new Square.XYAngle(
                        squares[i].XYAngleValue.XAxis + (mouseMovement.X / 10),
                        squares[i].XYAngleValue.YAxis + (mouseMovement.Y / 10));
            }
            //this.Invalidate();
        }

    }
}
