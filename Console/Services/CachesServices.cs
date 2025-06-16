using Console.Data;
using Console.Entities.TicketEntities;
using Console.Entities.TaskEntities;
using Console.Entities.CustomerEntities;
using Console.Entities.ModelEntities;
using Console.Entities.UserEntity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace Console.Services
{

    public class CachesServices
    {
        // Singleton instance
        private static readonly object _lock = new object();
        private static CachesServices _instance;
        public static CachesServices Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CachesServices();
                        }
                    }
                }
                return _instance;
            }
        }

        // Entity full properties
        #region Ticket Entities
        private List<Contact> _contacts;
        public List<Contact> Contacts
        {
            get => _contacts ?? (_contacts = new List<Contact>());
            set => _contacts = value;
        }

        private List<Country> _countries;
        public List<Country> Countries
        {
            get => _countries ?? (_countries = new List<Country>());
            set => _countries = value;
        }

        private List<Customer> _customers;
        public List<Customer> Customers
        {
            get => _customers ?? (_customers = new List<Customer>());
            set => _customers = value;
        }

        private List<Instrument> _instruments;
        public List<Instrument> Instruments
        {
            get => _instruments ?? (_instruments = new List<Instrument>());
            set => _instruments = value;
        }

        private List<InstrumentStatus> _instrumentStatuses;
        public List<InstrumentStatus> InstrumentStatuses
        {
            get => _instrumentStatuses ?? (_instrumentStatuses = new List<InstrumentStatus>());
            set => _instrumentStatuses = value;
        }

        private List<Issue> _issues;
        public List<Issue> Issues
        {
            get => _issues ?? (_issues = new List<Issue>());
            set => _issues = value;
        }

        private List<Manufacturer> _manufacturers;
        public List<Manufacturer> Manufacturers
        {
            get => _manufacturers ?? (_manufacturers = new List<Manufacturer>());
            set => _manufacturers = value;
        }

        private List<Model> _models;
        public List<Model> Models
        {
            get => _models ?? (_models = new List<Model>());
            set => _models = value;
        }

        private List<ModelFile> _modelFiles;
        public List<ModelFile> ModelFiles
        {
            get => _modelFiles ?? (_modelFiles = new List<ModelFile>());
            set => _modelFiles = value;
        }

        private List<ModelSpecification> _modelSpecifications;
        public List<ModelSpecification> ModelSpecifications
        {
            get => _modelSpecifications ?? (_modelSpecifications = new List<ModelSpecification>());
            set => _modelSpecifications = value;
        }

        private List<ModelType> _modelTypes;
        public List<ModelType> ModelTypes
        {
            get => _modelTypes ?? (_modelTypes = new List<ModelType>());
            set => _modelTypes = value;
        }
        private List<ModelGroup> _modelGroups;
        public List<ModelGroup> ModelGroups
        {
            get => _modelGroups ?? (_modelGroups = new List<ModelGroup>());
            set => _modelGroups = value;
        }

        private List<Ticket> _tickets;
        public List<Ticket> Tickets
        {
            get => _tickets ?? (_tickets = new List<Ticket>());
            set => _tickets = value;
        }

        private List<TicketFile> _ticketFiles;
        public List<TicketFile> TicketFiles
        {
            get => _ticketFiles ?? (_ticketFiles = new List<TicketFile>());
            set => _ticketFiles = value;
        }

        private List<TicketPriority> _ticketPriorities;
        public List<TicketPriority> TicketPriorities
        {
            get => _ticketPriorities ?? (_ticketPriorities = new List<TicketPriority>());
            set => _ticketPriorities = value;
        }

        private List<TicketStatus> _ticketStatuses;
        public List<TicketStatus> TicketStatuses
        {
            get => _ticketStatuses ?? (_ticketStatuses = new List<TicketStatus>());
            set => _ticketStatuses = value;
        }

        private List<TicketType> _ticketTypes;
        public List<TicketType> TicketTypes
        {
            get => _ticketTypes ?? (_ticketTypes = new List<TicketType>());
            set => _ticketTypes = value;
        }
        private List<ModelSpecificationGroup> _modelSpecificationGroups;
        public List<ModelSpecificationGroup> ModelSpecificationGroups
        {
            get => _modelSpecificationGroups ?? (_modelSpecificationGroups = new List<ModelSpecificationGroup>());
            set => _modelSpecificationGroups = value;
        }
        private int _currentTicketId;
        public int CurrentTicketId
        {
            get => _currentTicketId;
            set => _currentTicketId = value;
        }
        private int _nextTicketId;
        public int NextTicketId
        {
            get => _nextTicketId;
            set => _nextTicketId = value;
        }
        #endregion

        #region Task Entities
        private List<AppTask> _tasks;
        public List<AppTask> Tasks
        {
            get => _tasks ?? (_tasks = new List<AppTask>());
            set => _tasks = value;
        }

        private List<AppTaskStatus> _taskStatuses;
        public List<AppTaskStatus> TaskStatuses
        {
            get => _taskStatuses ?? (_taskStatuses = new List<AppTaskStatus>());
            set => _taskStatuses = value;
        }

        private List<TaskFile> _taskFiles;
        public List<TaskFile> TaskFiles
        {
            get => _taskFiles ?? (_taskFiles = new List<TaskFile>());
            set => _taskFiles = value;
        }

        private List<TaskPriority> _taskPriorities;
        public List<TaskPriority> TaskPriorities
        {
            get => _taskPriorities ?? (_taskPriorities = new List<TaskPriority>());
            set => _taskPriorities = value;
        }

        private List<TaskTicketRelation> _taskTicketRelations;
        public List<TaskTicketRelation> TaskTicketRelations
        {
            get => _taskTicketRelations ?? (_taskTicketRelations = new List<TaskTicketRelation>());
            set => _taskTicketRelations = value;
        }

        private List<TaskType> _taskTypes;
        public List<TaskType> TaskTypes
        {
            get => _taskTypes ?? (_taskTypes = new List<TaskType>());
            set => _taskTypes = value;
        }
        #endregion

        // AppDbContext
        private readonly OfflineDbContext _context;
        // Constructor
        private CachesServices()
        {
            _context = new OfflineDbContext();
        }

        // Phương thức để reset lại instance Singleton
        public static void ResetInstance()
        {
            lock (_lock)
            {
                _instance = new CachesServices();
            }
        }

        // Get all data
        public async Task GetAllData()
        {
            // Ticket Entities
            _issues = await _context.Issues.ToListAsync();
            _tickets = await _context.Tickets.ToListAsync();
            _ticketFiles = await _context.TicketFiles.ToListAsync();
            _ticketPriorities = await _context.TicketPriorities.ToListAsync();
            _ticketStatuses = await _context.TicketStatuses.ToListAsync();
            _ticketTypes = await _context.TicketTypes.ToListAsync();
            
            int ticketCount = await _context.Tickets.CountAsync();
            if (ticketCount > 0)
            {
                _currentTicketId = await _context.Tickets.MaxAsync(t => t.Id);
                _nextTicketId = _currentTicketId + 1;
            }
            else
            {
                _currentTicketId = 0;
                _nextTicketId = 1;
            }
            // Model Entities
            _countries = await _context.Countries.ToListAsync();
            _instruments = await _context.Instruments.ToListAsync();
            _instrumentStatuses = await _context.InstrumentStatuses.ToListAsync();
            _manufacturers = await _context.Manufacturers.ToListAsync();
            _models = await _context.Models.ToListAsync();
            _modelFiles = await _context.ModelFiles.ToListAsync();
            _modelSpecifications = await _context.ModelSpecifications.ToListAsync();
            _modelTypes = await _context.ModelTypes.ToListAsync();
            _modelGroups = await _context.ModelGroups.ToListAsync();
            _modelSpecificationGroups = await _context.ModelSpecificationGroups.ToListAsync();

            // User Entities

            // Customer Entities

            _contacts = await _context.Contacts.ToListAsync();
            
            _customers = await _context.Customers.ToListAsync();
            
            
            
            
            
            

            // Task Entities
            _tasks = await _context.Tasks.ToListAsync();
            _taskStatuses = await _context.TaskStatuses.ToListAsync();
            _taskFiles = await _context.TaskFiles.ToListAsync();
            _taskPriorities = await _context.TaskPriorities.ToListAsync();
            _taskTicketRelations = await _context.TaskTicketRelations.ToListAsync();
            _taskTypes = await _context.TaskTypes.ToListAsync();

            // Dispose context
            _context.Dispose();
            System.Console.WriteLine("Kết nối database thành công");
            System.Console.WriteLine("---------------------------------------------------");
            System.Console.WriteLine("Nhấn phím bất kỳ để tiếp tục");
        }
    }
}
