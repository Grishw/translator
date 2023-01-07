// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

const string MODE_1 = "mealy-to-moore";
const string MODE_2 = "moore-to-mealy";
const string CSV_MASK = "[a-z]*.csv";
const string NEW_POINT_NAME_PART = "q";

//check for csv file
Regex r = new Regex(CSV_MASK, RegexOptions.IgnoreCase);
bool isCSV(string name = "")
{
    return r.Match(name).Success;
};

// mealy - to - moore
// create assotiation on old point and new point name
// only unique point/combination(q/y)
void fill_new_point_set(ref Dictionary<string, string> point, ref string[] point_set)
{ 
    for (int i = 1; i < point_set.Length; i++)
    {
        if (!point.ContainsKey(point_set[i]))
        {
            point.Add(point_set[i], NEW_POINT_NAME_PART + point.Count.ToString());
        }
    };
};

// mealy - to - moore
//print assotiation on old point and new point name
void print_point_set(ref Dictionary<string, string> point)
{
    Console.WriteLine("Point assotiation: ");
    foreach (KeyValuePair<string, string> entry in point)
    {
        Console.WriteLine("{0} -> {1}", entry.Key, entry.Value);
    }
}

// mealy - to - moore
//create 2 first csv line to output (q/y)
string create_header_for_csv(ref Dictionary<string, string> point)
{
    string line_y = ";";
    string line_q = ";";
    foreach (KeyValuePair<string, string> entry in point)
    {
        line_y += entry.Key.Split('/')[1] + ";";
        line_q += entry.Value + ";";
    }
    return line_y + '\n' + line_q;
}


//standart for both way
if (args.Length != 3)
{
    Environment.Exit(0);
};

if (!isCSV(args[1]) || !isCSV(args[2]))
{
    Environment.Exit(0);
};

if (!File.Exists(args[1]))
{
    Environment.Exit(0);
};

//input.csv mealy-to-moore
//some used variables
string line = "";
var point = new Dictionary<string, string>(); // assotiation on old point and new point name
List<string> signal_list = new List<string>(); // all signals

using (StreamReader sr = new StreamReader(args[1]))
{
    if(!sr.EndOfStream)//get all old point
    {
        line = sr.ReadLine();
    };

    while (sr.Peek() >= 0)
    {
        line = sr.ReadLine();
        string[] line_list = line.Split(';');
        signal_list.Add(line_list[0]);
        fill_new_point_set(ref point, ref line_list);
    }
}


print_point_set(ref point);
Console.WriteLine(create_header_for_csv(ref point));



Console.WriteLine("END!!!");
