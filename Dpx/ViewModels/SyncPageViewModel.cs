using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dpx.ViewModels
{
    /// <summary>
    /// 同步页面ViewModel。
    /// </summary>
    public class SyncPageViewModel : ViewModelBase
    {
        //******** 私有变量
        /// <summary>
        /// Ondrive同步服务
        /// </summary>
        private ISyncService _oneDriveSyncService;

        /// <summary>
        /// OneDrive收藏存储
        /// </summary>
        private IRemoteFavoriteStorage _oneDriveFavoriteStorage;

        //******** 构造函数

        /// <summary>
        /// 同步页ViewModel
        /// </summary>
        /// <param name="oneDriveFavoriteStorage">OneDrive收藏存储</param>
        /// /// <param name="localFavoriteStorage">本地的收藏存储</param>
        public SyncPageViewModel(
            OneDriveFavoriteStorage oneDriveFavoriteStorage,
            IFavoriteStorage localFavoriteStorage)
        {
            _oneDriveFavoriteStorage = oneDriveFavoriteStorage;
            _oneDriveSyncService = new SyncService(localFavoriteStorage,oneDriveFavoriteStorage);

            _oneDriveSyncService.StatusChanged += _oneDriveSyncService_StatusChanged;
            _oneDriveFavoriteStorage.StatusChanged += _oneDriveFavoriteStorage_StatusChanged;
        }

        private void _oneDriveFavoriteStorage_StatusChanged(object sender, EventArgs e)
        {
            OneDriveStatus = _oneDriveFavoriteStorage.Status;
        }

        private void _oneDriveSyncService_StatusChanged(object sender, EventArgs e)
        {
            OneDriveStatus = _oneDriveSyncService.Status;
        }
        //******** 绑定属性
        /// <summary>
        /// OneDrive状态
        /// </summary>
        private string _onewDriveStatus;

        public string OneDriveStatus
        {
            get => _onewDriveStatus;
            set => Set(nameof(OneDriveStatus), ref _onewDriveStatus, value);
        }

        /// <summary>
        /// 已登录
        /// </summary>
        private bool _oneDriveSignedIn;

        public bool OneDrivSignedIn
        {
            get => _oneDriveSignedIn;
            set => Set(nameof(OneDrivSignedIn), ref _oneDriveSignedIn, value);
        }

        /// <summary>
        /// 正在登录
        /// </summary>
        private bool _oneDriveLoading;

        public bool OneDriveLoading
        {
            get => _oneDriveLoading;
            set => Set(nameof(OneDriveLoading), ref _oneDriveLoading, value);
        }


        /// <summary>
        /// 页面显示命令
        /// </summary>
        private RelayCommand __pageAppearing;

        public RelayCommand PageAppearingCommand =>
            __pageAppearing ?? (__pageAppearing = new RelayCommand(async () => await PageAppearingCommandFunction()));

        public async Task PageAppearingCommandFunction()
        {
            OneDriveLoading = true;
            OneDrivSignedIn = await _oneDriveFavoriteStorage.IsSignedInAsync();
            OneDriveLoading = false;

        }

        /// <summary>
        /// OneDrive登录命令
        /// </summary>
        private RelayCommand _oneDriveSignInCommand;

        public RelayCommand OneDriveSignInCommand =>
            _oneDriveSignInCommand ?? (_oneDriveSignInCommand = new RelayCommand(async () => await OneDriveSignInCommandFunction()));

        public async Task OneDriveSignInCommandFunction()
        {
            OneDriveLoading = true;
            OneDrivSignedIn = await _oneDriveFavoriteStorage.SignInAsync();
            OneDriveLoading = false;
        }


        /// <summary>
        /// OneDrive注销命令
        /// </summary>
        private RelayCommand _oneDriveSignOutCommand;

        public RelayCommand OneDriveSignOutCommand =>
            _oneDriveSignOutCommand ?? (_oneDriveSignOutCommand = new RelayCommand(async () => await OneDriveSignOutCommandFunction()));

        public async Task OneDriveSignOutCommandFunction()
        {
            OneDriveLoading = true;
            await _oneDriveFavoriteStorage.SignOutAsync();
            OneDrivSignedIn = false;
            OneDriveLoading = false;
        }

        /// <summary>
        /// OneDrive同步命令。
        /// </summary>
        private RelayCommand _oneDriveSyncCommand;

        public RelayCommand OneDriveSyncCommand =>
            _oneDriveSyncCommand ?? (_oneDriveSyncCommand = new RelayCommand(async () => await OneDriveSyncCommandFunction()));

        public async Task OneDriveSyncCommandFunction()
        {
            OneDriveLoading = true;
            await _oneDriveSyncService.SyncAsync();
            OneDriveLoading = false;
        }
    }
}
