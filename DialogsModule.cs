using Prism.Ioc;
using Prism.Modularity;
using RYCBEditorX.Dialogs.Views;
using RYCBEditorX.Utils;

namespace RYCBEditorX.Dialogs;
public class DialogsModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        GlobalConfig.CurrentLogger.Log("Dialogs模块初始化完成.", module: EnumLogModule.CUSTOM, customModuleName: "Dialogs模块:初始化");
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterDialog<FileCreator>();
        containerRegistry.RegisterDialog<ProjectCreator>();
        containerRegistry.RegisterDialog<FluentMessageBox>();
        containerRegistry.RegisterDialog<LightTip>();
    }
}