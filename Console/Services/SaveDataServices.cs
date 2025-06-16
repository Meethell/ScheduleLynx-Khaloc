using Console.Data;
using Console.Entities.TicketEntities;
using Console.Entities.TaskEntities;
using Console.Entities.CustomerEntities;
using Console.Entities.ModelEntities;
using Console.Entities.UserEntity;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Console.Services
{
    public class SaveDataServices
    {
        public SaveDataServices()
        {

        }

        #region Ticket entities
        //Lưu giá trị của Contact
        public async Task SaveContactAsync(Contact contact)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
                if (item == null)
                {
                    _context.Contacts.Add(contact);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(contact);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveContactListAsync(List<Contact> ContactList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in ContactList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.Contacts.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Contacts.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của Country
        public async Task SaveCountryAsync(Country country)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Countries.FirstOrDefault(x => x.Id == country.Id);
                if (item == null)
                {
                    _context.Countries.Add(country);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(country);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của Customer
        public async Task SaveCustomerAsync(Customer customer)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Customers.FirstOrDefault(x => x.Id == customer.Id);
                if (item == null)
                {
                    _context.Customers.Add(customer);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(customer);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveCustomerListAsync(List<Customer> CustomerList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in CustomerList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.Customers.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Customers.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của Instrument
        public async Task SaveInstrumentAsync(Instrument instrument)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Instruments.FirstOrDefault(x => x.Id == instrument.Id);
                if (item == null)
                {
                    _context.Instruments.Add(instrument);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(instrument);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveInstrumentListAsync(List<Instrument> InstrumentList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in InstrumentList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.Instruments.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Instruments.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của InstrumentType
        public async Task SaveInstrumentTypeAsync(ModelType instrumentType)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.ModelTypes.FirstOrDefault(x => x.Id == instrumentType.Id);
                if (item == null)
                {
                    _context.ModelTypes.Add(instrumentType);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(instrumentType);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của InstrumentStatus
        public async Task SaveInstrumentStatusAsync(InstrumentStatus instrumentStatus)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.InstrumentStatuses.FirstOrDefault(x => x.Id == instrumentStatus.Id);
                if (item == null)
                {
                    _context.InstrumentStatuses.Add(instrumentStatus);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(instrumentStatus);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của Issue
        public async Task SaveIssueAsync(Issue issue)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Issues.FirstOrDefault(x => x.Id == issue.Id);
                if (item == null)
                {
                    _context.Issues.Add(issue);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(issue);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của Manufacturer
        public async Task SaveManufacturerAsync(Manufacturer manufacturer)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Manufacturers.FirstOrDefault(x => x.Id == manufacturer.Id);
                if (item == null)
                {
                    _context.Manufacturers.Add(manufacturer);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(manufacturer);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveManufacturerListAsync(List<Manufacturer> ManufacturerList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in ManufacturerList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.Manufacturers.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Manufacturers.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của Model
        public async Task SaveModelAsync(Model model)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Models.FirstOrDefault(x => x.Id == model.Id);
                if (item == null)
                {
                    _context.Models.Add(model);
                    _context.ModelSpecificationGroups.Add(new ModelSpecificationGroup { ModelId = model.Id, Name = "Tính năng tổng quát", IsBase = true });
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(model);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveModelListAsync(List<Model> ModelList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in ModelList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.Models.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Models.Add(item);
                        _context.ModelSpecificationGroups.Add(new ModelSpecificationGroup { ModelId = item.Id, Name = "Tính năng tổng quát", IsBase = true });
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của ModelFile
        public async Task SaveModelFileAsync(ModelFile modelFile)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.ModelFiles.FirstOrDefault(x => x.Id == modelFile.Id);
                if (item == null)
                {
                    _context.ModelFiles.Add(modelFile);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(modelFile);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của ModelSpecification
        public async Task SaveModelSpecificationAsync(ModelSpecification modelSpecification)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.ModelSpecifications.FirstOrDefault(x => x.Id == modelSpecification.Id);
                if (item == null)
                {
                    _context.ModelSpecifications.Add(modelSpecification);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(modelSpecification);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveModelSpecificationListAsync(List<ModelSpecification> ModelSpecificationList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in ModelSpecificationList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.ModelSpecifications.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.ModelSpecifications.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của ModelSpecificationGroup
        public async Task SaveModelSpecificationGroupAsync(ModelSpecificationGroup modelSpecificationGroup)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.ModelSpecificationGroups.FirstOrDefault(x => x.Id == modelSpecificationGroup.Id);
                if (item == null)
                {
                    _context.ModelSpecificationGroups.Add(modelSpecificationGroup);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(modelSpecificationGroup);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveModelSpecificationGroupListAsync(List<ModelSpecificationGroup> ModelSpecificationGroupList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in ModelSpecificationGroupList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.ModelSpecificationGroups.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.ModelSpecificationGroups.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của ModelGroup
        public async Task SaveModelGroupAsync(ModelGroup modelGroup)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.ModelGroups.FirstOrDefault(x => x.Id == modelGroup.Id);
                if (item == null)
                {
                    _context.ModelGroups.Add(modelGroup);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(modelGroup);
                }
                await _context.SaveChangesAsync();
            }
        }

        //Lưu giá trị của TicketPriority
        public async Task SaveTicketPriorityAsync(TicketPriority priority)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TicketPriorities.FirstOrDefault(x => x.Id == priority.Id);
                if (item == null)
                {
                    _context.TicketPriorities.Add(priority);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(priority);
                }
                await _context.SaveChangesAsync();
            }
        }

        //Lưu giá trị của Ticket
        public async Task SaveTicketAsync(Ticket ticket)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Tickets.FirstOrDefault(x => x.Id == ticket.Id);
                if (item == null)
                {
                    _context.Tickets.Add(ticket);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(ticket);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveTicketListAsync(List<Ticket> TicketList)
        {
            using (var _context = new OfflineDbContext())
            {
                foreach (var item in TicketList)
                {
                    dynamic existingItem = null;
                    existingItem = _context.Tickets.Find(item.Id); // Giả sử mỗi item có trường Id là khóa duy nhất
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Tickets.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        //Lưu giá trị của TicketStatus
        public async Task SaveTicketStatusAsync(TicketStatus ticketStatus)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TicketStatuses.FirstOrDefault(x => x.Id == ticketStatus.Id);
                if (item == null)
                {
                    _context.TicketStatuses.Add(ticketStatus);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(ticketStatus);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của TicketType
        public async Task SaveTicketTypeAsync(TicketType ticketType)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TicketTypes.FirstOrDefault(x => x.Id == ticketType.Id);
                if (item == null)
                {
                    _context.TicketTypes.Add(ticketType);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(ticketType);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của TicketFile
        public async Task SaveTicketFileAsync(TicketFile ticketFile)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TicketFiles.FirstOrDefault(x => x.Id == ticketFile.Id);
                if (item == null)
                {
                    _context.TicketFiles.Add(ticketFile);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(ticketFile);
                }
                await _context.SaveChangesAsync();
            }
        }
        #endregion
        #region Task entities
        //Lưu giá trị của AppTaskStatus
        public async Task SaveTaskStatusAsync(AppTaskStatus taskStatus)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TaskStatuses.FirstOrDefault(x => x.Id == taskStatus.Id);
                if (item == null)
                {
                    _context.TaskStatuses.Add(taskStatus);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(taskStatus);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của TaskType
        public async Task SaveTaskTypeAsync(TaskType taskType)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TaskTypes.FirstOrDefault(x => x.Id == taskType.Id);
                if (item == null)
                {
                    _context.TaskTypes.Add(taskType);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(taskType);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của TaskFile
        public async Task SaveTaskFileAsync(TaskFile taskFile)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TaskFiles.FirstOrDefault(x => x.Id == taskFile.Id);
                if (item == null)
                {
                    _context.TaskFiles.Add(taskFile);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(taskFile);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của AppTask
        public async Task SaveTaskAsync(AppTask task)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.Tasks.FirstOrDefault(x => x.Id == task.Id);
                if (item == null)
                {
                    _context.Tasks.Add(task);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(task);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của TaskTicketRelation
        public async Task SaveTaskTicketRelationAsync(TaskTicketRelation taskTicketRelation)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TaskTicketRelations.FirstOrDefault(x => x.Id == taskTicketRelation.Id);
                if (item == null)
                {
                    _context.TaskTicketRelations.Add(taskTicketRelation);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(taskTicketRelation);
                }
                await _context.SaveChangesAsync();
            }
        }
        //Lưu giá trị của TaskPriority
        public async Task SaveTaskPriorityAsync(TaskPriority taskPriority)
        {
            using (var _context = new OfflineDbContext())
            {
                var item = _context.TaskPriorities.FirstOrDefault(x => x.Id == taskPriority.Id);
                if (item == null)
                {
                    _context.TaskPriorities.Add(taskPriority);
                }
                else
                {
                    _context.Entry(item).CurrentValues.SetValues(taskPriority);
                }
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
