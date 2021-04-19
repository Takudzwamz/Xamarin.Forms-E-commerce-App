﻿
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace RealWorldApp.Droid
{
    [Activity(Label = "TJ Bakery", Icon = "@mipmap/TasheLogo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //below is partial code for Web Authenticator too. 
        const string CALLBACK_SCHEME = "myapp"; //this must be relative to the app bundle. com.appcompany.name

        [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
        [IntentFilter(new[] { Android.Content.Intent.ActionView },
            Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
            DataScheme = CALLBACK_SCHEME)]
        public class WebAuthenticationCallbackActivity : Xamarin.Essentials.WebAuthenticatorCallbackActivity
        {
        }
        protected override void OnResume()
        {
            base.OnResume();
            Xamarin.Essentials.Platform.OnResume(); //this will allow the app to continue the flow of execution 
        }
    }

}