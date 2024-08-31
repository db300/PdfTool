using OpenCvSharp;
using System.Drawing;

namespace ImageTool.Helpers
{
    internal static class SketchHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputImagePath"></param>
        /// <param name="outputImagePath"></param>
        /// <param name="blurKernelSize">1.高斯模糊的核大小：Cv2.GaussianBlur 方法中的 new OpenCvSharp.Size(21, 21) 参数决定了模糊的程度。你可以调整这个值来改变模糊的强度。</param>
        /// <param name="blurSigma">2.高斯模糊的标准差：Cv2.GaussianBlur 方法中的 0 参数是高斯模糊的标准差。你可以调整这个值来改变模糊的效果。</param>
        /// <param name="sketchScale">3.素描图像的比例：Cv2.Divide 方法中的 scale: 256.0 参数决定了灰度图像和反转模糊图像之间的比例。你可以调整这个值来改变素描的对比度。</param>
        internal static void ConvertToSketch(string inputImagePath, string outputImagePath, int blurKernelSize = 21, double blurSigma = 0, double sketchScale = 256.0)
        {
            // 读取图像
            var src = Cv2.ImRead(inputImagePath, ImreadModes.Color);
            // 转换为灰度图像
            var gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            // 反转颜色
            var invertedGray = new Mat();
            Cv2.BitwiseNot(gray, invertedGray);
            // 高斯模糊
            var blurred = new Mat();
            if (blurKernelSize % 2 == 0) blurKernelSize++;
            Cv2.GaussianBlur(invertedGray, blurred, new OpenCvSharp.Size(blurKernelSize, blurKernelSize), blurSigma);
            // 反转模糊图像
            var invertedBlurred = new Mat();
            Cv2.BitwiseNot(blurred, invertedBlurred);
            // 创建素描图像
            var sketch = new Mat();
            Cv2.Divide(gray, invertedBlurred, sketch, scale: sketchScale);
            // 保存素描图像
            Cv2.ImWrite(outputImagePath, sketch);
        }

        internal static Bitmap ConvertToSketch(string inputImagePath, int blurKernelSize = 21, double blurSigma = 0, double sketchScale = 256.0)
        {
            // 读取图像
            var src = Cv2.ImRead(inputImagePath, ImreadModes.Color);
            // 转换为灰度图像
            var gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            // 反转颜色
            var invertedGray = new Mat();
            Cv2.BitwiseNot(gray, invertedGray);
            // 高斯模糊
            var blurred = new Mat();
            if (blurKernelSize % 2 == 0) blurKernelSize++;
            Cv2.GaussianBlur(invertedGray, blurred, new OpenCvSharp.Size(blurKernelSize, blurKernelSize), blurSigma);
            // 反转模糊图像
            var invertedBlurred = new Mat();
            Cv2.BitwiseNot(blurred, invertedBlurred);
            // 创建素描图像
            var sketch = new Mat();
            Cv2.Divide(gray, invertedBlurred, sketch, scale: sketchScale);
            // 将素描图像转换为Bitmap对象
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(sketch);
        }
    }
}
