using Syncfusion.SfDataGrid.XForms;
using System;
using Xamarin.Forms;

namespace SampleBrowser
{
    public class CustomLayout : Grid
    {
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
                this.Children[0].Layout(new Rectangle(0, 0, this.Width, this.Height));
        }

        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            return false;
        }

        protected override bool ShouldInvalidateOnChildRemoved(View child)
        {
            return false;
        }
    }

    public class FormsView : StackLayout
    {
        #region Property

        public Columns Columns { get; set; }
        public StackLayout HeaderView { get; set; }
        public StackLayout EditorView { get; set; }
        private Button Save { get; set; }
        private Button Cancel { get; set; }
        public Label Title { get; set; }
        private StackLayout FormContentView { get; set; }
        public StackLayout Footer { get; set; }

        public bool Visibility
        {
            get { return this.visibility; }
            set
            {
                this.visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        #endregion

        #region Fields

        private OrderInfo swipedRowData;
        private SfDataGrid grid;
        private bool isSuspend;
        private bool isLoaded;
        private bool visibility = true;

        #endregion

        #region Constructor 
        
        public FormsView(SfDataGrid grid)
        {
            isSuspend = true;
            this.grid = grid;
            if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows)
                this.Visibility = false;
            else
                this.IsVisible = false;
            this.Columns = grid.Columns;
            Spacing = 10;
            BackgroundColor = Color.FromRgb(43, 43, 43);
            this.Orientation = StackOrientation.Vertical;
            this.Padding = new Thickness(10);
            FormContentView = new StackLayout() { Orientation = StackOrientation.Horizontal };
            Footer = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End };
            Save = new Button { FontSize = 12, Text = "Save", HorizontalOptions = LayoutOptions.End, BackgroundColor = Color.FromRgb(25, 25, 25), TextColor = Color.White };
            Cancel = new Button { FontSize = 12, Text = "Cancel", HorizontalOptions = LayoutOptions.End, BackgroundColor = Color.FromRgb(25, 25, 25), TextColor = Color.White };
            Save.Clicked += Save_Clicked;
            Cancel.Clicked += Cancel_Clicked;
            Footer.Children.Add(new ContentView() { Content = Save });
            Footer.Children.Add(new ContentView() { Content = Cancel });
            Title = new Label { Text = "Edit Details", FontSize = 20, HeightRequest = 30, TextColor = Color.White };
            HeaderView = new StackLayout() { Orientation = StackOrientation.Vertical, Spacing = 5, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            EditorView = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Spacing = 5 };
            EditorView.BindingContextChanged += EditView_BindingContextChanged;
            isSuspend = false;
        }

        #endregion

        #region Private Methods 

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            var editedrow = EditorView.BindingContext as OrderInfo;
            editedrow.CustomerID = swipedRowData.CustomerID;
            editedrow.EmployeeID = swipedRowData.EmployeeID;
            editedrow.OrderID = swipedRowData.OrderID;
            editedrow.ShipCountry = swipedRowData.ShipCountry;
            editedrow.FirstName = swipedRowData.FirstName;
            if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows)
                this.Visibility = false;
            else
                this.IsVisible = false;
            grid.Opacity = 1.0;
            grid.IsEnabled = true;
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            if (Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows)
                this.Visibility = false;
            else
                this.IsVisible = false;
            grid.Opacity = 1.0;
            grid.IsEnabled = true;
        }

        private void EditView_BindingContextChanged(object sender, EventArgs e)
        {
            var orderInfo = EditorView.BindingContext as OrderInfo;
            swipedRowData = new OrderInfo() { ShipCountry = orderInfo.ShipCountry, OrderID = orderInfo.OrderID, CustomerID = orderInfo.CustomerID, FirstName = orderInfo.FirstName };
            foreach (ContentView child in EditorView.Children)
            {
                child.Content.BindingContext = EditorView.BindingContext;
            }
        }

        #endregion

        #region Protected Methods
        
        protected override bool ShouldInvalidateOnChildAdded(View child)
        {
            return false;
        }

        protected override bool ShouldInvalidateOnChildRemoved(View child)
        {
            return false;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (this.isSuspend || isLoaded)
                return;

            foreach (var column in Columns)
            {
                var labelContentView = new ContentView();
                var editorContentView = new ContentView();
                var label = new Label() { BackgroundColor = Color.FromRgb(50, 50, 50), WidthRequest = (this.Parent as VisualElement).Width / 3, HeightRequest = 50, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, FontSize = 15, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, Text = column.HeaderText, BindingContext = this.BindingContext };
                var editor = new Entry() { BackgroundColor = Color.FromRgb(50, 50, 50), TextColor = Color.White, WidthRequest = (this.Parent as VisualElement).Width / 3, HeightRequest = 50, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, BindingContext = this.BindingContext };
                //new Entry() { BackgroundColor = Color.Black, WidthRequest = this.ParentView.Width / 3, HeightRequest = 50, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, TextColor = Color.White, BindingContext = this.BindingContext };

                editor.SetBinding(Entry.TextProperty, new Binding(column.MappingName, BindingMode.TwoWay));
                labelContentView.Content = label;
                editorContentView.Content = editor;
                HeaderView.Children.Add(labelContentView);
                EditorView.Children.Add(editorContentView);
            }

            this.FormContentView.Children.Add(HeaderView);
            this.FormContentView.Children.Add(EditorView);
            this.Children.Add(this.Title);
            this.Children.Add(FormContentView);
            this.Children.Add(Footer);
            isLoaded = true;

        }

        #endregion
    }

    public class RowTemplate : Grid
    {        
        #region Constructor

        public RowTemplate()
        {
            this.BackgroundColor = Color.White;
            this.Children.Add(CreateLabel("OrderID"));
            this.Children.Add(new BoxView() { Color = Color.Gray, WidthRequest = 1 });
            this.Children.Add(CreateLabel("EmployeeID"));
            this.Children.Add(new BoxView() { Color = Color.Gray, WidthRequest = 1 });
            this.Children.Add(CreateLabel("CustomerID"));
            this.Children.Add(new BoxView() { Color = Color.Gray, WidthRequest = 1 });
            this.Children.Add(CreateLabel("FirstName"));
            this.Children.Add(new BoxView() { Color = Color.Gray, WidthRequest = 1 });
            this.Children.Add(CreateLabel("LastName"));
        }

        #endregion

        #region Private Method

        private ContentView CreateLabel(string Property)
        {
            var label = new Label();
            label.TextColor = Color.Black;
            label.LineBreakMode = LineBreakMode.NoWrap;
            label.FontSize = 12;
            label.HorizontalTextAlignment = TextAlignment.Center;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.SetBinding(Label.TextProperty, Property);
            return new ContentView() { Content = label };
        }

        #endregion

        #region Override Method

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            foreach (var child in Children)
            {
                if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
                {
                    if (child is ContentView)
                        child.Layout(new Rectangle(x, y, (width / ((Children.Count + 1) / 2)) - 0.5, height));
                    else
                        child.Layout(new Rectangle(x, y, 0.5, height));
                }
                else
                {
                    if (child is ContentView)
                        child.Layout(new Rectangle(x, y, (width / ((Children.Count + 1) / 2)) - 1, height));
                    else
                        child.Layout(new Rectangle(x, y, 1, height));

                }
                x += child.Width;
            }
        }

        #endregion
    }

}
