using System;
using System.Windows.Controls;
using GoogleAds;

namespace ChatRooms.Control
{
    public partial class AdsFull : UserControl
    {
        private InterstitialAd interstitialAd;

        public AdsFull()
        {
            InitializeComponent();

            OnRequestInterstitialClick();
        }

        private void OnShowInterstitialClick(object sender, EventArgs e)
        {
            // Show Interstitial can only be clicked after an interstitial is received.
            interstitialAd.ShowAd();
        }

        private void OnRequestInterstitialClick()
        {
            // NOTE: Edit "UA-51287516-7" with your interstitial
            // ad unit id.
            interstitialAd = new InterstitialAd("ca-app-pub-9908242075575541/9823166712");
            // NOTE: You can edit the event handler to do something custom here. Once the
            // interstitial is received it can be shown whenever you want.
            AdRequest adRequest = new AdRequest();
            adRequest.ForceTesting = false;
            interstitialAd.LoadAd(adRequest);
        }
    }
}
