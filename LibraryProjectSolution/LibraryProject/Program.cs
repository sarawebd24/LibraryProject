
namespace LibraryProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the library!");
            Console.WriteLine();

            bool loggedInAdmin = false;
            bool loggedInUser = false;

            while (true)
            {

                if (!loggedInUser && !loggedInAdmin)
                {
                    //HUVUDMENY
                    Console.WriteLine("\nChoose a number in the menu");
                    Console.WriteLine("1. Register as new user and become a member");
                    Console.WriteLine("2. Log in for members");
                    Console.WriteLine("3. Register - ADMIN ONLY");
                    Console.WriteLine("4. Log in - ADMIN ONLY");
                    Console.WriteLine("5. Exit program");
                    Console.WriteLine("------------------------");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": //längre fram kan det vara bra att ha kontroll så det inte är tomt/null kanske

                            Console.WriteLine("Register new user");
                            Console.WriteLine("Choose a username: ");
                            string newUsername = Console.ReadLine().ToUpper();

                            Console.WriteLine("Choose a password: ");
                            string newPassword = Console.ReadLine();

                            AddUser(newUsername, newPassword);
                            Console.WriteLine($"Hi {newUsername}, you're now a member at our library! ");
                            break;

                        case "2":
                     
                            Console.WriteLine("Log in as a member");
                            UserLogin();
                            loggedInUser = true;
                             while (loggedInUser == true)
                            {
                                //User meny
                                Console.WriteLine("\nChoose a number in the menu");
                                Console.WriteLine("1. Borrow a new book");
                                Console.WriteLine("2. Return a borrowed book");
                                Console.WriteLine("3. Search and see all books");
                                Console.WriteLine("4. Exit the library and log out.");
                                string userChoice = Console.ReadLine();

                                switch (userChoice)
                                {
                                    case "1":
                                        BorrowBook();
                                        break;
                                    case "2":
                                        ReturnBook();
                                        break;
                                    case "3":
                                        SeeAndSearchBooks();
                                        break;
                                    case "4":
                                        Console.WriteLine("You're exiting the library and logging out. Bye!");
                                        loggedInUser = false;
                                        
                                        return;
                                    default:
                                        Console.WriteLine("Invalid input. Try again.");
                                        break;

                                }
                             }
                            break;

                        case "3": 
                            //admin registrering + kontroll så inte vem som helst kan bli admin
                            AddAdminUser();
                            
                            break;


                        case "4":
                            bool result = AdminLogin();

                            if (result = true)
                            {
                                loggedInAdmin = true;
                            }
                            
                            break;

                        case "5":
                            Console.WriteLine("You're leaving the library. Goodbye and have a pleasant day!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }
                }
                else if (loggedInAdmin)
                {
                   
                    while (true)
                    {
                        //Adminmeny
                        Console.WriteLine("\nChoose a number in the menu");
                        Console.WriteLine("1. Add a new book to the library");
                        Console.WriteLine("2. Update a existing book");
                        Console.WriteLine("3. Remove a book");
                        Console.WriteLine("4. Exit program");
                        Console.WriteLine("------------------------");

                        string adminChoice = Console.ReadLine();

                        switch (adminChoice)
                        {
                            case "1":
                                AddBook();
                                break;
                            case "2":
                                UpdateBookInfo();
                                break;
                            case "3":
                                RemoveBook();
                                break;
                            case "4":
                                return;
                            default:
                                Console.WriteLine("Something went wrong. Please try again.");
                                break;
                        }
                    }
                }
            }
        }



        public static void AddUser(string username, string password) //idé inför framtiden är att ha hashed password så det inte syns
        {
            using (var context = new UserContext())
            {
                var user = new User()
                {
                    Username = username,
                    Password = password
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static void UserLogin()
        {
            Console.WriteLine("Enter your username: ");
            string newUsername = Console.ReadLine().ToUpper();

            Console.WriteLine("Enter your password: ");
            string newPassword = Console.ReadLine();

            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Username == newUsername);

                if (user != null && user.Password == newPassword)
                {
                    Console.WriteLine($"Hi {newUsername}, you're now logged in!");
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again or create a new account.");
                }
            }
        }

        public static bool AdminLogin()
        {
            bool result = false;
            Console.WriteLine("Enter your username: ");
            string newAdminUsername = Console.ReadLine().ToUpper();
            Console.WriteLine("Enter your password: ");
            string newAdminPassword = Console.ReadLine();
            using (var context = new UserContext())
            {
                var admin = context.Admins.FirstOrDefault(a => a.UsernameAdmin == newAdminUsername);
                if (admin != null && admin.PasswordAdmin == newAdminPassword)
                {
                    Console.WriteLine($"Hi {newAdminUsername}, you're now logged in as admin.");
                    Console.WriteLine();
                    result = true;
                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                }
            }
            return result;
        }

        public static void SeeAndSearchBooks()
        {
            string findBookChoice = "";

            while (findBookChoice != "3")
            {
                Console.WriteLine("Press 1 to see all available books ");
                Console.WriteLine("Press 2 to search for a specific book");
                Console.WriteLine("Press 3 to exit");
                findBookChoice = Console.ReadLine().ToUpper();

                switch (findBookChoice)
                {
                    case "1":
                        using (var context = new UserContext())
                        {
                            var availableBooks = context.Books.Where(b => b.IsAvailable).ToList();

                            if (availableBooks.Count == 0)
                            {
                                Console.WriteLine("There are no available books for now. Please come back again!");
                                return;
                            }

                            foreach (var book in availableBooks)
                            {
                                Console.WriteLine($"{book.Title} - {book.Author} - ISBN number {book.ISBN}");
                                
                            }
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter the title, author or ISBN of the book: ");
                        string searchBook = Console.ReadLine().ToUpper();

                        if (string.IsNullOrEmpty(searchBook))
                        {
                            Console.WriteLine("Invalid input, try again.");
                            break;
                        }

                        using (var context = new UserContext())
                        {
                            var searchResult = context.Books.FirstOrDefault (t => t.Title == searchBook || t.Author == searchBook || t.ISBN == searchBook);
                            
                            if (searchResult != null)
                            {
                                Console.WriteLine("That book exists in our library.");
                            }
                            else
                            {
                                Console.WriteLine("Sorry, we don't have that book!");
                                return;
                            }

                        }
                        break;
                    case "3":
                        break;

                    default:
                        Console.WriteLine("Invalid input, try again.");
                        break;
                }

            }
        }

        static void BorrowBook()
        {
            Console.WriteLine("Here's all the available books in our library: ");
            using (var context = new UserContext())
            {
                var availableBooks = context.Books.Where(b => b.IsAvailable).ToList();

                if (availableBooks.Count == 0)
                {
                    Console.WriteLine("There are no available books for now. Please come back again!");
                    return;
                }

                foreach (var book in availableBooks)
                {
                    Console.WriteLine($"{book.Title} - {book.Author} - ISBN number {book.ISBN}");
                }

                Console.WriteLine("Enter the title of the book you want to borrow: ");
                string bookTitle = Console.ReadLine().ToUpper();
                var bookToBorrow = context.Books.FirstOrDefault(b => b.Title == bookTitle);

                if (bookToBorrow != null && bookToBorrow.IsAvailable)
                {
                    bookToBorrow.IsAvailable = false;
                    context.SaveChanges();
                    Console.WriteLine($"You've now borrowed {bookToBorrow.Title} and have to return it within 30 days.");
                }
                else
                {
                    Console.WriteLine("Unfortunately the books is not available to borrow. ");
                }
            }
        }

        static void ReturnBook()
        {
            Console.WriteLine("Enter the title of the book you want to return: ");
            string returnBookTitle = Console.ReadLine().ToUpper();

            using (var context = new UserContext())
            {
                var bookToReturn = context.Books.FirstOrDefault(b => b.Title.ToUpper() == returnBookTitle);

                if (bookToReturn != null && !bookToReturn.IsAvailable)
                {
                    bookToReturn.IsAvailable = true;
                    context.SaveChanges();
                    Console.WriteLine($"The book {returnBookTitle} is now returned. I hope you liked it!");
                }
            }
        }

        public static void AddAdminUser() 
        {

            Console.WriteLine("Register as admin");
            Console.WriteLine("To become an admin in the library you'll need to enter the access code: ");
            string adminAccess = Console.ReadLine();
            bool isAdmin = false;
            //koden är: admin123
            string newAdminUser;
            string newAdminPassword;

            if (adminAccess == "admin123")
            {
                Console.WriteLine("Choose a admin username: ");
                newAdminUser = Console.ReadLine();

                Console.WriteLine("Choose a password: ");
                newAdminPassword = Console.ReadLine();
                isAdmin = true;

                Console.WriteLine($"Welcome as admin, {newAdminUser}. Choose number 4 to add, update or delete books in our library.");
            }
            else
            {
                Console.WriteLine("Invalid access code, try again.");
                return;
            }

            using (var context = new UserContext())
            {
                var user = new UserAdmin()
                {
                    UsernameAdmin = newAdminUser,
                    PasswordAdmin = newAdminPassword
                };
                context.Admins.Add(user);
                context.SaveChanges();
            
            }
        }

        static void AddBook()
        {
            Console.WriteLine("Enter the title of the book: ");
            string bookTitle;
            while (true)
            {
                bookTitle = Console.ReadLine().ToUpper();
                if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrEmpty(bookTitle))
                {
                    Console.WriteLine("Invalid title input. Try again: ");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Enter the author of the book: ");
            string bookAuthor;
            while (true)
            {
                bookAuthor = Console.ReadLine().ToUpper();
                if (string.IsNullOrWhiteSpace(bookAuthor) || string.IsNullOrEmpty(bookAuthor))
                {
                    Console.WriteLine("Invalid author input. Try again: ");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Enter the ISBN number, for example 1234: ");
            string bookIsbn;
            while (true)
            {
                bookIsbn = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(bookIsbn))
                {
                    Console.WriteLine("Invalid ISBN input. Try again: ");
                }
                else
                {
                    break;
                }
            }


            using (var context = new UserContext())
            {
                var book = new Book()
                {
                    Title = bookTitle,
                    Author = bookAuthor,
                    ISBN = bookIsbn,
                    IsAvailable = true
                };

                context.Books.Add(book);
                context.SaveChanges();
            }
            Console.WriteLine($"{bookTitle} written by {bookAuthor} with {bookIsbn} ISBN number has been added to our library.");
        }

        public static void UpdateBookInfo()
        {
            Console.WriteLine("Enter the title of the book you want to update: ");
            string updateTitle = Console.ReadLine().ToUpper();

            using (var context = new UserContext())
            {
                var book = context.Books.FirstOrDefault(book => book.Title == updateTitle);
                if (book != null)
                {
                    Console.WriteLine("Book found. Write a new title: ");
                    string newTitle = Console.ReadLine().ToUpper();

                    book.Title = newTitle;
                    context.SaveChanges();
                    Console.WriteLine("The title has now been updated.");
                }
                else
                {
                    Console.WriteLine("The book cannot be found. Please try again.");
                }
            }
        }

        public static void RemoveBook()
        {
            Console.WriteLine("Enter the title of the book you want removed: ");
            string removedBook = Console.ReadLine().ToUpper();

            using (var context = new UserContext())
            {
                var bookRemoved = context.Books.FirstOrDefault(u => u.Title == removedBook);
                if (bookRemoved != null)
                {
                    bookRemoved.Title = removedBook;
                    context.Books.Remove(bookRemoved);
                    context.SaveChanges();
                    Console.WriteLine("The book is now removed from the library");
                }
                else
                {
                    Console.WriteLine("The book could not be found. Please try again.");
                }

            }
        }

        //public static void PauseProgram()
        //{
        //    Console.Write("\nClick on any button to continue: ");
        //    Console.ReadKey();
        //}
        // en eventuell lösning ifall programmet stängs ner för snabbt
    }

}

