using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.VisualScripting.ElementInterface;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

namespace Structure_Info
{
    // TODO: Replace the existing class name with your own class name.
    public class structureStatsActionPack : VisualScriptElement
    {
        public structureStatsActionPack() { }
        public structureStatsActionPack(IVisualScriptElementRuntimeHost host) { }

        public override bool RequiresRuntimeConsole { get { return false; } }
        public override bool RequiresDatabaseModifications { get { return false; } }


        [ActionPackExecuteMethod]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public IEnumerable<structureStats> Execute(IEnumerable<StructureSet> structureSets)
        {
            // TODO: Add your code here.
            List<structureStats> ss_list = new List<structureStats>();
            foreach(StructureSet ss in structureSets)
            {
                ss_list.Add(new structureStats
                {
                    Id = ss.Id,
                    Value = "Number of structures",
                    Evaluation = ss.Structures.Count().ToString(),
                });
                
                foreach (Structure s in ss.Structures.Where(o=>o.DicomType == "GTV"))
                {
                    ss_list.Add(new structureStats
                    {
                        Id = s.Id,
                        Value = "GTV Volume [cc]",
                        Evaluation = s.Volume.ToString("F2"),
                    });
                }
            }
            return ss_list;
        }

        public override string DisplayName
        {
            get
            {
                // TODO: Replace "Element Name" with the name that you want to be displayed in the Visual Scripting UI.
                return "Structure Statistics";
            }
        }

        IDictionary<string, string> m_options = new Dictionary<string, string>();
        public override void SetOption(string key, string value)
        {
            m_options.Add(key, value);
        }

        public override IEnumerable<KeyValuePair<string, IEnumerable<string>>> AllowedOptions
        {
            get
            {
                return new KeyValuePair<string, IEnumerable<string>>[] {
            new KeyValuePair<string, IEnumerable<string>>("TestOption", new string[] { "Test Value" })
          };
            }
        }
        public class structureStats
        {
            public string Id { get; set; }
            public string Value { get; set; }
            public string Evaluation { get; set; }
        }
    }
}
