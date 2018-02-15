using Discord;
using Discord.Commands;
using Discord.WebSocket;
using UCSbot.Music.Handlers;
using UCSbot.Music.Models;
using UCSbot.Music.Processes;
using UCSbot.Music.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UCSbot.Music.Modules
{
    public class Voice : ModuleBase
    {
        private readonly Settings _settings;
        private readonly Media _media;
        private readonly Youtube _youtube;
        uint color = 3901951;

        private int _voteCount;

        public Voice(Settings settings, Media media, Youtube youtube)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(Settings));
            _media = media ?? throw new ArgumentNullException(nameof(Media));
            _youtube = youtube ?? throw new ArgumentNullException(nameof(Youtube));
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannel()
        {
            if (_settings.VoiceClient == null)
            {
                var _voiceChannel = Context.Message.Author as IGuildUser;
                var channel = await Context.Client.GetChannelAsync(_voiceChannel.VoiceChannel.Id) as SocketVoiceChannel;
                if (channel != null) _settings.VoiceClient = await channel.ConnectAsync();
            }
        }

        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveChannel()
        {
            if (_settings.VoiceClient != null)
            {
                _media.StopCurrentStreamAsync();
                await _settings.VoiceClient.StopAsync();

                _settings.VoiceClient = null;
                _settings.PlayList.Clear();
                _settings.CurrentSong = "";
            }
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayMusic([Remainder]string url)
        {
            if (_settings.VoiceClient != null)
            {
                if (await _youtube.GetYoutubeSong(url, _settings))
                    await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + $" song: {_settings.PlayList.Last().Title} has been added to the queue.");
                else
                    await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + $" Error occured while adding a song to the queue, possible causes: {Environment.NewLine}- Song is too long.{Environment.NewLine}- Song does not exist.{Environment.NewLine}- Song is already in the queue.{Environment.NewLine}- No audio encoding received.");
            }
        }

        [Command("removesong", RunMode = RunMode.Async)]
        public async Task RemoveMusic(int queueNr)
        {
            await Task.Run(() =>
            {
                if (_settings.VoiceClient != null && queueNr > 0 && queueNr <= _settings.PlayList.Count)
                {
                    _settings.PlayList.RemoveAt(queueNr - 1);
                }
            });
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopMusic()
        {
            if (_settings.VoiceClient == null) return;

            _media.StopCurrentStreamAsync();

            _settings.PlayList.Clear();
            _settings.CurrentSong = "";

            await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + " Stopped playing music and cleared the queue.");
        }

        [Command("next", RunMode = RunMode.Async)]
        public async Task SkipCurrentSong()
        {
            if (_settings.VoiceClient != null && _settings.CurrentSong != "")
            {
                var _voiceChannel = Context.Message.Author as IGuildUser;
                _voteCount++;
                var voiceChannel = await Context.Client.GetChannelAsync(_voiceChannel.VoiceChannel.Id) as SocketVoiceChannel;
                if (voiceChannel != null)
                {
                    var userCount = voiceChannel.Users.Count;
                    if (_voteCount >= (userCount / 2))
                    {
                        _media.StopCurrentStreamAsync();
                        await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + $" Stopped playing {_settings.CurrentSong}");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync($"{_voteCount} people have voted to skip this song. {(userCount / 2) - _voteCount} more votes required to skip this song.");
                    }
                }
            }
        }

        [Command("queue", RunMode = RunMode.Async)]
        public async Task ShowQueue()
        {
            if (_settings.PlayList.Any())
            {
                var songNr = 0;
                var songList = "**Current song:** " + _settings.CurrentSong + "\n \n";
                songList += "**Song Queue:** \n \n";

                foreach (var song in _settings.PlayList)
                {
                    if (songNr == 25) break;
                    songNr++;

                    songList += "**" + songNr + ".** ";
                    songList += song.Title + " ";
                    songList += song.Duration.Minutes + ":" + song.Duration.Seconds + "\n";
                }

                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                .WithDescription(songList);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            else
                await Context.Channel.SendMessageAsync(Context.Message.Author.Mention + " No songs in the queue.");
        }
    }
}