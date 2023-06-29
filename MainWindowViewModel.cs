using OnlyUpCheat.Core;
using OnlyUpCheat.Game;
using OnlyUpCheat.KeyBoard;
using System.Threading.Tasks;
using System.Windows;

namespace OnlyUpCheat
{
    internal class MainWindowViewModel : ObservableObject
    {
        private string _gameStatus;
        public string GameStatus
        {
            get { return _gameStatus; }
            set
            {
                _gameStatus = value;
                OnPropertyChanged(nameof(GameStatus));
            }
        }
        private string _flyHackStatus;
        public string FlyHackStatus
        {
            get { return _flyHackStatus; }
            set
            {
                _flyHackStatus = value;
                OnPropertyChanged(nameof(FlyHackStatus));
            }
        }
        private void GameStatusChanged(bool status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (status)
                {
                    GameStatus = "RUNNING";
                    KeyBoardManager.Start();
                    Cheat.Start();
                    KeyBoardManager.FlyHackKeyPressed += FlyHackStatusChanged;
                }
                else
                {
                    GameStatus = "WAITING";
                    KeyBoardManager.Stop();
                    Cheat.Stop();
                    KeyBoardManager.FlyHackKeyPressed -= FlyHackStatusChanged;
                    FlyHackStatus = "DISABLED";
                }
            });
        }
        private void FlyHackStatusChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Cheat.FlyHackEnabled)
                    FlyHackStatus = "ENABLED";
                else
                    FlyHackStatus = "DISABLED";
            });
        }
        public string FlyHackEnableDisableHotkey
        {
            get { return Config.FlyHackEnableDisableHotkey.Letter; }
        }
        public string SaveCheckpointHotkey
        {
            get { return Config.SaveCheckpointHotkey.Letter; }
        }
        public string TeleportToCheckpointHotkey
        {
            get { return Config.TeleportToCheckpointHotkey.Letter; }
        }
        public MainWindowViewModel()
        {
            GameStatus = "WAITING";
            FlyHackStatus = "DISABLED";

            GameStatusWatcher.StatusChanged += GameStatusChanged;
            Task.Run(() => GameStatusWatcher.Start());
            Cheat.AddrError += FlyHackStatusChanged;
        }
        ~MainWindowViewModel()
        {
            GameStatusWatcher.StatusChanged -= GameStatusChanged;
            KeyBoardManager.FlyHackKeyPressed -= FlyHackStatusChanged;
            Cheat.AddrError -= FlyHackStatusChanged;
            GameStatusWatcher.Stop();
            KeyBoardManager.Stop();
            Cheat.Stop();
        }
    }
}