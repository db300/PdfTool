namespace PdfDesignHelper
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenTemp = new System.Windows.Forms.Button();
            this.txtPdfTemp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numCellCount = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnSelectFont = new System.Windows.Forms.Button();
            this.btnParse = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numCellCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenTemp
            // 
            this.btnOpenTemp.Location = new System.Drawing.Point(12, 12);
            this.btnOpenTemp.Name = "btnOpenTemp";
            this.btnOpenTemp.Size = new System.Drawing.Size(75, 23);
            this.btnOpenTemp.TabIndex = 0;
            this.btnOpenTemp.Text = "选择模板";
            this.btnOpenTemp.UseVisualStyleBackColor = true;
            // 
            // txtPdfTemp
            // 
            this.txtPdfTemp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPdfTemp.Location = new System.Drawing.Point(93, 14);
            this.txtPdfTemp.Name = "txtPdfTemp";
            this.txtPdfTemp.ReadOnly = true;
            this.txtPdfTemp.Size = new System.Drawing.Size(874, 21);
            this.txtPdfTemp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "单元格数量：";
            // 
            // numCellCount
            // 
            this.numCellCount.Location = new System.Drawing.Point(93, 41);
            this.numCellCount.Name = "numCellCount";
            this.numCellCount.Size = new System.Drawing.Size(60, 21);
            this.numCellCount.TabIndex = 3;
            this.numCellCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 67);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(650, 371);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(240, 41);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 0;
            this.btnPreview.Text = "预览";
            this.btnPreview.UseVisualStyleBackColor = true;
            // 
            // btnSelectFont
            // 
            this.btnSelectFont.Location = new System.Drawing.Point(159, 41);
            this.btnSelectFont.Name = "btnSelectFont";
            this.btnSelectFont.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFont.TabIndex = 5;
            this.btnSelectFont.Text = "选择字体";
            this.btnSelectFont.UseVisualStyleBackColor = true;
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(665, 41);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(75, 23);
            this.btnParse.TabIndex = 0;
            this.btnParse.Text = "解析";
            this.btnParse.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(665, 67);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(302, 371);
            this.txtLog.TabIndex = 6;
            this.txtLog.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 450);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.btnSelectFont);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.numCellCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPdfTemp);
            this.Controls.Add(this.btnOpenTemp);
            this.Name = "MainForm";
            this.Text = "PDF设计辅助器";
            ((System.ComponentModel.ISupportInitialize)(this.numCellCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenTemp;
        private System.Windows.Forms.TextBox txtPdfTemp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numCellCount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnSelectFont;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.TextBox txtLog;
    }
}

