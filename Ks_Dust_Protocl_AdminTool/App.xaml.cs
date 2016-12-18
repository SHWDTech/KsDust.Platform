using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using SHWDTech.Platform.ProtocolService.DataBase;
using SHWDTech.Platform.ProtocolService.ProtocolEncoding;
using SHWDTech.Platform.Utility;
using ReportService = Ks_Dust_Protocl_AdminTool.Common.ReportService;
using System.Collections.Generic;

namespace Ks_Dust_Protocl_AdminTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Current.DispatcherUnhandledException += AppUnhandleExceptionHandler;
            var ctx = new ProtocolContext();
            var protocols = new List<IProtocol>();
            protocols.AddRange(ctx.Protocols.Include("ProtocolStructures")
                .Include("ProtocolCommands.CommandDatas")
                .Include("Firmwares")
                .ToList());
            EncodingManager.LoadEncoder(protocols);
        }

        /// <summary>
        /// 未处理异常捕获器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            LogService.Instance.Fatal("未处理异常。", (Exception)e.ExceptionObject);
            MessageBox.Show("系统运行出现严重错误！");
        }

        /// <summary>
        /// 未处理异常捕获器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void AppUnhandleExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ReportService.Instance.Fatal("程序出现未处理异常。", e.Exception);
        }
    }
}
