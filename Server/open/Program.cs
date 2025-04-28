using System.CommandLine;
using System.IO;
using System.Xml.Linq;
string[] allLanguage = { "pdf"};
List<string> allFiles = new List<string>();
bool sourceCode = false;
//------------------------------bundle------------------------------
//פקודת bundle
var bundleComand = new Command("bundle", "bundle code files for a single file");

//language
var languageOption = new Option<string>("--language", "List of languages");
languageOption.IsRequired = true;
languageOption.AddAlias("-l");
//output
var bundleOption = new Option<FileInfo>("--output", "File path and name");
bundleOption.IsRequired = false;
bundleOption.AddAlias("-o");
//note
var noteOption = new Option<string>("--note", "do you want to write the source code");
noteOption.IsRequired = false;
noteOption.AddAlias("-n");
//sort
var sortOption = new Option<string>("--sort", "how you want to sort the files");
sortOption.IsRequired = false;
sortOption.AddAlias("-s");
//remove-empty-lines
var removeEmptyLinesOption = new Option<string>("--removeEmptyLines", "the name of author");
removeEmptyLinesOption.IsRequired = false;
removeEmptyLinesOption.AddAlias("-r");
//author
var authorOption = new Option<string>("--author", "the name of author");
authorOption.IsRequired = false;
authorOption.AddAlias("-a");


bundleComand.AddOption(bundleOption);
bundleComand.AddOption(languageOption);
bundleComand.AddOption(sortOption);
bundleComand.AddOption(noteOption);
bundleComand.AddOption(authorOption);
bundleComand.AddOption(removeEmptyLinesOption);

bundleComand.SetHandler((output, language, sort, note, author, removeEmptyLines) =>
{
    //note 
    try
    {
        if (note.Equals("y"))
        {
            sourceCode = true;
        }
    }
    catch (System.NullReferenceException se)
    {
        Console.WriteLine("the note is null!!!");
    }

    //output
    try
    {

        if (output != null)
        {
            var f = File.Create(output.FullName);
            f.Close();
        }
        else
            Console.WriteLine("input path!!!!!");
    }
    catch (DirectoryNotFoundException e)
    {
        Console.WriteLine("the path is invalid!!!");
    }
    string[] lan = language.Split(",");
    int count = 0;
    bool flag = false;
    if (language.Equals("all") == false)
    {
        for (int i = 0; i < lan.Length; i++)
        {
            flag = false;
            foreach (var item in allLanguage)
            {
                if (item.Equals(lan[i]))
                    flag = true;
            }
            if (flag == true)
            {
                count = Directory.GetFiles(Directory.GetCurrentDirectory(), "*." + lan[i], SearchOption.AllDirectories).Length;
                for (int j = 0; j < count; j++)
                {
                    allFiles.Add(Directory.GetFiles(Directory.GetCurrentDirectory(), "*." + lan[i], SearchOption.AllDirectories)[j]);
                }

            }
            else
            {
                Console.WriteLine("the language " + lan[i] + " is not exists");
            }
        }
    }
    else
    {

        for (int i = 0; i < allLanguage.Length; i++)
        {
            count = Directory.GetFiles(Directory.GetCurrentDirectory(), "*." + allLanguage[i], SearchOption.AllDirectories).Length;
            for (int j = 0; j < count; j++)
            {
                allFiles.Add(Directory.GetFiles(Directory.GetCurrentDirectory(), "*." + allLanguage[i], SearchOption.AllDirectories)[j]);
            }
        }
    }


    //sort:
    try
    {
        if (sort.Equals("y"))
        {
            allFiles.Sort((a, b) =>
            {
                a = a.Substring(a.LastIndexOf(".") + 1);
                b = b.Substring(b.LastIndexOf(".") + 1);
                return a.CompareTo(b);
            });

        }
        else
        {
            allFiles.Sort((a, b) =>
            {
                a = a.Substring(a.LastIndexOf("\\"));
                b = b.Substring(b.LastIndexOf("\\"));
                return a.CompareTo(b);
            });

        }
    }

    catch (Exception e)
    {
        allFiles.Sort((a, b) =>
        {
            a = a.Substring(a.LastIndexOf("\\"));
            b = b.Substring(b.LastIndexOf("\\"));
            return a.CompareTo(b);
        });
    }
    //remove-empty-lines
    try
    {

        string text = "", s = "";
        if (author != "")
            s = "auothor:" + author + "\n";
        if (removeEmptyLines.Equals("y"))
        {
            for (int i = 0; i < allFiles.Count; i++)
            {
                if (!(allFiles[i].Contains("bin") || allFiles[i].Contains("Debug")))
                {
                    if (sourceCode == true)
                        text += "the path is: " + allFiles[i] + "\n";
                    for (int j = 0; j < File.ReadAllLines(allFiles[i]).Where(s => s.Trim() != string.Empty).ToArray().Length; j++)
                    {
                        text += File.ReadAllLines(allFiles[i]).Where(s => s.Trim() != string.Empty).ToArray()[j];
                        text += "\n";
                    }
                    s += text;
                }


            }
            File.WriteAllText(output.FullName, s);
        }
        else
        {
            for (int i = 0; i < allFiles.Count; i++)
            {
                if (!(allFiles[i].Contains("bin") || allFiles[i].Contains("Debug"))) {
                    if (sourceCode == true)
                        s += "the path is: " + allFiles[i] + "\n";
                    s += File.ReadAllText(allFiles[i]);
                    s += "\n";
                }
                    
            }
            File.WriteAllText(output.FullName, s);
         
        }
    }
    catch (System.NullReferenceException sn)
    {
        
    }
}, bundleOption, languageOption, sortOption, noteOption, authorOption, removeEmptyLinesOption);
//------------------------------create-rsp------------------------------
var rspCommand = new Command("rsp", "");
rspCommand.SetHandler(() =>
{

    string word, s = "bundle ";
    Console.WriteLine("Enter a folder name");
    word = Console.ReadLine();
    try
    {
        File.Create(word);
        s += " --output " + word;
        word = "";
        while (word.Equals(""))
        {
            Console.WriteLine("Enter  language");
            word = Console.ReadLine();
        }
        s += " --language " + word;
        Console.WriteLine("Do you want to sort by type?? (y/n)");
        word = Console.ReadLine();
        if (word.Equals("y"))
            s += " --sort y";
        else
            s += " --sort n";
        Console.WriteLine("do you want to write the note (y/n)");
        word = Console.ReadLine();
        if (word.Equals("y"))
            s += " --note y";
        else
            s += " --note n";
        Console.WriteLine("Enter author name:");
        word = Console.ReadLine();
        if (!word.Equals(""))
            s += " --author " + word;
        Console.WriteLine("do you want to remove empty lines (y/n)");
        word = Console.ReadLine();
        if (word.Equals("y"))
            s += " --removeEmptyLines y";
        else
            s += " --removeEmptyLines n";
        File.WriteAllText("resp.rsp", s);

        Console.WriteLine(s);
    }
    catch
    {
        Console.WriteLine("the path is invalid!!!");
    }

});




var rootCommand = new RootCommand("root command for file bunder CLI");
rootCommand.AddCommand(rspCommand);
rootCommand.AddCommand(bundleComand);
rootCommand.InvokeAsync(args);