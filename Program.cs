using System;
using System.Windows.Forms;
using OpenCvSharp;

namespace OpenCV
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void 기초1()
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

        static void contour()
        {
            Mat src = new Mat("./Resources/all_mask_key.png");
            Mat src1 = new Mat(), src2 = new Mat();
            src.CopyTo(src1);
            src.CopyTo(src2);
            Cv2.ImShow("src", src);

            Mat bin = new Mat();
            Cv2.CvtColor(src, bin, ColorConversionCodes.BGR2GRAY);
            // cv2.threshold(src, thresh, maxval, type) → retval, dst
            //이진화란 영상을 흑/백으로 분류하여 처리하는 것을 말합니다. 
            Cv2.Threshold(bin, bin, 200, 255, ThresholdTypes.Binary);

            Mat hierarchy1 = new Mat();
            Cv2.FindContours(bin, out Mat[] contour1, hierarchy1,
                RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

        /*    for (int i = 0; i < contour1.Length; i++) 
            {
                Cv2.DrawContours(src1, contour1, i, Scalar.Red, 3, LineTypes.AntiAlias);
            }
            Cv2.ImShow("1", src1);
*/
            Cv2.FindContours(bin, out Point[][] contour2, out HierarchyIndex[] hierarchy2,
                RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);


            for (int i = 0; i < contour2.Length; i++)
            {
                Cv2.DrawContours(src2, contour2, i, Scalar.Yellow, 3, LineTypes.AntiAlias);
                Console.WriteLine(Cv2.BoundingRect(contour1[i]));

            }
            Cv2.ImShow("2", src2);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

        }
        static void Main()
        {

            /*기초1();*/
            contour();



        }
    }
}