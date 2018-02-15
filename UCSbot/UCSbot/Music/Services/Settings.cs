using Discord.Audio;
using UCSbot.Music.Models;
using System.Collections.Generic;

namespace UCSbot.Music.Services
{
    public class Settings
    {
        public string CurrentSong = "";
        public List<Song> PlayList = new List<Song>();
        public IAudioClient VoiceClient = null;
    }
}