using BenchmarkDotNet.Attributes;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using StringMatch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringMatch
{
    [MemoryDiagnoser, RankColumn]
    public class TestRegex
    {


        //string pattern = "[#@!]";

        private static List<string> SpecialNotationLst = new List<string>() {
            "!",
            "\"",
            "#",
            "$",
            "%",
            "&",
            "'",
            ":",
            ";",
            "<",
            "=",
            ">",
            "?",
            "@",
            "~"
        };
        public TestRegex() { InitData(); }

        public TestRegex(string pattern) { InitData(); }

        private void InitData()
        {
            //SpecialNotationLst = JsonConvert.DeserializeObject<List<NotationModel>>
            //    (File.ReadAllText(@"F:\EH\M2SH_Modeler_Platform3.0\PUBLISH_DATA\Data\NamingConventions\SpecialNotation.json"));
        }

        [Benchmark]
        public bool Match()
        {
            string pattern = "[!\\\"#\\$%&\\':;<=>\\?@~]";
            string str = "Hello, world! # @";
            bool containsSpecialChars = Regex.IsMatch(str, pattern);

            if (containsSpecialChars)
            {
                Console.WriteLine("Y");
            }
            else
            {
                Console.WriteLine("N");
            }
            return containsSpecialChars;
        }

        [Benchmark]
        public bool MatchByStringParallel()
        {
            string str = "Hello, world! # @";
            //SpecialNotationLst = JsonConvert.DeserializeObject<List<NotationModel>>
            //    (File.ReadAllText(@"F:\EH\M2SH_Modeler_Platform3.0\PUBLISH_DATA\Data\NamingConventions\SpecialNotation.json"));
            foreach (var s in SpecialNotationLst.AsParallel())
            {
                if (str.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }
        [Benchmark]
        public bool MatchByString()
        {
            string str = "Hello, world! # @";
            //SpecialNotationLst = JsonConvert.DeserializeObject<List<NotationModel>>
            //    (File.ReadAllText(@"F:\EH\M2SH_Modeler_Platform3.0\PUBLISH_DATA\Data\NamingConventions\SpecialNotation.json"));
            foreach (var s in SpecialNotationLst.AsParallel())
            {
                if (str.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
