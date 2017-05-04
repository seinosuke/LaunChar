using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace LaunChar
{
    public partial class MainWindow : Window
    {
        private KeyboardHook keyboardHook;

        private System.Drawing.PointF dpiFactor;

        private static Random rand = new System.Random();
        public static Random Rand { get { return MainWindow.rand; } } 

        // 各キーを飛ばす位置を格納したテーブル
        private static Hashtable keyTable;
        public static Hashtable KeyTable {
            get { return keyTable; }
        }

        // 画面の幅と高さ
        private static double screenWidth;
        public static double ScreenWidth { get { return MainWindow.screenWidth; } }
        private static double screenHeight;
        public static double ScreenHeight { get { return MainWindow.screenHeight; } }

        public MainWindow()
        {
            InitializeComponent();

            // アイコンをセット
            this.Icon = new BitmapImage(
                new Uri("pack://application:,,,/launchar.ico", UriKind.Absolute)
            );

            keyboardHook = new KeyboardHook(OnKeyboardKeyDown);

            // ディスプレイの高さと幅を取得する
            dpiFactor = GetDpiFactor();
            var area = System.Windows.Forms.Screen.GetWorkingArea(System.Drawing.Point.Empty);
            screenWidth = (area.Width / dpiFactor.X);
            screenHeight = (area.Height / dpiFactor.Y);

            SetKeyTable();
        }

        /// <summary>
        /// キーが押されたとき文字ウィンドウを生成する
        /// </summary>
        private void OnKeyboardKeyDown(object sender, KeyboardHookedEventArgs e)
        {
            if (e.UpDown == KeyboardUpDown.Down)
            {
                if (keyTable[e.KeyCode] != null) {
                    CharWindow window = new CharWindow(e.KeyCode);
                    window.Show();
                }
            }
        }

        /// <summary>
        /// ウィンドウを閉じるとき keyboardHook も破棄する
        /// </summary>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            base.OnClosing(e);
            keyboardHook.Dispose();
        }

        /// <summary>
        /// keyTable を生成する
        /// </summary>
        private void SetKeyTable() {
            double tmp = screenWidth / 12;
            keyTable = new Hashtable();

            keyTable[Keys.Q] = tmp * 1;
            keyTable[Keys.W] = tmp * 2;
            keyTable[Keys.E] = tmp * 3;
            keyTable[Keys.R] = tmp * 4;
            keyTable[Keys.T] = tmp * 5;
            keyTable[Keys.Y] = tmp * 6;
            keyTable[Keys.U] = tmp * 7;
            keyTable[Keys.I] = tmp * 8;
            keyTable[Keys.O] = tmp * 9;
            keyTable[Keys.P] = tmp * 10;

            keyTable[Keys.A] = tmp * 1;
            keyTable[Keys.S] = tmp * 2;
            keyTable[Keys.D] = tmp * 3;
            keyTable[Keys.F] = tmp * 4;
            keyTable[Keys.G] = tmp * 5;
            keyTable[Keys.H] = tmp * 6;
            keyTable[Keys.J] = tmp * 7;
            keyTable[Keys.K] = tmp * 8;
            keyTable[Keys.L] = tmp * 9;

            keyTable[Keys.Z] = tmp * 2;
            keyTable[Keys.X] = tmp * 3;
            keyTable[Keys.C] = tmp * 4;
            keyTable[Keys.V] = tmp * 5;
            keyTable[Keys.B] = tmp * 6;
            keyTable[Keys.N] = tmp * 7;
            keyTable[Keys.M] = tmp * 8;
        }

        public System.Drawing.PointF GetDpiFactor() {
            var g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            return new System.Drawing.PointF((float)(g.DpiX / 96.0), (float)(g.DpiY / 96.0));
        }
    }
}
