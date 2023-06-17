﻿using GTA5Menu.Data;

using GTA5Core.RAGE.Vehicles;
using GTA5Core.Features;
using GTA5Shared.Helper;

namespace GTA5Menu.Views.OnlineVehicle;

/// <summary>
/// AllVehicleView.xaml 的交互逻辑
/// </summary>
public partial class AllVehicleView : UserControl
{
    public AllVehicleView()
    {
        InitializeComponent();
        GTA5MenuWindow.WindowClosingEvent += GTA5MenuWindow_WindowClosingEvent;

        // 载具分类列表
        foreach (var vClass in VehicleHash.VehicleClasses)
        {
            ListBox_VehicleClasses.Items.Add(new IconMenu()
            {
                Icon = vClass.Icon,
                Title = vClass.Name
            });
        }
        ListBox_VehicleClasses.SelectedIndex = 0;
    }

    private void GTA5MenuWindow_WindowClosingEvent()
    {

    }

    /////////////////////////////////////////////////

    private void ListBox_VehicleTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lock (this)
        {
            var index = ListBox_VehicleClasses.SelectedIndex;
            if (index == -1)
                return;

            ListBox_VehicleInfos.Items.Clear();

            foreach (var item in VehicleHash.VehicleClasses[index].VehicleInfos)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, () =>
                {
                    if (index != ListBox_VehicleClasses.SelectedIndex)
                        return;

                    ListBox_VehicleInfos.Items.Add(new ModelInfo()
                    {
                        Class = item.Class,
                        Name = item.Name,
                        Value = item.Value,
                        Image = item.Image,
                        Mod = item.Mod
                    });
                });
            }
        }
    }

    private async void MenuItem_SpawnVehicleA_Click(object sender, RoutedEventArgs e)
    {
        AudioHelper.PlayClickSound();

        if (ListBox_VehicleInfos.SelectedItem is ModelInfo info)
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

        if (ListBox_VehicleInfos.SelectedItem is ModelInfo info)
        {
            await Vehicle2.SpawnVehicle(info.Value, 0.0f, 5, info.Mod);
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "请选择正确的载具，操作取消");
        }
    }

    private void MenuItem_AddMyFavorite_Click(object sender, RoutedEventArgs e)
    {
        AudioHelper.PlayClickSound();

        if (ListBox_VehicleInfos.SelectedItem is ModelInfo info)
        {
            MyVehicleView.ActionAddMyFavorite(info);
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "请选择正确的载具，操作取消");
        }
    }

    private void ListBox_VehicleInfos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        MenuItem_SpawnVehicleA_Click(null, null);
    }
}
