using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DependencyServicesSample.Droid;
using DependencyServicesSample.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryImplementation))]
namespace DependencyServicesSample.Droid
{
    public class BatteryImplementation : IBattery
    {
        public BatteryImplementation()
        {
        }

        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

                            return (int)Math.Floor(level * 100D / scale);
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public DependencyServicesSample.Interface.BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)Interface.BatteryStatus.Charging || status == (int)Interface.BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;
                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return DependencyServicesSample.Interface.BatteryStatus.Charging;

                            switch (status)
                            {
                                case (int)Interface.BatteryStatus.Charging:
                                    return DependencyServicesSample.Interface.BatteryStatus.Charging;
                                case (int)Interface.BatteryStatus.Discharging:
                                    return DependencyServicesSample.Interface.BatteryStatus.Discharging;
                                case (int)Interface.BatteryStatus.Full:
                                    return DependencyServicesSample.Interface.BatteryStatus.Full;
                                case (int)Interface.BatteryStatus.NotCharging:
                                    return DependencyServicesSample.Interface.BatteryStatus.NotCharging;
                                default:
                                    return DependencyServicesSample.Interface.BatteryStatus.Unknown;
                            }
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)Interface.BatteryStatus.Charging || status == (int)Interface.BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;

                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);

                            if (!isCharging)
                                return DependencyServicesSample.Interface.PowerSource.Battery;
                            else if (usbCharge)
                                return DependencyServicesSample.Interface.PowerSource.Usb;
                            else if (acCharge)
                                return DependencyServicesSample.Interface.PowerSource.Ac;
                            else if (wirelessCharge)
                                return DependencyServicesSample.Interface.PowerSource.Wireless;
                            else
                                return DependencyServicesSample.Interface.PowerSource.Other;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
                }
            }
        }
    }