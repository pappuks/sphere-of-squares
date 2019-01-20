using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace SphereOfSquares {
    class Square {

        public class XYAngle {
            private double xAxisAngle;
            private double yAxisAngle;

            public XYAngle(double xAxis, double yAxis) {
                xAxisAngle = xAxis;
                yAxisAngle = yAxis;
            }

            public double XAxis {
                get {
                    return xAxisAngle;
                }
                set {
                    xAxisAngle = value;
                }
            }

            public double YAxis {
                get {
                    return yAxisAngle;
                }
                set {
                    yAxisAngle = value;
                }
            }

            public override bool Equals(object obj) {
                XYAngle equalTo = obj as XYAngle;
                bool retVal = false;
                if (equalTo != null) {
                    retVal = (equalTo.XAxis == xAxisAngle) && (equalTo.YAxis == yAxisAngle);
                }
 	            return retVal;
            }

            public override int GetHashCode() {
 	             return base.GetHashCode();
            }
        }
        
        private double radius;
        private double sideLength;
        private double centerAngle;
        private XYAngle xyAngle;
        private static Brush[] myBrushes = new Brush[] {
            Brushes.Violet,
            Brushes.Indigo,
            Brushes.Brown,
            Brushes.Aquamarine,
            Brushes.Black, 
            Brushes.Green,
            Brushes.Yellow,
            Brushes.Blue,
            Brushes.Orange,
            Brushes.Beige            
            };
        
        /// <summary>
        /// The way coordinates are stored
        /// (1)-------------------(2)
        ///  |                     |
        ///  |                     |
        ///  |                     |
        ///  |                     |
        /// (4)-------------------(3)
        /// </summary>
        private PointF[] coordinates = new PointF[4];

        private ManualResetEvent queueEvent;
        private Queue<int> queue;
        private int indexInArray;
        private bool isVisible;
        private Brush myBrush;
        private bool isDirty;

        public Square(
            double radius, 
            double sideLength, 
            double centerAngle, 
            XYAngle xyAngle, 
            Queue<int> queue, 
            ManualResetEvent queueEvent,
            int indexInArray
        ) {
            this.radius = radius;
            this.sideLength = sideLength;
            this.centerAngle = centerAngle;
            this.xyAngle = xyAngle;
            this.queue = queue;
            this.queueEvent = queueEvent;
            this.indexInArray = indexInArray;
            Random rnd = new Random(indexInArray);
            this.myBrush = myBrushes[rnd.Next(myBrushes.Length - 1)];
            isDirty = false;
            UpdateCoordinates();
        }

        public PointF[] Coordinates {
            get {
                return coordinates;
            }
        }

        public XYAngle XYAngleValue {
            get {
                return xyAngle;
            }
            set {
                if (!xyAngle.Equals(value)) {
                    xyAngle = null;
                    xyAngle = value;
                    // Fire angle change, so that we can recalculate coordinates
                    UpdateCoordinates();
                }
            }
        }

        public bool IsVisible {
            get {
                return isVisible;
            }
        }

        public Brush MyBrush {
            get {
                return myBrush;
            }
        }

        public bool IsDirty {
            get {
                return isDirty;
            }
            set {
                isDirty = value;
            }
        }

        private void UpdateCoordinates() {

            //double x1 =
            //    radius * 
            //    Math.Sin(
            //        ConvertToRadians(xyAngle.XAxis + (centerAngle / 2))
            //    );

            // Check if the square is visible
            isVisible = Math.Cos(ConvertToRadians(xyAngle.XAxis)) * Math.Cos(ConvertToRadians(xyAngle.YAxis)) >= 0;
            if (isVisible) {
                double x1 =
                    radius *
                    Math.Cos(
                        ConvertToRadians(xyAngle.YAxis + (centerAngle / 2))
                    ) *
                    Math.Sin(
                        ConvertToRadians(xyAngle.XAxis - (centerAngle / 2))
                    );
                double x2 =
                    radius *
                    Math.Cos(
                        ConvertToRadians(xyAngle.YAxis + (centerAngle / 2))
                    ) *
                    Math.Sin(
                        ConvertToRadians(xyAngle.XAxis + (centerAngle / 2))
                    );
                double x3 =
                    radius *
                    Math.Cos(
                        ConvertToRadians(xyAngle.YAxis - (centerAngle / 2))
                    ) *
                    Math.Sin(
                        ConvertToRadians(xyAngle.XAxis + (centerAngle / 2))
                    );
                double x4 =
                    radius *
                    Math.Cos(
                        ConvertToRadians(xyAngle.YAxis - (centerAngle / 2))
                    ) *
                    Math.Sin(
                        ConvertToRadians(xyAngle.XAxis - (centerAngle / 2))
                    );
                double y1 =
                    radius *
                    Math.Sin(
                        ConvertToRadians(xyAngle.YAxis + (centerAngle / 2))
                    );
                double y2 =
                    radius *
                    Math.Sin(
                        ConvertToRadians(xyAngle.YAxis - (centerAngle / 2))
                    );
                //double y3;
                //double y4;
                coordinates[0].X = (float)x1 + (float)radius;
                coordinates[0].Y = (float)y1 + (float)radius;
                coordinates[1].X = (float)x2 + (float)radius;
                coordinates[1].Y = (float)y1 + (float)radius;
                coordinates[2].X = (float)x3 + (float)radius;
                coordinates[2].Y = (float)y2 + (float)radius;
                coordinates[3].X = (float)x4 + (float)radius;
                coordinates[3].Y = (float)y2 + (float)radius;
                // Fire coordinates updates event
                //queue.Enqueue(indexInArray);
                queueEvent.Set();
                isDirty = true;
            }
        }

        private double ConvertToRadians(double degree) {
            return (degree * Math.PI * 2) / 360;
        }
    }
}
