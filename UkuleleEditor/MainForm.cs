using System.Reflection;
using System.ComponentModel;
using System.Text.Json;

namespace UkuleleEditor
{
    public partial class MainForm : Form
    {
        #region Fields
        private SheetMusic _sheetMusic;
        private string? _currentFilePath;
        private bool _isModified;
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            var asmVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? Application.ProductVersion;
            Text = $"尤克里里编辑器 Ukulele Editor v{asmVersion}";

            // 初始化空白曲谱
            _sheetMusic = new SheetMusic();
            _currentFilePath = null;
            _isModified = false;

            // 设置 DataGridView
            dgvItems.AutoGenerateColumns = false;

            // 加载测试数据（可选）
            LoadTestData();
        }
        #endregion

        #region Test Data
        private void LoadTestData()
        {
            try
            {
                var jsonContent = File.ReadAllText("tests\\test.json");
                _sheetMusic = JsonSerializer.Deserialize<SheetMusic>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new SheetMusic();

                RefreshLineList();
                if (_sheetMusic.Lines.Count > 0)
                {
                    listBoxLines.SelectedIndex = 0;
                }
                RefreshPreview();
            }
            catch
            {
                // 如果加载失败，保持空白曲谱
            }
        }
        #endregion

        #region Line Management
        private void RefreshLineList()
        {
            int selectedIndex = listBoxLines.SelectedIndex;
            listBoxLines.Items.Clear();

            for (int i = 0; i < _sheetMusic.Lines.Count; i++)
            {
                var line = _sheetMusic.Lines[i];
                listBoxLines.Items.Add($"第 {i + 1} 行 ({line.Items.Count} 个音符)");
            }

            if (selectedIndex >= 0 && selectedIndex < listBoxLines.Items.Count)
            {
                listBoxLines.SelectedIndex = selectedIndex;
            }
            else if (listBoxLines.Items.Count > 0)
            {
                listBoxLines.SelectedIndex = listBoxLines.Items.Count - 1;
            }
        }

        private void BtnAddLine_Click(object? sender, EventArgs e)
        {
            _sheetMusic.Lines.Add(new SheetMusicLine());
            RefreshLineList();
            listBoxLines.SelectedIndex = _sheetMusic.Lines.Count - 1;
            _isModified = true;
            UpdateTitle();
        }

        private void BtnDeleteLine_Click(object? sender, EventArgs e)
        {
            int index = listBoxLines.SelectedIndex;
            if (index >= 0 && index < _sheetMusic.Lines.Count)
            {
                if (MessageBox.Show($"确定要删除第 {index + 1} 行吗？", "确认删除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _sheetMusic.Lines.RemoveAt(index);
                    RefreshLineList();
                    RefreshPreview();
                    _isModified = true;
                    UpdateTitle();
                }
            }
        }

        private void BtnMoveLineUp_Click(object? sender, EventArgs e)
        {
            int index = listBoxLines.SelectedIndex;
            if (index > 0)
            {
                var line = _sheetMusic.Lines[index];
                _sheetMusic.Lines.RemoveAt(index);
                _sheetMusic.Lines.Insert(index - 1, line);
                RefreshLineList();
                listBoxLines.SelectedIndex = index - 1;
                RefreshPreview();
                _isModified = true;
                UpdateTitle();
            }
        }

        private void BtnMoveLineDown_Click(object? sender, EventArgs e)
        {
            int index = listBoxLines.SelectedIndex;
            if (index >= 0 && index < _sheetMusic.Lines.Count - 1)
            {
                var line = _sheetMusic.Lines[index];
                _sheetMusic.Lines.RemoveAt(index);
                _sheetMusic.Lines.Insert(index + 1, line);
                RefreshLineList();
                listBoxLines.SelectedIndex = index + 1;
                RefreshPreview();
                _isModified = true;
                UpdateTitle();
            }
        }

        private void ListBoxLines_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int index = listBoxLines.SelectedIndex;
            if (index >= 0 && index < _sheetMusic.Lines.Count)
            {
                LoadItemsToGrid(_sheetMusic.Lines[index]);
            }
            else
            {
                dgvItems.DataSource = null;
            }
        }
        #endregion

        #region Item Management
        private void LoadItemsToGrid(SheetMusicLine line)
        {
            var bindingList = new BindingList<SheetMusicItem>(line.Items);
            bindingList.ListChanged += (s, e) =>
            {
                _isModified = true;
                UpdateTitle();
            };
            dgvItems.DataSource = bindingList;
        }

