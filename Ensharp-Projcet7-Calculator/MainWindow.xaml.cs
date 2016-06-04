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

        // 전의 계산이 무엇인지 체크하
        int BEFORE_CAL = 0;

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

            func.btn_plus.Click += btn_Plus_Click;
            func.btn_minus.Click += btn_Minus_Click;
            func.btn_multiple.Click += btn_Multiple_Click;
            func.btn_division.Click += btn_Division_Click;
            func.btn_result.Click += btn_Result_Click;

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
                case Key.Add:
                case Key.OemPlus:
                    //calculator("＋");
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    //calculator("-");
                    break;
                case Key.Divide:
                    //calculator("÷");
                    break;
                case Key.Multiply:
                    //calculator("×");
                    break;
                case Key.Return:
                    //result_Click(null, null);
                    break;
                case Key.C:
                    resetAll(null, null);
                    break;
            }
        }

        // 드래그 했을 때 창 이동시켜주는 메소드
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
            appearNumber(((ContentControl)sender).Content.ToString());
        }

        // 숫자를 나타내주는 기능
        public void appearNumber(string num)
        {
            if (MODE_CAL)
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

        public void btn_Plus_Click(object sender, RoutedEventArgs e)
        {
            // 해당 연산모드를 가져오고
            string calMode = ((ContentControl)sender).Content.ToString();

            int compareOp = setFormula(calMode);

            if (BEFORE_CAL.Equals(compareOp) || COUNT_OPERATION.Equals(0))
            {
                // 그다음에 연산결과에 +=
                calculator(compareOp);
            }
            else
            {
                calculator(BEFORE_CAL);
            }

            // 해당모드의 연산자로 지정
            BEFORE_CAL = compareOp;

            // 입력하는 부분 위에 기록 남겨둠
            func.lbl_result.Content += strOnlyNumber + calMode;

            // 입력한 값 OnlyNumber 에 Write
            strOnlyNumber = storeNum.ToString();

            // 콤마찍어주고 화면에 보여줌
            setComma(strOnlyNumber);
            appearNumber();

            // 연산이 되었음을 알려줌 (숫자입력시 초기화를 위해)
            MODE_CAL = true;

            // 연산 횟수 증가시켜줌
            COUNT_OPERATION++;
        }

        public void btn_Minus_Click(object sender, RoutedEventArgs e)
        {
            // 해당 연산모드를 가져오고
            string calMode = ((ContentControl)sender).Content.ToString();

            int compareOp = setFormula(calMode);

            if (BEFORE_CAL.Equals(compareOp) || COUNT_OPERATION.Equals(0))
            {
                // 그다음에 연산결과에 +=
                calculator(compareOp);
            }
            else
            { 
               calculator(BEFORE_CAL);
            }

            // 이제 해당모드의 연산자로 지정
            BEFORE_CAL = compareOp;

            // 입력하는 부분 위에 기록 남겨둠
            func.lbl_result.Content += strOnlyNumber + calMode;

            // 입력한 값 OnlyNumber 에 Write
            strOnlyNumber = storeNum.ToString();

            // 콤마찍어주고 화면에 보여줌
            setComma(strOnlyNumber);
            appearNumber();

            // 연산이 되었음을 알려줌 (숫자입력시 초기화를 위해)
            MODE_CAL = true;

            // 연산 횟수 증가시켜줌
            COUNT_OPERATION++;
        }

        public void btn_Division_Click(object sender, RoutedEventArgs e)
        {
            // 해당 연산모드를 가져오고
            string calMode = ((ContentControl)sender).Content.ToString();

            int compareOp = setFormula(calMode);

            if ((!strOnlyNumber.Equals("")) && storeNum.Equals(0))
            {
                double num = double.Parse(strOnlyNumber);
            }
            else if (storeNum.Equals(0) && COUNT_OPERATION.Equals(0))
            {
                // 이제 해당모드의 연산자로 지정
                BEFORE_CAL = compareOp;

                // 입력한 값 OnlyNumber 에 Write
                strOnlyNumber = storeNum.ToString();

                // 입력하는 부분 위에 기록 남겨둠
                func.lbl_result.Content += strOnlyNumber + calMode;

                // 콤마찍어주고 화면에 보여줌
                setComma(strOnlyNumber);
                appearNumber();

                // 연산이 되었음을 알려줌 (숫자입력시 초기화를 위해)
                MODE_CAL = true;

                // 연산 횟수 증가시켜줌
                COUNT_OPERATION++;

                return;
            }
            
            if (BEFORE_CAL.Equals(compareOp) || COUNT_OPERATION.Equals(0))
            {
                // 그다음에 연산결과에 +=
                calculator(compareOp);
            }
            else
            {
                calculator(BEFORE_CAL);
            }

            // 이제 해당모드의 연산자로 지정
            BEFORE_CAL = compareOp;

            // 입력하는 부분 위에 기록 남겨둠
            func.lbl_result.Content += strOnlyNumber + calMode;

            // 입력한 값 OnlyNumber 에 Write
            strOnlyNumber = storeNum.ToString();

            // 콤마찍어주고 화면에 보여줌
            setComma(strOnlyNumber);
            appearNumber();

            // 연산이 되었음을 알려줌 (숫자입력시 초기화를 위해)
            MODE_CAL = true;

            // 연산 횟수 증가시켜줌
            COUNT_OPERATION++;
        }

        public void btn_Multiple_Click(object sender, RoutedEventArgs e)
        {
            if(strOnlyNumber.Length > 0 && storeNum.Equals(0))
            {
                storeNum = 1;
            }

            // 해당 연산모드를 가져오고
            string calMode = ((ContentControl)sender).Content.ToString();

            int compareOp = setFormula(calMode);

            if (BEFORE_CAL.Equals(compareOp) || COUNT_OPERATION.Equals(0))
            {
                // 그다음에 연산결과에 +=
                calculator(compareOp);
            }
            else
            {
                calculator(BEFORE_CAL);
            }

            // 이제 해당모드의 연산자로 지정
            BEFORE_CAL = compareOp;

            // 입력하는 부분 위에 기록 남겨둠
            func.lbl_result.Content += strOnlyNumber + calMode;

            // 입력한 값 OnlyNumber 에 Write
            strOnlyNumber = storeNum.ToString();

            // 콤마찍어주고 화면에 보여줌
            setComma(strOnlyNumber);
            appearNumber();

            // 연산이 되었음을 알려줌 (숫자입력시 초기화를 위해)
            MODE_CAL = true;

            // 연산 횟수 증가시켜줌
            COUNT_OPERATION++;
        }

        public void btn_Result_Click(object sender, RoutedEventArgs e)
        {
            // 해당 연산모드를 가져오고
            string calMode = ((ContentControl)sender).Content.ToString();

            // 이전의 연산결과를 수행한다
            calculator(BEFORE_CAL);

            strOnlyNumber = storeNum.ToString();

            // 결과패널 사라지게
            func.lbl_result.Content = "";

            // 콤마찍어주고 화면에 보여줌
            setComma(strOnlyNumber);
            appearNumber();
        }

        public void calculator(int op)
        {
            switch (op)
            {
                case 1:
                    storeNum += double.Parse(strOnlyNumber);
                    break;
                case 2:
                    storeNum -= double.Parse(strOnlyNumber);
                    break;
                case 3:
                    storeNum *= double.Parse(strOnlyNumber);
                    break;
                case 4:
                    storeNum /= double.Parse(strOnlyNumber);
                    break;
            }
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


    }
}
