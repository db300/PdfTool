# PdfConsoleApp 测试指南

## 快速测试

### 方法 1: 使用测试脚本（推荐）

1. 双击运行 `test.bat` 脚本
2. 脚本会自动编译项目并运行测试
3. 成功后会自动打开生成的 PDF 文件

### 方法 2: 手动测试

#### 步骤 1: 编译项目

在 Visual Studio 中：
- 右键点击 `PdfConsoleApp` 项目
- 选择"重新生成"

或使用命令行：
```cmd
cd D:\GitHub\MyTools\PdfConsoleApp
msbuild PdfConsoleApp.csproj /p:Configuration=Debug /t:Rebuild
```

#### 步骤 2: 运行测试

打开命令提示符，执行以下命令：

```cmd
cd D:\GitHub\MyTools\PdfConsoleApp\bin\Debug
PdfConsoleApp.exe ..\..\sample.json
```

#### 步骤 3: 验证结果

检查当前目录下是否生成了 `output.pdf` 文件

## 测试用例

项目中包含以下测试 JSON 文件：

### 1. sample.json - 标准测试
- 测试多行文本
- 测试空行
- 使用默认 A4 页面
- 使用宋体字体

```cmd
PdfConsoleApp.exe ..\..\sample.json
```

### 2. test_simple.json - 最简测试
- 仅包含必填参数
- 使用默认值

```cmd
PdfConsoleApp.exe ..\..\test_simple.json
```

### 3. test_multiline.json - 多行文本测试
- 测试换行符处理
- 测试不同字体（微软雅黑）
- 测试不同字号（14）

```cmd
PdfConsoleApp.exe ..\..\test_multiline.json
```

### 4. test_custom_page.json - 自定义页面测试
- 测试 Letter 纸张尺寸
- 测试英文字体（Arial）
- 测试自定义边距

```cmd
PdfConsoleApp.exe ..\..\test_custom_page.json
```

## 预期结果

每个测试都应该：
1. 在控制台输出：`PDF 生成成功: <文件名>`
2. 在 `bin\Debug` 目录下生成对应的 PDF 文件
3. PDF 文件可以正常打开并显示内容

## 常见问题

### 问题 1: 找不到 JSON 文件

**错误信息：**
```
错误: JSON 文件不存在 - <路径>
```

**解决方法：**
- 确保 JSON 文件路径正确
- 使用绝对路径或相对于可执行文件的路径

### 问题 2: 字体不存在

**现象：** PDF 生成成功但显示效果不正确

**解决方法：**
- 确保系统中安装了指定的字体
- 使用系统默认字体，如"宋体"、"微软雅黑"、"Arial"

### 问题 3: 输出文件名不能为空

**错误信息：**
```
错误: 输出文件名不能为空
```

**解决方法：**
- 确保 JSON 文件中包含 `OutputFileName` 字段

### 问题 4: 文本内容不能为空

**错误信息：**
```
错误: 文本内容不能为空
```

**解决方法：**
- 确保 JSON 文件中包含 `TextContent` 字段

## 调试技巧

### 1. 查看详细错误信息

程序会在控制台输出详细的错误信息和堆栈跟踪

### 2. 验证 JSON 格式

使用在线 JSON 验证工具检查 JSON 格式是否正确：
- https://jsonlint.com/

### 3. 检查生成的文件

生成的 PDF 文件位置：
```
D:\GitHub\MyTools\PdfConsoleApp\bin\Debug\<OutputFileName>
```

## 进阶测试

### 测试长文本

创建包含大量文本的 JSON 文件，测试是否正确换行和处理超出页面的情况。

### 测试特殊字符

测试包含特殊字符的文本，如：
- 中文标点符号
- 英文标点符号
- 数字
- 特殊符号

### 测试不同字体

测试系统中不同的字体，确保兼容性。

## 性能测试

测试生成 PDF 的速度：

```cmd
@echo off
echo 开始时间: %time%
PdfConsoleApp.exe sample.json
echo 结束时间: %time%
```
