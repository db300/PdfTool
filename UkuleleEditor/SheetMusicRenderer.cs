using System.Text.Json;

namespace UkuleleEditor
{
    /// <summary>
    /// 乌克丽丽谱面渲染器
    /// </summary>
    internal class SheetMusicRenderer
    {
        private const int StringCount = 4; // 乌克丽丽4根弦
        private const int StringSpacing = 40; // 弦之间的间距（像素）
        private const int FretWidth = 50; // 每个品位单元宽度
        private const int Padding = 40; // 整体边距
        private const int LineGap = 60; // 不同行之间的间隙
        private const int LyricOffset = 12; // 从最下一根弦到歌词基线的偏移

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

            // 每一行宽度（按该行项数计算）
            var lineWidths = sheetMusic.Lines
                .Select(l => Math.Max(1, l.Items?.Count ?? 0) * FretWidth)
                .ToList();

            int contentWidth = lineWidths.Count > 0 ? lineWidths.Max() : FretWidth;
            int width = contentWidth + Padding * 2;

            int stringsAreaHeight = (StringCount - 1) * StringSpacing;
            int lineHeight = stringsAreaHeight + LyricOffset + 20; // 20 用于歌词行高度缓冲
            int height = sheetMusic.Lines.Count * lineHeight
                         + (sheetMusic.Lines.Count - 1) * LineGap
                         + Padding * 2;

            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                for (int lineIndex = 0; lineIndex < sheetMusic.Lines.Count; lineIndex++)
                {
                    var line = sheetMusic.Lines[lineIndex];
                    int thisLineWidth = lineWidths[lineIndex];
                    int xStart = Padding;
                    int xEnd = Padding + thisLineWidth;
                    int yTop = Padding + lineIndex * (lineHeight + LineGap);

                    // 绘制当前行的4根弦（仅限本行宽度）
                    DrawStrings(graphics, xStart, xEnd, yTop);

                    // 绘制当前行音符和统一歌词位置
                    DrawLineNotesAndLyrics(graphics, line, xStart, yTop, stringsAreaHeight);
                }
            }

            return bitmap;
        }

        /// <summary>
        /// 绘制指定范围的4根弦（从 xStart 到 xEnd）
        /// </summary>
        private static void DrawStrings(Graphics graphics, int xStart, int xEnd, int yTop)
        {
            using (var pen = new Pen(Color.LightGray, 2))
            {
                for (int i = 0; i < StringCount; i++)
                {
                    int y = yTop + i * StringSpacing;
                    graphics.DrawLine(pen, xStart, y, xEnd, y);
                }
            }
        }

        /// <summary>
        /// 绘制一行的品位数字和统一的歌词位置（歌词全部画在该行四线谱最下方）
        /// </summary>
        private static void DrawLineNotesAndLyrics(Graphics graphics, SheetMusicLine line, int xStart, int yTop, int stringsAreaHeight)
        {
            if (line?.Items == null || line.Items.Count == 0)
                return;

            // 统一歌词 Y（位于最下一根弦下方）
            int lyricY = yTop + stringsAreaHeight + LyricOffset;

            // 绘制每个项的品位和歌词（歌词在同一行的相同基线）
            for (int i = 0; i < line.Items.Count; i++)
            {
                var item = line.Items[i];
                int x = xStart + i * FretWidth + FretWidth / 2;
                int stringIndex = Math.Clamp(item.String - 1, 0, StringCount - 1);
                int y = yTop + stringIndex * StringSpacing;

                DrawFretNumber(graphics, x, y, item.Fret);

                if (!string.IsNullOrEmpty(item.Lyric))
                {
                    DrawLyricAtLineBase(graphics, x, lyricY, item.Lyric);
                }
            }
        }

        /// <summary>
        /// 绘制品位数字（居中于 x,y）
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
        /// 在行的统一歌词基线上绘制歌词（居中于 x）
        /// </summary>
        private static void DrawLyricAtLineBase(Graphics graphics, int x, int lyricY, string lyric)
        {
            using (var brush = new SolidBrush(Color.Red))
            using (var font = new Font("宋体", 10))
            {
                var size = graphics.MeasureString(lyric, font);
                graphics.DrawString(lyric, font, brush, x - size.Width / 2, lyricY);
            }
        }
    }
}