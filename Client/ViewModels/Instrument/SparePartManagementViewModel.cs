using Caliburn.Micro;
using Client.Entities.CustomerEntities;
using Client.Entities.ModelEntities;
using Client.Entities.TicketEntities;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public class SparePartManagementViewModel : Screen
    {

        // Full Properties
        //---------------------------------------------------------------
        // Manufacturer Properties
        private ObservableCollection<Manufacturer> manufacturers = new ObservableCollection<Manufacturer>(CachesServices.Instance.Manufacturers.OrderBy(x => x.Name));
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set
            {
                manufacturers = value;
                NotifyOfPropertyChange(() => Manufacturers);
            }
        }
        private Manufacturer selectedManufacturer;
        public Manufacturer SelectedManufacturer
        {
            get { return selectedManufacturer; }
            set
            {
                selectedManufacturer = value;
                NotifyOfPropertyChange(() => SelectedManufacturer);
                // Nút thêm mới SpareParts Enable khi SelectedManufacturer != null
                IsSparePartNewButtonEnable = selectedManufacturer != null && !IsSparePartUserEditting;
                LoadSpareParts();
            }
        }
        private string searchManufacturer;
        public string SearchManufacturer
        {
            get { return searchManufacturer; }
            set
            {
                searchManufacturer = value;
                NotifyOfPropertyChange(() => SearchManufacturer);
                LoadManufacturer();
            }
        }
        //------------------------------------------------------------------
        // SparePart Properties
        private ObservableCollection<SpareParts> spareParts = new ObservableCollection<SpareParts>();
        public ObservableCollection<SpareParts> SpareParts
        {
            get { return spareParts; }
            set
            {
                spareParts = value;
                NotifyOfPropertyChange(() => SpareParts);
                if (SpareParts.Count > 0)
                {
                    IsSparePartEditButtonEnable = true;

                }
                else
                {
                    IsSparePartEditButtonEnable = false;

                }
            }
        }
        private bool isSearchTextEnable = true;
        public bool IsSearchTextEnable
        {
            get { return isSearchTextEnable; }
            set
            {
                isSearchTextEnable = value;
                NotifyOfPropertyChange(() => IsSearchTextEnable);
            }
        }
        private SpareParts selectedSparePart;
        public SpareParts SelectedSparePart
        {
            get { return selectedSparePart; }
            set
            {
                selectedSparePart = value;
                NotifyOfPropertyChange(() => SelectedSparePart);
                SetSparePartDeleteButtonEnable();
                LoadTicket(DateFrom, DateTo);
            }
        }
        private List<string> _filters = new List<string> { "Tên phụ tùng", "Mã phụ tùng", "Tất cả" };
        public List<string> Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;
                NotifyOfPropertyChange(() => Filters);
            }
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                NotifyOfPropertyChange(() => SelectedFilter);
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
                LoadSpareParts();
            }
        }

        //-----------------------------------------------------------------
        // Ticket Properties
        private ObservableCollection<Ticket> tickets;
        public ObservableCollection<Ticket> Tickets
        {
            get { return tickets; }
            set
            {
                tickets = value;
                NotifyOfPropertyChange(() => Tickets);
            }
        }
        private Ticket selectedTicket;
        public Ticket SelectedTicket
        {
            get { return selectedTicket; }
            set
            {
                selectedTicket = value;
                NotifyOfPropertyChange(() => SelectedTicket);
            }
        }
        //-------------------------------------------------------------------
        // Button Check Properties
        // IsSparePartUserEditting - KEY - Để xác định xem người dùng đang editting hay không
        // - Gía trị ngươc lại với IsSparePartReadOnly
        // - Cho phép nút Ok và Cancel hiện lên khi IsSparePartUserEditting = true
        private List<SpareParts> _tempSparePart = new List<SpareParts>();
        private bool _isSparePartUserEditting;
        public bool IsSparePartUserEditting
        {
            get { return _isSparePartUserEditting; }
            set
            {
                _isSparePartUserEditting = value;
                NotifyOfPropertyChange(() => IsSparePartUserEditting);
                IsSparePartReadOnly = !value;
                IsSparePartNewButtonEnable = !value;
                IsSearchTextEnable = !value;
                SetSparePartDeleteButtonEnable();
            }
        }
        // IsSparePartReadOnly - Để xác định xem người dùng có thể chỉnh sửa SparePartDataGrid hay không
        // - Cho phép nút Edit hiện lên khi IsSparePartReadOnly = true
        private bool _isSparePartReadOnly = true;
        public bool IsSparePartReadOnly
        {
            get { return _isSparePartReadOnly; }
            set
            {
                _isSparePartReadOnly = value;
                NotifyOfPropertyChange(() => IsSparePartReadOnly);
            }
        }
        // IsSparePartEnable - Để xác định xem người dùng có thể chỉnh sửa SparePartDataGrid hay không
        private bool _isSparePartEnable = true;
        public bool IsSparePartEnable
        {
            get { return _isSparePartEnable; }
            set
            {
                _isSparePartEnable = value;
                NotifyOfPropertyChange(() => IsSparePartEnable);
            }
        }


        // Button Properties

        // IsSparePartEditButtonEnable - Để xác định xem nút Edit có được enable hay không
        // - Nếu SparePartDataGrid không có dữ liệu thì nút Edit sẽ không được enable
        private bool _isSparePartEditButtonEnable;
        public bool IsSparePartEditButtonEnable
        {
            get { return _isSparePartEditButtonEnable; }
            set
            {
                _isSparePartEditButtonEnable = value;
                NotifyOfPropertyChange(() => IsSparePartEditButtonEnable);
            }
        }
        // IsSparePartDeleteButtonEnable - Để xác định xem nút Delete có được enable hay không
        // - Nếu SelectedSparePart = null thì nút Delete sẽ không được enable
        private bool _isSparePartDeleteButtonEnable;
        public bool IsSparePartDeleteButtonEnable
        {
            get { return _isSparePartDeleteButtonEnable; }
            set
            {
                _isSparePartDeleteButtonEnable = value;
                NotifyOfPropertyChange(() => IsSparePartDeleteButtonEnable);
            }
        }
        // IsSparePartNewButtonEnable - Để xác định xem nút New có được enable hay không
        // - Nếu IsSparePartUserEditting = true thì nút New sẽ không được enable
        // - Nếu SpareParts = null thì nút New sẽ được enable
        private bool _isSparePartNewButtonEnable;
        public bool IsSparePartNewButtonEnable
        {
            get { return _isSparePartNewButtonEnable; }
            set
            {
                _isSparePartNewButtonEnable = value;
                NotifyOfPropertyChange(() => IsSparePartNewButtonEnable);
            }
        }
        // Lưu trạng thái của các nút
        private bool _tempIsSparePartNewButtonEnable;
        private bool _tempIsSparePartEditButtonEnable;
        private bool _tempIsSparePartDeleteButtonEnable;
        private bool _tempIsSparePartEnable;

        private bool isManufacturerEnable = true;
        public bool IsManufacturerEnable
        {
            get { return isManufacturerEnable; }
            set
            {
                isManufacturerEnable = value;
                NotifyOfPropertyChange(() => IsManufacturerEnable);
            }
        }

        private bool _tempIsManufacturerEnable;
        //-----------------------------------------------------------------
        // Query SparePart Statistic
        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                _dateFrom = value;
                NotifyOfPropertyChange(() => DateFrom);
            }
        }
        private DateTime _dateTo;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set
            {
                _dateTo = value;
                NotifyOfPropertyChange(() => DateTo);
            }
        }
        private int outNumber;
        public int OutNumber
        {
            get { return outNumber; }
            set
            {
                outNumber = value;
                NotifyOfPropertyChange(() => OutNumber);
            }
        }

        // Constructor
        private readonly InstrumentViewModel _instrumentViewModel;
        private readonly MainViewModel _mainViewModel;
        public SparePartManagementViewModel(InstrumentViewModel instrumentViewModel, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _instrumentViewModel = instrumentViewModel;
            // Lấy ngày bắt đầu và kết thúc của tháng hiện tại
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateFrom.AddMonths(1).AddDays(-1);
        }

        public async Task LoadSpareParts()
        {
            // Load spare parts from database
            var sparePartsQuery = CachesServices.Instance.SpareParts.AsQueryable();

            if (SelectedFilter == "Tất cả")
            {
                SpareParts = new BindableCollection<SpareParts>(sparePartsQuery.OrderBy(x => x.Name));
                return;
            }


            if (SelectedManufacturer != null)
            {
                sparePartsQuery = sparePartsQuery.Where(x => x.ManufacturerId == SelectedManufacturer.Id);
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                switch (SelectedFilter)
                {
                    case "Tên phụ tùng":
                        sparePartsQuery = sparePartsQuery
                            .Where(x => x.Name != null && x.Name.ToLower().Contains(SearchText.ToLower()));
                        break;
                    case "Mã phụ tùng":
                        sparePartsQuery = sparePartsQuery
                            .Where(x => x.PartNumber != null && x.PartNumber.ToLower().Contains(SearchText.ToLower()));
                        break;
                    case "Tất cả":
                        SpareParts = new BindableCollection<SpareParts>(CachesServices.Instance.SpareParts.Where(x => x.Name != null && x.Name.ToLower().Contains(SearchText.ToLower())));
                        return;
                    default:
                        sparePartsQuery = sparePartsQuery
                            .Where(x => x.Name != null && x.Name.ToLower().Contains(SearchText.ToLower()));
                        break;
                }
            }

            SpareParts = new BindableCollection<SpareParts>(sparePartsQuery.OrderBy(x => x.Name));
        }
        public async Task LoadTicket(DateTime dateFrom, DateTime dateTo)
        {
            // Load tickets related to the selected spare part base in TicketSpareParts
            if (SelectedSparePart != null)
            {
                var ticketIds = CachesServices.Instance.TicketSpareParts
                    .Where(x => x.SparePartId == SelectedSparePart.Id)
                    .Select(x => x.TicketId)
                    .ToList();

                // Filter tickets by date range
                var filteredTickets = CachesServices.Instance.Tickets
                    .Where(x => ticketIds.Contains(x.Id) && x.DateCreated >= dateFrom && x.DateCreated <= dateTo)
                    .OrderByDescending(x => x.DateCreated)
                    .ToList();

                Tickets = new BindableCollection<Ticket>(filteredTickets);

                // Tính số lượng SparePart mà các ticket sử dụng trong khoảng thời gian
                var filteredTicketIds = filteredTickets.Select(t => t.Id).ToList();
                OutNumber = CachesServices.Instance.TicketSpareParts
                    .Where(x => x.SparePartId == SelectedSparePart.Id && filteredTicketIds.Contains(x.TicketId))
                    .Sum(x => x.Quantity);
            }
            else
            {
                Tickets = new BindableCollection<Ticket>();
                OutNumber = 0;
            }

            // Với mỗi Ticket, tạo danh sách TicketSparePart có trong ticket đó và kết hợp lại thành danh sách ShowTicket
            ShowTickets = new ObservableCollection<ShowTicket>();
            foreach (var ticket in Tickets)
            {
                var ticketSpareParts = CachesServices.Instance.TicketSpareParts
                    //.Where(x => x.TicketId == ticket.Id && x.SparePartId == SelectedSparePart?.Id)
                    .Where(x => x.TicketId == ticket.Id )
                    .ToList();
                ShowTickets.Add(new ShowTicket
                {
                    Ticket = ticket,
                    TicketSpareParts = ticketSpareParts
                });
            }
        }
        // QuerySparePartStatistics
        public async void QuerySparePartStatistics()
        {
            // Load tickets related to the selected spare part base in TicketSpareParts
            await LoadTicket(DateFrom, DateTo);
        }

        // Xem chi tiết ticket
        public async void OpenTicketDetail(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement fe)
            {
                if (fe.DataContext is ShowTicket ticket)
                {
                    await _mainViewModel.OpenTicketDetailView(CachesServices.Instance.Tickets.Find(x => x.Id == ticket.Ticket.Id));
                }
            }
        }
        public void SetSparePartDeleteButtonEnable()
        {
            // Delete Button enable khi SelectedSparePart != null và IsSparePartUserEditting = false
            IsSparePartDeleteButtonEnable = (SelectedSparePart != null && !IsSparePartUserEditting);
        }

        #region Button Click SparePart
        // Edit Button Click
        public void bnSparePartEdit()
        {
            _tempSparePart = new List<SpareParts>();
            // Lưu trữ dữ liệu ban đầu
            foreach (var item in SpareParts)
            {
                _tempSparePart.Add(new SpareParts
                {
                    Id = item.Id,
                    Name = item.Name,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    QuantityInStock = item.QuantityInStock,
                    ManufacturerId = item.ManufacturerId,
                    Manufacturer = item.Manufacturer
                });
            }

            IsSparePartUserEditting = true;
            // Lưu trạng thái của các nút Customer

            _tempIsManufacturerEnable = IsManufacturerEnable;
            // Deactive các nút Manufacturer

            IsManufacturerEnable = false;
        }
        // Ok Button Click
        public async void bnSparePartSave()
        {
            IsSparePartUserEditting = false;
            // Lưu dữ liêu hiển thị
            ObservableCollection<SpareParts> temp = new ObservableCollection<SpareParts>(SpareParts);
            foreach (var SparePart in temp)
            {
                SparePart.ManufacturerId = SelectedManufacturer.Id;
                SparePart.Manufacturer = null;
            }
            // Lưu dữ liệu vào database
            await new SaveDataServices().SaveSparePartListAsync(temp.ToList());
            // Cập nhật lại dữ liệu trong Caches lấy từ database
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Load lại dữ liệu
            LoadSpareParts();
            // Trả lại trạng thái của các nút Customer

            IsManufacturerEnable = _tempIsManufacturerEnable;
        }
        // Cancel Button Click
        public void bnSparePartCancel()
        {
            // Đưa dữ liệu về trạng thái ban đầu

            SpareParts = new ObservableCollection<SpareParts>();
            foreach (var item in _tempSparePart)
            {
                SpareParts.Add(new SpareParts
                {
                    Id = item.Id,
                    Name = item.Name,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    QuantityInStock = item.QuantityInStock,
                    ManufacturerId = item.ManufacturerId,
                    Manufacturer = item.Manufacturer
                });
            }

            IsSparePartUserEditting = false;
            // Trả lại trạng thái của các nút Customer

            IsManufacturerEnable = _tempIsManufacturerEnable;
        }
        // New Button Click
        public void bnSparePartNew()
        {
            _tempSparePart = new List<SpareParts>();
            // Lưu trữ dữ liệu ban đầu
            foreach (var item in SpareParts)
            {
                _tempSparePart.Add(new SpareParts
                {
                    Id = item.Id,
                    Name = item.Name,
                    PartNumber = item.PartNumber,
                    Description = item.Description,
                    QuantityInStock = item.QuantityInStock,
                    ManufacturerId = item.ManufacturerId,
                    Manufacturer = item.Manufacturer
                });
            }

            SpareParts.Add(new SpareParts { Name = "Tên phụ tùng ...", ManufacturerId = SelectedManufacturer.Id, Manufacturer = SelectedManufacturer, QuantityInStock = 0, Description = "", PartNumber = "" });
            NotifyOfPropertyChange(() => SpareParts);
            SelectedSparePart = SpareParts[SpareParts.Count - 1];
            IsSparePartUserEditting = true;
            // Lưu trạng thái của các nút Manufacturer

            _tempIsManufacturerEnable = IsManufacturerEnable;
            // Deactive các nút Manufacturer

            IsManufacturerEnable = false;
        }
        // Delete Button Click
        public async void bnSparePartDelete()
        {
            if (SelectedSparePart == null)
            {
                return;
            }
            // Bật hội thoại xác nhận
            var customMessageBoxViewModel = new CustomMessageBoxViewModel
            {
                Message = "Xác nhận!!",
                TxtMessage = "Bạn có muốn xóa phụ tùng này? Việc xóa phụ tùng sẽ dẫn đến mất dữ liệu phụ tùng của các Thẻ kỹ thuật liên quan!",
                IsConfirmation = true
            };
            var windowManager = new WindowManager();
            await windowManager.ShowDialogAsync(customMessageBoxViewModel);
            if (customMessageBoxViewModel.DialogResult == MessageBoxResult.Yes)
            {

                await new DeleteDataServices().DeleteSparePartsAsync(SelectedSparePart);
                // Cập nhật lại dữ liệu trong Caches lấy từ database
                CachesServices.ResetInstance();
                await CachesServices.Instance.GetAllData();
                LoadSpareParts();
                SelectedSparePart = null;

            }
        }
        #endregion

        public async Task LoadManufacturer()
        {
            // Tải manufacturer theo từ khóa ManufacturerSearchText
            IsManufacturerEnable = true;
            Manufacturers = new ObservableCollection<Manufacturer>(
                CachesServices.Instance.Manufacturers
                .Where(x => x.Name.ToLower().Contains(SearchManufacturer.ToLower()))
                .OrderBy(x => x.Name));

        }

        //------------------------------------------------------------------------
        // Ticket SparePart
        public class ShowTicket
        {
            public Ticket Ticket { get; set; }
            public List<TicketSpareParts> TicketSpareParts { get; set; }
        }
        private ObservableCollection<ShowTicket> _showTickets;
        public ObservableCollection<ShowTicket> ShowTickets
        {
            get { return _showTickets; }
            set
            {
                _showTickets = value;
                NotifyOfPropertyChange(() => ShowTickets);
            }
        }
    }
}
