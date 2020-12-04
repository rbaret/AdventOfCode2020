using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day4
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day4\\input.txt";

            IEnumerable<Dictionary<string, string>> passportList = GeneratePassportList(path);
            Tuple<int, int> validPassports = GetValidPassportsNb(passportList);
            Console.WriteLine("Valid passports (according to Part 1 criteria) : " + validPassports.Item1);
            Console.WriteLine("Valid passports (according to Part 2 criteria) : " + validPassports.Item2);
            Console.ReadKey();
        }

        private static IEnumerable<Dictionary<string, string>> GeneratePassportList(string path)
        {
            List<Dictionary<string, string>> passportList = new List<Dictionary<string, string>>();
            string rawText = File.ReadAllText(@path);
            string[] credentials = rawText.Split("\n\n"); //remove all blank lines
            foreach (string credential in credentials)
            {
                string[] fields = credential.Replace('\n', ' ').Split(' '); // Make all credentials one-liners
                Dictionary<string, string> entry = new Dictionary<string, string>(); // Creating a dictionary of keypairs with fields name/values
                foreach (string field in fields) // Fill the dictionary with keypairs
                {
                    int position = field.IndexOf(":");
                    if (position < 0)
                        continue;
                    entry.Add(field.Substring(0, position), field.Substring(position + 1));
                }
                passportList.Add(entry);
            }
            return passportList;
        }
        private static Tuple<int,int> GetValidPassportsNb(IEnumerable<Dictionary<string, string>> passportList)
        {
            int validPassports = 0;
            int passportWithValidData = 0;
            int passportIndex = 1;
            foreach(Dictionary<string, string> passport in passportList)
            {
                string[] validFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" }; 
                bool isCidPresent = false;
                int validFieldsNr = 0;                
                foreach (string validField in validFields) {
                    if (passport.ContainsKey(validField))
                    {
                        validFieldsNr++;
                        if (validField == "cid") isCidPresent = true;
                    }
                }
                if ((validFieldsNr == 8) || (validFieldsNr == 7 && (isCidPresent == false))) { 
                    validPassports++;
                    if (HasValidData(passport)) passportWithValidData++;
                }
                passportIndex++;

            }
            
            return new Tuple<int,int>(validPassports,passportWithValidData);
        }

        private static bool HasValidData(Dictionary<string,string> entry)
        {
            bool hasValidData = true;
            // Let's validate
            foreach(KeyValuePair<string,string> field in entry)
            {
                switch (field.Key)
                {
                    case "byr":
                        if (!IsValidBirthYear(int.Parse(field.Value))) hasValidData = false;
                        break;
                    case "iyr":
                        if (!IsValidIssuanceYear(int.Parse(field.Value))) hasValidData = false;
                        break;
                    case "eyr":
                        if (!IsValidExpirationYear(int.Parse(field.Value))) hasValidData = false;
                        break;
                    case "hgt":
                        if (!IsValidHeight(field.Value)) hasValidData = false;
                        break;
                    case "hcl":
                        if (!IsValidHairColor(field.Value)) hasValidData = false;
                        break;
                    case "ecl":
                        if (!IsValidEyeColor(field.Value)) hasValidData = false;
                        break;
                    case "pid":
                        if (!IsValidPid(field.Value)) hasValidData = false;
                        break;
                    default:
                        break;
                }
            }
            return hasValidData;
        }

        private static bool IsValidHairColor(string hcl)
        {
            string validHcl = "^#[0-9a-f]{6}$";
            return Regex.IsMatch(hcl, validHcl);
        }

        private static bool IsValidPid(string pid)
        {
            string validPid = "^[0-9]{9}$";
            return Regex.IsMatch(pid, validPid);
        }

        private static bool IsValidEyeColor(string ecl)
        {
            string[] validEcl = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            return Array.Exists(validEcl, element => element==ecl);

        }

        private static bool IsValidHeight(string hgt)
        {
            Tuple<int, int> validHgtCm = new Tuple<int, int>(150, 193);
            Tuple<int, int> validHgtIn = new Tuple<int, int>(59, 76);
            if (Regex.IsMatch(hgt, "^[0-9]{3}cm")){
                int height = int.Parse(hgt.Substring(0, 3));
                if ((validHgtCm.Item1 <= height) && (validHgtCm.Item2 >= height)) return true;
            }
            else if (Regex.IsMatch(hgt, "^[0-9]{2}in"))
                {
                    int height = int.Parse(hgt.Substring(0, 2));
                    if ((validHgtIn.Item1 <= height) && (validHgtIn.Item2 >= height)) return true;
                }
            return false;
        }

        private static bool IsValidExpirationYear(int eyr)
        {
            Tuple<int, int> validEyr = new Tuple<int, int>(2020, 2030);
            if ((validEyr.Item1 <= eyr) && (validEyr.Item2 >= eyr)) return true;
            return false;
        }
        
        private static bool IsValidIssuanceYear(int iyr)
        {
            Tuple<int, int> validIyr = new Tuple<int, int>(2010, 2020);
            if ((validIyr.Item1 <= iyr) && (validIyr.Item2 >= iyr)) return true;
            return false;
        }

        private static bool IsValidBirthYear(int byr)
        {
            Tuple<int, int> validByr = new Tuple<int, int>(1920, 2002);
            if ((validByr.Item1 <= byr) && (validByr.Item2 >= byr)) return true;
            return false;
        }
    }
}
