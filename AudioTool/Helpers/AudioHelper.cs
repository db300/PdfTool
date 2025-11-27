using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AudioTool.Helpers
{
    /// <summary>
    /// 音频工具辅助方法
    /// 提供使用 FFmpeg 获取 mp3 时长的无额外依赖实现。
    /// </summary>
    internal static class AudioHelper
    {
        /// <summary>
        /// 使用本地 ffmpeg.exe 解析音频信息并尝试获取时长（同步实现）。
        /// 默认查找路径：程序集目录下的 "ffmpeg\ffmpeg.exe"。
        /// 返回值：成功返回 true 并通过 out 返回时长；失败返回 false。
        /// </summary>
        public static bool TryGetMp3DurationByFfmpeg(string mp3Path, out TimeSpan duration, string ffmpegExePath = null, int timeoutMilliseconds = 5000)
        {
            duration = TimeSpan.Zero;

            if (string.IsNullOrWhiteSpace(mp3Path) || !File.Exists(mp3Path))
            {
                return false;
            }

            // 默认 ffmpeg 路径（与 Joiner.cs 保持一致的查找策略）
            if (string.IsNullOrWhiteSpace(ffmpegExePath))
            {
                var asmDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                ffmpegExePath = Path.Combine(asmDir ?? "", "ffmpeg", "ffmpeg.exe");
            }

            if (!File.Exists(ffmpegExePath))
            {
                return false;
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = ffmpegExePath,
                    Arguments = $"-i \"{mp3Path}\"",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                };

                using (var proc = new Process { StartInfo = startInfo })
                {
                    proc.Start();

                    // ffmpeg 将媒体信息写到 stderr
                    var stderr = proc.StandardError.ReadToEnd();

                    // 等待进程退出（超时后尝试杀掉）
                    if (!proc.WaitForExit(timeoutMilliseconds))
                    {
                        try { proc.Kill(); } catch { }
                    }

                    // 寻找 Duration: 00:01:23.45 格式
                    var m = Regex.Match(stderr, @"Duration:\s*(\d{2}:\d{2}:\d{2}(?:\.\d+)?)");
                    if (!m.Success) return false;

                    var durStr = m.Groups[1].Value; // e.g. "00:01:23.45"

                    // TimeSpan.Parse 支持 "hh:mm:ss" 和带小数点的秒部分
                    if (TimeSpan.TryParse(durStr, out duration))
                    {
                        return true;
                    }

                    // 备用解析（手动拆分）
                    var parts = durStr.Split(new[] { ':' }, 3);
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out var hh) &&
                        int.TryParse(parts[1], out var mm) &&
                        double.TryParse(parts[2], out var secD))
                    {
                        duration = TimeSpan.FromSeconds(hh * 3600 + mm * 60 + secD);
                        return true;
                    }

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 异步版：在线程池上执行同步方法以避免阻塞 UI。
        /// 返回 Task{(bool ok, TimeSpan duration)}。
        /// </summary>
        public static Task<(bool ok, TimeSpan duration)> TryGetMp3DurationByFfmpegAsync(string mp3Path, string ffmpegExePath = null, int timeoutMilliseconds = 5000)
        {
            return Task.Run(() =>
            {
                var ok = TryGetMp3DurationByFfmpeg(mp3Path, out var dur, ffmpegExePath, timeoutMilliseconds);
                return (ok, dur);
            });
        }
    }
}