using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Bitango_.Styles
{
    public partial class GeneralDevicesStyle : ResourceDictionary
    {
        public static GeneralDevicesStyle SharedInstance { get; } = new GeneralDevicesStyle();
        public GeneralDevicesStyle()
        {
            InitializeComponent();
        }
    }
}
