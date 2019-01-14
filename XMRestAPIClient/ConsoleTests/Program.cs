using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XMRestAPIClient;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //  SetAPISettings();
            var service = new SymbolService();
            Console.ReadKey();
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

    public class BaseService<T> : XMBaseDataService<T> where T : IXMModel
    {
        public override int ApiVersion => 1;
        public override string BaseAPIUrl => "";
        public BaseService():base()
        {

        }
    }

    public class SymbolService : BaseService<Symbol>
    {
        public override string ApiName => "symbols";
        public SymbolService()
        {
            SaveItemAsync(new Symbol()
            {
                Id = "6be58520-e45e-476d-89f0-c05f3c8e86e6",
                Description = "No desk",
                Name = "Test mabrouk 3",
                SymbolFileName = "no file",
                SymbolNumber = 25,
                SymbolSize = 60
            }).ContinueWith(p =>
            {

                GetAllItemsAsync().ContinueWith(i => { });
            });

        }

    }

    public class RawMaterialService : XMBaseDataService<RawMaterial>
    {
        public override string ApiName => "rawMaterials";
        public RawMaterialService()
        {
            GetData();

        }

        private async void GetData()
        {
            var lst = await this.GetAllItemsAsync();
            var item = lst.FirstOrDefault();
            item.SupplierPartNumber = "333";
            var res = await SaveItemAsync(item);
        }
    }
}
