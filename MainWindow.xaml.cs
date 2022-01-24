using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cunning_Linguist
{
    public partial class MainWindow : Window
    {
        private Linguist _linguist;

        public MainWindow()
        {
            _linguist = new();
            InitializeComponent();

            UpdateFromLinguist();
            this.Topmost = true;
        }

        private void UpdateFromLinguist()
        {
            SuggestionsList.Text = _linguist.GetAllSuggestedWords();

            Suggestion.Text = string.Join(" ", _linguist.GetSuggestedWord().ToUpper().ToArray());
        }

        private void UpdateLinguist()
        {
            var fixedLetters = new string[5]
            {
                Fixed1.Text.ToLower(),
                Fixed2.Text.ToLower(),
                Fixed3.Text.ToLower(),
                Fixed4.Text.ToLower(),
                Fixed5.Text.ToLower(),
            };

            _linguist.Process(fixedLetters, KnownList.Text, BadList.Text);
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            UpdateLinguist();
            UpdateFromLinguist();
        }

        private void FixedTextboxDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox box)
            {
                box.Text = "";
            }
        }
    }
}