
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Syncfusion.Presentation;
using System.IO;

namespace SampleBrowser
{

    public class PresentationBehavior : Behavior<SamplePage>
    {
        #region Overrides
        protected override void OnAttachedTo(SamplePage bindable)
        {
            Title = bindable.FindByName<Label>("SampleTitle");
            Description = bindable.FindByName<Label>("Description");
            SelectButton = bindable.FindByName<Button>("btnGenerate");
            SetLabelBehavior();
            base.OnAttachedTo(bindable);
        }
        #endregion
        
        #region Properties
        public Label Title { get; private set; }
        public Label Description { get; private set; }
        public Button SelectButton { get; private set; }
        #endregion

        #region Helper Methods
        private void SetLabelBehavior()
        {
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                Title.HorizontalOptions = LayoutOptions.Start;
                Description.HorizontalOptions = LayoutOptions.Start;
                SelectButton.HorizontalOptions = LayoutOptions.Start;

                Title.VerticalOptions = LayoutOptions.Center;
                Description.VerticalOptions = LayoutOptions.Center;
                SelectButton.VerticalOptions = LayoutOptions.Center;
                SelectButton.BackgroundColor = Color.Gray;
            }
            else if (Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                if (!SampleBrowser.App.isUWP)
                {
                    Description.FontSize = 18.5;
                }
                else
                {
                    Description.FontSize = 13.5;
                }
                Title.VerticalOptions = LayoutOptions.Center;
                Description.VerticalOptions = LayoutOptions.Center;
                SelectButton.VerticalOptions = LayoutOptions.Center;
            }
        }
        #endregion

    }
}
