using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DependencyServicesSample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BatteryDemo : ContentPage
    {
        public BatteryDemo()
        {
            InitializeComponent();

            StackLayout stack = new StackLayout();

            var button = new Button
            {
                Text = "Click for battery info",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            button.Clicked += (sender, e) =>
            {
                var bat = DependencyService.Get<Interface.IBattery>();

                switch (bat.PowerSource)
                {
                    case Interface.PowerSource.Battery:
                        button.Text = "Battery - ";
                        break;
                    case Interface.PowerSource.Ac:
                        button.Text = "AC - ";
                        break;
                    case Interface.PowerSource.Usb:
                        button.Text = "USB - ";
                        break;
                    case Interface.PowerSource.Wireless:
                        button.Text = "Wireless - ";
                        break;
                    case Interface.PowerSource.Other:
                    default:
                        button.Text = "Unknown - ";
                        break;
                }
                switch (bat.Status)
                {
                    case Interface.BatteryStatus.Charging:
                        button.Text += "Charging";
                        break;
                    case Interface.BatteryStatus.Discharging:
                        button.Text += "Discharging";
                        break;
                    case Interface.BatteryStatus.NotCharging:
                        button.Text += "Not Charging";
                        break;
                    case Interface.BatteryStatus.Full:
                        button.Text += "Full";
                        break;
                    case Interface.BatteryStatus.Unknown:
                    default:
                        button.Text += "Unknown";
                        break;
                }
            };
            stack.Children.Add(button);
            Content = stack;

        }
    }
}