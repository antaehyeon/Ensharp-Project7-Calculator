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
        double storeNum = 0.0;

        // 전의 계산이 무엇인지 체크하
        int BEFORE_CAL = 0;

        // 연산모드
        bool MODE_CAL = false;

        // = 이 연속으로 찍히는지 CHECK
        bool MODE_RESULT_CONTINUE = false;

        // 이전의 연산값을 알기위한 데이터
        int BEFORE_MODE = 0;

        // 연산이 몇번 됬는지도 체크
        int COUNT_OPERATION = 0;

        // = 연산 모드 체크
        int MODE_RESU = 0;

        // 연산이 두번연속으로 눌렸을 때 원래의 값을 저장하기 위한 데이터
        double beforeDataForResult = 0;

        // 현재 값이 음수인지 체크하는 모드
        bool MODE_NEGATIVE = false;

        // CE가 눌렸는지 안눌렸는지 체크하는 모드
        bool MODE_CE = false;

        // = 연산이 되고나서 숫자입력 후에 = 연산이 다시 들어올때
        bool MODE_RESULTNEXTINPUT = false;

        // CE 가 눌리고나서 숫자가 눌렸는지 판단
        bool MODE_CEANDNUMBER = false;

        // 뒤로가기 버튼이 눌렸는지 체크
        bool MODE_BACK = false;

        // +/- 전환을 한번이라도 했다면, 체크
        bool MODE_PM = false;

        // . Mode 를 전환해줌
        bool MODE_DOT = false;

        // . 찍히고 나서 숫자가 입력됬을경우
        // = 에서 판단하기 위함임 ( 0. = 은 0 , 0.숫자는 = 0.숫자 그대로 들어가기 때문에)
        bool MODE_DotNextNumber = false;

        // . 이 찍히기 전 앞부분
        string beforeDotNum = "";

        // 0. 에서 +/- 전환하기 위해 체크 데이터
        bool MODE_plusAndMinusForDotMode = false;

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

            func.btn_plus.Click += btn_Operation_Click;
            func.btn_minus.Click += btn_Operation_Click;
            func.btn_multiple.Click += btn_Operation_Click;
            func.btn_division.Click += btn_Operation_Click;
            func.btn_result.Click += btn_Result_Click;
            func.btn_dot.Click += btn_dot_Click;

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
                    calculatorByOperation("＋");
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    calculatorByOperation("-");
                    break;
                case Key.Divide:
                    calculatorByOperation("÷");
                    break;
                case Key.Multiply:
                    calculatorByOperation("×");
                    break;
                case Key.Enter:
                    btn_Result_Click(null, null);
                    break;
                case Key.C:
                    resetAll(null, null);
                    break;
                case Key.E:
                    resetInput(null, null);
                    break;
                case Key.M:
                    plusAndMinus_Click(null, null);
                    break;
                case Key.Decimal:
                    btn_dot_Click(null, null);
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
            strOnlyNumber = "";
            strCurrentNum = "0";

            // 연산자의 라벨만 바꿔주는 조건을 만족시켜주기위해
            MODE_CAL = false;
            // CE가 눌렸다고 알려줌
            MODE_CE = true;

            // 음수모드 초기화
            MODE_NEGATIVE = false;

            MODE_PM = false;

            MODE_BACK = false;

            MODE_CEANDNUMBER = false;

            MODE_DOT = false;
            beforeDotNum = "";
            MODE_DotNextNumber = false;
            MODE_plusAndMinusForDotMode = false;

            appearNumber();
        }

        // 전부 초기화 해주는 기능 - C
        public void resetAll(object sender, RoutedEventArgs e)
        {
            func.lbl_result.Content = "";
            strCurrentNum = "0";
            strOnlyNumber = "";
            storeNum = 0.0;
            BEFORE_CAL = 0;
            MODE_CAL = false;
            MODE_RESULT_CONTINUE = false;
            BEFORE_MODE = 0;
            COUNT_OPERATION = 0;
            MODE_RESU = 0;
            beforeDataForResult = 0;
            MODE_NEGATIVE = false;
            currentFormula = 0;
            MODE_CE = false;
            MODE_CEANDNUMBER = false;
            MODE_BACK = false;
            MODE_PM = false;

            MODE_DOT = false;
            beforeDotNum = "";
            MODE_DotNextNumber = false;
            MODE_plusAndMinusForDotMode = false;

            appearNumber();
        }

        // 숫자 버튼이 클릭되었을 때
        public void btn_Num_Click(object sender, RoutedEventArgs e)
        {
            appearNumber(((ContentControl)sender).Content.ToString());
        }

        // 숫자를 나타내주는 기능
        public void appearNumber(string numStr)
        {
            // 명호 연산 해결하기 위해 (= 계속눌리고나서 새로 숫자 입력된다음에 다시 =)
            //MODE_RESULT_CONTINUE = false;

            // = 연산이 눌리고 나서 다시 숫자가 입력되는데, 실수모드일 경우
            if (MODE_RESULT_CONTINUE && MODE_DOT)
            {
                MODE_DOT = false;
            }

            // 실수모드라면 (.이 눌린상태라면)
            if (MODE_DOT)
            {
                realNumber(numStr);
                // 점찍히고 숫자눌렸다는것을 체크
                // = 에서 쓰기위함임
                MODE_DotNextNumber = true;
                return;
            }

            if (MODE_CAL)
            {
                strOnlyNumber = "";
                strCurrentNum = "0";
                appearNumber();
                MODE_CAL = false;
            }

            // CE 가 눌리고나서 숫자가 눌렸을경우
            if (MODE_CE)
            {
                // CE그리고 숫자 같이눌림은 킴
                MODE_CEANDNUMBER = true;
                // MODE CE는 끔
                MODE_CE = false;
            }

            // 더이상 숫자 입력이 되면 안될 때
            if (strOnlyNumber.Length.Equals(16)) { return; }

            // 맨 처음상태인데, 0이 입력될경우
            else if (strCurrentNum.Equals("0"))
            {
                strCurrentNum = "";
                strOnlyNumber = "";
            }
            // = 눌리고 나서 연산자가 들어올 경우
            else if (MODE_RESULT_CONTINUE)
            {
                storeNum = double.Parse(numStr);
                strCurrentNum = "";
                strOnlyNumber = "";
                MODE_RESULT_CONTINUE = false;
                // =눌리고 나서 숫자입력이 됬음을 알려주는 데이터
                MODE_RESULTNEXTINPUT = true;
            }

            // 입력된 버튼을 string 끼리 더해줌
            strCurrentNum += numStr;

            // 계산을 위해 숫자만 더해진 문자열을 따로 저장
            strOnlyNumber += numStr;

            // 숫자에 콤마를 찍어준다
            setComma(strOnlyNumber);

            // Label Update
            appearNumber();
        }

        // 실수 모드
        void realNumber(string number)
        {
            // 해당 들어온 숫자를 더해줌
            // 앞에 정수부분은 따로 저장되어 있음 (beforeDotNum)
            strCurrentNum += number;
            strOnlyNumber += number;
            appearNumber();

            // 실제 실수형 데이터를 저장시킴 (storeNum)
            //storeNum = Convert.ToDouble(strOnlyNumber); 

            // CE 가 눌리고나서 숫자가 눌렸을경우
            if (MODE_CE)
            {
                // CE그리고 숫자 같이눌림은 킴
                MODE_CEANDNUMBER = true;
                // MODE CE는 끔
                MODE_CE = false;
            }
        }

        // 콤마 찍어주는 메소드
        public void setComma(string str)
        {
            double tmpNum;
            tmpNum = Convert.ToDouble(str);

            // 실수라면 (문자열에 . 이 없으면 -1을 반환함)
            if(!str.IndexOf('.').Equals(-1))
            {
                // 받아온 문자열을 점으로 자른다
                string[] result = str.Split('.');

                // 소수점 앞부분은 콤마구분을 해주기 위해서 DOUBLE형으로 변환
                double theFrontResult = double.Parse(result[0]);
                // 소수점 앞부분 콤마찍은 문자열이랑 + 콤마 + 뒤에 소수점데이터
                strCurrentNum = theFrontResult.ToString("#,##0") + "." + result[1];

                // 마이너스 모드라면 맨앞에 - 를 더해줌
                // 단, . 앞의숫자가 0일경우에만
                if(MODE_NEGATIVE && beforeDotNum.Equals("0"))
                {
                    strCurrentNum = "-" + strCurrentNum;
                }
                return;
            }

            strCurrentNum = tmpNum.ToString("#,##0");
        }

        // 뒤로가기 버튼을 눌렀을 때
        // 숫자의 카운트가 0 일때
        public void backSpace_Click(object sender, RoutedEventArgs e)
        {
            // 숫자만 저장되있는 문자열의 길이를 계산
            var len = strOnlyNumber.Length - 1;

            // 뒤로가기가 눌렸다를 알려줌
            MODE_BACK = true;

            // 아무것도 입력이 되지 않은 상태일 경우
            if (strOnlyNumber.Length.Equals(0))
            {
                return;
            }
            // 한자리만 남았을 경우
            else if (strOnlyNumber.Length.Equals(1))
            {
                strCurrentNum = "0";
                strOnlyNumber = "";
            }
            // 두자리 남았는데 음수일경우
            else if (strOnlyNumber.Length.Equals(2) && MODE_NEGATIVE)
            {
                strCurrentNum = "0";
                strOnlyNumber = "";
                MODE_NEGATIVE = false;
            }
            // 연산자가 선택됬거나 =이 연속적으로 눌린 상태일 경우 뒤로가기 버튼 비활성화
            else if (MODE_CAL || MODE_RESULT_CONTINUE)
            {
                return;
            }
            // 실수 일경우
            else if (MODE_DOT)
            {
                // 콤마가 포함된 String 을 같이 지워주기 위해서 따로 Length를 계산함
                var lenWithComma = strCurrentNum.Length - 1;
                // 지우는 문자가 . 일경우 실수모드를 해제시킴
                if(strOnlyNumber[len].Equals('.'))
                {
                    MODE_DOT = false;
                }
                // 끝에서부터 한글자씩 지워버림
                strOnlyNumber = strOnlyNumber.Remove(len);
                strCurrentNum = strCurrentNum.Remove(lenWithComma);
            }
            else
            {
                strOnlyNumber = strOnlyNumber.Remove(len);
                setComma(strOnlyNumber);
            }
            appearNumber();
        }

        // 음수양수 전환하는 버튼을 눌렀을 때
        public void plusAndMinus_Click(object sender, RoutedEventArgs e)
        {
            // 음수양수 전환을 한번이라도 했습니다 체크
            MODE_PM = true;

            // 0 상태에서는 -/+ 전환이 불가능하게 함
            if (strOnlyNumber.Equals(""))
            {
                return;
            }

            // 실수모드인데 +/- 전환할경우
            if (MODE_DOT)
            {
                string[] str;
                str = strOnlyNumber.Split('.');
                // -X. 상태일경우
                if (MODE_NEGATIVE)
                {
                    strCurrentNum = str[0].Remove(0, 1) + ".";
                    strOnlyNumber = strCurrentNum;
                    MODE_NEGATIVE = false;
                }
                else if (str[1].Equals(""))
                {
                    strCurrentNum = "-" + strCurrentNum;
                    strOnlyNumber = strCurrentNum;
                    MODE_NEGATIVE = true;
                }
                appearNumber();
                return;
            }

            // 음수상태에서 버튼이 다시 눌렸을 경우
            //if (MODE_NEGATIVE) { MODE_NEGATIVE = false; }
            //else { MODE_NEGATIVE = true; }
            MODE_NEGATIVE = MODE_NEGATIVE.Equals(true) ? false : true;

            // 맨 처음 상태에서 전환버튼이 눌렸을 때
            if (strOnlyNumber.Equals(""))
            {
                return;
            }

            //storeNum = double.Parse(strCurrentNum);
            //storeNum *= -1;
            //strOnlyNumber = storeNum.ToString();



            var number = Convert.ToDouble(strOnlyNumber);
            number *= -1;
            strOnlyNumber = number.ToString();

            setComma(strOnlyNumber);
            appearNumber();
        }


        // 연산자 버튼을 클릭했을 때
        public void btn_Operation_Click(object sender, RoutedEventArgs e)
        {
            string calMode = ((ContentControl)sender).Content.ToString();

            calculatorByOperation(calMode);
        }

        // 1
        public void calculatorByOperation(string calMode)
        {
            // 우선 실수모드라면
            if(MODE_DOT)
            {
                // 실수를 . 앞뒤로 찢어버림
                string[] resentString;
                resentString = strOnlyNumber.Split('.');
                
                // 만약 . 앞으로 "" 이라면 ( 0. xxxxx)
                if(resentString[0].Equals(""))
                {
                    strOnlyNumber = strCurrentNum;
                }

                // 만약 . 뒤로 아무것도 입력되지 않았다면
                if (resentString[1].Equals(""))
                {
                    strOnlyNumber = resentString[0];
                }
                // 연산자가 선택되면 실수모드를 해제함
                MODE_DOT = false;
            }

            // 연산이 처음이라면 ( = 이 눌리고 처음 연산이 되었을 때도 처음이므로 )
            if (COUNT_OPERATION.Equals(0))
            {
                storeNum = 0;
            }

            // 플러스마이너스 전환이 한번이라도 됬다면 연산모드를 꺼버림
            // 해당 입력된 값으로 계산이 되야되기 때문에
            if(MODE_PM)
            {
                MODE_CAL = false;
            }

            // 이미 연산자가 선택된 상태라면 result label 의 연산자만 바꿔줌
            if (MODE_CAL)
            {
                string str = func.lbl_result.Content.ToString();

                func.lbl_result.Content = func.lbl_result.Content.ToString().Remove(str.Length - 1);
                func.lbl_result.Content += calMode;
                BEFORE_CAL = setFormula(calMode);

                return;
            }

            // 연산자가 진짜 처음이라면 ( 프로그램 실행 후에 연산자 바로들어올시)
            // CE가 눌리고 연산자가 눌렸다면
            if (COUNT_OPERATION.Equals(0) && strOnlyNumber.Equals(""))
            {
                strOnlyNumber = "0";
            }

            // CE가 눌리고나서 숫자가 눌렸다면 계속수행
            // 숫자가 눌리지 않았다면 "" 에서 "0"으로 초기화
            if (MODE_CEANDNUMBER) { }
            else if (MODE_CE)
            {
                strOnlyNumber = "0";
            }

            // 뒤로가기가 눌린 상태인데, 아무것도 입력이 되지 않았다면
            if (MODE_BACK && strOnlyNumber.Equals(""))
            {
                strOnlyNumber = "0";
            }

            // 연산이 되었음을 알려줌 (숫자입력시 초기화를 위해)
            MODE_CAL = true;

            // 해당 연산자를 숫자로 바꿔줌
            // 1: + 2: - 3: / 4: *
            int compareOp = setFormula(calMode);

            // 연산에 따라 예외처리가 달라지므로, 분기로 나눔
            switch (compareOp)
            {
                case 1: // 덧셈
                    break;
                case 2: // 뺄셈
                    if (storeNum.Equals(0))
                    {
                        storeNum = double.Parse(strOnlyNumber);
                        storeNum *= 2;
                    }
                    break;
                case 3: // 나눗셈
                    // 처음 연산이 아닌데, 저장되있는 숫자가 0일경우 ???
                    // 처음 연산일 경우
                    if (storeNum.Equals(0))
                    {
                        // 지금 입력된 숫자 가져와서 storeNum에 저장하고
                        // storeNum 제곱해줌 - 나누기 한번 눌렀을 때 strCurrent에 제대로 나오게 하기위해서
                        storeNum = double.Parse(strOnlyNumber);
                        storeNum *= storeNum;
                    }
                    // 저장되있는 숫자도 0이고, 처음 연산일 때
                    else if (storeNum.Equals(0) && COUNT_OPERATION.Equals(0))
                    {
                        calculatorAdditional(compareOp, calMode);
                        return;
                    }
                    break;
                case 4: // 곱셈
                    // 처음 연산일경우
                    if (strOnlyNumber.Length > 0 && COUNT_OPERATION.Equals(0))
                    {
                        // 저장되있는 숫자 1로 바꿔준다
                        storeNum = 1;
                    }
                    break;
            }

            // = 가 계속 눌리고, 연산자가 선택됬다면
            if (MODE_RESULT_CONTINUE)
            {
                // strOnlyNumber 에 현재 써져있는 숫자 넣어줌
                strOnlyNumber = func.lbl_current.Content.ToString();
                storeNum = Convert.ToDouble(strOnlyNumber);
                MODE_RESULT_CONTINUE = false;
                calculatorAdditional(compareOp, calMode);
                return;
            }
            
            // 음수 상태에서 연산자가 들어왔다면
            if (MODE_NEGATIVE)
            {
                
            }

            // 이전 연산결과와 같다면 || 연산자가 처음 눌렸다면
            if (BEFORE_CAL.Equals(compareOp) || COUNT_OPERATION.Equals(0))
            {
                calculator(compareOp);
            }
            else
            {
                calculator(BEFORE_CAL);
            }

            // 다음 메소드로 넘어감
            calculatorAdditional(compareOp, calMode);
        }

        // 2
        public void calculatorAdditional(int compareOp, string calMode)
        {
            // 해당모드의 연산자로 지정
            BEFORE_CAL = compareOp;

            // 입력하는 부분 위에 기록 남겨둠
            func.lbl_result.Content += strOnlyNumber + calMode;

            // 입력한 값 OnlyNumber 에 Write
            strOnlyNumber = storeNum.ToString();

            // 콤마찍어주고 화면에 보여줌
            setComma(strOnlyNumber);
            appearNumber();
            
            // 연산 횟수 증가시켜줌
            COUNT_OPERATION++;
        }


        // '='
        public void btn_Result_Click(object sender, RoutedEventArgs e)
        {
            // 점이 찍힌상태인데, 바로 = 을 눌렀다면
            if (MODE_DOT && !MODE_DotNextNumber)
            {
                strOnlyNumber = "";
                strCurrentNum = "0";
                appearNumber();
                MODE_DOT = false;
            }
            // 프로그램이 아예 초기상태에서 = 이 눌린다면
            // CE가 눌리고 왔다면 조건에 해당시키지 않음
            if ((!MODE_RESULT_CONTINUE) && strOnlyNumber.Length.Equals(0) && !MODE_CE)
            {
                return;
            }
            // 이전의 연산이 없다면
            if (BEFORE_CAL.Equals(0)) { return; }
            // = 이 눌린다음에 숫자가 눌리고 다시 = 이 눌렸을경우
            if (MODE_RESULTNEXTINPUT)
            {
                MODE_RESULT_CONTINUE = true;
                MODE_RESULTNEXTINPUT = false;
            }
            // = 연산이 첫번 째 눌리는 거라면
            if (!MODE_RESULT_CONTINUE)
            {
                // 연산이 첫번 째 눌리는것인데, CE를 누르고 왔다면
                if(MODE_CE)
                {
                    strOnlyNumber = "0";
                }
                // 이전의 연산데이터를 가지고 있는다 우선은
                beforeDataForResult = double.Parse(strOnlyNumber);
            }

            // 연산이 2번째 눌리는 거라면
            if (MODE_RESULT_CONTINUE)
            {
                // 만약 CE 눌리고 들어왔다면
                if (MODE_CE)
                {
                    strOnlyNumber = "0";
                    storeNum = 0;
                    MODE_CE = false;
                }
                // 연산모드가 아님을 알려주고
                MODE_CAL = false;

                // 연산이 첫번 째 눌릴때 저장해놨던 데이터 다시가져온다
                strOnlyNumber = beforeDataForResult.ToString();

                // 이전의 연산결과를 수행한다
                calculator(BEFORE_CAL);

                strOnlyNumber = storeNum.ToString();

                // 결과패널 사라지게
                func.lbl_result.Content = "";

                // 콤마찍어주고 화면에 보여줌
                setComma(strOnlyNumber);
                appearNumber();

                strOnlyNumber = "";

                return;
            }

            // = 연산이 됬다고 체크
            MODE_RESULT_CONTINUE = true;

            // 이전의 연산결과를 수행한다
            calculator(BEFORE_CAL);

            strOnlyNumber = storeNum.ToString();

            // 결과패널 사라지게
            func.lbl_result.Content = "";

            // 콤마찍어주고 화면에 보여줌
            setComma(strOnlyNumber);
            appearNumber();

            // COUNT 값 초기화
            COUNT_OPERATION = 0;
        }

        public void btn_dot_Click(object sender, RoutedEventArgs e)
        {
            // .이 눌리면 해당 숫자에서 . 앞부분의 숫자를 따로 보관해둠 -> beforeDotNum 에
            string[] realNumber;
            realNumber = strCurrentNum.Split('.');
            beforeDotNum = realNumber[0];

            // 이미 실수모드라면
            if(MODE_DOT) { }
            // 아니라면 .을 추가해줌
            else
            {
                strCurrentNum += ".";
                strOnlyNumber += ".";
                appearNumber();
            }

            // 연산자가 눌리고 나서 바로 .이 눌렸다면
            if (MODE_CAL || MODE_RESULT_CONTINUE)
            {
                strCurrentNum = "0.";
                strOnlyNumber = "0.";
                appearNumber();
                MODE_CAL = false;
                // . 뒤에 숫자를 추가해주기 위함임
                // 이게 없으면 . 하고나서 숫자가 그냥 추가되어버림 (숫자찍는 메소드의 조건문에 의해서)
                MODE_RESULT_CONTINUE = false;
            }

            // 삼항연산자 사용
            //MODE_DOT = MODE_DOT.Equals(true) ? false : true;
            MODE_DOT = true;
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
                    storeNum /= double.Parse(strOnlyNumber);
                    break;
                case 4:
                    storeNum *= double.Parse(strOnlyNumber);
                    break;
            }
        }

        public int setFormula(string str)
        {
            if (str.Equals("＋"))
                return 1;
            else if (str.Equals("-"))
                return 2;
            else if (str.Equals("÷"))
                return 3;
            else if (str.Equals("×"))
                return 4;

            return 0;
        }
    }
}
