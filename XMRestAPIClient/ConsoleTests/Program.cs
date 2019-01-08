using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMRestAPIClient;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            SetAPISettings();
            var service = new SymbolService();
            Console.ReadKey();
        }
        private static void SetAPISettings()
        {
            XMRestSettings.ApiVersion = 1;
            XMRestSettings.BaseUrl = "http://192.168.2.215:2080/";
        }
    }

    public class RawMaterial : IXMModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TypeDefinitionId { get; set; }
        public string StateId { get; set; }
        public string MaterialPositionGroupId { get; set; }
        public string DxfConversionParamsId { get; set; }
        public string MaterialNumber { get; set; }
        public string TechMaterialNumber { get; set; }
        public string Description { get; set; }
        public string SupplierPartNumber { get; set; }

    }

    public class Symbol : IXMModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public int SymbolNumber { get; set; }
        public string Description { get; set; }
        public string SymbolFileName { get; set; }
        public double SymbolSize { get; set; }


    }

    public class SymbolService : XMBaseDataService<Symbol>
    {
        protected override string ApiName => "symbols";
        public SymbolService()
        {
            SaveItem(new Symbol()
            {
                Id = "6be58520-e45e-476d-89f0-c05f3c8e86e6",
                Description = "No desk",
                Name = "Test mabrouk 3",
                SymbolFileName = "no file",
                SymbolNumber = 66,
                SymbolSize = 60
            }).ContinueWith(p => { });

        }
    }

    public class RawMaterialService : XMBaseDataService<RawMaterial>
    {
        protected override string ApiName => "rawMaterials";
        public RawMaterialService()
        {
            GetData();

        }

        private async void GetData()
        {
            var lst = await this.GetAllItems();
            var item = lst.FirstOrDefault();
            item.SupplierPartNumber = "333";
            var res = await SaveItem(item);
        }
    }
}
