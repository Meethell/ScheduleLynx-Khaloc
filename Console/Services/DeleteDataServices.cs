using Console.Data;
using Console.Entities.CustomerEntities;
using Console.Entities.ModelEntities;
using Console.Entities.TaskEntities;
using Console.Entities.TicketEntities;
using System.Linq;
using System.Threading.Tasks;

namespace Console.Services
{
    public class DeleteDataServices
    {
        //Constructor
        public DeleteDataServices()
        {

        }

        #region Ticket Entities
        //Xóa Contact
        public async Task DeleteContactAsync(Contact contact)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem contact có tồn tại trong DB không
                var item = _context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
                if (item != null)
                {
                    _context.Contacts.Attach(item);
                    _context.Contacts.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa Country
        public async Task DeleteCountryAsync(Country country)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem country có tồn tại trong DB không
                var item = _context.Countries.FirstOrDefault(x => x.Id == country.Id);
                if (item != null)
                {
                    _context.Countries.Attach(item);
                    _context.Countries.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }

        }
        //Xóa Customer
        public async Task DeleteCustomerAsync(Customer customer)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem customer có tồn tại trong DB không
                var item = _context.Customers.FirstOrDefault(x => x.Id == customer.Id);
                if (item != null)
                {
                    _context.Customers.Attach(item);
                    _context.Customers.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa Instrument
        public async Task DeleteInstrumentAsync(Instrument instrument)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem instrument có tồn tại trong DB không
                var item = _context.Instruments.FirstOrDefault(x => x.Id == instrument.Id);
                if (item != null)
                {
                    _context.Instruments.Attach(item);
                    _context.Instruments.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa InstrumentType
        public async Task DeleteInstrumentTypeAsync(ModelType instrumentType)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem instrumentType có tồn tại trong DB không
                var item = _context.ModelTypes.FirstOrDefault(x => x.Id == instrumentType.Id);
                if (item != null)
                {
                    _context.ModelTypes.Attach(item);
                    _context.ModelTypes.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa InstrumentStatus
        public async Task DeleteInstrumentStatusAsync(InstrumentStatus instrumentStatus)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem instrumentStatus có tồn tại trong DB không
                var item = _context.InstrumentStatuses.FirstOrDefault(x => x.Id == instrumentStatus.Id);
                if (item != null)
                {
                    _context.InstrumentStatuses.Attach(item);
                    _context.InstrumentStatuses.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa Issue
        public async Task DeleteIssueAsync(Issue issue)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem issue có tồn tại trong DB không
                var item = _context.Issues.FirstOrDefault(x => x.Id == issue.Id);
                if (item != null)
                {
                    _context.Issues.Attach(item);
                    _context.Issues.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa Manufacturer
        public async Task DeleteManufacturerAsync(Manufacturer manufacturer)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem manufacturer có tồn tại trong DB không
                var item = _context.Manufacturers.FirstOrDefault(x => x.Id == manufacturer.Id);
                if (item != null)
                {
                    _context.Manufacturers.Attach(item);
                    _context.Manufacturers.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa Model
        public async Task DeleteModelAsync(Model model)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem model có tồn tại trong DB không
                var item = _context.Models.FirstOrDefault(x => x.Id == model.Id);
                if (item != null)
                {
                    _context.Models.Attach(item);
                    _context.Models.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        // Xóa ModelGroup
        public async Task DeleteModelGroupAsync(ModelGroup modelGroup)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem ModelGroup có tồn tại trong DB không
                var item = _context.ModelGroups.FirstOrDefault(x => x.Id == modelGroup.Id);
                if (item != null)
                {
                    _context.ModelGroups.Attach(item);
                    _context.ModelGroups.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa TicketPriority
        public async Task DeleteTicketPriorityAsync(TicketPriority priority)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem Priority có tồn tại trong DB không
                var item = _context.TicketPriorities.FirstOrDefault(x => x.Id == priority.Id);
                if (item != null)
                {
                    _context.TicketPriorities.Attach(item);
                    _context.TicketPriorities.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa Ticket
        public async Task DeleteTicketAsync(Ticket ticket)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem Ticket có tồn tại trong DB không
                var item = _context.Tickets.FirstOrDefault(x => x.Id == ticket.Id);
                if (item != null)
                {
                    _context.Tickets.Attach(item);
                    _context.Tickets.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }

        }
        //Xóa TicketStatus
        public async Task DeleteTicketStatusAsync(TicketStatus ticketStatus)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem TicketStatus có tồn tại trong DB không
                var item = _context.TicketStatuses.FirstOrDefault(x => x.Id == ticketStatus.Id);
                if (item != null)
                {
                    _context.TicketStatuses.Attach(item);
                    _context.TicketStatuses.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }

        }
        //Xóa TicketType
        public async Task DeleteTicketTypeAsync(TicketType ticketType)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem TicketType có tồn tại trong DB không
                var item = _context.TicketTypes.FirstOrDefault(x => x.Id == ticketType.Id);
                if (item != null)
                {
                    _context.TicketTypes.Attach(item);
                    _context.TicketTypes.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa TicketFile
        //public async Task DeleteTicketFileAsync(TicketFile ticketFile)
        //{
        //    using (var _context = new AppDbContext())
        //    {
        //        // Kiểm tra xem TicketFile có tồn tại trong DB không
        //        var item = _context.TicketFiles.FirstOrDefault(x => x.Id == ticketFile.Id);
        //        if (item != null)
        //        {
        //            // Xóa file trong thư mục lưu trữ
        //            FileServices fileServices = new FileServices();
        //            fileServices.DeleteFile(item.FilePath);
        //            _context.TicketFiles.Attach(item);
        //            _context.TicketFiles.Remove(item);
        //            await _context.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            // Nếu không tồn tại thì hiển thị thông báo trong customMessageBox
        //            var customMessageBoxViewModel = new CustomMessageBoxViewModel
        //            {
        //                Message = "Error",
        //                TxtMessage = "Item Not found",
        //                IsInformation = true
        //            };
        //            var windowManager = new WindowManager();
        //            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
        //        }
        //    }
        //}
        // Xóa ModelFile
        //public async Task DeleteModelFileAsync(ModelFile modelFile)
        //{
        //    using (var _context = new AppDbContext())
        //    {
        //        // Kiểm tra xem ModelFile có tồn tại trong DB không
        //        var item = _context.ModelFiles.FirstOrDefault(x => x.Id == modelFile.Id);
        //        if (item != null)
        //        {
        //            // Xóa file trong thư mục lưu trữ
        //            FileServices fileServices = new FileServices();
        //            fileServices.DeleteFile(item.FilePath);
        //            _context.ModelFiles.Attach(item);
        //            _context.ModelFiles.Remove(item);
        //            await _context.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            // Nếu không tồn tại thì hiển thị thông báo trong customMessageBox
        //            var customMessageBoxViewModel = new CustomMessageBoxViewModel
        //            {
        //                Message = "Error",
        //                TxtMessage = "Item Not found",
        //                IsInformation = true
        //            };
        //            var windowManager = new WindowManager();
        //            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
        //        }
        //    }
        //}
        // Xóa ModelSpecification
        public async Task DeleteModelSpecificationAsync(ModelSpecification modelSpecification)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem ModelSpecification có tồn tại trong DB không
                var item = _context.ModelSpecifications.FirstOrDefault(x => x.Id == modelSpecification.Id);
                if (item != null)
                {
                    _context.ModelSpecifications.Attach(item);
                    _context.ModelSpecifications.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        // Xóa ModelSpecificationGroup
        //public async Task DeleteModelSpecificationGroupAsync(ModelSpecificationGroup modelSpecificationGroup)
        //{
        //    using (var _context = new AppDbContext())
        //    {
        //        // Kiểm tra xem ModelSpecificationGroup có tồn tại trong DB không
        //        var item = _context.ModelSpecificationGroups.FirstOrDefault(x => x.Id == modelSpecificationGroup.Id);
        //        if (item != null)
        //        {
        //            // Không được xóa nếu item.IsBase = true
        //            if (item.IsBase)
        //            {
        //                var customMessageBoxViewModel = new CustomMessageBoxViewModel
        //                {
        //                    Message = "Error",
        //                    TxtMessage = "Cannot delete base group",
        //                    IsInformation = true
        //                };
        //                var windowManager = new WindowManager();
        //                await windowManager.ShowDialogAsync(customMessageBoxViewModel);
        //                return;
        //            }
        //            _context.ModelSpecificationGroups.Attach(item);
        //            _context.ModelSpecificationGroups.Remove(item);
        //            await _context.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            // Nếu không tồn tại thì hiển thị thông báo trong customMessageBox
        //            var customMessageBoxViewModel = new CustomMessageBoxViewModel
        //            {
        //                Message = "Error",
        //                TxtMessage = "Item Not found",
        //                IsInformation = true
        //            };
        //            var windowManager = new WindowManager();
        //            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
        //        }
        //    }
        //}

        #endregion
        #region Task Entities
        //Xóa AppTask
        public async Task DeleteAppTaskAsync(AppTask appTask)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem AppTask có tồn tại trong DB không
                var item = _context.Tasks.FirstOrDefault(x => x.Id == appTask.Id);
                if (item != null)
                {
                    _context.Tasks.Attach(item);
                    _context.Tasks.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa AppTaskStatus
        public async Task DeleteAppTaskStatusAsync(AppTaskStatus appTaskStatus)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem AppTaskStatus có tồn tại trong DB không
                var item = _context.TaskStatuses.FirstOrDefault(x => x.Id == appTaskStatus.Id);
                if (item != null)
                {
                    _context.TaskStatuses.Attach(item);
                    _context.TaskStatuses.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa AppTaskType
        public async Task DeleteTaskTypeAsync(TaskType appTaskType)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem AppTaskType có tồn tại trong DB không
                var item = _context.TaskTypes.FirstOrDefault(x => x.Id == appTaskType.Id);
                if (item != null)
                {
                    _context.TaskTypes.Attach(item);
                    _context.TaskTypes.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa TaskTicketRelation
        public async Task DeleteTaskTicketRelationAsync(TaskTicketRelation taskTicketRelation)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem TaskTicketRelation có tồn tại trong DB không
                var item = _context.TaskTicketRelations.FirstOrDefault(x => x.Id == taskTicketRelation.Id);
                if (item != null)
                {
                    _context.TaskTicketRelations.Attach(item);
                    _context.TaskTicketRelations.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }
        }
        //Xóa TaskFile
        //public async Task DeleteTaskFileAsync(TaskFile taskFile)
        //{
        //    using (var _context = new AppDbContext())
        //    {
        //        // Kiểm tra xem TaskFile có tồn tại trong DB không
        //        var item = _context.TaskFiles.FirstOrDefault(x => x.Id == taskFile.Id);
        //        if (item != null)
        //        {
        //            // Xóa file trong thư mục lưu trữ
        //            FileServices fileServices = new FileServices();
        //            //fileServices.DeleteFile(item.FilePath);
        //            _context.TaskFiles.Attach(item);
        //            _context.TaskFiles.Remove(item);
        //            await _context.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            // Nếu không tồn tại thì hiển thị thông báo trong customMessageBox
        //            var customMessageBoxViewModel = new CustomMessageBoxViewModel
        //            {
        //                Message = "Error",
        //                TxtMessage = "Item Not found",
        //                IsInformation = true
        //            };
        //            var windowManager = new WindowManager();
        //            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
        //        }
        //    }
        //}
        // Xóa TaskPriority
        public async Task DeleteTaskPriorityAsync(TaskPriority taskPriority)
        {
            using (var _context = new OfflineDbContext())
            {
                // Kiểm tra xem TaskPriority có tồn tại trong DB không
                var item = _context.TaskPriorities.FirstOrDefault(x => x.Id == taskPriority.Id);
                if (item != null)
                {
                    _context.TaskPriorities.Attach(item);
                    _context.TaskPriorities.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu không tồn tại thì hiển thị thông báo trên console
                    System.Console.WriteLine("Item Not found");
                }
            }

        }
        #endregion
    }
}
