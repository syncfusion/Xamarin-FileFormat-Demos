
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using System.ComponentModel;

namespace SampleBrowser
{

    public class PDFViewModel : BindableObject
    {
        public static INavigation Navigation;

        #region Properties
        public MergePDFCommand MergeCommand
        {
            get
            {
                return (MergePDFCommand)GetValue(MergePDFCommandProperty);
            }
            set
            {
                SetValue(MergePDFCommandProperty, value);
            }
        }       
        public TableFeaturesCommand TableCommand
        {
            get
            {
                return (TableFeaturesCommand)GetValue(TableFeaturesCommandProperty);
            }
            set
            {
                SetValue(TableFeaturesCommandProperty, value);
            }
        }
        public BarcodeCommand BarCommand
        {
            get
            {
                return (BarcodeCommand)GetValue(BarcodeCommandProperty);
            }
            set
            {
                SetValue(BarcodeCommandProperty, value);
            }
        }
        public GettingStartedPDFCommand GsCommand
        {
            get
            {
                return (GettingStartedPDFCommand)GetValue(GettingStartedPDFCommandProperty);
            }
            set
            {
                SetValue(GettingStartedPDFCommandProperty, value);
            }
        }
        public StampingCommand StampCommand
        {
            get
            {
                return (StampingCommand)GetValue(StampingCommandProperty);
            }
            set
            {
                SetValue(StampingCommandProperty, value);
            }
        }
        public MailAttachmentCommand MailCommand
        {
            get
            {
                return (MailAttachmentCommand)GetValue(MailAttachmentCommandProperty);
            }
            set
            {
                SetValue(MailAttachmentCommandProperty, value);
            }
        }
        #endregion

        #region Static 
        public static readonly BindableProperty MergePDFCommandProperty = BindableProperty.Create<PDFViewModel, MergePDFCommand>(s => s.MergeCommand, new MergePDFCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty TableFeaturesCommandProperty = BindableProperty.Create<PDFViewModel, TableFeaturesCommand>(s => s.TableCommand, new TableFeaturesCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty BarcodeCommandProperty = BindableProperty.Create<PDFViewModel, BarcodeCommand>(s => s.BarCommand, new BarcodeCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty GettingStartedPDFCommandProperty = BindableProperty.Create<PDFViewModel, GettingStartedPDFCommand>(s => s.GsCommand, new GettingStartedPDFCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty StampingCommandProperty = BindableProperty.Create<PDFViewModel, StampingCommand>(s => s.StampCommand, new StampingCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty MailAttachmentCommandProperty = BindableProperty.Create<PDFViewModel, MailAttachmentCommand>(s => s.MailCommand, new MailAttachmentCommand(), BindingMode.OneWay, null, null);

        #endregion
    }

}
