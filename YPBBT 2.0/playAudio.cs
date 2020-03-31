using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YPBBT_2._0
{
   
    class playAudio
    {
        MediaPlayer myPlayer = new MediaPlayer();
        public void playBossAlertaudio()
        {
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossSpawnAlert.wav");
            //player.Play();               
                myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossSpawnAlert.wav"));
                myPlayer.Play();     
        }
       
        public void playNightAlertaudio()
        {           
                myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/NightTimeAlert.wav"));
                myPlayer.Play();               
        }
        public void playImperialResetAlertaudio()
        {
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(System.IO.Directory.GetCurrentDirectory() + "/Resources/ImperialResetAlert.wav");
            //player.Play();         
                myPlayer.Open(new System.Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/ImperialResetAlert.wav"));
                myPlayer.Play();                       
        }
      

    }
}
