using System;
using System.Collections.Generic;
using System.Linq;
using front.Core;
using front.Core.impl;
using NUnit.Framework;
using Moq;
namespace front.Tests
{
    [TestFixture]
    public class CombiningModulePackagerFacts
    {
        private ModuleInfo BuildModuleInfo(string name, string dependencies)
        {
            return new ModuleInfo()
            {
                Content = name,
                Name = name,
                Dependencies = new List<string>(string.IsNullOrWhiteSpace(dependencies) ? new string[]{} : dependencies.Split(','))
            };
        }

        
        IEnumerable<ModuleInfo> GetModuleArrayFromDependencyGraph(IEnumerable<string> dependencyGraph, out ModuleInfo mainModule)
        {
            var modules = new List<ModuleInfo>();
            mainModule = null;
            var first = true;
            foreach (var module in dependencyGraph)
            {
                var parts = module.Split(new []{"=>"}, StringSplitOptions.None);
                var moduleInfo = BuildModuleInfo(parts[0], parts[1]);
                if(first && !(first=false))
                    mainModule = moduleInfo;
                modules.Add(BuildModuleInfo(parts[0], parts[1]));
            }
            
            return modules;
        }

        IScriptModuleRepository GetMockedScriptModuleRepositoryForDependencyGraph(IEnumerable<string> dependencyGraph, out ModuleInfo mainModule)
        {
            var moduleRepo = new Moq.Mock<IScriptModuleRepository>();
            foreach (var moduleInfo in GetModuleArrayFromDependencyGraph(dependencyGraph, out mainModule))
            {
                moduleRepo.Setup((repo) => repo.GetModule(moduleInfo.Name)).Returns(moduleInfo);
            }
            return moduleRepo.Object;
        }

        [Test]
        public void WhenPackagingAModule_TheResultingPackageContainsTheModuleAndAllDependenciesIncludingTransitiveOnes()
        {
            ModuleInfo entryModule;
            var moduleRepo = GetMockedScriptModuleRepositoryForDependencyGraph(new []
            {
                "a=>b,c",
                "b=>c,d",
                "c=>d",
                "d=>"
            }, out entryModule);
            
            var formatter = new Mock<ICommonJsFormatter>();
            formatter.Setup(p => p.GetCommonJsModule(It.IsAny<ModuleInfo>())).Returns((ModuleInfo i) => i.Content);

            var sutPackager = new CombiningModulePackager(moduleRepo,formatter.Object);
            var package = sutPackager.GetPackage(entryModule.Name);
            Assert.True(package.IndexOf("a")>=0);
            Assert.True(package.IndexOf("b")>=0);
            Assert.True(package.IndexOf("c")>=0);
            Assert.True(package.IndexOf("d") >= 0);
        }
    }
}
