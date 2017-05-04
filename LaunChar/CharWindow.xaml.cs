using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;

// 非同期タスク
using System.Threading;

namespace LaunChar
{
    public partial class CharWindow : Window
    {
        private CancellationTokenSource tokenSource = null;

        private double prevTop = 0;
        private const int defaultG = -20;
        private int gravity = defaultG;
        private int adder = 0;

        // ジャンプ力ぅ
        private int jumpPower = 100;

        public CharWindow(Keys keyCode)
        {
            InitializeComponent();

            this.Left = (double)MainWindow.KeyTable[keyCode];
            this.Char.Text = keyCode.ToString();

            // 各パラメータの初期値を設定
            this.Top = MainWindow.ScreenHeight;
            prevTop = this.Top;
            jumpPower += MainWindow.Rand.Next(-5, 5);
            gravity = jumpPower;
            adder = MainWindow.Rand.Next(-10, 10);

            if(tokenSource == null) tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            PhysicsThread(token);
        }

        public int Hoge()
        {
            return 1;
        }

        /// <summary>
        ///	このウィンドウの落下を制御するスレッド
        /// </summary>
        public async void PhysicsThread(CancellationToken token)
        {
            double tmp = 0;
            double diff = 0;
            await Task.Run(() =>
            {
                while(true)
                {
                    if(token.IsCancellationRequested) { break; }
                    this.Dispatcher.Invoke(() =>
                    {
                        this.Left += adder;
                        tmp = this.Top;
                        diff = this.Top - prevTop;
                        this.Top += diff - gravity;
                        prevTop = tmp;

                        if(gravity == jumpPower) { gravity = defaultG; }

                        // 画面外へ落ちたらループを抜ける
                        if(this.Top > MainWindow.ScreenHeight) { this.Cancel(); }
                    });
                    Thread.Sleep(100);
                }
            }, token);
            this.Close();
        }

        /// <summary>
        /// スレッドを終了させる
        /// </summary>
        public void Cancel()
        {
            tokenSource.Cancel();
        }
    }
}
