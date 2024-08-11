using ICSharpCode.AvalonEdit;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace RYCBEditorX.Dialogs.ViewModels
{
    public class SearchPanelViewModel : BindableBase
    {
        private bool _caseSensitive;
        private bool _matchWholeWord;
        private bool _regex;
        private TextEditor _editor;

        public bool CaseSensitive
        {
            get => _caseSensitive;
            set => SetProperty(ref _caseSensitive, value);
        }

        public bool MatchWholeWord
        {
            get => _matchWholeWord;
            set => SetProperty(ref _matchWholeWord, value);
        }

        public bool Regex
        {
            get => _regex;
            set => SetProperty(ref _regex, value);
        }

        public TextEditor Editor
        {
            get => _editor;
            set => SetProperty(ref _editor, value);
        }

        public SearchOptions GetSearchOptions()
        {
            // Return search options based on the ViewModel properties
            return new SearchOptions
            {
                CaseSensitive = CaseSensitive,
                MatchWholeWord = MatchWholeWord,
                Regex = Regex
            };
        }
    }

    public class SearchOptions
    {
        public bool CaseSensitive
        {
            get; set;
        }
        public bool MatchWholeWord
        {
            get; set;
        }
        public bool Regex
        {
            get; set;
        }
    }
}