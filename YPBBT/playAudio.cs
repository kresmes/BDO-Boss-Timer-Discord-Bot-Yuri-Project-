using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YPBBT
{
   
    class playAudio
    {
        MediaPlayer myPlayer = new MediaPlayer();
        public void playBossAlertaudio()
        {            
                myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/audio/BossSpawnAlert.mp3"));
                myPlayer.Play();     
        }
       
        public void playNightAlertaudio()
        {           
                myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/audio/NightTimeAlert.mp3"));
                myPlayer.Play();               
        }
        public void playImperialResetAlertaudio()
        {      
                myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/audio/ImperialResetAlert.mp3"));
                myPlayer.Play();                       
        }
        public void playBarteringAlertaudio()
        {
            myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/audio/BarteringAlert.mp3"));
            myPlayer.Play();
        }
        public void playImperialTradingResetAlertaudio()
        {
            myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/audio/ImperialTradingResetAlert.mp3"));
            myPlayer.Play();
        }


    }
}
