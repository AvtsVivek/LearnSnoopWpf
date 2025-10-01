namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using JetBrains.Annotations;


    [Serializable]
    public sealed class SettingsCoreNew : SettingsBaseNew<SettingsCoreNew>
    {
        private static readonly XmlSerializer serializer = new(typeof(SettingsCoreNew));

        private bool clearAfterDelve = true;
        private int maximumTrackedEvents = 100;
        private bool showDefaults = true;
        private bool showPreviewer;
        private bool isDefaultSettingsFile;
        private ThemeModeNew themeMode;

        public SettingsCoreNew()
        {
            this.SettingsFile = SettingsHelperNew.GetSettingsFileForCurrentApplication();
        }

        public static SettingsCoreNew Default { get; } = new SettingsCoreNew().Load();

        protected override XmlSerializer Serializer => serializer;

        [XmlIgnore]
        public bool IsDefaultSettingsFile
        {
            get => this.isDefaultSettingsFile;
            set
            {
                if (value == this.isDefaultSettingsFile)
                {
                    return;
                }

                this.isDefaultSettingsFile = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowDefaults
        {
            get => this.showDefaults;
            set
            {
                if (value == this.showDefaults)
                {
                    return;
                }

                this.showDefaults = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowPreviewer
        {
            get => this.showPreviewer;
            set
            {
                if (value == this.showPreviewer)
                {
                    return;
                }

                this.showPreviewer = value;
                this.OnPropertyChanged();
            }
        }

        public bool ClearAfterDelve
        {
            get => this.clearAfterDelve;
            set
            {
                if (value == this.clearAfterDelve)
                {
                    return;
                }

                this.clearAfterDelve = value;
                this.OnPropertyChanged();
            }
        }

        public WINDOWPLACEMENT? SnoopUIWindowPlacement { get; set; }

        public WINDOWPLACEMENT? ZoomerWindowPlacement { get; set; }

        public ObservableCollection<PropertyFilterSetNew> UserDefinedPropertyFilterSets { get; private set; } = new();

        public ObservableCollection<SnoopSingleFilterNew> SnoopDebugFilters { get; private set; } = new();

        public ObservableCollection<EventTrackerSettingsItemNew> EventTrackers { get; private set; } = new();

        public int MaximumTrackedEvents
        {
            get => this.maximumTrackedEvents;
            set
            {
                if (value == this.maximumTrackedEvents)
                {
                    return;
                }

                this.maximumTrackedEvents = value;
                this.OnPropertyChanged();
            }
        }

        public ThemeModeNew ThemeMode
        {
            get => this.themeMode;
            set
            {
                this.themeMode = value;
                this.OnPropertyChanged();
                ThemeManagerNew.Current.ApplyTheme(value);
            }
        }

        protected override void UpdateWith(SettingsCoreNew settings)
        {
            this.ThemeMode = settings.ThemeMode;
            this.ShowDefaults = settings.ShowDefaults;
            this.ShowPreviewer = settings.ShowPreviewer;
            this.ClearAfterDelve = settings.ClearAfterDelve;
            this.MaximumTrackedEvents = settings.MaximumTrackedEvents;

            this.SnoopUIWindowPlacement = settings.SnoopUIWindowPlacement;
            this.ZoomerWindowPlacement = settings.ZoomerWindowPlacement;

            this.UserDefinedPropertyFilterSets.UpdateWith(settings.UserDefinedPropertyFilterSets);
            this.SnoopDebugFilters.UpdateWith(settings.SnoopDebugFilters);
            this.EventTrackers.UpdateWith(settings.EventTrackers);
        }

        [NotifyPropertyChangedInvocator]
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.SettingsFile))
            {
                this.IsDefaultSettingsFile = Path.GetFileName(this.SettingsFile).Equals("DefaultSettings.xml", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
    [PublicAPI]
    public enum ThemeModeNew
    {
        Auto = 0,
        Dark = 1,
        Light = 2
    }
}
