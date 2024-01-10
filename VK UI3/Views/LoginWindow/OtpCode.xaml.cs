using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Octokit;
using System;
using System.Threading.Tasks;
using VK_UI3.DB;
using VK_UI3.VKs;
using VkNet.AudioBypassService.Models.Auth;
using Page = Microsoft.UI.Xaml.Controls.Page;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VK_UI3.Views.LoginWindow
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public  partial class OtpCode : Page
    {
        internal VK vk;

        public int CodeLength { get; set; }
        public string? Info { get; set; }
        public LoginWay loginWay { get; set; } = LoginWay.None;
        public bool HasAnotherVerificationMethods { get; set; }

    

      
     
        public TaskCompletionSource<string?> Submitted { get; private set; } = new();

        public OtpCode()
        {
            this.InitializeComponent();


        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // �������� ���������� ��� ������, ������� ��� ����� �������� � ������� ����
            var viewModel = e.Parameter as OtpCode;

            if (viewModel != null)
            {
              

                CodeLength = (int)viewModel.CodeLength;
                Info =  viewModel.Info;
                loginWay = viewModel.loginWay;
                var txt = "��� ��� ��������� ��� �����������.";

                if (loginWay == LoginWay.Codegen)
                    txt = "��� ��� ��� ������������ ����������� ��� ���������";
               
                if (loginWay == LoginWay.CallReset)
                    txt = "��� ������ �������� � ���������� ��� �����������";

                if (loginWay == LoginWay.Email)
                    txt = "��� ��� ����������� ��� ��������� �� ��� Mail";

                if (loginWay == LoginWay.Sms)
                    txt = "��� ��� ����������� ��� ��������� � ��� ���������";

                if (loginWay == LoginWay.Push)
                    txt = "��� ��� ����������� ��� ��������� � ���������� �� �� ����� ��������� ����������.";

                if (loginWay == LoginWay.ReserveCode)
                    txt = "����������� ��������� ��� �� ����������� ���� ������.";

           

                passpey.Text = txt;

                HasAnotherVerificationMethods = viewModel.HasAnotherVerificationMethods;

                if (!HasAnotherVerificationMethods) goAnotherBTN.Visibility = Visibility.Collapsed;

                Submitted =   viewModel.Submitted;

                this.vk = viewModel.vk;
                
            
            }


        }

        private async void BackButton(object sender, RoutedEventArgs e)
        {
            // _ = await InputTextDialogAsync("hello!", this.XamlRoot);
            AccountsDB.activeAccount = new AccountsDB.Accounts();
            this.Frame.Navigate(typeof(Login));
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            sumbit();


        }

        private void sumbit() 
        {

            Submitted.SetResult(CodeBox.Text);
            Submitted = new TaskCompletionSource<string?>();


            vk.Vk2FaCompleteAsync(CodeBox.Text);

        }

        private void ShowAnotherVerificationMethodsButton_Click(object sender, RoutedEventArgs e)
        {
            vk.ShowAnotherVerificationMethodsAsync();
        }

        private void passpey_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                sumbit();
            }
            
        }
    }
}
