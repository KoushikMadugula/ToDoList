using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ToDoListClassLibrary;

namespace ToDoListUnitTests
{
    [TestClass]
    public class ToDoItemTests
    {
        private static ToDoContext _context;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseSqlite("Data Source=/workspaces/ToDoList/TodoList.API/todolist.db;")
                .Options;

            _context = new ToDoContext(options);
            _context.Database.EnsureCreated();
        }


        [TestMethod]
    public void Constructor_SetsDueDateOneWeekAhead_IfNull_And_SaveToDatabase()
    {
    
    var item = new ToDoItem();

    
    _context.ToDoItems.Add(item); 
    _context.SaveChanges(); 

    
    var expectedDate = DateTime.Now.AddDays(7).Date;
    Assert.AreEqual(expectedDate, item.DueDate?.Date, "DueDate is not set to one week ahead.");

    
    var itemFromDb = _context.ToDoItems.FirstOrDefault(i => i.Id == item.Id);
    Assert.IsNotNull(itemFromDb, "Item was not saved to the database.");
    Assert.AreEqual(expectedDate, itemFromDb.DueDate?.Date, "Saved item's DueDate does not match the expected value.");
}

        [TestMethod]
        public void Save_ToDoItemToDatabase()
        {
            var newItem = new ToDoItem { Description = "Checking two" };
            _context.Add(newItem);
            _context.SaveChanges();

            var itemFromDb = _context.ToDoItems.FirstOrDefault(item => item.Description == "Checking two");
            Assert.IsNotNull(itemFromDb);
            Assert.AreEqual("Checking two", itemFromDb.Description);
        }

        [TestMethod]
        public void Update_ToDoItemCompletedDate()
        {
            var newItem = new ToDoItem { Description = "Checking three" };
            _context.Add(newItem);
            _context.SaveChanges();

            var itemFromDb = _context.ToDoItems.FirstOrDefault(item => item.Description == "Checking three");
            Assert.IsNotNull(itemFromDb);

            itemFromDb.CompletedDate = DateTime.Now;
            _context.SaveChanges();

            var updatedItem = _context.ToDoItems.FirstOrDefault(item => item.Description == "Checking three");
            Assert.IsNotNull(updatedItem.CompletedDate);
        }

        [TestMethod]
    public void SetAndGet_ToDoItemDescription_Success()
    {
    
    var expectedDescription = "Checking four";
    var toDoItem = new ToDoItem { Description = expectedDescription };

    
    _context.Add(toDoItem);
    _context.SaveChanges();

    
    var savedItem = _context.ToDoItems.FirstOrDefault(item => item.Description == expectedDescription);
    Assert.IsNotNull(savedItem, "Failed to retrieve the ToDoItem from the database.");
    Assert.AreEqual(expectedDescription, savedItem.Description, "The Description of the retrieved ToDoItem does not match the expected value.");
    }

       
    }

    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }

        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }
    }
}