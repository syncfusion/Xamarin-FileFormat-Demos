
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Syncfusion.Presentation;
using System.IO;
using System.ComponentModel;

namespace SampleBrowser
{
    public class PresentationViewModel : BindableObject
    {
        #region Properties
        /// <summary>
        /// Represents the instance of the image command.
        /// </summary>
        public ImagesCommand ImagesCommand
        {
            get
            {
                return (ImagesCommand)GetValue(ImagesCommandProperty);
            }
            set
            {
                SetValue(ImagesCommandProperty, value);
            }
        }

        /// <summary>
        /// Represents the instance of getting started command.
        /// </summary>
        public GettingStartedCommand GettingStartedCommand
        {
            get
            {
                return (GettingStartedCommand)GetValue(GettingStartedCommandProperty);
            }
            set
            {
                SetValue(GettingStartedCommandProperty, value);
            }
        }

        /// <summary>
        /// Represents the instance of slides command.
        /// </summary>
        public SlidesCommand SlidesCommand
        {
            get
            {
                return (SlidesCommand)GetValue(SlidesCommandProperty);
            }
            set
            {
                SetValue(SlidesCommandProperty, value);
            }
        }

        /// <summary>
        /// Represents the instance of tables command.
        /// </summary>
        public TablesCommand TablesCommand
        {
            get
            {
                return (TablesCommand)GetValue(TablesCommandProperty);
            }
            set
            {
                SetValue(TablesCommandProperty, value);
            }
        }

        /// <summary>
        /// Represents the instance of charts command.
        /// </summary>
        public ChartsCommand ChartsCommand
        {
            get
            {
                return (ChartsCommand)GetValue(ChartsCommandProperty);
            }
            set
            {
                SetValue(ChartsCommandProperty, value);
            }
        }

        #region Bindable properties
        public static readonly BindableProperty ImagesCommandProperty = BindableProperty.Create<PresentationViewModel, ImagesCommand>(s => s.ImagesCommand, new ImagesCommand(), BindingMode.OneWay, null, null);

        public static readonly BindableProperty SlidesCommandProperty = BindableProperty.Create<PresentationViewModel, SlidesCommand>(s => s.SlidesCommand, new SlidesCommand(), BindingMode.OneWay, null, null);

        public static readonly BindableProperty TablesCommandProperty = BindableProperty.Create<PresentationViewModel, TablesCommand>(s => s.TablesCommand, new TablesCommand(), BindingMode.OneWay, null, null);

        public static readonly BindableProperty ChartsCommandProperty = BindableProperty.Create<PresentationViewModel, ChartsCommand>(s => s.ChartsCommand, new ChartsCommand(), BindingMode.OneWay, null, null);

        public static readonly BindableProperty GettingStartedCommandProperty = BindableProperty.Create<PresentationViewModel, GettingStartedCommand>(s => s.GettingStartedCommand, new GettingStartedCommand(), BindingMode.OneWay, null, null);
        #endregion Bindable properties

        #endregion
    }
}
