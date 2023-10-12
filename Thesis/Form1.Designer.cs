namespace Thesis
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.coresButton = new System.Windows.Forms.Button();
            this.veneersButton = new System.Windows.Forms.Button();
            this.customersButton = new System.Windows.Forms.Button();
            this.ordersButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Setup";
            // 
            // coresButton
            // 
            this.coresButton.Location = new System.Drawing.Point(299, 333);
            this.coresButton.Name = "coresButton";
            this.coresButton.Size = new System.Drawing.Size(128, 44);
            this.coresButton.TabIndex = 1;
            this.coresButton.Text = "Cores";
            this.coresButton.UseVisualStyleBackColor = true;
            this.coresButton.Click += new System.EventHandler(this.coresButton_Click);
            // 
            // veneersButton
            // 
            this.veneersButton.Location = new System.Drawing.Point(533, 333);
            this.veneersButton.Name = "veneersButton";
            this.veneersButton.Size = new System.Drawing.Size(128, 44);
            this.veneersButton.TabIndex = 2;
            this.veneersButton.Text = "Veneers";
            this.veneersButton.UseVisualStyleBackColor = true;
            // 
            // customersButton
            // 
            this.customersButton.Location = new System.Drawing.Point(62, 333);
            this.customersButton.Name = "customersButton";
            this.customersButton.Size = new System.Drawing.Size(128, 44);
            this.customersButton.TabIndex = 3;
            this.customersButton.Text = "Customer registry";
            this.customersButton.UseVisualStyleBackColor = true;
            this.customersButton.Click += new System.EventHandler(this.customersButton_Click);
            // 
            // ordersButton
            // 
            this.ordersButton.Location = new System.Drawing.Point(180, 90);
            this.ordersButton.Name = "ordersButton";
            this.ordersButton.Size = new System.Drawing.Size(385, 44);
            this.ordersButton.TabIndex = 4;
            this.ordersButton.Text = "Orders";
            this.ordersButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ordersButton);
            this.Controls.Add(this.customersButton);
            this.Controls.Add(this.veneersButton);
            this.Controls.Add(this.coresButton);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button coresButton;
        private System.Windows.Forms.Button veneersButton;
        private System.Windows.Forms.Button customersButton;
        private System.Windows.Forms.Button ordersButton;
    }
}