        private void BtnAddItem_Click(object? sender, EventArgs e)
        {
            int lineIndex = listBoxLines.SelectedIndex;
            if (lineIndex >= 0 && lineIndex < _sheetMusic.Lines.Count)
            {
                var line = _sheetMusic.Lines[lineIndex];
                line.Items.Add(new SheetMusicItem { String = 1, Fret = 0, Lyric = "" });
                LoadItemsToGrid(line);
                RefreshLineList();
                listBoxLines.SelectedIndex = lineIndex;
                _isModified = true;
                UpdateTitle();
            }
            else
            {
                MessageBox.Show("请先选择一行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteItem_Click(object? sender, EventArgs e)
        {
            int lineIndex = listBoxLines.SelectedIndex;
            if (lineIndex >= 0 && lineIndex < _sheetMusic.Lines.Count && dgvItems.SelectedRows.Count > 0)
            {
                int rowIndex = dgvItems.SelectedRows[0].Index;
                var line = _sheetMusic.Lines[lineIndex];
                if (rowIndex >= 0 && rowIndex < line.Items.Count)
                {
                    line.Items.RemoveAt(rowIndex);
                    LoadItemsToGrid(line);
                    RefreshLineList();
                    listBoxLines.SelectedIndex = lineIndex;
                    _isModified = true;
                    UpdateTitle();
                }
            }
        }

        private void BtnMoveItemUp_Click(object? sender, EventArgs e)
        {
            int lineIndex = listBoxLines.SelectedIndex;
            if (lineIndex >= 0 && lineIndex < _sheetMusic.Lines.Count && dgvItems.SelectedRows.Count > 0)
            {
                int rowIndex = dgvItems.SelectedRows[0].Index;
                var line = _sheetMusic.Lines[lineIndex];
                if (rowIndex > 0 && rowIndex < line.Items.Count)
                {
                    var item = line.Items[rowIndex];
                    line.Items.RemoveAt(rowIndex);
                    line.Items.Insert(rowIndex - 1, item);
                    LoadItemsToGrid(line);
                    dgvItems.Rows[rowIndex - 1].Selected = true;
                    _isModified = true;
                    UpdateTitle();
                }
            }
        }

        private void BtnMoveItemDown_Click(object? sender, EventArgs e)
        {
            int lineIndex = listBoxLines.SelectedIndex;
            if (lineIndex >= 0 && lineIndex < _sheetMusic.Lines.Count && dgvItems.SelectedRows.Count > 0)
            {
                int rowIndex = dgvItems.SelectedRows[0].Index;
                var line = _sheetMusic.Lines[lineIndex];
                if (rowIndex >= 0 && rowIndex < line.Items.Count - 1)
                {
                    var item = line.Items[rowIndex];
                    line.Items.RemoveAt(rowIndex);
                    line.Items.Insert(rowIndex + 1, item);
                    LoadItemsToGrid(line);
                    dgvItems.Rows[rowIndex + 1].Selected = true;
                    _isModified = true;
                    UpdateTitle();
                }
            }
        }

        private void DgvItems_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                RefreshLineList();
                RefreshPreview();
                _isModified = true;
                UpdateTitle();
            }
        }
        #endregion

        #region Preview
        private void RefreshPreview()
        {
            try
            {
                if (_sheetMusic.Lines.Count == 0)
                {
                    pictureBoxPreview.Image = null;
                    return;
                }

                // 临时保存到内存
                var tempPath = Path.GetTempFileName();
                var jsonContent = JsonSerializer.Serialize(_sheetMusic, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                });
                File.WriteAllText(tempPath, jsonContent);

                // 渲染
                var oldImage = pictureBoxPreview.Image;
                var bitmap = SheetMusicRenderer.RenderSheetMusic(tempPath);
                pictureBoxPreview.Image = bitmap;
                oldImage?.Dispose();

                // 清理临时文件
                File.Delete(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"渲染预览失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            if (pictureBoxPreview.Image == null)
            {
                MessageBox.Show("没有可导出的图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "JPEG 图片|*.jpg|PNG 图片|*.png|所有文件|*.*",
                DefaultExt = "jpg",
                FileName = "ukulele_sheet.jpg"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBoxPreview.Image.Save(sfd.FileName);
                    MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导出失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region File Operations
        private void MenuNew_Click(object? sender, EventArgs e)
        {
            if (PromptSaveIfModified())
            {
                _sheetMusic = new SheetMusic();
                _currentFilePath = null;
                _isModified = false;
                RefreshLineList();
                RefreshPreview();
                UpdateTitle();
            }
        }

        private void MenuOpen_Click(object? sender, EventArgs e)
        {
            if (!PromptSaveIfModified())
                return;

            using var ofd = new OpenFileDialog
            {
                Filter = "JSON 文件|*.json|所有文件|*.*",
                DefaultExt = "json"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var jsonContent = File.ReadAllText(ofd.FileName);
                    _sheetMusic = JsonSerializer.Deserialize<SheetMusic>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new SheetMusic();

                    _currentFilePath = ofd.FileName;
                    _isModified = false;
                    RefreshLineList();
                    if (_sheetMusic.Lines.Count > 0)
                    {
                        listBoxLines.SelectedIndex = 0;
                    }
                    RefreshPreview();
                    UpdateTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"打开文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MenuSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                MenuSaveAs_Click(sender, e);
            }
            else
            {
                SaveToFile(_currentFilePath);
            }
        }

        private void MenuSaveAs_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "JSON 文件|*.json|所有文件|*.*",
                DefaultExt = "json",
                FileName = "sheet_music.json"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveToFile(sfd.FileName);
            }
        }

        private void SaveToFile(string filePath)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(_sheetMusic, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                });
                File.WriteAllText(filePath, jsonContent);
                _currentFilePath = filePath;
                _isModified = false;
                UpdateTitle();
                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!PromptSaveIfModified())
            {
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private bool PromptSaveIfModified()
        {
            if (_isModified)
            {
                var result = MessageBox.Show("文件已修改，是否保存？", "提示",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MenuSave_Click(null, EventArgs.Empty);
                    return !_isModified; // 如果保存失败，仍然是修改状态
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }

        private void UpdateTitle()
        {
            var asmVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? Application.ProductVersion;
            var fileName = string.IsNullOrEmpty(_currentFilePath) ? "未命名" : Path.GetFileName(_currentFilePath);
            var modifiedMark = _isModified ? " *" : "";
            Text = $"尤克里里编辑器 Ukulele Editor v{asmVersion} - {fileName}{modifiedMark}";
        }
        #endregion
    }
}
