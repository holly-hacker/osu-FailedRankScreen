using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace osu_FailedRankScreen
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Debug.WriteLine("Trying to find osu! location");
            Process[] pl = Process.GetProcessesByName("osu!");
            if (pl.Length == 0) ExitError("osu! is not running!");
            Process p = pl.First();
            string path = p.MainModule.FileName;

            Debug.WriteLine("Loading assembly");
            Assembly ass = Assembly.LoadFrom(path);

            Debug.WriteLine("Finding type");
            Type t = ass.ExportedTypes.First(a => a.FullName == "osu.Helpers.InterProcessOsu");

            Debug.WriteLine("Getting InterProcessOsu");
            object ipo = Activator.GetObject(t, "ipc://osu!/loader");

            Debug.WriteLine("Executing method");
            t.GetMethod("ChangeMode").Invoke(ipo, new object[] { 7 });

            MessageBox.Show("Done!");
        }

        private static void ExitError(string message)
        {
            MessageBox.Show(message, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(-1);
        }

        /*
        public static void Unsafe() //in 3 lines :)
        {
            string p = Process.GetProcessesByName("osu!")[0].MainModule.FileName;
            Type t = Assembly.LoadFrom(p).ExportedTypes.First(a => a.FullName == "osu.Helpers.InterProcessOsu");
            t.GetMethod("ChangeMode").Invoke(Activator.GetObject(t, "ipc://osu!/loader"), new object[] { 7 });
        }
        */
    }
}
