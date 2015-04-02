namespace Аналізатор
{
    partial class VariableEntering
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EnteringName = new System.Windows.Forms.Button();
            this.VariableValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // EnteringName
            // 
            this.EnteringName.Location = new System.Drawing.Point(85, 52);
            this.EnteringName.Name = "EnteringName";
            this.EnteringName.Size = new System.Drawing.Size(75, 23);
            this.EnteringName.TabIndex = 0;
            this.EnteringName.Text = "Enter";
            this.EnteringName.UseVisualStyleBackColor = true;
            this.EnteringName.Click += new System.EventHandler(this.EnteringName_Click);
            // 
            // VariableValue
            // 
            this.VariableValue.Location = new System.Drawing.Point(12, 26);
            this.VariableValue.Name = "VariableValue";
            this.VariableValue.Size = new System.Drawing.Size(213, 20);
            this.VariableValue.TabIndex = 2;
            // 
            // VariableEntering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 82);
            this.Controls.Add(this.VariableValue);
            this.Controls.Add(this.EnteringName);
            this.Name = "VariableEntering";
            this.Text = "VariableEntering";
            this.Load += new System.EventHandler(this.VariableEntering_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EnteringName;
        private System.Windows.Forms.TextBox VariableValue;
    }
}