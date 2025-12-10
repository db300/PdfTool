using System.Text.Json;
using System.Text.Json.Serialization;

namespace UkuleleEditor
{
    /// <summary>
    /// 乌克丽丽谱面渲染器
    /// </summary>
    internal class SheetMusicRenderer
    {
        private const int StringCount = 4; // 乌克丽丽4根弦
        private const int StringSpacing = 40; // 弦之间的间距（像素）
        private const int FretWidth = 50; // 品位宽度
        private const int Padding = 40; // 边距

        /// <summary>
        /// 根据 JSON 文件绘制谱面图
        /// </summary>
        public static Bitmap RenderSheetMusic(string jsonFilePath)
        {
            // 读取并解析 JSON 文件
            var jsonContent = File.ReadAllText(jsonFilePath);
            var sheetMusic = JsonSerializer.Deserialize<SheetMusic>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (sheetMusic?.Lines == null || sheetMusic.Lines.Count == 0)
                throw new InvalidOperationException("谱面数据为空");

            // 计算画布尺寸
            int totalItems = sheetMusic.Lines.Sum(line => line.Items.Count);
            int width = totalItems * FretWidth + Padding * 2;
            int height = StringCount * StringSpacing + Padding * 2;

            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                DrawStrings(graphics, width, height);
                DrawNotes(graphics, sheetMusic);
            }

            return bitmap;
        }

        /// <summary>
        /// 绘制4根弦
        /// </summary>
        private static void DrawStrings(Graphics graphics, int width, int height)
        {
            using (var pen = new Pen(Color.Black, 2))
            {
                for (int i = 0; i < StringCount; i++)
                {
                    int y = Padding + i * StringSpacing;
                    graphics.DrawLine(pen, Padding, y, width - Padding, y);
                }
            }
        }

        /// <summary>
        /// 绘制音符和歌词
        /// </summary>
        private static void DrawNotes(Graphics graphics, SheetMusic sheetMusic)
        {
            int itemIndex = 0;

            foreach (var line in sheetMusic.Lines)
            {
                foreach (var item in line.Items)
                {
                    int x = Padding + itemIndex * FretWidth + FretWidth / 2;
                    int stringIndex = item.String - 1; // 弦编号从1开始，转换为0-based
                    int y = Padding + stringIndex * StringSpacing;

                    // 绘制品位数字
                    DrawFretNumber(graphics, x, y, item.Fret);

                    // 绘制歌词
                    DrawLyric(graphics, x, y, item.Lyric);

                    itemIndex++;
                }
            }
        }

        /// <summary>
        /// 绘制品位数字
        /// </summary>
        private static void DrawFretNumber(Graphics graphics, int x, int y, int fret)
        {
            using (var brush = new SolidBrush(Color.Black))
            using (var font = new Font("Arial", 12, FontStyle.Bold))
            {
                var fretText = fret.ToString();
                var size = graphics.MeasureString(fretText, font);
                graphics.DrawString(fretText, font, brush, x - size.Width / 2, y - size.Height / 2);
            }
        }

        /// <summary>
        /// 绘制歌词
        /// </summary>
        private static void DrawLyric(Graphics graphics, int x, int y, string? lyric)
        {
            if (string.IsNullOrEmpty(lyric))
                return;

            using (var brush = new SolidBrush(Color.Red))
            using (var font = new Font("宋体", 10))
            {
                var size = graphics.MeasureString(lyric, font);
                graphics.DrawString(lyric, font, brush, x - size.Width / 2, y + 25);
            }
        }
    }
}