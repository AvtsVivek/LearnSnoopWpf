namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;


    [Serializable]
    public class SnoopSingleFilterNew : SnoopFilterNew, ICloneable
    {
        private string text;
        private FilterTypeNew filterType;

        public SnoopSingleFilterNew()
        {
            this.text = string.Empty;
        }

        public FilterTypeNew FilterType
        {
            get => this.filterType;
            set
            {
                if (value == this.filterType)
                {
                    return;
                }

                this.filterType = value;
                this.RaisePropertyChanged(nameof(this.FilterType));
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.RaisePropertyChanged(nameof(this.Text));
            }
        }

        public override bool FilterMatches(string? debugLine)
        {
            debugLine = debugLine?.ToLower() ?? string.Empty;
            var lowerText = this.Text.ToLower();
            var filterMatches = false;
            switch (this.FilterType)
            {
                case FilterTypeNew.Contains:
                    filterMatches = debugLine.Contains(lowerText, StringComparison.Ordinal);
                    break;
                case FilterTypeNew.StartsWith:
                    filterMatches = debugLine.StartsWith(lowerText, StringComparison.Ordinal);
                    break;
                case FilterTypeNew.EndsWith:
                    filterMatches = debugLine.EndsWith(lowerText, StringComparison.Ordinal);
                    break;
                case FilterTypeNew.RegularExpression:
                    filterMatches = TryMatch(debugLine, lowerText);
                    break;
            }

            if (this.IsInverse)
            {
                filterMatches = !filterMatches;
            }

            return filterMatches;
        }

        private static bool TryMatch(string input, string pattern)
        {
            try
            {
                return Regex.IsMatch(input, pattern);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public object Clone()
        {
            var newFilter = new SnoopSingleFilterNew
            {
                IsGrouped = this.IsGrouped,
                GroupId = this.GroupId,
                Text = this.Text,
                FilterType = this.FilterType,
                IsInverse = this.IsInverse
            };
            return newFilter;
        }
    }
}
