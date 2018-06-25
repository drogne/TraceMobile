// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace PassFailSample.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

        #region Boolean Properties
        
        public bool BoolBarcodeScannedScreenEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(BoolBarcodeScannedScreenEnabled), true);
            set => AppSettings.AddOrUpdateValue(nameof(BoolBarcodeScannedScreenEnabled), value);
        }

        public bool BoolFailureFeedbackScreenEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(BoolFailureFeedbackScreenEnabled), true);
            set => AppSettings.AddOrUpdateValue(nameof(BoolFailureFeedbackScreenEnabled), value);
        }
        public bool BoolCustomInputScreenEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(BoolCustomInputScreenEnabled), true);
            set => AppSettings.AddOrUpdateValue(nameof(BoolCustomInputScreenEnabled), value);
        }
        public bool BoolUserLoginScreenEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(BoolUserLoginScreenEnabled), true);
            set => AppSettings.AddOrUpdateValue(nameof(BoolUserLoginScreenEnabled), value);
        }
        public bool BoolDemoMode
        {
            get => AppSettings.GetValueOrDefault(nameof(BoolDemoMode), false);
            set => AppSettings.AddOrUpdateValue(nameof(BoolDemoMode), value);
        }
        public bool BoolUseLoggedEntryTime
        {
            get => AppSettings.GetValueOrDefault(nameof(BoolUseLoggedEntryTime), false);
            set => AppSettings.AddOrUpdateValue(nameof(BoolUseLoggedEntryTime), value);
        }

        #endregion

        #region Text Properties

        public string AppName
        {
            get => AppSettings.GetValueOrDefault(nameof(AppName), "COGNEX");
            set => AppSettings.AddOrUpdateValue(nameof(AppName), value);
        }
        public string NavigationBarTitle
        {
            get => AppSettings.GetValueOrDefault(nameof(NavigationBarTitle), "Demo App");
            set => AppSettings.AddOrUpdateValue(nameof(NavigationBarTitle), value);
        }
        public string TextPassButton
        {
            get => AppSettings.GetValueOrDefault(nameof(TextPassButton), "ACCEPTABLE");
            set => AppSettings.AddOrUpdateValue(nameof(TextPassButton), value);
        }

        public string TextFailButton
        {
            get => AppSettings.GetValueOrDefault(nameof(TextFailButton), "UNACCEPTBALE");
            set => AppSettings.AddOrUpdateValue(nameof(TextFailButton), value);
        }

        #endregion

        #region List Properties

        public string ListFailureReasons
        {
            get => AppSettings.GetValueOrDefault(nameof(ListFailureReasons), "CONVEYOR DIRTY; HOPPER DIRTY; HARDWARE ERROR");
            set => AppSettings.AddOrUpdateValue(nameof(ListFailureReasons), value);
        }
        public string ListAdditionalStatusOptions
        {
            get => AppSettings.GetValueOrDefault(nameof(ListAdditionalStatusOptions), "");
            set => AppSettings.AddOrUpdateValue(nameof(ListAdditionalStatusOptions), value);
        }

        #endregion

        #region Prompt Properties

        public string PromptScanStation
        {
            get => AppSettings.GetValueOrDefault(nameof(PromptScanStation), "Please scan a station barcode to begin");
            set => AppSettings.AddOrUpdateValue(nameof(PromptScanStation), value);
        }
        public string PromptSelectFailureReason
        {
            get => AppSettings.GetValueOrDefault(nameof(PromptSelectFailureReason), "Select the reason the station failed inspection");
            set => AppSettings.AddOrUpdateValue(nameof(PromptSelectFailureReason), value);
        }
        public string PromptSpecifyFailureReason
        {
            get => AppSettings.GetValueOrDefault(nameof(PromptSpecifyFailureReason), "Specify the reason the station failed inspection");
            set => AppSettings.AddOrUpdateValue(nameof(PromptSpecifyFailureReason), value);
        }
        public string PromptUserLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(PromptUserLogin), "Enter or Scan your ID");
            set => AppSettings.AddOrUpdateValue(nameof(PromptUserLogin), value);
        }

        #endregion

        #region Color Properties

        public string ColorPass
        {
            get => AppSettings.GetValueOrDefault(nameof(ColorPass), "#34AD87");
            set => AppSettings.AddOrUpdateValue(nameof(ColorPass), value);
        }
        public string ColorFail
        {
            get => AppSettings.GetValueOrDefault(nameof(ColorFail), "#F04B48");
            set => AppSettings.AddOrUpdateValue(nameof(ColorFail), value);
        }
        public string ColorStandardButton
        {
            get => AppSettings.GetValueOrDefault(nameof(ColorStandardButton), "#333333");
            set => AppSettings.AddOrUpdateValue(nameof(ColorStandardButton), value);
        }

        #endregion
    }
}