using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dpx.Services;
using Dpx.Services.Implementations;
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

        private ISyncService _azureSyncService;

        /// <summary>
        /// OneDrive收藏存储
        /// </summary>
        private IRemoteFavoriteStorage _oneDriveFavoriteStorage;

        /// <summary>
        /// Auzre收藏存储
        /// </summary>
        private IRemoteFavoriteStorage _azureFavoriteStorage;

        //******** 构造函数

        /// <summary>
        /// 同步页ViewModel
        /// </summary>
        /// <param name="oneDriveFavoriteStorage">OneDrive收藏存储</param>
        /// /// <param name="localFavoriteStorage">本地的收藏存储</param>
        public SyncPageViewModel(
            OneDriveFavoriteStorage oneDriveFavoriteStorage,
            AzureFavoriteStorage azureFavoriteStorage,
            IFavoriteStorage localFavoriteStorage)
        {
            _oneDriveFavoriteStorage = oneDriveFavoriteStorage;
            _oneDriveSyncService = new SyncService(localFavoriteStorage,oneDriveFavoriteStorage);

            _oneDriveSyncService.StatusChanged += _oneDriveSyncService_StatusChanged;
            _oneDriveFavoriteStorage.StatusChanged += _oneDriveFavoriteStorage_StatusChanged;


            _azureFavoriteStorage = azureFavoriteStorage;
            _azureSyncService = new SyncService(localFavoriteStorage, azureFavoriteStorage);

            _azureSyncService.StatusChanged += _azureSyncService_StatusChanged;
            _azureFavoriteStorage.StatusChanged += _azureFavoriteStorage_StatusChanged;
        }

        //事件
        private void _oneDriveFavoriteStorage_StatusChanged(object sender, EventArgs e)
        {
            OneDriveStatus = _oneDriveFavoriteStorage.Status;
        }

        private void _oneDriveSyncService_StatusChanged(object sender, EventArgs e)
        {
            OneDriveStatus = _oneDriveSyncService.Status;
        }

        private void _azureFavoriteStorage_StatusChanged(object sender, EventArgs e)
        {
            AzureStatus = _azureFavoriteStorage.Status;
        }

        private void _azureSyncService_StatusChanged(object sender, EventArgs e)
        {
            AzureStatus = _azureSyncService.Status;
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

        public bool OneDriveSignedIn
        {
            get => _oneDriveSignedIn;
            set => Set(nameof(OneDriveSignedIn), ref _oneDriveSignedIn, value);
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
        /// Azure状态
        /// </summary>
        private string _azureStatus;

        public string AzureStatus
        {
            get => _azureStatus;
            set => Set(nameof(AzureStatus), ref _azureStatus, value);
        }

        /// <summary>
        /// 已登录
        /// </summary>
        private bool _azureSignedIn;

        public bool AzureSignedIn
        {
            get => _azureSignedIn;
            set => Set(nameof(AzureSignedIn), ref _azureSignedIn, value);
        }

        /// <summary>
        /// 正在登录
        /// </summary>
        private bool _azureLoading;

        public bool AzureLoading
        {
            get => _azureLoading;
            set => Set(nameof(AzureLoading), ref _azureLoading, value);
        }

        //******** 绑定命令

        /// <summary>
        /// 页面显示命令
        /// </summary>
        private RelayCommand __pageAppearing;

        public RelayCommand PageAppearingCommand =>
            __pageAppearing ?? (__pageAppearing = new RelayCommand( () =>  PageAppearingCommandFunction()));

        public void PageAppearingCommandFunction()
        {
            Task.Run(async () =>
            {
                OneDriveLoading = true;
                OneDriveSignedIn = await _oneDriveFavoriteStorage.IsSignedInAsync();
                OneDriveLoading = false;
            });
            

            Task.Run(async () =>
            {
                AzureLoading = true;
                AzureSignedIn = await _azureFavoriteStorage.IsSignedInAsync();
                AzureLoading = false;
            });
            

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
            OneDriveSignedIn = await _oneDriveFavoriteStorage.SignInAsync();
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
            OneDriveSignedIn = false;
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

        /// <summary>
        /// Azure登录命令
        /// </summary>
        private RelayCommand _azureSignInCommand;

        public RelayCommand AzureSignInCommand =>
            _azureSignInCommand ?? (_azureSignInCommand = new RelayCommand(async () => await AzureSignInCommandFunction()));

        public async Task AzureSignInCommandFunction()
        {
            AzureLoading = true;
            AzureSignedIn = await _azureFavoriteStorage.SignInAsync();
            AzureLoading = false;
        }


        /// <summary>
        /// Azure注销命令
        /// </summary>
        private RelayCommand _azureSignOutCommand;

        public RelayCommand AzureSignOutCommand =>
            _azureSignOutCommand ?? (_azureSignOutCommand = new RelayCommand(async () => await AzureSignOutCommandFunction()));

        public async Task AzureSignOutCommandFunction()
        {
            AzureLoading = true;
            await _azureFavoriteStorage.SignOutAsync();
            AzureSignedIn = false;
            AzureLoading = false;
        }

        /// <summary>
        /// Azure同步命令。
        /// </summary>
        private RelayCommand _azureSyncCommand;

        public RelayCommand AzureSyncCommand =>
            _azureSyncCommand ?? (_azureSyncCommand = new RelayCommand(async () => await AzureSyncCommandFunction()));

        public async Task AzureSyncCommandFunction()
        {
            AzureLoading = true;
            await _azureSyncService.SyncAsync();
            AzureLoading = false;
        }
    }
}
