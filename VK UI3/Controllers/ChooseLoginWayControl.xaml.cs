using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using VkNet.AudioBypassService.Models.Auth;
using VkNet.AudioBypassService.Models.Ecosystem;
using static VK_UI3.DB.AccountsDB;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VK_UI3.Controllers
{
    public sealed partial class ChooseLoginWayControl : UserControl
    {
        private readonly DependencyProperty loginWayD = DependencyProperty.Register(
            "loginWay", typeof(EcosystemVerificationMethod), typeof(ChooseLoginWayControl), new PropertyMetadata(default(EcosystemVerificationMethod), onLoiginWayProrertyChanged));

        public DependencyProperty LoginWayD => loginWayD;
        public EcosystemVerificationMethod loginWay
        {
            get { return (EcosystemVerificationMethod)GetValue(loginWayD); }
            set { SetValue(loginWayD, value); }
        }

        string hello = "";
        private static void onLoiginWayProrertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        public ChooseLoginWayControl()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => {
               // Bindings.Update();

                if (loginWay != null)
                {
                  

                    if (loginWay.Name == LoginWay.Codegen)
                    {
                        fontIcon.Glyph = "\uECAD";
                        MainTxT.Text = "��������� ����";
                        secondTXT.Text = "�������������� ����� �� ���������� ��������� ����� �����������";
                    } 
                    else if (loginWay.Name == LoginWay.Push)
                    {
                        fontIcon.Glyph = "\uE90A";
                        MainTxT.Text = "PUSH �����������";
                        secondTXT.Text = "��� ����� ���������� PUSH ����������� ��� �����������";
                    }
                    else if (loginWay.Name == LoginWay.Passkey)
                    {
                        fontIcon.Glyph = "\uE928";
                        MainTxT.Text = "OnePass";
                        secondTXT.Text = "�������������� OnePass ��� �����������";
                    }
                    else if (loginWay.Name == LoginWay.CallReset)
                    {
                        fontIcon.Glyph = "\uE717";
                        MainTxT.Text = "������";
                        secondTXT.Text = "��� �������� � ���������� ���.";
                    }
                    else if (loginWay.Name == LoginWay.Sms)
                    {
                        fontIcon.Glyph = "\uE8BD";
                        MainTxT.Text = "Sms �����������";
                        secondTXT.Text = "��� ����� ���������� ��� ��������� ��� �����������";
                    }
                    else if (loginWay.Name == LoginWay.ReserveCode)
                    {
                        fontIcon.Glyph = "\uE821";
                        MainTxT.Text = "��������� ���";
                        secondTXT.Text = "�������������� ��������� ����� ��� �����������";
                    }
                    else if (loginWay.Name == LoginWay.Email)
                    {
                        fontIcon.Glyph = "\uE715";
                        MainTxT.Text = "����������� �����";
                        secondTXT.Text = "��� ����� ��������� ��� �� ����������� �����";
                    }
                    else if (loginWay.Name == LoginWay.Password)
                    {
                        fontIcon.Glyph = "\uE8AC";
                        MainTxT.Text = "������";
                        secondTXT.Text = "������� ������ ��� ����� � ���� �������.";
                    }
                    else
                    {
                        fontIcon.Glyph = "?";
                        MainTxT.Text = "��������������� ����� ("+loginWay.Name.ToString()+")";
                        secondTXT.Text = "???";
                    }
                }

            };

            Loaded += ChooseLoginWayControl_Loaded;

           
        }

        private void ChooseLoginWayControl_Loaded(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
