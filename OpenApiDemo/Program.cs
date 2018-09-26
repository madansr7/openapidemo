using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.OData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenApiDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = (args.Length == 0) ? "content/TripService.xml" : args[0];
            var edmModel = LoadEdmModel(filePath);

            var openApiConvertorSettings = new OpenApiConvertSettings()
            {
                EnableKeyAsSegment = true,
                EnableNavigationPropertyPath = true,
                EnableUnqualifiedCall = true,
                PrefixEntityTypeNameBeforeKey = true
            };
            var openapiObject = edmModel.ConvertToOpenApi(openApiConvertorSettings);
            File.WriteAllText(filePath.Replace(".xml",".json"), openapiObject.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0));
            Console.WriteLine("JSON done.");
            File.WriteAllText(filePath.Replace(".xml",".yaml"), openapiObject.SerializeAsYaml(OpenApiSpecVersion.OpenApi3_0));
            Console.WriteLine("YAML done.");
            Console.ReadLine();
        }

        public static IEdmModel LoadEdmModel(string file)
        {
            try
            {
                string csdl = File.ReadAllText(file);
                return CsdlReader.Parse(XElement.Parse(csdl).CreateReader());
            }
            catch
            {
                Console.WriteLine("Cannot load EDM from file: " + file);
                return null;
            }
        }
    }
}
