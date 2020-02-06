//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using Microsoft.VisualBasic;
//using System.Runtime.InteropServices; //required for APIs
//using System.Diagnostics;
//using System.IO;
//using System.Threading;

//namespace MyDummy
//{

//    // Estrutura de coordenada 
    
   
//    partial class Form1
//    {
//        struct loc
//        {
//            public int x;
//            public int y;
//        }

//        [DllImport("user32.dll")]
//        private static extern bool SetForegroundWindow(IntPtr hWnd);
//        [DllImport("user32.dll")]
//        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
//        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
//        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
//        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
//        private const int MOUSEEVENTF_LEFTUP = 0x04;
//        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
//        private const int MOUSEEVENTF_RIGHTUP = 0x10;
//        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
//        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
//        public const int QKEY = 0x51; // tecla Q 
//        public const int WKEY = 0x57; // tecla W 
//        public const int EKEY = 0x45; // tecla E 
//        public const int SKEY = 0x53; // tecla S 
//        public const int SPACEBAR = 0x20; // espaço
//        public const int PLAYERSTATSX = 1574; // pixel do status do player no addon
//        public const int PLAYERSTATSY = 470; // pixel do status do player no addon 
//        public const int RANGEX = 1662; // pixel do status do player no addon
//        public const int RANGEY = 6003; // pixel do status do player no addon 
//        public const int FACINGX = 1577; // pixel do status do player no addon
//        public const int FACINGY = 715; // pixel do status do player no addon 
//        public const int UM = 0x31; // tecla 1 
//        public const int DOIS = 0x32; // tecla 2 
//        public const int TRES = 0x33; // tecla 3 
//        public const int QUATRO = 0x34; // tecla 4 
//        public const int CINCO = 0x35; // tecla 5 
//        public const int SEIS = 0x36; // tecla 6 
//        public const int SETE = 0x37; // tecla 7 
//        public const int OITO = 0x38; // tecla 8 
//        public const int NOVE = 0x39; // tecla 9 
//        public const int ZERO = 0x30; // tecla 0 
//        public const int N1 = 0x61; // numpad key 1 
//        public const int N2 = 0x62; // tecla 2 
//        public const int N3 = 0x63; // tecla 3 
//        public const int N4 = 0x64; // tecla 4 
//        public const int N5 = 0x65; // tecla 5 
//        public const int N6 = 0x66; // tecla 6 
//        public const int N7 = 0x67; // tecla 7 
//        public const int N8 = 0x68; // tecla 8 
//        public const int N9 = 0x69; // tecla 9 
//        public const int N0 = 0x60; // tecla 0 



//        public void wait(int milliseconds) // It will wait number of miliseconds. 
//        {
//            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
//            while (sw.ElapsedMilliseconds <= milliseconds)
//            {
//                Application.DoEvents();
//            }
//        }


//        void focawow() // Focus world of warcraft windows
//        {
//            var prc = Process.GetProcessesByName("wow");
//            if (prc.Length > 0)
//            {
//                SetForegroundWindow(prc[0].MainWindowHandle);
//            }
//            else
//                MessageBox.Show("Wow window not found");

//        }


//        // função que move o cursor.... (em construção) 
//        void mousemove(int x, int y)
//        {
//            this.Cursor = new Cursor(Cursor.Current.Handle);
//            Cursor.Position = new Point(x, y);
//        }



//        // envia tecla para o wow 
//        void aperta(byte key, int time = 50) // it will send coded key, and keep it pressed number of miliseconds in argument 2. 50miliseconds will be default. 
//        {
//            focawow(); // this is the focust window method above
//            if (time != 2) keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0);
//            if (time != 2) wait(time); // this is wait method above 
//            if (time > 0) keybd_event(key, 0, KEYEVENTF_KEYUP, 0); // solta a tecla
//        }


//        public void DoMouseClick(int botao = 1) // argument is button, 1 or 2, 1 is default
//        {
//            //Call the imported function with the cursor's current position
//            int X = Cursor.Position.X;
//            int Y = Cursor.Position.Y;
//            if (botao == 1) mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
//            else if (botao == 2)
//                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
//        }

//        void clica(int x, int y, int botao = 1)
//        {
//            mousemove(x, y);
//            DoMouseClick(botao);
//        }

//        //Color GetColorAt(int x, int y)
//        //{
//        //   // Bitmap bmp = new Bitmap();
//        //    Rectangle bounds = new Rectangle(x, y, 1, 1);
//        //    using (Graphics g = Graphics.FromImage(bmp))
//        //        g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
//        //    return bmp.GetPixel(0, 0);
//        //}




//        double getangle(double y1, double y2, double x1, double x2)
//        {
//            double ang = Math.Atan2(x1 - x2, y1 - y2) / Math.PI;
//            if (ang < 0) ang += 2; // this is used to avoind negative numbers. 
//            return Math.Round(ang * 1000);
//        }



//        int dist(loc orig, loc tar)
//        {
//            double distance = (Math.Sqrt(Math.Pow(Math.Abs(orig.x - tar.x), 2) +
//                Math.Pow(Math.Abs(orig.y - tar.y), 2))); // formula da distancia entre pontos
//            int temp = (int)distance; // converte double para int 
//            // tbdist.Text = temp.ToString(); // escreve distancia no campo do texto, for debug 
//            return temp; // retorna distancia entre os pontos 
//        }



//        public static bool botIsRunning;

//        private void button1_Click(object sender, EventArgs e)
//        {
//            startNewThread();
//        }

//        public  void mainBotMethod()
//        {
//            while (botIsRunning)
//            {
//                // All your stuff in here


//                focawow();

//                // Check it from time to time just to make sure
//                if (!botIsRunning)
//                {
//                    break;
//                }
//            }
//        }

//        public  void startNewThread()
//        {
//            // Run the botmethods in a different thread, then the UI won't freeze.
//            // this starts the above method
//            Thread botMethod = new Thread(mainBotMethod);
//            botMethod.Start();
//        }








//        /// <summary>
//        /// Variable del diseñador necesaria.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Limpiar los recursos que se estén usando.
//        /// </summary>
//        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Código generado por el Diseñador de Windows Forms

//        /// <summary>
//        /// Método necesario para admitir el Diseñador. No se puede modificar
//        /// el contenido de este método con el editor de código.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.button1 = new System.Windows.Forms.Button();
//            this.SuspendLayout();
//            // 
//            // button1
//            // 
//            this.button1.Location = new System.Drawing.Point(621, 108);
//            this.button1.Name = "button1";
//            this.button1.Size = new System.Drawing.Size(75, 23);
//            this.button1.TabIndex = 0;
//            this.button1.Text = "button1";
//            this.button1.UseVisualStyleBackColor = true;
//            this.button1.Click += new System.EventHandler(this.button1_Click_1);
//            // 
//            // Form1
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(800, 450);
//            this.Controls.Add(this.button1);
//            this.Name = "Form1";
//            this.Text = "Form1";
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private Button button1;
//    }
//}

