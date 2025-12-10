@echo off
chcp 65001 >nul
echo ========================================
echo PdfConsoleApp 测试脚本
echo ========================================
echo.

echo 正在编译项目...
cd /d "%~dp0"
msbuild PdfConsoleApp.csproj /p:Configuration=Debug /t:Rebuild /v:q

if %ERRORLEVEL% NEQ 0 (
    echo 编译失败！
    pause
    exit /b 1
)

echo 编译成功！
echo.
echo 正在运行测试...
echo.

cd bin\Debug

echo 测试 1: 使用示例 JSON 文件生成 PDF
echo ----------------------------------------
PdfConsoleApp.exe ..\..\test_chinese.json

if exist output.pdf (
    echo ? 测试成功！生成了 output.pdf
    echo 文件位置: %cd%\output.pdf
    start output.pdf
) else (
    echo ? 测试失败！未找到 output.pdf
)

echo.
echo ========================================
echo 测试完成
echo ========================================
pause
