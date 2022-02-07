using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bitango_.iOS.CustomRenderers;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Bitango_.Customs;

[assembly: ExportRenderer(typeof(CustomShell), typeof(CustomShellRenderer))]
namespace Bitango_.iOS.CustomRenderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        {
            return new CustomTabbarAppearance();
        }
    }

    public class CustomTabbarAppearance : IShellTabBarAppearanceTracker
    {
        public void Dispose()
        {
        }

        public void ResetAppearance(UITabBarController controller)
        {
        }

        public void SetAppearance(UITabBarController controller, ShellAppearance appearance)
        {
            UITabBar myTabBar = controller.TabBar;

            if (myTabBar.Items != null)
            {
                UITabBarItem home = myTabBar.Items[0];
                UITabBarItem gift = myTabBar.Items[1];
                UITabBarItem info = myTabBar.Items[2];
                UITabBarItem profile = myTabBar.Items[3];

                home.Image = UIImage.FromBundle("home.png");
                home.SelectedImage = UIImage.FromBundle("home_selected.png");


                gift.Image = UIImage.FromBundle("gift.png");
                gift.SelectedImage = UIImage.FromBundle("gift_selected.png");

                info.Image = UIImage.FromBundle("info.png");
                info.SelectedImage = UIImage.FromBundle("info_selected.png");

                profile.Image = UIImage.FromBundle("profile.png");
                profile.SelectedImage = UIImage.FromBundle("profile_selected.png");

                foreach (UITabBarItem tabbaritem in controller.TabBar.Items)
                {
                    if (tabbaritem.Image == null) continue;

                    tabbaritem.Image = tabbaritem.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                    tabbaritem.SelectedImage = tabbaritem.SelectedImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                }
            }
        }

        public void UpdateLayout(UITabBarController controller)
        {
            foreach (UIViewController vc in controller.ViewControllers)
            {
                vc.TabBarItem.ImageInsets = new UIEdgeInsets(242, 237, 232, 237);
            }
        }
    }
}