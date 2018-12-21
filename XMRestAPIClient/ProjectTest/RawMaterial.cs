using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMRestAPIClient;

namespace ProjectTest
{
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


    public class RawMaterialService : XMBaseDataService<RawMaterial>
    {
        protected override string ApiName => "rawMaterials";
        public RawMaterialService()
        {
            this.GetItem("69745e8f-8094-4165-92f8-075dbbe4b106").ContinueWith(p => {
                var item = p.Result;
                var str = ";";
            });
        }
    }
}
