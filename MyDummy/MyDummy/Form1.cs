using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices; //required for APIs
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MyDummy
{
    public partial class frm1 : Form
    {
        struct loc
        {
            public int x;
            public int y;
        }

        public enum BotState
        {
            WaitingToEnter,
            JustEnter,
            Alive,
            Dead,
            DeadWaitingRelease
        }
        public BotState botState = BotState.WaitingToEnter;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int QKEY = 0x51; // tecla Q 
        public const int WKEY = 0x57; // tecla W 
        public const int EKEY = 0x45; // tecla E 
        public const int SKEY = 0x53; // tecla S 
        public const int SPACEBAR = 0x20; // espaço
        public const int PLAYERSTATSX = 1574; // pixel do status do player no addon
        public const int PLAYERSTATSY = 470; // pixel do status do player no addon 
        public const int RANGEX = 1662; // pixel do status do player no addon
        public const int RANGEY = 6003; // pixel do status do player no addon 
        public const int FACINGX = 1577; // pixel do status do player no addon
        public const int FACINGY = 715; // pixel do status do player no addon 
        public const int UM = 0x31; // tecla 1 
        public const int DOIS = 0x32; // tecla 2 
        public const int TRES = 0x33; // tecla 3 
        public const int QUATRO = 0x34; // tecla 4 
        public const int CINCO = 0x35; // tecla 5 
        public const int SEIS = 0x36; // tecla 6 
        public const int SETE = 0x37; // tecla 7 
        public const int OITO = 0x38; // tecla 8 
        public const int NOVE = 0x39; // tecla 9 
        public const int ZERO = 0x30; // tecla 0 
        public const int N1 = 0x61; // numpad key 1 
        public const int N2 = 0x62; // tecla 2 
        public const int N3 = 0x63; // tecla 3 
        public const int N4 = 0x64; // tecla 4 
        public const int N5 = 0x65; // tecla 5 
        public const int N6 = 0x66; // tecla 6 
        public const int N7 = 0x67; // tecla 7 
        public const int N8 = 0x68; // tecla 8 
        public const int N9 = 0x69; // tecla 9 
        public const int N0 = 0x60; // tecla 0 


        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        Random rnd1 = new Random();
        public void wait(int milliseconds, int random = 10) // It will wait number of miliseconds. 
        {
            int randomMin, randomMax;

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            randomMin = milliseconds - (milliseconds / random);
            randomMax = milliseconds + (milliseconds / random);

            milliseconds = rnd1.Next(randomMin, randomMax);
            while (sw.ElapsedMilliseconds <= milliseconds)
            {
                Application.DoEvents();
            }
        }


        void focawow() // Focus world of warcraft windows
        {
            var prc2 = Process.GetProcesses();

            //var prc = Process.GetProcessesByName("WowClassic");
            var prc = Process.GetProcessesByName(txtWowProcess.Text);
            if (prc.Length > 0)
            {
                SetForegroundWindow(prc[0].MainWindowHandle);
            }
            else
                MessageBox.Show("Wow window not found");

        }


        // função que move o cursor.... (em construção) 
        void mousemove(int x, int y)
        {
            Invoke(new Action(() =>
            {
                this.Cursor = new Cursor(Cursor.Current.Handle);
                Cursor.Position = new Point(x, y);
            }));


        }



        // envia tecla para o wow 
        void DoKeyPress(byte key, int time = 50) // it will send coded key, and keep it pressed number of miliseconds in argument 2. 50miliseconds will be default. 
        {
            focawow(); // this is the focust window method above
            if (time != 2) keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0);
            if (time != 2) wait(time); // this is wait method above 
            if (time > 0) keybd_event(key, 0, KEYEVENTF_KEYUP, 0); // solta a tecla
        }


        public void DoMouseClick(int botao = 1) // argument is button, 1 or 2, 1 is default
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            if (botao == 1) mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            else if (botao == 2)
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
        }

        void DoClick(int x, int y, int botao = 1)
        {
            mousemove(x, y);
            wait(200);
            DoMouseClick(botao);
        }

        Color GetColorAt(int x, int y)
        {
            Bitmap bmp =
        new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp.GetPixel(0, 0);
        }




        double getangle(double y1, double y2, double x1, double x2)
        {
            double ang = Math.Atan2(x1 - x2, y1 - y2) / Math.PI;
            if (ang < 0) ang += 2; // this is used to avoind negative numbers. 
            return Math.Round(ang * 1000);
        }



        int dist(loc orig, loc tar)
        {
            double distance = (Math.Sqrt(Math.Pow(Math.Abs(orig.x - tar.x), 2) +
                Math.Pow(Math.Abs(orig.y - tar.y), 2))); // formula da distancia entre pontos
            int temp = (int)distance; // converte double para int 
            // tbdist.Text = temp.ToString(); // escreve distancia no campo do texto, for debug 
            return temp; // retorna distancia entre os pontos 
        }



        public static bool botIsRunning;

        private void button1_Click(object sender, EventArgs e)
        {
            botIsRunning = true;
            botState = BotState.WaitingToEnter;
            startNewThread();
        }

        public void mainBotMethod()
        {
            while (botIsRunning)
            {
                // All your stuff in here

                wait(2000);
                focawow();
                wait(2000);

                //if(botState == BotState.Dead || botState == BotState.DeadWaitingRelease)
                //{
                if (CheckIsAlive())
                {
                    if (!CheckIsInBG())
                    {
                        botState = BotState.WaitingToEnter;
                    }
                }
                //}

                switch (botState)
                {
                    case BotState.WaitingToEnter:
                        WaitingToEnter();
                        break;
                    case BotState.JustEnter:
                        JustEnter();
                        break;
                    case BotState.Alive:
                        Alive();
                        break;
                    case BotState.Dead:
                        Dead();
                        break;
                    case BotState.DeadWaitingRelease:
                        DeadWaitingRelease();
                        break;
                    default:
                        break;
                }

                //var xColor = GetColorAt(1557, 517);
                //var RGBxColor = xColor.ToArgb();

                // Check it from time to time just to make sure
                if (!botIsRunning)
                {
                    break;
                }
            }
        }

        private void DeadWaitingRelease()
        {
            // TODO
            wait(200);
            TryToCloseBGWindow();
            wait(200);
        }

        private void Dead()
        {
            // TODO
            wait(200);
            TryToCloseBGWindow();
            wait(200);

        }

        private void Alive()
        {
            wait(200);
            TryToCloseBGWindow();
            wait(200);

            if (CheckIsAlive())
            {
                //TODO
                var rnd = rnd1.Next(1, 4);
                switch (rnd)
                {
                    case 1:
                        DoKeyPress(EKEY);
                        break;
                    case 2:
                        DoKeyPress(WKEY);
                        break;
                    case 3:
                        DoKeyPress(SKEY);
                        break;
                    case 4:
                        DoKeyPress(QKEY);
                        break;
                    //case 5:
                    //    DoKeyPress(SPACEBAR);
                    //    break;
                    default:
                        DoKeyPress(SPACEBAR);
                        break;
                }

                wait(60000);
            }
            else
            {
                botState = CheckDeadState();
            }
        }

        private bool CheckIsAlive()
        {
            Color mouseColor = GetColorAt(Int32.Parse(txtXIsAlive.Text), Int32.Parse(txtYIsAlive.Text));
            return (mouseColor.G == 255 && mouseColor.R == 0 && mouseColor.B == 0);
        }

        private BotState CheckDeadState()
        {
            Color mouseColor = GetColorAt(Int32.Parse(txtHealthX.Text), Int32.Parse(txtHealthY.Text));
            if (mouseColor.R == 255 && mouseColor.R == 0 && mouseColor.B == 0)
                return BotState.DeadWaitingRelease;
            else
                return BotState.Dead;
        }

        private void JustEnter()
        {
            //TODO
            wait(13000);
            if (!rbAlliance.Checked)
            {
                DoKeyPress(N1, 320);
                wait(1000);
            }
            DoKeyPress(WKEY, 2000);
            wait(1000);
            if (rbAlliance.Checked)
            {
                DoKeyPress(N3, 280);
                wait(1000);
            }
            else
            {
                DoKeyPress(N3, 220);
                wait(1000);
            }

            WalkJumping(20000);

            wait(90000);

            WalkJumping(20000);

            wait(1000);
            DoKeyPress(N3, 100);
            wait(500);

            WalkJumping(20000);

            wait(1000);
            DoKeyPress(N1, 200);
            wait(500);

            WalkJumping(20000);

            wait(500);
            DoClick(Int32.Parse(txtXInsignia.Text), Int32.Parse(txtYInsignia.Text));
            wait(9000);

            wait(5000);
            if (rbAlliance.Checked)
            {
                DoKeyPress(N3, 120);
            }
            else
            {
                DoKeyPress(N3, 750);
            }
            wait(500);

            WalkJumping(20000);

            botState = BotState.Alive;

        }

        private void WalkJumping(int totalMilliseconds)
        {
            var partialMilliseconds = totalMilliseconds / 4;

            int randomMin, randomMax;

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            randomMin = partialMilliseconds - (partialMilliseconds / 10);
            randomMax = partialMilliseconds + (partialMilliseconds / 10);

            DoKeyPress(SPACEBAR, 10);
            DoKeyPress(WKEY, rnd1.Next(randomMin, randomMax));
            DoKeyPress(SPACEBAR, 10);
            DoKeyPress(WKEY, rnd1.Next(randomMin, randomMax));
            DoKeyPress(SPACEBAR, 10);
            DoKeyPress(WKEY, rnd1.Next(randomMin, randomMax));
        }

        bool IsInBG;
        private void WaitingToEnter()
        {
            wait(500);
            OpenCloseWeakAuras();
            wait(1000);
            IsInBG = CheckIsInBG();
            wait(1000);
            if (!IsInBG)
                TryToJoin();
            else
                botState = BotState.JustEnter;

        }

        private bool CheckIsInBG()
        {
            Color mouseColor = GetColorAt(Int32.Parse(txtCoordsXX.Text), Int32.Parse(txtCoordsXY.Text));
            return (mouseColor.R == 255);
        }

        private void TryToJoin()
        {
            
            wait(1000);
            DoClick(Int32.Parse(txtXBattlemaster.Text), Int32.Parse(txtYBattlemaster.Text));
            wait(1000);
            DoKeyPress(N9);
            wait(1000);
            DoClick(Int32.Parse(txtXJoin.Text), Int32.Parse(txtYJoin.Text));
            wait(3000);
            DoClick(Int32.Parse(txtXJoin.Text), Int32.Parse(txtYJoin.Text));
            //wait(1000);
            //DoClick(Int32.Parse(txtXLeave.Text), Int32.Parse(txtYLeave.Text));
        }

        private void OpenCloseWeakAuras()
        {
            DoClick(Int32.Parse(txtXOpenCloseWA.Text), Int32.Parse(txtYOpenCloseWA.Text));
            wait(300);
            DoClick(Int32.Parse(txtXOpenCloseWA.Text), Int32.Parse(txtYOpenCloseWA.Text));
        }

        private void TryToCloseBGWindow()
        {
            DoClick(Int32.Parse(txtXLeaveBG.Text), Int32.Parse(txtYLeaveBG.Text));
        }


        public void startNewThread()
        {
            // Run the botmethods in a different thread, then the UI won't freeze.
            // this starts the above method
            Thread botMethod = new Thread(mainBotMethod);
            botMethod.Start();
        }

        public void startConfigThread()
        {
            // Run the botmethods in a different thread, then the UI won't freeze.
            // this starts the above method
            Thread botMethod = new Thread(configBotMethod);
            botMethod.Start();
        }

        Color mouseColor;
        Color mouseColorReturn;
        Color mouseColorReturnX;
        Color mouseColorReturnY;
        Color mouseColorReturnH;
        Color mouseColorReturnAlive;
        POINT cursorPos;
        public void configBotMethod()
        {
            while (true)
            {
                wait(200);
                // All your stuff in here

                GetCursorPos(out cursorPos);

                try
                {

                    Invoke(new Action(() =>
                    {
                        txtMouseX.Text = cursorPos.X.ToString();
                        txtMouseY.Text = cursorPos.Y.ToString();
                    }));

                    mouseColorReturn = SetBackColor(cursorPos, grpMouseColor);
                    mouseColorReturnX = SetBackColor(Int32.Parse(txtCoordsXX.Text), Int32.Parse(txtCoordsXY.Text), grpXResult);
                    mouseColorReturnY = SetBackColor(Int32.Parse(txtCoordsYX.Text), Int32.Parse(txtCoordsYY.Text), grpYResult);
                    mouseColorReturnH = SetBackColor(Int32.Parse(txtHealthX.Text), Int32.Parse(txtHealthY.Text), grpHResult);
                    mouseColorReturnAlive = SetBackColor(Int32.Parse(txtXIsAlive.Text), Int32.Parse(txtYIsAlive.Text), grpAliveResult);


                    Invoke(new Action(() =>
                    {
                        lblXResult.Text = string.Format("{0},{1}", mouseColorReturnX.R, mouseColorReturnX.G);
                        lblYResult.Text = string.Format("{0},{1}", mouseColorReturnY.R, mouseColorReturnY.G);
                        lblHResult.Text = string.Format("{0},{1},{2}", mouseColorReturnH.R, mouseColorReturnH.G, mouseColorReturnH.B);
                        lblMouseCoordsResult.Text = string.Format("{0},{1},{2}", mouseColorReturn.R, mouseColorReturn.G, mouseColorReturn.B);
                        lblAliveResult.Text = string.Format("{0},{1},{2}", mouseColorReturnAlive.R, mouseColorReturnAlive.G, mouseColorReturnAlive.B);

                    }));


                }
                catch (Exception ex)
                {
                   // MessageBox.Show(ex.Message);
                    Environment.Exit(1);
                }
            }
        }

        private Color SetBackColor(int X, int Y, Control control)
        {
            cursorPos.X = X;
            cursorPos.Y = Y;
            return SetBackColor(cursorPos, control);
        }

        private Color SetBackColor(POINT cursorPos, Control control)
        {
            mouseColor = GetColorAt(cursorPos.X, cursorPos.Y);
            control.BackColor = mouseColor;
            return mouseColor;
        }








        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCoordsXX = new System.Windows.Forms.TextBox();
            this.txtCoordsXY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCoordsYY = new System.Windows.Forms.TextBox();
            this.txtCoordsYX = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpYResult = new System.Windows.Forms.Label();
            this.grpXResult = new System.Windows.Forms.Label();
            this.lblYResult = new System.Windows.Forms.Label();
            this.lblXResult = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpAliveResult = new System.Windows.Forms.Label();
            this.lblAliveResult = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtXIsAlive = new System.Windows.Forms.TextBox();
            this.txtYIsAlive = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.grpHResult = new System.Windows.Forms.Label();
            this.lblHResult = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtHealthX = new System.Windows.Forms.TextBox();
            this.txtHealthY = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rbHorde = new System.Windows.Forms.RadioButton();
            this.rbAlliance = new System.Windows.Forms.RadioButton();
            this.label39 = new System.Windows.Forms.Label();
            this.txtXInsignia = new System.Windows.Forms.TextBox();
            this.txtYInsignia = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.txtXLeaveBG = new System.Windows.Forms.TextBox();
            this.txtYLeaveBG = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.txtXLeave = new System.Windows.Forms.TextBox();
            this.txtYLeave = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.lblMouseCoordsResult = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtXOpenCloseWA = new System.Windows.Forms.TextBox();
            this.txtYOpenCloseWA = new System.Windows.Forms.TextBox();
            this.txtXJoin = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtYJoin = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtXBattlemaster = new System.Windows.Forms.TextBox();
            this.txtYBattlemaster = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtWowProcess = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grpMouseColor = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.txtMouseY = new System.Windows.Forms.TextBox();
            this.txtMouseX = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(108, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 38);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "X WeakAuras";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y WeakAuras";
            // 
            // txtCoordsXX
            // 
            this.txtCoordsXX.Location = new System.Drawing.Point(34, 44);
            this.txtCoordsXX.Name = "txtCoordsXX";
            this.txtCoordsXX.Size = new System.Drawing.Size(44, 20);
            this.txtCoordsXX.TabIndex = 4;
            this.txtCoordsXX.Text = "1557";
            this.txtCoordsXX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCoordsXY
            // 
            this.txtCoordsXY.Location = new System.Drawing.Point(35, 70);
            this.txtCoordsXY.Name = "txtCoordsXY";
            this.txtCoordsXY.Size = new System.Drawing.Size(44, 20);
            this.txtCoordsXY.TabIndex = 5;
            this.txtCoordsXY.Text = "517";
            this.txtCoordsXY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(94, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "X";
            // 
            // txtCoordsYY
            // 
            this.txtCoordsYY.Location = new System.Drawing.Point(123, 70);
            this.txtCoordsYY.Name = "txtCoordsYY";
            this.txtCoordsYY.Size = new System.Drawing.Size(44, 20);
            this.txtCoordsYY.TabIndex = 9;
            this.txtCoordsYY.Text = "517";
            this.txtCoordsYY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCoordsYX
            // 
            this.txtCoordsYX.Location = new System.Drawing.Point(122, 44);
            this.txtCoordsYX.Name = "txtCoordsYX";
            this.txtCoordsYX.Size = new System.Drawing.Size(44, 20);
            this.txtCoordsYX.TabIndex = 8;
            this.txtCoordsYX.Text = "1607";
            this.txtCoordsYX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grpYResult);
            this.groupBox1.Controls.Add(this.grpXResult);
            this.groupBox1.Controls.Add(this.lblYResult);
            this.groupBox1.Controls.Add(this.lblXResult);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtCoordsXX);
            this.groupBox1.Controls.Add(this.txtCoordsYY);
            this.groupBox1.Controls.Add(this.txtCoordsXY);
            this.groupBox1.Controls.Add(this.txtCoordsYX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(4, 197);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 174);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WeakAuras Coords";
            // 
            // grpYResult
            // 
            this.grpYResult.BackColor = System.Drawing.Color.Gainsboro;
            this.grpYResult.Location = new System.Drawing.Point(124, 99);
            this.grpYResult.Name = "grpYResult";
            this.grpYResult.Size = new System.Drawing.Size(42, 20);
            this.grpYResult.TabIndex = 26;
            // 
            // grpXResult
            // 
            this.grpXResult.BackColor = System.Drawing.Color.Gainsboro;
            this.grpXResult.Location = new System.Drawing.Point(37, 99);
            this.grpXResult.Name = "grpXResult";
            this.grpXResult.Size = new System.Drawing.Size(42, 20);
            this.grpXResult.TabIndex = 25;
            // 
            // lblYResult
            // 
            this.lblYResult.AutoSize = true;
            this.lblYResult.Location = new System.Drawing.Point(134, 121);
            this.lblYResult.Name = "lblYResult";
            this.lblYResult.Size = new System.Drawing.Size(14, 13);
            this.lblYResult.TabIndex = 15;
            this.lblYResult.Text = "Y";
            // 
            // lblXResult
            // 
            this.lblXResult.AutoSize = true;
            this.lblXResult.Location = new System.Drawing.Point(50, 121);
            this.lblXResult.Name = "lblXResult";
            this.lblXResult.Size = new System.Drawing.Size(14, 13);
            this.lblXResult.TabIndex = 14;
            this.lblXResult.Text = "X";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grpAliveResult);
            this.groupBox2.Controls.Add(this.lblAliveResult);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.txtXIsAlive);
            this.groupBox2.Controls.Add(this.txtYIsAlive);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.grpHResult);
            this.groupBox2.Controls.Add(this.lblHResult);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtHealthX);
            this.groupBox2.Controls.Add(this.txtHealthY);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Location = new System.Drawing.Point(203, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 174);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WeakAuras Bars";
            // 
            // grpAliveResult
            // 
            this.grpAliveResult.BackColor = System.Drawing.Color.Gainsboro;
            this.grpAliveResult.Location = new System.Drawing.Point(128, 99);
            this.grpAliveResult.Name = "grpAliveResult";
            this.grpAliveResult.Size = new System.Drawing.Size(42, 20);
            this.grpAliveResult.TabIndex = 34;
            // 
            // lblAliveResult
            // 
            this.lblAliveResult.AutoSize = true;
            this.lblAliveResult.Location = new System.Drawing.Point(141, 121);
            this.lblAliveResult.Name = "lblAliveResult";
            this.lblAliveResult.Size = new System.Drawing.Size(10, 13);
            this.lblAliveResult.TabIndex = 33;
            this.lblAliveResult.Text = " ";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(98, 28);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(41, 13);
            this.label27.TabIndex = 28;
            this.label27.Text = "Is Alive";
            // 
            // txtXIsAlive
            // 
            this.txtXIsAlive.Location = new System.Drawing.Point(126, 44);
            this.txtXIsAlive.Name = "txtXIsAlive";
            this.txtXIsAlive.Size = new System.Drawing.Size(44, 20);
            this.txtXIsAlive.TabIndex = 29;
            this.txtXIsAlive.Text = " 1616";
            this.txtXIsAlive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtYIsAlive
            // 
            this.txtYIsAlive.Location = new System.Drawing.Point(127, 70);
            this.txtYIsAlive.Name = "txtYIsAlive";
            this.txtYIsAlive.Size = new System.Drawing.Size(44, 20);
            this.txtYIsAlive.TabIndex = 30;
            this.txtYIsAlive.Text = "278";
            this.txtYIsAlive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(98, 47);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(14, 13);
            this.label28.TabIndex = 31;
            this.label28.Text = "X";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(98, 73);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(14, 13);
            this.label29.TabIndex = 32;
            this.label29.Text = "Y";
            // 
            // grpHResult
            // 
            this.grpHResult.BackColor = System.Drawing.Color.Gainsboro;
            this.grpHResult.Location = new System.Drawing.Point(37, 99);
            this.grpHResult.Name = "grpHResult";
            this.grpHResult.Size = new System.Drawing.Size(42, 20);
            this.grpHResult.TabIndex = 27;
            // 
            // lblHResult
            // 
            this.lblHResult.AutoSize = true;
            this.lblHResult.Location = new System.Drawing.Point(50, 121);
            this.lblHResult.Name = "lblHResult";
            this.lblHResult.Size = new System.Drawing.Size(14, 13);
            this.lblHResult.TabIndex = 14;
            this.lblHResult.Text = "X";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Health";
            // 
            // txtHealthX
            // 
            this.txtHealthX.Location = new System.Drawing.Point(34, 44);
            this.txtHealthX.Name = "txtHealthX";
            this.txtHealthX.Size = new System.Drawing.Size(44, 20);
            this.txtHealthX.TabIndex = 4;
            this.txtHealthX.Text = " 1616";
            this.txtHealthX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtHealthY
            // 
            this.txtHealthY.Location = new System.Drawing.Point(35, 70);
            this.txtHealthY.Name = "txtHealthY";
            this.txtHealthY.Size = new System.Drawing.Size(44, 20);
            this.txtHealthY.TabIndex = 5;
            this.txtHealthY.Text = "278";
            this.txtHealthY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 47);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "X";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 13);
            this.label14.TabIndex = 7;
            this.label14.Text = "Y";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(222, 109);
            this.tabControl1.TabIndex = 17;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(214, 83);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bot";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rbHorde);
            this.tabPage2.Controls.Add(this.rbAlliance);
            this.tabPage2.Controls.Add(this.label39);
            this.tabPage2.Controls.Add(this.txtXInsignia);
            this.tabPage2.Controls.Add(this.txtYInsignia);
            this.tabPage2.Controls.Add(this.label40);
            this.tabPage2.Controls.Add(this.label41);
            this.tabPage2.Controls.Add(this.label36);
            this.tabPage2.Controls.Add(this.txtXLeaveBG);
            this.tabPage2.Controls.Add(this.txtYLeaveBG);
            this.tabPage2.Controls.Add(this.label37);
            this.tabPage2.Controls.Add(this.label38);
            this.tabPage2.Controls.Add(this.label33);
            this.tabPage2.Controls.Add(this.txtXLeave);
            this.tabPage2.Controls.Add(this.txtYLeave);
            this.tabPage2.Controls.Add(this.label34);
            this.tabPage2.Controls.Add(this.label35);
            this.tabPage2.Controls.Add(this.lblMouseCoordsResult);
            this.tabPage2.Controls.Add(this.label24);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.txtXOpenCloseWA);
            this.tabPage2.Controls.Add(this.txtYOpenCloseWA);
            this.tabPage2.Controls.Add(this.txtXJoin);
            this.tabPage2.Controls.Add(this.label25);
            this.tabPage2.Controls.Add(this.txtYJoin);
            this.tabPage2.Controls.Add(this.label26);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.label19);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtXBattlemaster);
            this.tabPage2.Controls.Add(this.txtYBattlemaster);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.txtWowProcess);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.grpMouseColor);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.txtMouseY);
            this.tabPage2.Controls.Add(this.txtMouseX);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(428, 489);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rbHorde
            // 
            this.rbHorde.AutoSize = true;
            this.rbHorde.Location = new System.Drawing.Point(333, 423);
            this.rbHorde.Name = "rbHorde";
            this.rbHorde.Size = new System.Drawing.Size(54, 17);
            this.rbHorde.TabIndex = 54;
            this.rbHorde.Text = "Horde";
            this.rbHorde.UseVisualStyleBackColor = true;
            // 
            // rbAlliance
            // 
            this.rbAlliance.AutoSize = true;
            this.rbAlliance.Checked = true;
            this.rbAlliance.Location = new System.Drawing.Point(333, 397);
            this.rbAlliance.Name = "rbAlliance";
            this.rbAlliance.Size = new System.Drawing.Size(62, 17);
            this.rbAlliance.TabIndex = 53;
            this.rbAlliance.TabStop = true;
            this.rbAlliance.Text = "Alliance";
            this.rbAlliance.UseVisualStyleBackColor = true;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(234, 388);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(43, 13);
            this.label39.TabIndex = 48;
            this.label39.Text = "Insignia";
            // 
            // txtXInsignia
            // 
            this.txtXInsignia.Location = new System.Drawing.Point(262, 420);
            this.txtXInsignia.Name = "txtXInsignia";
            this.txtXInsignia.Size = new System.Drawing.Size(44, 20);
            this.txtXInsignia.TabIndex = 49;
            this.txtXInsignia.Text = " 1616";
            this.txtXInsignia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtYInsignia
            // 
            this.txtYInsignia.Location = new System.Drawing.Point(263, 446);
            this.txtYInsignia.Name = "txtYInsignia";
            this.txtYInsignia.Size = new System.Drawing.Size(44, 20);
            this.txtYInsignia.TabIndex = 50;
            this.txtYInsignia.Text = "278";
            this.txtYInsignia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(234, 423);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(14, 13);
            this.label40.TabIndex = 51;
            this.label40.Text = "X";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(234, 449);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(14, 13);
            this.label41.TabIndex = 52;
            this.label41.Text = "Y";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(123, 388);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(55, 13);
            this.label36.TabIndex = 43;
            this.label36.Text = "Leave BG";
            // 
            // txtXLeaveBG
            // 
            this.txtXLeaveBG.Location = new System.Drawing.Point(151, 420);
            this.txtXLeaveBG.Name = "txtXLeaveBG";
            this.txtXLeaveBG.Size = new System.Drawing.Size(44, 20);
            this.txtXLeaveBG.TabIndex = 44;
            this.txtXLeaveBG.Text = " 1616";
            this.txtXLeaveBG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtYLeaveBG
            // 
            this.txtYLeaveBG.Location = new System.Drawing.Point(152, 446);
            this.txtYLeaveBG.Name = "txtYLeaveBG";
            this.txtYLeaveBG.Size = new System.Drawing.Size(44, 20);
            this.txtYLeaveBG.TabIndex = 45;
            this.txtYLeaveBG.Text = "278";
            this.txtYLeaveBG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(123, 423);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(14, 13);
            this.label37.TabIndex = 46;
            this.label37.Text = "X";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(123, 449);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(14, 13);
            this.label38.TabIndex = 47;
            this.label38.Text = "Y";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(258, 111);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(98, 13);
            this.label33.TabIndex = 38;
            this.label33.Text = "Battlemaster Leave";
            // 
            // txtXLeave
            // 
            this.txtXLeave.Location = new System.Drawing.Point(286, 127);
            this.txtXLeave.Name = "txtXLeave";
            this.txtXLeave.Size = new System.Drawing.Size(44, 20);
            this.txtXLeave.TabIndex = 39;
            this.txtXLeave.Text = "1557";
            this.txtXLeave.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtYLeave
            // 
            this.txtYLeave.Location = new System.Drawing.Point(287, 153);
            this.txtYLeave.Name = "txtYLeave";
            this.txtYLeave.Size = new System.Drawing.Size(44, 20);
            this.txtYLeave.TabIndex = 40;
            this.txtYLeave.Text = "517";
            this.txtYLeave.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(258, 130);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(14, 13);
            this.label34.TabIndex = 41;
            this.label34.Text = "X";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(258, 156);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(14, 13);
            this.label35.TabIndex = 42;
            this.label35.Text = "Y";
            // 
            // lblMouseCoordsResult
            // 
            this.lblMouseCoordsResult.AutoSize = true;
            this.lblMouseCoordsResult.Location = new System.Drawing.Point(344, 63);
            this.lblMouseCoordsResult.Name = "lblMouseCoordsResult";
            this.lblMouseCoordsResult.Size = new System.Drawing.Size(14, 13);
            this.lblMouseCoordsResult.TabIndex = 37;
            this.lblMouseCoordsResult.Text = "X";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(16, 388);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(67, 26);
            this.label24.TabIndex = 28;
            this.label24.Text = "Open/Close \r\nWA Macro";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(138, 111);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Battlemaster Join";
            // 
            // txtXOpenCloseWA
            // 
            this.txtXOpenCloseWA.Location = new System.Drawing.Point(44, 420);
            this.txtXOpenCloseWA.Name = "txtXOpenCloseWA";
            this.txtXOpenCloseWA.Size = new System.Drawing.Size(44, 20);
            this.txtXOpenCloseWA.TabIndex = 29;
            this.txtXOpenCloseWA.Text = " 1616";
            this.txtXOpenCloseWA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtYOpenCloseWA
            // 
            this.txtYOpenCloseWA.Location = new System.Drawing.Point(45, 446);
            this.txtYOpenCloseWA.Name = "txtYOpenCloseWA";
            this.txtYOpenCloseWA.Size = new System.Drawing.Size(44, 20);
            this.txtYOpenCloseWA.TabIndex = 30;
            this.txtYOpenCloseWA.Text = "278";
            this.txtYOpenCloseWA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtXJoin
            // 
            this.txtXJoin.Location = new System.Drawing.Point(166, 127);
            this.txtXJoin.Name = "txtXJoin";
            this.txtXJoin.Size = new System.Drawing.Size(44, 20);
            this.txtXJoin.TabIndex = 33;
            this.txtXJoin.Text = "1557";
            this.txtXJoin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(16, 423);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(14, 13);
            this.label25.TabIndex = 31;
            this.label25.Text = "X";
            // 
            // txtYJoin
            // 
            this.txtYJoin.Location = new System.Drawing.Point(167, 153);
            this.txtYJoin.Name = "txtYJoin";
            this.txtYJoin.Size = new System.Drawing.Size(44, 20);
            this.txtYJoin.TabIndex = 34;
            this.txtYJoin.Text = "517";
            this.txtYJoin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(16, 449);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(14, 13);
            this.label26.TabIndex = 32;
            this.label26.Text = "Y";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(138, 130);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(14, 13);
            this.label18.TabIndex = 35;
            this.label18.Text = "X";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(138, 156);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 13);
            this.label19.TabIndex = 36;
            this.label19.Text = "Y";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Battlemaster Target";
            // 
            // txtXBattlemaster
            // 
            this.txtXBattlemaster.Location = new System.Drawing.Point(38, 127);
            this.txtXBattlemaster.Name = "txtXBattlemaster";
            this.txtXBattlemaster.Size = new System.Drawing.Size(44, 20);
            this.txtXBattlemaster.TabIndex = 28;
            this.txtXBattlemaster.Text = "1557";
            this.txtXBattlemaster.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtYBattlemaster
            // 
            this.txtYBattlemaster.Location = new System.Drawing.Point(39, 153);
            this.txtYBattlemaster.Name = "txtYBattlemaster";
            this.txtYBattlemaster.Size = new System.Drawing.Size(44, 20);
            this.txtYBattlemaster.TabIndex = 29;
            this.txtYBattlemaster.Text = "517";
            this.txtYBattlemaster.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "X";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 156);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Y";
            // 
            // txtWowProcess
            // 
            this.txtWowProcess.Location = new System.Drawing.Point(118, 20);
            this.txtWowProcess.Name = "txtWowProcess";
            this.txtWowProcess.Size = new System.Drawing.Size(139, 20);
            this.txtWowProcess.TabIndex = 26;
            this.txtWowProcess.Text = "WowClassic";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "WoW Process Name";
            // 
            // grpMouseColor
            // 
            this.grpMouseColor.BackColor = System.Drawing.Color.Gainsboro;
            this.grpMouseColor.Location = new System.Drawing.Point(288, 56);
            this.grpMouseColor.Name = "grpMouseColor";
            this.grpMouseColor.Size = new System.Drawing.Size(42, 20);
            this.grpMouseColor.TabIndex = 24;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(328, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 22;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtMouseY
            // 
            this.txtMouseY.Enabled = false;
            this.txtMouseY.Location = new System.Drawing.Point(213, 56);
            this.txtMouseY.Name = "txtMouseY";
            this.txtMouseY.Size = new System.Drawing.Size(44, 20);
            this.txtMouseY.TabIndex = 21;
            this.txtMouseY.Text = "1557";
            this.txtMouseY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMouseX
            // 
            this.txtMouseX.Enabled = false;
            this.txtMouseX.Location = new System.Drawing.Point(118, 56);
            this.txtMouseX.Name = "txtMouseX";
            this.txtMouseX.Size = new System.Drawing.Size(44, 20);
            this.txtMouseX.TabIndex = 20;
            this.txtMouseX.Text = "1557";
            this.txtMouseX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(181, 59);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(14, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "Y";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(98, 59);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(14, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "X";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 59);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "Mouse Coords";
            // 
            // frm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 107);
            this.Controls.Add(this.tabControl1);
            this.Name = "frm1";
            this.Text = "Dummy";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button button1;

        public frm1()
        {
            InitializeComponent();
        }

        private Button button2;

        private void button2_Click(object sender, EventArgs e)
        {
            botIsRunning = false;
        }

        private Label label1;
        private Label label2;
        private TextBox txtCoordsXX;
        private TextBox txtCoordsXY;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox txtCoordsYY;
        private TextBox txtCoordsYX;
        private GroupBox groupBox1;
        private Label lblYResult;
        private Label lblXResult;
        private GroupBox groupBox2;
        private Label lblHResult;
        private Label label9;
        private TextBox txtHealthX;
        private TextBox txtHealthY;
        private Label label13;
        private Label label14;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox txtMouseY;
        private TextBox txtMouseX;
        private Label label17;
        private Label label16;
        private Label label15;

        private void Form1_Load(object sender, EventArgs e)
        {
            txtCoordsXX.Text = Settings1.Default.txtCoordsXX;
            txtCoordsXY.Text = Settings1.Default.txtCoordsXY;
            txtCoordsYY.Text = Settings1.Default.txtCoordsYY;
            txtCoordsYX.Text = Settings1.Default.txtCoordsYX;
            txtHealthX.Text = Settings1.Default.txtHealthX;
            txtHealthY.Text = Settings1.Default.txtHealthY;
            txtWowProcess.Text = Settings1.Default.ProcessName;
            txtXBattlemaster.Text = Settings1.Default.txtXBattlemaster;
            txtYBattlemaster.Text = Settings1.Default.txtYBattlemaster;
            txtXJoin.Text = Settings1.Default.txtXJoin;
            txtYJoin.Text = Settings1.Default.txtYJoin;
            txtXOpenCloseWA.Text = Settings1.Default.txtXOpenCloseWA;
            txtYOpenCloseWA.Text = Settings1.Default.txtYOpenCloseWA;
            txtXIsAlive.Text = Settings1.Default.txtXIsAlive;
            txtYIsAlive.Text = Settings1.Default.txtYIsAlive;
            txtXLeave.Text = Settings1.Default.txtXLeave;
            txtYLeave.Text = Settings1.Default.txtYLeave;
            txtXLeaveBG.Text = Settings1.Default.txtXLeaveBG;
            txtYLeaveBG.Text = Settings1.Default.txtYLeaveBG;
            txtXInsignia.Text = Settings1.Default.txtXInsignia;
            txtYInsignia.Text = Settings1.Default.txtYInsignia;


            rbAlliance.Checked = Settings1.Default.rbAlliance;
            rbHorde.Checked = !Settings1.Default.rbAlliance;



            startConfigThread();
        }

        private Button button3;

        private void button3_Click(object sender, EventArgs e)
        {
            Settings1.Default.txtCoordsXX = txtCoordsXX.Text;
            Settings1.Default.txtCoordsXY = txtCoordsXY.Text;
            Settings1.Default.txtCoordsYX = txtCoordsYX.Text;
            Settings1.Default.txtCoordsYY = txtCoordsYY.Text;
            Settings1.Default.txtHealthX = txtHealthX.Text;
            Settings1.Default.txtHealthY = txtHealthY.Text;
            Settings1.Default.ProcessName = txtWowProcess.Text;
            Settings1.Default.txtXBattlemaster = txtXBattlemaster.Text;
            Settings1.Default.txtYBattlemaster = txtYBattlemaster.Text;
            Settings1.Default.txtXJoin = txtXJoin.Text;
            Settings1.Default.txtYJoin = txtYJoin.Text;
            Settings1.Default.txtXOpenCloseWA = txtXOpenCloseWA.Text;
            Settings1.Default.txtYOpenCloseWA = txtYOpenCloseWA.Text;
            Settings1.Default.txtXIsAlive = txtXIsAlive.Text;
            Settings1.Default.txtYIsAlive = txtYIsAlive.Text;
            Settings1.Default.txtXLeave = txtXLeave.Text;
            Settings1.Default.txtYLeave = txtYLeave.Text;
            Settings1.Default.txtXLeaveBG = txtXLeaveBG.Text;
            Settings1.Default.txtYLeaveBG = txtYLeaveBG.Text;
            Settings1.Default.txtXInsignia = txtXInsignia.Text;
            Settings1.Default.txtYInsignia = txtYInsignia.Text;

            Settings1.Default.rbAlliance = rbAlliance.Checked;

            Settings1.Default.Save();
        }

        private Label grpMouseColor;
        private Label grpYResult;
        private Label grpXResult;
        private Label grpHResult;
        private TextBox txtWowProcess;
        private Label label7;
        private Label label8;
        private TextBox txtXBattlemaster;
        private TextBox txtYBattlemaster;
        private Label label10;
        private Label label11;
        private Label label12;
        private TextBox txtXJoin;
        private TextBox txtYJoin;
        private Label label18;
        private Label label19;
        private Label label24;
        private TextBox txtXOpenCloseWA;
        private TextBox txtYOpenCloseWA;
        private Label label25;
        private Label label26;
        private Label grpAliveResult;
        private Label lblAliveResult;
        private Label label27;
        private TextBox txtXIsAlive;
        private TextBox txtYIsAlive;
        private Label label28;
        private Label label29;
        private Label lblMouseCoordsResult;
        private Label label33;
        private TextBox txtXLeave;
        private TextBox txtYLeave;
        private Label label34;
        private Label label35;
        private Label label36;
        private TextBox txtXLeaveBG;
        private TextBox txtYLeaveBG;
        private Label label37;
        private Label label38;
        private Label label39;
        private TextBox txtXInsignia;
        private TextBox txtYInsignia;
        private Label label40;
        private Label label41;
        private RadioButton rbHorde;
        private RadioButton rbAlliance;


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(((TabControl)sender).SelectedIndex == 0)
            {
                frm1.ActiveForm.SetBounds(frm1.ActiveForm.Location.X, frm1.ActiveForm.Location.Y, 235, 145);
            }
            else if(((TabControl)sender).SelectedIndex == 1)
            {
                frm1.ActiveForm.SetBounds(frm1.ActiveForm.Location.X, frm1.ActiveForm.Location.Y, 450, 552);
            }
        }
    }
}
