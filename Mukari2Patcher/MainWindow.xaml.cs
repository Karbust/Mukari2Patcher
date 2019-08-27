using System;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Diagnostics;

namespace Mukari2Patcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Patch.UI.Window = this;
        }

        public static void ClickStart()
        {
            try
            {
                Funcs.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível abrir o ficheiro " + Configs.Settings.BinaryName + ".", Configs.Settings.ServerName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Patch.Start();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Configs.Settings.ServerUrl);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ClickStart();
        }

        private void SupportButton_OnClick_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Configs.Settings.ServerUrlSupport);
        }

        private void CommunityButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(Configs.Settings.ServerUrlForum);
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(Configs.Settings.ServerUrlRegister);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Button_Mouse(object sender, MouseEventArgs e)
        {
            if (e.RoutedEvent == MouseEnterEvent)
            {
                double opacity = 0.75;
                if (e.Source.Equals(StartButton))
                    StartButton.Opacity = opacity;
                else if (e.Source.Equals(HomeButton))
                    HomeButton.Opacity = opacity;
                else if (e.Source.Equals(RegisterButton))
                    RegisterButton.Opacity = opacity;
                else if (e.Source.Equals(CommunityButton))
                    CommunityButton.Opacity = opacity;
                else if (e.Source.Equals(SupportButton))
                    SupportButton.Opacity = opacity;
            }
            else if (e.RoutedEvent == MouseLeaveEvent)
            {
                double opacity = 0.5;
                if (e.Source.Equals(StartButton))
                    StartButton.Opacity = opacity;
                else if (e.Source.Equals(HomeButton))
                    HomeButton.Opacity = opacity;
                else if (e.Source.Equals(RegisterButton))
                    RegisterButton.Opacity = opacity;
                else if (e.Source.Equals(CommunityButton))
                    CommunityButton.Opacity = opacity;
                else if (e.Source.Equals(SupportButton))
                    SupportButton.Opacity = opacity;
            }
            else if (e.RoutedEvent == PreviewMouseLeftButtonDownEvent)
            {
                double opacity = 1;
                if (e.Source.Equals(StartButton))
                    StartButton.Opacity = opacity;
                else if (e.Source.Equals(HomeButton))
                    HomeButton.Opacity = opacity;
                else if (e.Source.Equals(RegisterButton))
                    RegisterButton.Opacity = opacity;
                else if (e.Source.Equals(CommunityButton))
                    CommunityButton.Opacity = opacity;
                else if (e.Source.Equals(SupportButton))
                    SupportButton.Opacity = opacity;
            }
            else if (e.RoutedEvent == PreviewMouseLeftButtonUpEvent)
            {
                double opacity = 0.75;
                if (e.Source.Equals(StartButton))
                    StartButton.Opacity = opacity;
                else if (e.Source.Equals(HomeButton))
                    HomeButton.Opacity = opacity;
                else if (e.Source.Equals(RegisterButton))
                    RegisterButton.Opacity = opacity;
                else if (e.Source.Equals(CommunityButton))
                    CommunityButton.Opacity = opacity;
                else if (e.Source.Equals(SupportButton))
                    SupportButton.Opacity = opacity;
            }
        }

        private void StartButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StartButton.Opacity = StartButton.IsEnabled ? 0.6 : 0.3;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void PnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IntPtr hWnd = new WindowInteropHelper(this).EnsureHandle();
                ReleaseCapture();
                SendMessage(hWnd, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
