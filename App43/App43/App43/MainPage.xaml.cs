using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Xamarin.Essentials;
using System.Globalization;

namespace App43
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        public MediaFile _mediaFile { get; private set; }
        public Record _record{ get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _record = new Record();
            string myValue = Preferences.Get("my_File_Path", "default_value");
            _record.ImageFile = myValue;

            BindingContext = _record;
        }

        private async void PickPhotoClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Pick Photo", ":( No Pick Photo available", "OK");
                return;
            }

            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null)
            {
                return;
            }

            await DisplayAlert("File Path", _mediaFile.Path, "OK");

            Preferences.Set("my_File_Path", _mediaFile.Path);


            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediaFile.GetStream();
            });
        }
    }

    public class myImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;
            return ImageSource.FromFile(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Record : INotifyPropertyChanged
    {
        public string imageFile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Record()
        {
        }

        public string ImageFile
        {
            set
            {
                if (imageFile != value)
                {
                    imageFile = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ImageFile"));
                    }
                }
            }
            get
            {
                return imageFile;
            }
        }
    }
}
