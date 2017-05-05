using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPMaterialControls
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            
        }

        public void InvalidBox_TextChanged()
        {
            if (InvalidBox.Text.Contains("-"))
                InvalidBox.Valid = false;
            else
                InvalidBox.Valid = true;
        }
    }
}
