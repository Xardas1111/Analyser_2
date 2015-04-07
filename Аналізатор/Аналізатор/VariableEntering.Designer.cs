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
            this.label1 = new System.Windows.Forms.Label();
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
            this.VariableValue.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.VariableValue.Location = new System.Drawing.Point(12, 26);
            this.VariableValue.Name = "VariableValue";
            this.VariableValue.Size = new System.Drawing.Size(213, 21);
            this.VariableValue.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // VariableEntering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 82);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
    }
}