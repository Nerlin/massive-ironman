namespace MarsRover.Forms
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.ViewPort = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.FrameTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ViewPort
            // 
            this.ViewPort.AccumBits = ((byte)(0));
            this.ViewPort.AutoCheckErrors = false;
            this.ViewPort.AutoFinish = false;
            this.ViewPort.AutoMakeCurrent = false;
            this.ViewPort.AutoSwapBuffers = true;
            this.ViewPort.BackColor = System.Drawing.Color.Black;
            this.ViewPort.ColorBits = ((byte)(32));
            this.ViewPort.DepthBits = ((byte)(16));
            this.ViewPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewPort.Location = new System.Drawing.Point(0, 0);
            this.ViewPort.Name = "ViewPort";
            this.ViewPort.Size = new System.Drawing.Size(784, 562);
            this.ViewPort.StencilBits = ((byte)(0));
            this.ViewPort.TabIndex = 0;
            this.ViewPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.ViewPort.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewPort_MouseMove);
            this.ViewPort.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseWheel);
            // 
            // FrameTimer
            // 
            this.FrameTimer.Enabled = true;
            this.FrameTimer.Interval = 30;
            this.FrameTimer.Tick += new System.EventHandler(this.FrameTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.ViewPort);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MarsRover";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl ViewPort;
        private System.Windows.Forms.Timer FrameTimer;
    }
}