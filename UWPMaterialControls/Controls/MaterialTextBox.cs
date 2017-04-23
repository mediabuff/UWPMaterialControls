using System;
using System.Linq;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPMaterialControls.Controls
{
    public sealed class MaterialTextBox : Control
    {
        private TextBox textBoxControl;
        private ContentPresenter labelPresenter;
        private ContentPresenter overlayTextPresenter;
        private ContentPresenter characterCountPresenter;
        private StackPanel errorTextPanel;
        private bool isInFoucs;
        private bool isPointerOver;
        private bool isTemplatApplied;

        public MaterialTextBox()
        {
            this.DefaultStyleKey = typeof(MaterialTextBox);
        }

        protected override void OnApplyTemplate()
        {
            // TextChanged Event can trigger calls to characterCountPresenter ==> needs to be loaded first
            characterCountPresenter = GetTemplateChild<ContentPresenter>("CharacterCountPresenter");
            textBoxControl = GetTemplateChild<TextBox>("TextTextBox");
            textBoxControl.TextChanged += (s, e) => TextChanged?.Invoke(s, e);
            textBoxControl.KeyDown += TextBoxControl_KeyDown;
            textBoxControl.GotFocus += MaterialTextBox_GotFocus;
            textBoxControl.LostFocus += MaterialTextBox_LostFocus;
            labelPresenter = GetTemplateChild<ContentPresenter>("LabelPresenter");
            labelPresenter.Loaded += LabelPresenter_Loaded;
            overlayTextPresenter = GetTemplateChild<ContentPresenter>("OverlayTextPresenter");
            errorTextPanel = GetTemplateChild<StackPanel>("ErrorTextPanel");

            this.PointerEntered += MaterialTextBox_PointerEntered;
            this.PointerExited += MaterialTextBox_PointerExited;
            
            SetCommonVisualState(false);
            SetOverlayTextVisualState(false);
            //SetLabelVisualState called in laberPresenter.Loaded event
            UpdateCharacterCountPresenter();
            TextChanged += MaterialTextBox_TextChanged;

            isTemplatApplied = true;
        }

        public event EventHandler<RoutedEventArgs> TextChanged;
        public event EventHandler TextAutoCompleted;
        public event EventHandler ToManyCharacters;

        public string AutocompleteText
        {
            get { return (string)GetValue(AutocompleteTextProperty); }
            set
            {
                SetValue(AutocompleteTextProperty, value);
                if(isTemplatApplied)
                    SetOverlayTextVisualState();
            }
        }
        // Using a DependencyProperty as the backing store for AutocompleteText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutocompleteTextProperty =
            DependencyProperty.Register(nameof(AutocompleteText), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(""));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(""));

        public double LabelFontSize
        {
            get { return (double)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LabelFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register(nameof(LabelFontSize), typeof(double), typeof(MaterialTextBox), new PropertyMetadata(12));

        public string HelperText
        {
            get { return (string)GetValue(HelperTextProperty); }
            set
            {
                SetValue(HelperTextProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for HelperText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HelperTextProperty =
            DependencyProperty.Register(nameof(HelperText), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(""));

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ErrorText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(""));

        public bool Valid
        {
            get { return (bool)GetValue(ValidProperty); }
            set
            {
                SetValue(ValidProperty, value);
                SetCommonVisualState();
            }
        }
        // Using a DependencyProperty as the backing store for Valid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidProperty =
            DependencyProperty.Register(nameof(Valid), typeof(bool), typeof(MaterialTextBox), new PropertyMetadata(true));

        public Brush PrimaryTextForeground
        {
            get { return (Brush)GetValue(PrimaryTextForegroundProperty); }
            set { SetValue(PrimaryTextForegroundProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PrimaryTextForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrimaryTextForegroundProperty =
            DependencyProperty.Register(nameof(PrimaryTextForeground), typeof(Brush), typeof(MaterialTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public Brush SecondaryTextForeground
        {
            get { return (Brush)GetValue(SecondaryTextForegroundProperty); }
            set { SetValue(SecondaryTextForegroundProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PrimaryTextForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondaryTextForegroundProperty =
            DependencyProperty.Register(nameof(SecondaryTextForeground), typeof(Brush), typeof(MaterialTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Brush InputLineForeground
        {
            get { return (Brush)GetValue(InputLineForegroundProperty); }
            set { SetValue(InputLineForegroundProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PrimaryTextForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputLineForegroundProperty =
            DependencyProperty.Register(nameof(InputLineForeground), typeof(Brush), typeof(MaterialTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set
            {
                SetValue(PlaceholderTextProperty, value);
                if (isTemplatApplied)
                {
                    SetOverlayTextVisualState();
                    SetLabelVisualStates(false);
                }
            }
        }
        // Using a DependencyProperty as the backing store for PlaceholderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(""));

        public int MaxCharacter
        {
            get { return (int)GetValue(MaxCharacterProperty); }
            set
            {
                SetValue(MaxCharacterProperty, value);
                if (characterCountPresenter != null)
                    UpdateCharacterCountPresenter();
            }
        }
        // Using a DependencyProperty as the backing store for MaxCharacter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxCharacterProperty =
            DependencyProperty.Register(nameof(MaxCharacter), typeof(int), typeof(MaterialTextBox), new PropertyMetadata(-1));

        public string SegoeMDL2Icon
        {
            get { return (string)GetValue(SegoeMDL2IconProperty); }
            set { SetValue(SegoeMDL2IconProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SegoeMDL2Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SegoeMDL2IconProperty =
            DependencyProperty.Register(nameof(SegoeMDL2Icon), typeof(string), typeof(MaterialTextBox), new PropertyMetadata(""));


        private T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            T child = GetTemplateChild(name) as T;
            if (child == null)
                throw new NullReferenceException();
            return child;
        }

        private void MaterialTextBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            isPointerOver = false;
            SetCommonVisualState();
        }

        private void MaterialTextBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            isPointerOver = true;
            SetCommonVisualState();
        }

        private void MaterialTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            isInFoucs = false;
            SetCommonVisualState();
            SetOverlayTextVisualState();
            SetLabelVisualStates();
        }

        private void MaterialTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isInFoucs = true;
            SetCommonVisualState();
            SetOverlayTextVisualState();
            SetLabelVisualStates();
        }

        private void TextBoxControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(AutocompleteText) && e.Key == VirtualKey.Enter)
            {
                textBoxControl.Text = AutocompleteText;
                textBoxControl.SelectionStart = textBoxControl.Text.Length;
                AutocompleteText = "";
                TextAutoCompleted?.Invoke(this, EventArgs.Empty);
            }

        }

        private void MaterialTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            SetOverlayTextVisualState();
            UpdateCharacterCountPresenter();
        }

        private void LabelPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            RowDefinition row = GetTemplateChild<RowDefinition>("LabelRow");
            row.Height = new GridLength(labelPresenter.ActualHeight + 2);
            SetLabelVisualStates(false);
        }

        private void SetCommonVisualState(bool animations = true)
        {
            if (isInFoucs)
            {
                if (Valid)
                    VisualStateManager.GoToState(this, "InFocusValid", animations);
                else
                    VisualStateManager.GoToState(this, "InFoucsInvalid", animations);
                
            }
            else if (isPointerOver)
            {
                if (Valid)
                    VisualStateManager.GoToState(this, "PointerOverValid", animations);
                else
                    VisualStateManager.GoToState(this, "PointerOverInavlid", animations);
            }

            else if (!isInFoucs)
            {
                if (Valid)
                    VisualStateManager.GoToState(this, "NotInFocusValid", animations);
                else
                    VisualStateManager.GoToState(this, "NotInFocusInavlid", animations);
            }
        }

        private void SetOverlayTextVisualState(bool animations = true)
        {
            if (isInFoucs)
            {

                if (!string.IsNullOrEmpty(AutocompleteText))
                {
                    VisualStateManager.GoToState(this, "AutoComplete", animations);
                    return;
                }
                else if (!string.IsNullOrEmpty(PlaceholderText) && string.IsNullOrEmpty(textBoxControl.Text))
                {
                    VisualStateManager.GoToState(this, "Placeholder", animations);
                    return;
                }
            }
            VisualStateManager.GoToState(this, "NoOverlayText", animations);
        }

        private void SetLabelVisualStates(bool animations = true)
        {
            if (isInFoucs || !String.IsNullOrEmpty(textBoxControl.Text))
            {
                VisualStateManager.GoToState(this, "LabelAboveTextBox", animations);
                
            }
            else
                VisualStateManager.GoToState(this, "LabelOnTopOfTextBox", animations);
        }

        private void UpdateCharacterCountPresenter()
        {
            if(MaxCharacter < 0)
            {
                characterCountPresenter.Visibility = Visibility.Collapsed;
                return;
            }

            characterCountPresenter.Visibility = Visibility.Visible;

            characterCountPresenter.Content = String.Format($"{textBoxControl.Text.Count()}/{MaxCharacter}");
            if(MaxCharacter < textBoxControl.Text.Count())
            {
                ToManyCharacters?.Invoke(this, EventArgs.Empty);
            }
        }

    }

}
