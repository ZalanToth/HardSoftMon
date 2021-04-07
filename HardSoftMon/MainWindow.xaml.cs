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

namespace HardSoftMon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        List<Ram> ramok = new List<Ram>();
        public MainWindow()
        {
            InitializeComponent();
            Alaplap();
            Processzor();
            VidKar();
            RAM();
            Rambe();
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
                /*mr_nev.Content = $"A memória: {item["Name"]}";
                mr_kapac.Content= $"A kapacitása: {SizeSuffix(Convert.ToInt64(item["Capacity"]))} ";
                mr_model.Content= $"A modell: {item["Model"]}";
                mr_tipus.Content= $"A típusa: {item["MemoryType"]}";*/
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

        private void Valt(object sender, SelectionChangedEventArgs e)
        {
            Ram slram = ramok.Where(x => x.tag == ramokvalaszt.SelectedItem.ToString()).First();
            mr_nev.Content = $"A memória: {slram.tag}";
            mr_kapac.Content= $"A kapacitása: {slram.kapacitas}";
            mr_tipus.Content= $"A típusa: {slram.tipus}";
        }

        private void Rambe()
        {
            foreach (var item in ramok)
            {
                ramokvalaszt.Items.Add(item.tag);
            }
        }
    }
}
