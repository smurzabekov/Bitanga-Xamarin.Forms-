using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Bitango_.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Bitango_.Customs;
using Android.Support.Design.Internal;
using Android.Util;
using Android.OS;
using Android.Support.Design.BottomNavigation;

[assembly: ExportRenderer(typeof(CustomShell), typeof(CustomShellRenderer))]
namespace Bitango_.Droid.CustomRenderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context)
        {
        }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new CustomBottomNavAppearance();
        }
    }
    public class CustomBottomNavAppearance : IShellBottomNavViewAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(BottomNavigationView bottomView)
        {
        }

        public void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            bottomView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityUnlabeled;
            bottomView.ItemIconTintList = null;

            IMenu myMenu = bottomView.Menu;
            IMenuItem home = myMenu.GetItem(0);
            IMenuItem gift = myMenu.GetItem(1);
            IMenuItem info = myMenu.GetItem(2);
            IMenuItem profile = myMenu.GetItem(3);

            if (home.IsChecked)
            {
                home.SetIcon(Resource.Drawable.home_selected);
            }
            else
            {
                home.SetIcon(Resource.Drawable.home);
            }

            if (gift.IsChecked)
            {
                gift.SetIcon(Resource.Drawable.gift_selected);
            }
            else
            {
                gift.SetIcon(Resource.Drawable.gift);
            }

            if (info.IsChecked)
            {
                info.SetIcon(Resource.Drawable.info_selected);
            }
            else
            {
                info.SetIcon(Resource.Drawable.info);
            }

            if (profile.IsChecked)
            {
                profile.SetIcon(Resource.Drawable.profile_selected);
            }
            else
            {
                profile.SetIcon(Resource.Drawable.profile);
            }
        }
    }
}