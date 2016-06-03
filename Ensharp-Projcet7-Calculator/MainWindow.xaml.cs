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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ensharp_Projcet7_Calculator
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        FunctionControl func;

        /*                  DATA                    */
        // 출력을 위한 데이터
        string strCurrentNum = "0";

        // 실제 계산을 위한 문자열
        string strOnlyNumber = "";

        // 실제 계산을 위한 정수형 데이터
        double storeNum = 0;

        //// 콤마를 찍기위한 데이터
        //int commaCount = 0;

        // 콤마모드
        const bool MODE_COMMA = false;

        public MainWindow()
        {
            InitializeComponent();

            func = new FunctionControl();

            MainGrid.Children.Add(func);

            appearNumber();

            func.btn_zero.Click += btn_Num_Click;
            func.btn_one.Click += btn_Num_Click;
            func.btn_two.Click += btn_Num_Click;
            func.btn_three.Click += btn_Num_Click;
            func.btn_four.Click += btn_Num_Click;
            func.btn_five.Click += btn_Num_Click;
            func.btn_six.Click += btn_Num_Click;
            func.btn_seven.Click += btn_Num_Click;
            func.btn_eight.Click += btn_Num_Click;
            func.btn_nine.Click += btn_Num_Click;

            func.btn_c.Click += resetAll;
            func.btn_ce.Click += resetInput;
            func.btn_backspace.Click += backSpace_Click;
            func.btn_plusAndMinus.Click += plusAndMinus_Click;
        }

        // Key가 입력됬을 때 버튼이 눌리는 Event 와 똑같이 처리
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.D0:
                    appearNumber("0");
                    break;
                case Key.NumPad1:
                case Key.D1:
                    appearNumber("1");
                    break;
                case Key.NumPad2:
                case Key.D2:
                    appearNumber("2");
                    break;
                case Key.NumPad3:
                case Key.D3:
                    appearNumber("3");
                    break;
                case Key.NumPad4:
                case Key.D4:
                    appearNumber("4");
                    break;
                case Key.NumPad5:
                case Key.D5:
                    appearNumber("5");
                    break;
                case Key.NumPad6:
                case Key.D6:
                    appearNumber("6");
                    break;
                case Key.NumPad7:
                case Key.D7:
                    appearNumber("7");
                    break;
                case Key.NumPad8:
                case Key.D8:
                    appearNumber("8");
                    break;
                case Key.NumPad9:
                case Key.D9:
                    appearNumber("9");
                    break;
                case Key.X:
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Back:
                    backSpace_Click(null, null);
                    break;
            }
        }

        private void titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // 숫자를 출력한다.
        public void appearNumber()
        {
            func.lbl_current.Content = strCurrentNum;
        }

        // 입력 부분만 초기화 해주는 기능 - CE
        public void resetInput(object sender, RoutedEventArgs e)
        {
            func.lbl_current.Content = "0";
            strCurrentNum = "";
            strOnlyNumber = "";
            storeNum = 0;
        }

        // 전부 초기화 해주는 기능 - C
        public void resetAll(object sender, RoutedEventArgs e)
        {
            func.lbl_result.Content = "";
            resetInput(sender, e);
        }

        // 숫자 버튼이 클릭되었을 때
        public void btn_Num_Click(object sender, RoutedEventArgs e)
        {
            appearNumber(((ContentControl)sender).Content.ToString());
        }

        // 숫자를 나타내주는 기능
        public void appearNumber(string num)
        {
            // 더이상 숫자 입력이 되면 안될 때
            if (strOnlyNumber.Length.Equals(16)) { return; }

            // 맨 처음상태라면
            if (strCurrentNum.Equals("0"))
            {
                strCurrentNum = "";
            }

            // 해당 눌린 버튼의 Content 를 추출
            string pushedBtnContent = num;

            // 입력된 버튼을 string 끼리 더해줌
            strCurrentNum += pushedBtnContent;
            // 계산을 위해 숫자만 더해진 문자열을 따로 저장
            strOnlyNumber += pushedBtnContent;

            // 숫자에 콤마를 찍어준다
            setComma();

            // Label Update
            appearNumber();
        }

        // 콤마 찍어주는 메소드
        public void setComma()
        {
            if (strOnlyNumber.Length >= 3)
            {
                double tmpNum;
                tmpNum = double.Parse(strOnlyNumber);
                strCurrentNum = tmpNum.ToString("#,##0");
            }
            else
            {
                strCurrentNum = strOnlyNumber;
            }
        }

        // 뒤로가기 버튼을 눌렀을 때
        // 숫자의 카운트가 0 일때
        public void backSpace_Click(object sender, RoutedEventArgs e)
        {
            if (strOnlyNumber.Length.Equals(0))
            {
                return;
            }
            else if (strOnlyNumber.Length.Equals(1))
            {
                strCurrentNum = "0";
            }
            else
            {
                var len = strOnlyNumber.Length - 1;

                strOnlyNumber = strOnlyNumber.Remove(len);
                setComma();
            }
            appearNumber();
        }

        // 음수양수 전환하는 버튼을 눌렀을 때
        public void plusAndMinus_Click(object sender, RoutedEventArgs e)
        {
            storeNum = double.Parse(strCurrentNum);

            storeNum *= -1;

            strOnlyNumber = storeNum.ToString();

            setComma();

            appearNumber();
        }







    }
}
