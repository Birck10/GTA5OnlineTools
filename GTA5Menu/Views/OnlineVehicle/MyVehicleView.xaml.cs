﻿using GTA5Menu.Data;
using GTA5Menu.Config;

using GTA5Core.RAGE;
using GTA5Core.RAGE.Vehicles;
using GTA5Core.Features;
using GTA5Shared.Helper;

namespace GTA5Menu.Views.OnlineVehicle;

/// <summary>
/// MyVehicleView.xaml 的交互逻辑
/// </summary>
public partial class MyVehicleView : UserControl
{
    public ObservableCollection<ModelInfo> MyFavorites { get; set; } = new();

    public static Action<ModelInfo> ActionAddMyFavorite;

    public MyVehicleView()
    {
        InitializeComponent();
        GTA5MenuWindow.WindowClosingEvent += GTA5MenuWindow_WindowClosingEvent;

        ActionAddMyFavorite = AddMyFavorite;

        ReadConfig();
    }

    private void GTA5MenuWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /////////////////////////////////////////////////

    /// <summary>
    /// 读取配置文件
    /// </summary>
    private void ReadConfig()
    {
        if (!File.Exists(FileHelper.File_Config_Vehicles))
            return;

        var vehicles = JsonHelper.ReadFile<List<Vehicles>>(FileHelper.File_Config_Vehicles);

        foreach (var item in vehicles)
        {
            var classes = VehicleHash.VehicleClasses.Find(v => v.Name == item.Class);
            if (classes != null)
            {
                var info = classes.VehicleInfos.Find(v => v.Value == item.Value);
                if (info != null)
                {
                    MyFavorites.Add(new()
                    {
                        Class = classes.Name,
                        Name = info.Name,
                        Value = info.Value,
                        Image = RAGEHelper.GetVehicleImage(info.Value),
                        Mod = info.Mod
                    });
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        if (!Directory.Exists(FileHelper.Dir_Config))
            return;

        var vehicles = new List<Vehicles>();
        foreach (ModelInfo info in ListBox_Vehicles.Items)
        {
            vehicles.Add(new()
            {
                Class = info.Class,
                Name = info.Name,
                Value = info.Value,
            });
        }
        // 写入到Json文件
        JsonHelper.WriteFile(FileHelper.File_Config_Vehicles, vehicles);
    }

    private void AddMyFavorite(ModelInfo model)
    {
        if (!MyFavorites.Contains(model))
        {
            MyFavorites.Add(model);

            NotifierHelper.Show(NotifierType.Success, $"添加载具 {model.Name} 到我的收藏成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, $"载具 {model.Name} 已存在，请勿重复添加");
        }
    }

    private async void MenuItem_SpawnVehicleA_Click(object sender, RoutedEventArgs e)
    {
        AudioHelper.PlayClickSound();

        if (ListBox_Vehicles.SelectedItem is ModelInfo info)
        {
            await Vehicle2.SpawnVehicle(info.Value, -255.0f, 5, info.Mod);
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "请选择正确的载具，操作取消");
        }
    }

    private async void MenuItem_SpawnVehicleB_Click(object sender, RoutedEventArgs e)
    {
        AudioHelper.PlayClickSound();

        if (ListBox_Vehicles.SelectedItem is ModelInfo info)
        {
            await Vehicle2.SpawnVehicle(info.Value, 0.0f, 5, info.Mod);
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "请选择正确的载具，操作取消");
        }
    }

    private void MenuItem_DeleteMyFavorite_Click(object sender, RoutedEventArgs e)
    {
        AudioHelper.PlayClickSound();

        if (ListBox_Vehicles.SelectedItem is ModelInfo info)
        {
            MyFavorites.Remove(info);

            NotifierHelper.Show(NotifierType.Success, $"从我的收藏删除载具 {info.Name} 成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "请选择正确的载具，操作取消");
        }
    }

    private void ListBox_Vehicles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        MenuItem_SpawnVehicleA_Click(null, null);
    }
}
