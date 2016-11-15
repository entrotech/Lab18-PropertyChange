using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.DataAccess.Ado;
using Talent.Domain;

namespace Talent.UI.ConsoleApp
{
    class Program
    {
        static GenreRepository genreRepository;

        static void Main(string[] args)
        {
            genreRepository = new GenreRepository();

            Console.WriteLine("Talent  Console Application - Edit Genres:\r\n");

            while (true)
            {
                Console.WriteLine("\r\nEnter # to edit a"
                + " Genre, n to create a new one, q to quit:");
                foreach (var g in genreRepository.Fetch()
                    .OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name))
                {
                    var str = String.Format("{0}:\t{1} \t({2}) \t{3} \t{4}",
                        g.Id, g.Name.PadRight(30), g.Code,
                        (g.IsInactive ? "Inactive" : ""), g.DisplayOrder);
                    Console.WriteLine(str);
                }
                Console.WriteLine();

                var cmd = Console.ReadLine();
                int id;
                if (cmd.ToUpper() == "N")
                {
                    CreateGenre();
                }
                else if (Int32.TryParse(cmd, out id))
                {
                    EditGenre(id);
                }
                else break;
            }
        }

        private static void CreateGenre()
        {
            var genre = new Genre();
            Console.WriteLine("Enter Code:");
            genre.Code = Console.ReadLine();
            Console.WriteLine("Enter Name:");
            genre.Name = Console.ReadLine();
            Console.WriteLine("Enter Inactive (T or F):");
            genre.IsInactive = Console.ReadLine()
                .ToUpper() == "T" ? true : false;
            Console.WriteLine("Enter DisplayOrder:");
            genre.DisplayOrder = Convert.ToInt32(Console.ReadLine());
            genreRepository.Persist(genre);
            Console.WriteLine("New Genre Saved!");
        }

        private static void EditGenre(int id)
        {
            var genre = genreRepository.Fetch(id).First();
            while (true)
            {
                Console.WriteLine("Pick the number of a property to"
                + " edit, s to save, c to cancel, d to delete:");
                Console.WriteLine("1: Code");
                Console.WriteLine("2: Name");
                Console.WriteLine("3: Inactive");
                Console.WriteLine("4: DisplayOrder\r\n");

                string cmd = Console.ReadLine();
                switch (cmd.ToUpper())
                {
                    case "S":
                        genre.IsDirty = true;
                        genreRepository.Persist(genre);
                        Console.WriteLine("Genre Changes Saved!");
                        return;
                    case "D":
                        genre.IsMarkedForDeletion = true;
                        genreRepository.Persist(genre);
                        Console.WriteLine("Genre Deleted!");
                        return;
                    case "1":
                        Console.WriteLine("Enter new Code:");
                        genre.Code = Console.ReadLine();
                        break;
                    case "2":
                        Console.WriteLine("Enter new Name:");
                        genre.Name = Console.ReadLine();
                        break;
                    case "3":
                        Console.WriteLine("Enter new Inactive (T/F:");
                        genre.IsInactive = Console.ReadLine()
                            .ToUpper() == "T" ? true : false;
                        break;
                    case "4":
                        Console.WriteLine("Enter new Code:");
                        genre.DisplayOrder = Convert
                            .ToInt32(Console.ReadLine());
                        break;
                    default:
                        return;

                }
            }

        }
    }
}

