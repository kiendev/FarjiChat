using System.Diagnostics;
using GoogleAds;

namespace ChatRooms.Utils
{
    public class AdsService
    {
        public static void OnRequestInterstitialClick()
        {
            // NOTE: Edit "UA-51287516-7" with your interstitial
            // ad unit id.
            App.InterstitialAd = new InterstitialAd("ca-app-pub-9908242075575541/9823166712");
            App.InterstitialAd.FailedToReceiveAd += OnFailedToReceiveAd;
            App.InterstitialAd.DismissingOverlay += OnDismissingOverlay;
            var adRequest = new AdRequest {ForceTesting = false};
            App.InterstitialAd.LoadAd(adRequest);
        }

        private static void OnDismissingOverlay(object sender, AdEventArgs e)
        {
            
        }

        private static void OnFailedToReceiveAd(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine("Not load ads");
        }
    }
}
