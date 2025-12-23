namespace UkuleleEditor
{
    /// <summary>
    /// 简谱转换器 - 将简谱转换为尤克里里指法
    /// </summary>
    internal class NotationConverter
    {
        /// <summary>
        /// 尤克里里弦的开放弦音（标准调弦：G4、C4、E4、A4）
        /// 第1弦(A4=9)、第2弦(E4=4)、第3弦(C4=0)、第4弦(G4=7)
        /// 使用MIDI音高的十二平均律半音序号（C=0, ..., B=11）表示，仅使用音高类不含具体八度。
        /// </summary>
        private static readonly int[] OpenStringNotes = { 9, 4, 0, 7 }; // 从第1弦到第4弦

        /// <summary>
        /// C调简谱对应的MIDI音符（C=0, D=2, E=4, F=5, G=7, A=9, B=11）
        /// 简谱：1=C, 2=D, 3=E, 4=F, 5=G, 6=A, 7=B, 0=休止符
        /// </summary>
        private static readonly int[] NotationToMidiInC = { -1, 0, 2, 4, 5, 7, 9, 11 }; // 索引0未使用

        /// <summary>
        /// 最大可播放的品位数（根据常见尤克里里设为15，可覆盖到第14品）
        /// </summary>
        private const int MaxFret = 15;

        /// <summary>
        /// 将简谱转换为尤克里里指法
        /// </summary>
        /// <param name="notation">简谱音符（1-7，0表示休止符）</param>
        /// <param name="key">调式（C调=0, D调=2等，通常为C调=0）</param>
        /// <param name="octaveOffset">八度偏移（0表示标准八度，1表示高一个八度，-1表示低一个八度）</param>
        /// <returns>转换后的 (弦, 品) 元组，如果无法转换则返回 (0, -1)</returns>
        public static (int String, int Fret) Convert(int notation, int key = 0, int octaveOffset = 0)
        {
            // 处理休止符
            if (notation <= 0 || notation > 7)
            {
                return (0, -1);
            }

            // 转换为MIDI音符（基于C调）
            int midiNote = NotationToMidiInC[notation] + key + (octaveOffset * 12);

            // 在四根弦上查找最佳指位
            return FindBestFingering(midiNote);
        }

        /// <summary>
        /// 在四根弦上查找最佳指位（优先选择较低品位）
        /// </summary>
        private static (int String, int Fret) FindBestFingering(int targetMidiNote)
        {
            int bestString = 0;
            int bestFret = -1;
            int bestDistance = int.MaxValue;

            // 遍历四根弦，从第1弦到第4弦
            for (int stringIndex = 0; stringIndex < 4; stringIndex++)
            {
                int stringNumber = stringIndex + 1; // 弦号从1到4
                int openNote = OpenStringNotes[stringIndex]; // 该弦的开放弦音

                // 计算所需品位
                int fret = targetMidiNote - openNote;

                // 检查是否在有效范围内
                if (fret >= 0 && fret <= MaxFret)
                {
                    // 优先选择品位较低的（更容易演奏）
                    if (fret < bestDistance)
                    {
                        bestDistance = fret;
                        bestString = stringNumber;
                        bestFret = fret;
                    }
                }
            }

            return bestFret >= 0 ? (bestString, bestFret) : (0, -1);
        }

        /// <summary>
        /// 批量转换一行的简谱
        /// </summary>
        public static void ConvertLine(SheetMusicLine line, int key = 0, int octaveOffset = 0)
        {
            if (line?.Items == null)
                return;

            foreach (var item in line.Items)
            {
                if (item.Notation > 0)
                {
                    var (stringNum, fret) = Convert(item.Notation, key, octaveOffset);
                    if (fret >= 0)
                    {
                        item.String = stringNum;
                        item.Fret = fret;
                    }
                }
            }
        }

        /// <summary>
        /// 批量转换整个曲谱
        /// </summary>
        public static void ConvertSheetMusic(SheetMusic sheetMusic, int key = 0, int octaveOffset = 0)
        {
            if (sheetMusic?.Lines == null)
                return;

            foreach (var line in sheetMusic.Lines)
            {
                ConvertLine(line, key, octaveOffset);
            }
        }
    }
}
