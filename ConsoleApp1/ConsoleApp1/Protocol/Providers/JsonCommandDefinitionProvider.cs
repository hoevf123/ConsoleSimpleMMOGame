using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace ConsoleApp1
{
    internal class JsonCommandDefinitionProvider : ICommandDefinitionProvider
    {
        public Dictionary<string, CommandDefinition> LoadProtocolDefinitions()
        {
            return LoadDefinitions<CommandDefinition>("CommandFactoryDecodeDefinition.json");
        }

        public Dictionary<string, GameActionDefinition> LoadGameActionDefinitions()
        {
            return LoadDefinitions<GameActionDefinition>("GameActionDefinition.json");
        }

        private Dictionary<string, TDefinition> LoadDefinitions<TDefinition>(string fileName)
        {
            string[] candidatePaths =
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commands", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Commands", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Commands", fileName),
                Path.Combine(Environment.CurrentDirectory, "ConsoleApp1", "ConsoleApp1", "Commands", fileName),
                Path.Combine(Environment.CurrentDirectory, "Commands", fileName)
            };

            string definitionPath = candidatePaths.FirstOrDefault(File.Exists);
            if (definitionPath == null)
            {
                throw new FileNotFoundException(fileName + " 파일을 찾을 수 없습니다.");
            }

            string json = File.ReadAllText(definitionPath);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, TDefinition> definitions = serializer.Deserialize<Dictionary<string, TDefinition>>(json);

            return definitions ?? new Dictionary<string, TDefinition>();
        }
    }
}
