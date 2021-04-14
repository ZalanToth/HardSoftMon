using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using Microsoft.Win32;
using System.IO;

namespace HardSoftMon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        List<Szoftver> szoftverek = new List<Szoftver>();
        List<Ram> ramok = new List<Ram>();
        List<Drives> driveok = new List<Drives>();
        bool ki = true;
        public MainWindow()
        {
            InitializeComponent();
            Alaplap();
            Processzor();
            VidKar();       
            RAM();
            Meghajtok();
            Szoftverek();
            ValtDrive();
            Betoltes();
        }

        public void Alaplap()
        {
            ManagementObjectSearcher alaplap = new ManagementObjectSearcher("SELECT * FROM Win32_Baseboard");
            foreach (var item in alaplap.Get())
            {
                al_gyarto.Content = $" A gyártó: {item["Manufacturer"]}";
                al_modell.Content = $" A modell: {item["Model"]}";
                al_sorozatszam.Content = $" A sorozatszám: { item["SerialNumber"]}";
                al_termek.Content = $" A terméknév: {item["Product"]}";
            }
        }

        public void Processzor()
        {
            ManagementObjectSearcher proc = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (var item in proc.Get())
            {
                pr_nev.Content = $"A processzor: {item["Name"]}";
                pr_magok.Content = $"A magok száma: {item["NumberOfCores"]}";
                pr_szalak.Content = $"A szálak száma: {item["ThreadCount"]}";
            }
        }

        public void VidKar()
        {
            ManagementObjectSearcher gpu = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (var item in gpu.Get())
            {
                vk_vram.Content = $"A Videómemória: {SizeSuffix(Convert.ToInt64(item["AdapterRAM"]))}";
                vk_nev.Content = $"A videókártya: {item["Name"]}";
            }
        }

        public void RAM()
        {
            ManagementObjectSearcher memoria = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            foreach (var item in memoria.Get())
            {
                if(item!=null)
                    ramok.Add(new Ram( item["Tag"].ToString(), SizeSuffix(Convert.ToInt64(item["Capacity"])).ToString(), item["MemoryType"].ToString() ));
            }
        }

        public void Meghajtok()
        {
            ManagementObjectSearcher meghajtok = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (var item in meghajtok.Get())
            {
                if(item!=null)
                    driveok.Add(new Drives(item["Manufacturer"].ToString(), item["Name"].ToString(), item["Partitions"].ToString(),SizeSuffix(Convert.ToInt64(item["Size"].ToString()))));
            }
        }
        
        private void ValtDrive()
        {
            /*Drives sldrive = driveok.Where(x => x.Gyarto == drivevalaszt.SelectedItem.ToString()).First();
            nev.Content = $"Név: {sldrive.Nev}";
            Gyart.Content= $"{sldrive.Gyarto}";
            osszhely.Content= $"{sldrive.Helyossz}";
            particiok.Content= $"{sldrive.Particio}";*/
        }
        

        

        private void ValtRAM(object sender, SelectionChangedEventArgs e)
        {
            Ram slram = ramok.Where(x => x.Tag == ramokvalaszt.SelectedItem.ToString()).First();
            mr_nev.Content = $"A memória: {slram.Tag}";
            mr_kapac.Content= $"A kapacitása: {slram.Kapacitas}";
            mr_tipus.Content= $"A típusa: {slram.Tipus}";
        }

        private void Betoltes()
        {
            foreach (var item in ramok)
            {
                ramokvalaszt.Items.Add(item.Tag);
            }
            /*foreach(var item in driveok)
            {
                drivevalaszt.Items.Add(item.Gyarto);
            }*/
        }

        private void Szoftverek()
        {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (subkey.GetValue("DisplayName") != null)
                        {
                            szoftverek.Add(new Szoftver
                            {
                                Nev = (string)subkey.GetValue("DisplayName"),
                                Verzio = (string)subkey.GetValue("DisplayVersion"),
                                Letoltes = (string)subkey.GetValue("InstallDate"),
                                Kiado = (string)subkey.GetValue("Publisher"),

                            });
                        }
                    }
                }
            }
            SzoftverDG.ItemsSource = szoftverek;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (ki)
            {
                tipusok.Visibility = Visibility.Hidden;
                ki = false;
            }
            else if(!ki)
            {
                tipusok.Visibility = Visibility.Visible;
                ki = true;
            }
            
        }

        private string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
