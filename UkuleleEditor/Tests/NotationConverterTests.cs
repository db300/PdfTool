namespace UkuleleEditor.Tests
{
    /// <summary>
    /// 简谱转换器测试
    /// </summary>
    internal class NotationConverterTests
    {
        /// <summary>
        /// 测试单个简谱的转换
        /// </summary>
        public static void TestSingleNotationConversion()
        {
            Console.WriteLine("=== 单个简谱转换测试 ===");
            
            // 测试简谱1-7的转换（C调）
            for (int notation = 1; notation <= 7; notation++)
            {
                var (stringNum, fret) = NotationConverter.Convert(notation, key: 0, octaveOffset: 0);
                Console.WriteLine($"简谱 {notation} -> 尤克里里第{stringNum}弦第{fret}品");
            }

            Console.WriteLine();
            
            // 测试不同调式（D调，向上移动2个半音）
            Console.WriteLine("=== D调转换测试 ===");
            for (int notation = 1; notation <= 7; notation++)
            {
                var (stringNum, fret) = NotationConverter.Convert(notation, key: 2, octaveOffset: 0);
                Console.WriteLine($"简谱 {notation} (D调) -> 尤克里里第{stringNum}弦第{fret}品");
            }

            Console.WriteLine();

            // 测试高八度
            Console.WriteLine("=== 高八度转换测试 ===");
            for (int notation = 1; notation <= 7; notation++)
            {
                var (stringNum, fret) = NotationConverter.Convert(notation, key: 0, octaveOffset: 1);
                Console.WriteLine($"简谱 {notation} (高八度) -> 尤克里里第{stringNum}弦第{fret}品");
            }

            Console.WriteLine();

            // 测试休止符
            Console.WriteLine("=== 休止符测试 ===");
            var (s0, f0) = NotationConverter.Convert(0);
            Console.WriteLine($"简谱 0 (休止符) -> 第{s0}弦第{f0}品");
        }

        /// <summary>
        /// 测试整行转换
        /// </summary>
        public static void TestLineConversion()
        {
            Console.WriteLine("\n=== 整行转换测试 ===");
            
            var line = new SheetMusicLine
            {
                Items = new List<SheetMusicItem>
                {
                    new SheetMusicItem { Notation = 1, Lyric = "我" },
                    new SheetMusicItem { Notation = 2, Lyric = "爱" },
                    new SheetMusicItem { Notation = 3, Lyric = "你" },
                    new SheetMusicItem { Notation = 5, Lyric = "啦" },
                    new SheetMusicItem { Notation = 0, Lyric = "" }, // 休止符
                    new SheetMusicItem { Notation = 1, Lyric = "哈" }
                }
            };

            NotationConverter.ConvertLine(line);

            foreach (var (index, item) in line.Items.Select((x, i) => (i, x)))
            {
                if (item.Notation > 0)
                {
                    Console.WriteLine($"项 {index}: 简谱{item.Notation} -> 第{item.String}弦第{item.Fret}品 ({item.Lyric})");
                }
                else
                {
                    Console.WriteLine($"项 {index}: 休止符");
                }
            }
        }
    }
}
