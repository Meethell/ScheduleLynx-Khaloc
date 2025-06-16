using Caliburn.Micro;
using Client.Entities.Constants;
using Client.Services;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace Client.ViewModels
{
    public class DataViewerViewModel : Screen
    {
        
        private List<string> _entityNameList = EntityName.NameList;
        public bool _canUserEdit;
        public bool CanUserEdit
        {
            get => _canUserEdit;
            set
            {
                _canUserEdit = value;
                NotifyOfPropertyChange(() => CanUserEdit);
            }
        }
        public bool _IsReadOnly;
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            set
            {
                _IsReadOnly = value;
                NotifyOfPropertyChange(() => IsReadOnly);
            }
        }
        public List<string> EntityNameList
        {
            get => _entityNameList;
            set
            {
                _entityNameList = value;
                NotifyOfPropertyChange(() => EntityNameList);
            }
        }

        // Hold the data for dgData
        private List<dynamic> _displayedData;
        public List<dynamic> DisplayedData
        {
            get => _displayedData;
            set
            {
                _displayedData = value;
                NotifyOfPropertyChange(() => DisplayedData);
            }
        }

        private string _selectedEntityName;
        public string SelectedEntityName
        {
            get => _selectedEntityName;
            set
            {
                _selectedEntityName = value;
                NotifyOfPropertyChange(() => SelectedEntityName);
                UpdateDisplayedData(); // Call method to update data based on the selected entity
                CanUserEdit = false;
                IsReadOnly = true;
            }
        }

        private IList<dynamic> _selectedItems;
        public IList<dynamic> SelectedItems
        {
            get => _selectedItems;
            set
            {
                _selectedItems = value;
                NotifyOfPropertyChange(() => SelectedItems);
            }
        }

        //Constructor hoặc một phương thức khởi tạo
        public DataViewerViewModel()
        {
            CanUserEdit = false;
            IsReadOnly = true;
        }

        private void UpdateDisplayedData()
        {
            switch (SelectedEntityName)
            {
                case "Contact":
                    DisplayedData = CachesServices.Instance.Contacts.Cast<dynamic>().ToList();
                    break;
                case "Country":
                    DisplayedData = CachesServices.Instance.Countries.Cast<dynamic>().ToList();
                    break;
                case "Customer":
                    DisplayedData = CachesServices.Instance.Customers.Cast<dynamic>().ToList();
                    break;
                case "Instrument":
                    DisplayedData = CachesServices.Instance.Instruments.Cast<dynamic>().ToList();
                    break;
                case "InstrumentType":
                    DisplayedData = CachesServices.Instance.ModelTypes.Cast<dynamic>().ToList();
                    break;
                case "InstrumentStatus":
                    DisplayedData = CachesServices.Instance.InstrumentStatuses.Cast<dynamic>().ToList();
                    break;
                case "Issue":
                    DisplayedData = CachesServices.Instance.Issues.Cast<dynamic>().ToList();
                    break;
                case "Manufacturer":
                    DisplayedData = CachesServices.Instance.Manufacturers.Cast<dynamic>().ToList();
                    break;
                case "Model":
                    DisplayedData = CachesServices.Instance.Models.Cast<dynamic>().ToList();
                    break;
                case "TicketPriority":
                    DisplayedData = CachesServices.Instance.TicketPriorities.Cast<dynamic>().ToList();
                    break;
                case "TaskPriority":
                    DisplayedData = CachesServices.Instance.TaskPriorities.Cast<dynamic>().ToList();
                    break;
                case "Ticket":
                    DisplayedData = CachesServices.Instance.Tickets.Cast<dynamic>().ToList();
                    break;
                case "TicketStatus":
                    DisplayedData = CachesServices.Instance.TicketStatuses.Cast<dynamic>().ToList();
                    break;
                case "TicketType":
                    DisplayedData = CachesServices.Instance.TicketTypes.Cast<dynamic>().ToList();
                    break;
                case "AppTask":
                    DisplayedData = CachesServices.Instance.Tasks.Cast<dynamic>().ToList();
                    break;
                case "AppTaskStatus":
                    DisplayedData = CachesServices.Instance.TaskStatuses.Cast<dynamic>().ToList();
                    break;
                case "TaskType":
                    DisplayedData = CachesServices.Instance.TaskTypes.Cast<dynamic>().ToList();
                    break;
                case "TaskFile":
                    DisplayedData = CachesServices.Instance.TaskFiles.Cast<dynamic>().ToList();
                    break;
                case "TaskTicketRelation":
                    DisplayedData = CachesServices.Instance.TaskTicketRelations.Cast<dynamic>().ToList();
                    break;
                case "TicketFile":
                    DisplayedData = CachesServices.Instance.TicketFiles.Cast<dynamic>().ToList();
                    break;
                case "ModelFile":
                    DisplayedData = CachesServices.Instance.ModelFiles.Cast<dynamic>().ToList();
                    break;
                case "ModelSpecification":
                    DisplayedData = CachesServices.Instance.ModelSpecifications.Cast<dynamic>().ToList();
                    break;
                case "ModelGroup":
                    DisplayedData = CachesServices.Instance.ModelGroups.Cast<dynamic>().ToList();
                    break;
                default:
                    DisplayedData = null;
                    break;
            }
        }
        // Save Data
        public async void bnSave()
        {
            // Lưu dữ liệu vào DB

            if (DisplayedData != null)
            {
                switch (SelectedEntityName)
                {
                    case "Contact":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveContactAsync(item);
                        }
                        break;
                    case "Country":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveCountryAsync(item);
                        }
                        break;
                    case "Customer":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveCustomerAsync(item);
                        }
                        break;
                    case "Instrument":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveInstrumentAsync(item);
                        }
                        break;
                    case "InstrumentType":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveInstrumentTypeAsync(item);
                        }
                        break;
                    case "InstrumentStatus":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveInstrumentStatusAsync(item);
                        }
                        break;
                    case "Issue":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveIssueAsync(item);
                        }
                        break;
                    case "Manufacturer":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveManufacturerAsync(item);
                        }
                        break;
                    case "Model":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveModelAsync(item);
                        }
                        break;
                    case "TaskPriority":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTaskPriorityAsync(item);
                        }
                        break;
                    case "TicketPriority":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTicketPriorityAsync(item);
                        }
                        break;
                    case "Ticket":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTicketAsync(item);
                        }
                        break;
                    case "TicketStatus":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTicketStatusAsync(item);
                        }
                        break;
                    case "TicketType":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTicketTypeAsync(item);
                        }
                        break;
                    case "AppTask":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTaskAsync(item);
                        }
                        break;
                    case "AppTaskStatus":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTaskStatusAsync(item);
                        }
                        break;
                    case "TaskType":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTaskTypeAsync(item);
                        }
                        break;
                    case "TaskFile":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTaskFileAsync(item);
                        }
                        break;
                    case "TaskTicketRelation":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTaskTicketRelationAsync(item);
                        }
                        break;
                    case "TicketFile":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveTicketFileAsync(item);
                        }
                        break;
                    case "ModelFile":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveModelFileAsync(item);
                        }
                        break;
                    case "ModelSpecification":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveModelSpecificationAsync(item);
                        }
                        break;
                    case "ModelGroup":
                        foreach (var item in DisplayedData)
                        {
                            await new SaveDataServices().SaveModelGroupAsync(item);
                        }
                        break;
                }
                CanUserEdit = false;
                IsReadOnly = true;

            }
            // Cập nhật Cache
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Lưu xong thì cập nhật lại danh sách
            UpdateDisplayedData();
        }
        // Delete Data 
        public async void bnDelete()
        {

            if (SelectedItems != null && SelectedItems.Count > 0)
            {
                switch (SelectedEntityName)
                {
                    case "Contact":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteContactAsync(SelectedItem);
                        }
                        break;
                    case "Country":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteCountryAsync(SelectedItem);
                        }
                        break;
                    case "Customer":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteCustomerAsync(SelectedItem);
                        }
                        break;
                    case "Instrument":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteInstrumentAsync(SelectedItem);
                        }
                        break;
                    case "InstrumentType":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteInstrumentTypeAsync(SelectedItem);
                        }
                        break;
                    case "InstrumentStatus":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteInstrumentStatusAsync(SelectedItem);
                        }
                        break;
                    case "Issue":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteIssueAsync(SelectedItem);
                        }
                        break;
                    case "Manufacturer":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteManufacturerAsync(SelectedItem);
                        }
                        break;
                    case "Model":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteModelAsync(SelectedItem);
                        }
                        break;
                    case "Ticket":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTicketAsync(SelectedItem);
                        }
                        break;
                    case "TicketStatus":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTicketStatusAsync(SelectedItem);
                        }
                        break;
                    case "TicketType":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTicketTypeAsync(SelectedItem);
                        }
                        break;
                    case "AppTask":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteAppTaskAsync(SelectedItem);
                        }
                        break;
                    case "AppTaskStatus":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteAppTaskStatusAsync(SelectedItem);
                        }
                        break;
                    case "TaskPriority":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTaskPriorityAsync(SelectedItem);
                        }
                        break;
                    case "TicketPriority":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTicketPriorityAsync(SelectedItem);
                        }
                        break;
                    case "TaskType":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTaskTypeAsync(SelectedItem);
                        }
                        break;
                    case "TaskFile":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTaskFileAsync(SelectedItem);
                        }
                        break;
                    case "TaskTicketRelation":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTaskTicketRelationAsync(SelectedItem);
                        }
                        break;
                    case "TicketFile":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteTicketFileAsync(SelectedItem);
                        }
                        break;
                    case "ModelFile":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteModelFileAsync(SelectedItem);
                        }
                        break;
                    case "ModelSpecification":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteModelSpecificationAsync(SelectedItem);
                        }
                        break;
                    case "ModelGroup":
                        foreach (var SelectedItem in SelectedItems.ToList())
                        {
                            await new DeleteDataServices().DeleteModelGroupAsync(SelectedItem);
                        }
                        break;
                }
                // Xóa xong thì cập nhật lại danh sách
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                UpdateDisplayedData();
                CanUserEdit = false;
                IsReadOnly = true;
            }
        }

        public void bnEdit()
        {
            if (CanUserEdit)
            {
                CanUserEdit = false;
                IsReadOnly = true;
            }
            else
            {
                CanUserEdit = true;
                IsReadOnly = false;
            }
        }
    }
}
