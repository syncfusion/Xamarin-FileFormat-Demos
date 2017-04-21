using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleBrowser
{
    public static class DeviceExt  
    {
        public static T OnPlatform<T>(T iOS, T Android, T WinPhone)
        {
            if (Device.OS == TargetPlatform.iOS)
                return iOS;
            else if (Device.OS == TargetPlatform.Android)
                return Android;
            else
                return WinPhone;
        }
    }
}
