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

        double first = 0;
        double second = 0;

        // 연산모드
        bool MODE_CAL = false;

        // = 이 연속으로 찍히는지 CHECK
        bool MODE_RESULT_CONTINUES = false;

        // 이전의 연산값을 알기위한 데이터
        int BEFORE_MODE = 0;

        // 연산이 몇번 됬는지도 체크
        int COUNT_OPERATION = 0;

        // 수식 const
        const int PLUS = 1;
        const int MINUS = 2;
        const int DIVISION = 3;
        const int MULTIPLE = 4;

        // 현재의 수식을 저장하기 위한 데이터
        int currentFormula = 0;

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

            func.btn_plus.Click += calButton_Click;
            func.btn_minus.Click += calButton_Click;
            func.btn_multiple.Click += calButton_Click;
            func.btn_division.Click += calButton_Click;

            func.btn_c.Click += resetAll;
            func.btn_ce.Click += resetInput;
            func.btn_backspace.Click += backSpace_Click;
            func.btn_plusAndMinus.Click += plusAndMinus_Click;
            func.btn_result.Click += result_Click;
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
                case Key.Add:
                case Key.OemPlus:
                    calculator("＋");
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    calculator("-");
                    break;
                case Key.Divide:
                    calculator("÷");
                    break;
                case Key.Multiply:
                    calculator("×");
                    break;
                case Key.Return:
                    result_Click(null, null);
                    break;
                case Key.C:
                    resetAll(null, null);
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
            MODE_CAL = false;
            MODE_RESULT_CONTINUES = false;
            currentFormula = 0;
            first = 0;
            second = 0;
            resetInput(sender, e);

            BEFORE_MODE = 0;
        }

        // 숫자 버튼이 클릭되었을 때
        public void btn_Num_Click(object sender, RoutedEventArgs e)
        {
            MODE_CAL = false;
            MODE_RESULT_CONTINUES = false;
            appearNumber(((ContentControl)sender).Content.ToString());
        }

        // 숫자를 나타내주는 기능
        public void appearNumber(string num)
        {
            if(MODE_CAL)
            {
                strOnlyNumber = "";
                strCurrentNum = "0";
                appearNumber();
                MODE_CAL = false;
            }

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
            setComma(strOnlyNumber);

            // Label Update
            appearNumber();
        }

        // 콤마 찍어주는 메소드
        public void setComma(string str)
        {
            if (str.Length >= 3)
            {
                double tmpNum;
                tmpNum = double.Parse(str);
                strCurrentNum = tmpNum.ToString("#,##0");
            }
            else
            {
                strCurrentNum = str;
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
                strOnlyNumber = "";
            }
            else if (MODE_CAL || MODE_RESULT_CONTINUES)
            {
                return;
            }
            else
            {
                var len = strOnlyNumber.Length - 1;
                strOnlyNumber = strOnlyNumber.Remove(len);
                setComma(strOnlyNumber);
            }
            appearNumber();
        }

        // 음수양수 전환하는 버튼을 눌렀을 때
        public void plusAndMinus_Click(object sender, RoutedEventArgs e)
        {
            storeNum = double.Parse(strCurrentNum);
            storeNum *= -1;
            strOnlyNumber = storeNum.ToString();
            setComma(strOnlyNumber);
            appearNumber();
        }

        // 계산쪽의 버튼을 눌렀을때 (+, -, *, ÷, =)
        public void calButton_Click(object sender, RoutedEventArgs e)
        {
            string calMode = ((ContentControl)sender).Content.ToString();
            calculator(calMode);
        }

        // 계산하는 메소드
        public void calculator(string calMode)
        {
            // 전의 연산값을 미리 더해줌
            if(COUNT_OPERATION > 0)
            {
                first = second + double.Parse(strOnlyNumber);
                func.lbl_current.Content = first;
                strOnlyNumber = first.ToString();
                strCurrentNum = first.ToString();
            }

            MODE_RESULT_CONTINUES = false;
            BEFORE_MODE = setFormula(calMode);

            // 이미 계산이 선택 됬다면
            if (MODE_CAL)
            {
                string str = func.lbl_result.Content.ToString();

                func.lbl_result.Content = func.lbl_result.Content.ToString().Remove(str.Length - 1);
                func.lbl_result.Content += calMode;
                currentFormula = setFormula(calMode);
                return;
            }
            else
            {
                //second = first;
                //first = 0;
                func.lbl_result.Content += strOnlyNumber + calMode;
            }

            MODE_CAL = true;

            if (BEFORE_MODE > 0 && COUNT_OPERATION > 0) { }

            else
            {
                switch (calMode)
                {
                    case "＋":
                        calculatorByOpeartion(PLUS);
                        break;
                    case "-":
                        calculatorByOpeartion(MINUS);
                        break;
                    case "×":
                        calculatorByOpeartion(MULTIPLE);
                        break;
                    case "÷":
                        calculatorByOpeartion(DIVISION);
                        break;
                }
            }

            COUNT_OPERATION++;
        }

        public void calculatorByOpeartion(int operation)
        {
            second = double.Parse(strOnlyNumber);

            if (operation.Equals(1)) { first += second; }
            else if (operation.Equals(2)) { first -= second; }
            else if (operation.Equals(3)) { first *= second; }
            else if (operation.Equals(4)) { first /= second; }

            strCurrentNum = first.ToString();
            strOnlyNumber = first.ToString();
            setComma(strCurrentNum);
            appearNumber();
            currentFormula = operation;
        }


        public int setFormula(string str)
        {
            if (str.Equals("＋"))
                return 1;
            else if (str.Equals("-"))
                return 2;
            else if (str.Equals("×"))
                return 3;
            else if (str.Equals("÷"))
                return 4;

            return 0;
        }
        
        // 결과 메소드
        public void result_Click(object sender, RoutedEventArgs e)
        {
            // 연산이 선택되지 않았다면
            if (currentFormula.Equals(0)) { return; }
            else { second = double.Parse(strOnlyNumber); }

            // = 연산이 이미 눌린 상태라면
            if (MODE_RESULT_CONTINUES) { }
            //else { second = first; }

            // = 연산이 선택됬음을 체크
            MODE_RESULT_CONTINUES = true;

            switch (currentFormula)
            {
                case PLUS:
                    first += second;
                    break;
                case MINUS:
                    first -= second;
                    break;
                case MULTIPLE:
                    first /= second;
                    break;
                case DIVISION:
                    first *= second;
                    break;
            }

            strOnlyNumber = first.ToString();
            setComma(strOnlyNumber);
            func.lbl_result.Content = "";
            appearNumber();

            MODE_CAL = false;
        }
    }
}
