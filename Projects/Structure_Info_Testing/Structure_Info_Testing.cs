using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
// [assembly: ESAPIScript(IsWriteable = true)]

namespace Structure_Info_Testing
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                using (Application app = Application.CreateApplication())
                {
                    Execute(app);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
        static void Execute(Application app)
        {
            // TODO: Add your code here.
            //access the patient
            Patient p = app.OpenPatientById("ECC-001");
            IEnumerable<StructureSet> ss_set = p.StructureSets;
            //test the approval history output.
            foreach (StructureSet ss in ss_set)
            {
                
                Console.WriteLine($"History for {ss.Id}\n{ss.HistoryDateTime} - {ss.HistoryUserName}");
                Console.WriteLine("Structure Code Info");
                foreach(Structure s in ss.Structures)
                {
                    //structure codes.
                    string output = s.StructureCodeInfos.Count() == 0 ?//if no structure codes
                        "No Codes on this structure" :
                        s.StructureCodeInfos.First().Code;
                    Console.WriteLine($"\t Code for {s.Id} = {output}");
                }
                //get center of mass
                Console.WriteLine("DICOM structure center [mm]");
                foreach(Structure s in ss.Structures)
                {
                    string output = s.Volume == 0 ?
                        "No COM" :
                        $"X: {s.CenterPoint.x}, Y: {s.CenterPoint.y}, Z: {s.CenterPoint.z}";
                    Console.WriteLine($"\t Center Point for {s.Id} = {output}");
                }
            }
            //compare center points of each GTV contoured.
            List<VVector> center = new List<VVector>();
            foreach(StructureSet ss in ss_set)
            {
                foreach(Structure s in ss.Structures.Where(x=>x.DicomType == "GTV"))
                {
                    if(s.Volume != 0)
                    {
                        center.Add(s.CenterPoint);
                    }
                }
            }
            string x_pos = string.Join(Convert.ToString(center.Select(o => o.x)), ", ");
            Console.WriteLine($"X position for all GTVs {x_pos}");
            Console.ReadLine();
        }
    }
}
