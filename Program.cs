using System;
using System.Windows.Forms;
using OpenCvSharp;

namespace OpenCV
{
    internal static class Program
    {
        static void Main()
        {

            /*도형그리기();*/
            contour();
        }
        [STAThread]
        static void 도형그리기()
        {
            Mat draw = new Mat(new Size(1000, 1000), MatType.CV_8UC3, Scalar.Black);

            Cv2.Circle(draw, new Point(500, 500), 350, Scalar.Yellow, -1, LineTypes.AntiAlias);

            Cv2.Rectangle(draw, new Point(185, 30), new Point(235, 80), Scalar.Navy, -1);

            Cv2.ImShow("Drawing", draw);

            int x = 80;
            while (x < 900)
            {
                Cv2.Rectangle(draw, new Point(185, x - 50), new Point(235, x++), Scalar.Black, -1);
                Cv2.Circle(draw, new Point(500, 500), 350, Scalar.Yellow, -1, LineTypes.AntiAlias);
                x += 10;
                Cv2.Rectangle(draw, new Point(185, x - 50), new Point(235, x), Scalar.Navy, -1);
                Cv2.ImShow("Drawing", draw);
                if (Cv2.WaitKey(6) == 'q') break;
                //  Cv2.DestroyAllWindows();
            }
        }

        static Mat ImageDraw()
        {
            return new Mat("./Resources/all_mask_key.png");
        }
        static void contour()
        {
            // 오버로드 방법.1
            /*
            Mat hierarchy1 = new Mat();
            Cv2.FindContours(bin, out Mat[] contour1, hierarchy1,
                RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                for (int i = 0; i < contour1.Length; i++) 
                {
                    Cv2.DrawContours(src1, contour1, i, Scalar.Red, 3, LineTypes.AntiAlias);
                }
                Cv2.ImShow("1", src1);
            */

            Mat src = ImageDraw();
            Mat src1 = new Mat(), src2 = new Mat();
            src.CopyTo(src1);
            src.CopyTo(src2);
            //Cv2.ImShow("src", src);

            Mat bin = new Mat();
            Cv2.CvtColor(src, bin, ColorConversionCodes.BGR2GRAY);
            // cv2.threshold(src, thresh, maxval, type) → retval, dst
            //이진화란 영상을 흑/백으로 분류하여 처리하는 것을 말합니다. 
            Cv2.Threshold(bin, bin, 200, 255, ThresholdTypes.Binary);

            // 오버로드 방법.2
            Cv2.FindContours(bin, out Point[][] contour2, out HierarchyIndex[] hierarchy2,
                RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            double crossX = 0.0, crossY = 0.0;
            Point[] crossBoxPoint = new Point[contour2.Length];
            for (int i = 0; i < contour2.Length; i++)
            {
                Cv2.DrawContours(src2, contour2, i, Scalar.Yellow, 3, LineTypes.AntiAlias);
                //Console.WriteLine(Cv2.BoundingRect(contour2[i]).X + " " + Cv2.BoundingRect(contour2[i]).Y + " " + Cv2.BoundingRect(contour2[i]).Height + " " + Cv2.BoundingRect(contour2[i]).Width);

                // 십자가 박스 중심점
                Moments mmt = Cv2.Moments(contour2[i]);
                double cx = mmt.M10 / mmt.M00,
                       cy = mmt.M01 / mmt.M00;
                Cv2.Circle(src2, new Point(cx, cy), 3, Scalar.Red, -1, LineTypes.AntiAlias);

                crossBoxPoint[i] = new Point(cx, cy);

                // 십자가 중심점
                if (i >= 1 && i <= 4)
                {
                    crossX += cx;
                    crossY += cy;
                }
            }
            Cv2.Circle(src2, new Point(crossX / 4, crossY / 4), 3, Scalar.YellowGreen, -1, LineTypes.AntiAlias);
            Console.WriteLine("crossX : {0}, crossY : {1}", crossX, crossY);
            // 십자가 x,y축 그리기
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

            // crossLinePoint의 좌표값을 가지고 라인 draw
            Mat draw = ImageDraw();
            Cv2.Line(draw, new Point(662, 260), new Point(925, 260), Scalar.Orange, 10, LineTypes.AntiAlias);
            Cv2.Line(draw, new Point(793, 129), new Point(793, 392), Scalar.Orange, 10, LineTypes.AntiAlias);
            Cv2.ImShow("Drawing", draw);


            // 
            Cv2.ImShow("2", src2);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        static void Moments()
        {

        }

    }
}