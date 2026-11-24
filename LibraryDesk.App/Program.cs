using System.Globalization;
using System.Text;
using LibraryDesk.Core;
using LibraryDesk.Core.Abstractions;
using LibraryDesk.Core.Domain;
using LibraryDesk.Core.Policies;
using LibraryDesk.Core.Repositories;
using LibraryDesk.Core.Services;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("uk-UA");

// Створення сховища та політики.
ILoanRepository repository = new InMemoryLoanRepository();
IPricingPolicy pricing = new StandardPricingPolicy();

// Створення сервісу з передаванням залежностей.
LoanService service = new(repository, pricing);

// Створення читача та книги.
Reader reader = new(1, "Іваненко", "ivan@example.com");
Book book = new("A-100", "Кобзар", "Тарас Шевченко");

// Створення формуляра.
Loan loan = new(1, reader);

DateOnly createdDate = DateOnly.FromDateTime(DateTime.Today);
DateOnly dueDate = createdDate.AddDays(7);

LoanItem item = new(book, createdDate, dueDate);

loan.AddItem(item);

// Збереження формуляра.
service.Place(loan);

// Обчислення вартості.
decimal total = service.TotalOf(loan);

Console.WriteLine($"Сума: {total:0.00} грн");
