using System;
using System.Windows.Forms;
using OpenCvSharp;

namespace OpenCV
{
    internal static class test
    {
        static Mat Contour(Point[][] contour,Mat src)
        {
            for (int i = 0; i < contour.Length; i++)
            {
                Cv2.DrawContours(src, contour, i, Scalar.Yellow, 3, LineTypes.AntiAlias);
                Console.WriteLine("사각형 X 축 : " + Cv2.BoundingRect(contour[i]).X + " / 사각형 Y 축 : " + Cv2.BoundingRect(contour[i]).Y
                    + " / 사각형 높이 : " + Cv2.BoundingRect(contour[i]).Height + " / 사각형 너비 " + Cv2.BoundingRect(contour[i]).Width);
            }
            return src;
        }
        static Mat DrawCenterPoint(Point[][] contour, Mat src)
        {
            double crossX = 0.0, crossY = 0.0;
            Point[] crossBoxPoint = new Point[contour.Length];
            for (int i = 1; i < contour.Length; i++)
            {
                Moments mmt = Cv2.Moments(contour[i]);
                double cx = mmt.M10 / mmt.M00,
                       cy = mmt.M01 / mmt.M00;
                Cv2.Circle(src, new Point(cx, cy), 3, Scalar.Red, -1, LineTypes.AntiAlias);

                crossBoxPoint[i] = new Point(cx, cy);

                // 십자가 중심점
                if (i >= 1 && i <= 4)
                {
                    crossX += cx;
                    crossY += cy;
                }
                
            }
            Cv2.Circle(src, new Point(crossX / 4, crossY / 4), 3, Scalar.YellowGreen, -1, LineTypes.AntiAlias);
            Console.WriteLine("crossX : {0}, crossY : {1}", crossX, crossY);

            return src;
        }
        static Mat DrawLine(Point[][] contour, Mat src)
        {
            double crossX = 0.0, crossY = 0.0;
            Point[] crossBoxPoint = new Point[contour.Length];
            Rect rect = Cv2.BoundingRect(contour[5]);

            for (int i = 1; i < contour.Length; i++)
            {
                Moments mmt = Cv2.Moments(contour[i]);
                double cx = mmt.M10 / mmt.M00,
                       cy = mmt.M01 / mmt.M00;
                crossBoxPoint[i] = new Point(cx, cy);


                // 십자가 중심점
                if (i >= 1 && i <= 4)
                {
                    crossX += cx;
                    crossY += cy;
                }

            }

            Point[] crossLinePoint = new Point[4];
            // 각 지점의 crossLinePoint 좌표 값 (6시를 기준으로 반시계 방향이 배열 순서)
            crossLinePoint[0] = crossBoxPoint[1] + crossBoxPoint[2];
            crossLinePoint[0] = new Point(crossLinePoint[0].X / 2, crossLinePoint[0].Y / 2);
            Console.WriteLine("crossLinePoint[0] : {0}", crossLinePoint[0]);

            crossLinePoint[1] = crossBoxPoint[1] + crossBoxPoint[3];
            crossLinePoint[1] = new Point(crossLinePoint[1].X / 2, crossLinePoint[1].Y / 2);
            Console.WriteLine("crossLinePoint[1] : {0}", crossLinePoint[1]);

            crossLinePoint[2] = crossBoxPoint[3] + crossBoxPoint[4];
            crossLinePoint[2] = new Point(crossLinePoint[2].X / 2, crossLinePoint[2].Y / 2);
            Console.WriteLine("crossLinePoint[2] : {0}", crossLinePoint[2]);

            crossLinePoint[3] = crossBoxPoint[2] + crossBoxPoint[4];
            crossLinePoint[3] = new Point(crossLinePoint[3].X / 2, crossLinePoint[3].Y / 2);
            Console.WriteLine("crossLinePoint[3] : {0}", crossLinePoint[3]);
             
            Cv2.Line(src, crossLinePoint[1], crossLinePoint[3], Scalar.Orange, 10, LineTypes.AntiAlias);
            Cv2.Line(src, crossLinePoint[0], crossLinePoint[2], Scalar.Orange, 10, LineTypes.AntiAlias);

            Cv2.Line(src, new Point(rect.X + rect.Width/2, rect.Y), new Point(rect.X + rect.Width / 2, rect.Y + rect.Height), Scalar.Orange, 10, LineTypes.AntiAlias);
            Cv2.Line(src, new Point(rect.X, rect.Y+ rect.Height/2), new Point(rect.X + rect.Width, rect.Y + rect.Height / 2), Scalar.Orange, 10, LineTypes.AntiAlias);
            return src;
        }
        static void Main()
        {
            Mat src = new Mat("./Resources/all_mask_key.png");
            Mat bin = new Mat();
            Cv2.CvtColor(src, bin, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(bin, bin, 200, 255, ThresholdTypes.Binary);

            Cv2.FindContours(bin, out Point[][] contour, out HierarchyIndex[] hierarchy,
             RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            src = Contour(contour, src);
            src = DrawLine(contour, src);
            src = DrawCenterPoint(contour, src);

            Cv2.ImShow("Paint",src );
         
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

    }

}