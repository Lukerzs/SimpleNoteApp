using System;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;

namespace SimpleNoteApp
{
    class SimpleNoteApp
    {
        private static string noteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Notes\";
        static void Main(string[] args)
        {
            ReadComand();
            Console.ReadLine();
        }


        private static void ReadComand()
        {
            Console.Write(Directory.GetDirectoryRoot(noteDirectory));
            string Command = Console.ReadLine();

            switch (Command.ToLower())
            {
                case "new":
                    NewNote();
                    Main(null);
                    break;
                case "edit":
                    EditNote();
                    Main(null);
                    break;
               /* case "read":
                    ReadNote();
                    Main(null);
                    break;*/
                case "delete":
                    DeleteNote();
                    Main(null);
                    break;
                case "dir":
                    NotesDirectory();
                    Main(null);
                    break;
                case "shownotes":
                    ShowNotes();
                    Main(null);
                    break;
                case "cls":
                    Console.Clear();
                    Main(null);
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    CommandsAvaliable();
                    Main(null);
                    break;

            }


        }

        private static void NotesDirectory()
        {
            Process.Start("explorer.exe", noteDirectory);
        }

        private static void ShowNotes()
        {
            string NoteLocation = noteDirectory;

            DirectoryInfo Dir = new DirectoryInfo(NoteLocation);

            if (Directory.Exists(NoteLocation))
            {

                FileInfo[] NoteFiles = Dir.GetFiles("*.xml");

                if (NoteFiles.Count() != 0)
                {

                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 2);
                    Console.WriteLine("+------------+");
                    foreach (var item in NoteFiles)
                    {
                        Console.WriteLine("  " + item.Name);
                    }

                    Console.WriteLine(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("No notes found.\n");
                }
            }
            else
            {

                Console.WriteLine(" Directory does not exist.....creating directory\n");

                Directory.CreateDirectory(NoteLocation);

                Console.WriteLine(" Directory: " + NoteLocation + " created successfully.\n");

            }
        }

        private static void Exit()
        {
            Environment.Exit(0);
        }

        private static void CommandsAvaliable()
        {
            Console.WriteLine(" New - Create a new note\n Edit - Edit a note\n Read -  Read a note\n ShowNotes - List all notes\n Exit - Exit the application\n Dir - Opens note directory\n Help - Shows this help message\n");
        }

        private static void DeleteNote()
        {
            Console.WriteLine("Please enter file name\n");

            string FileName = Console.ReadLine();


            if (File.Exists(noteDirectory + FileName))

            {

                Console.WriteLine(Environment.NewLine + "Are you sure you wish to delete this file? Y/N\n");

                string Confirmation = Console.ReadLine().ToLower();


                if (Confirmation == "y")

                {

                    try

                    {

                        File.Delete(noteDirectory + FileName);

                        Console.WriteLine("File has been deleted\n");

                    }

                    catch (Exception ex)

                    {

                        Console.WriteLine("File not deleted following error occured: " + ex.Message);

                    }

                }

                else if (Confirmation == "n")

                {

                    Main(null);

                }

                else

                {

                    Console.WriteLine("Invalid command\n");

                    DeleteNote();

                }

            }

            else

            {

                Console.WriteLine("File does not exist\n");

                DeleteNote();

            }
        }

        private static void EditNote()
        {
            Console.WriteLine("Please enter the Name of the File:\n");
            string fileName = Console.ReadLine().ToLower();

            if(File.Exists(noteDirectory + fileName))
            {
                XmlDocument doc = new XmlDocument();

                try
                {
                    doc.Load(noteDirectory + fileName);
                    Console.Write(doc.SelectSingleNode("//body").InnerText);
                    string readInput = Console.ReadLine();

                    if (readInput == "cancel")
                    {
                        Main(null); 
                    }
                    else
                    {
                        string newtext = doc.SelectSingleNode("//body").InnerText = readInput;
                        doc.Save(noteDirectory + fileName);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error.Could not edit note following the error ocurred: {ex.Message}");
                }
            }

        }

        private static void NewNote()
        {
            Console.WriteLine("Please Enter note: \n");
            string input = Console.ReadLine();

            XmlWriterSettings NoteSettings = new XmlWriterSettings();

            NoteSettings.CheckCharacters = false;
            NoteSettings.ConformanceLevel = ConformanceLevel.Auto;
            NoteSettings.Indent = true;


            string FileName = DateTime.Now.ToString("dd-MM-yy") + ".xml";

          using(XmlWriter newNote = XmlWriter.Create(noteDirectory + FileName, NoteSettings))
            {
                newNote.WriteStartDocument();
                newNote.WriteStartElement("note");
                newNote.WriteElementString("body",input);
                newNote.WriteEndElement();

                newNote.Flush();
                newNote.Close();
            }
        }
    }
}
