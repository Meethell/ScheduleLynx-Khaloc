using Caliburn.Micro;
using Client.Entities.TicketEntities;
using Client.Entities.UserEntity;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class TicketManagementViewModel : Screen
    {
        public class ShowTicket
        {
            public Ticket Ticket { get; set; }
            public List<User> AssignUsers { get; set; }
        }
        // Full properties
        private bool _isNewButtonVisible = false;
        public bool IsNewButtonVisible
        {
            get { return _isNewButtonVisible; }
            set
            {
                _isNewButtonVisible = value;
                NotifyOfPropertyChange(() => IsNewButtonVisible);
            }
        }
        private ObservableCollection<Ticket> _tickets;
        public ObservableCollection<Ticket> Tickets
        {
            get { return _tickets; }
            set
            {
                _tickets = value;
                NotifyOfPropertyChange(() => Tickets);
            }
        }
        private ObservableCollection<ShowTicket> showTickets;
        public ObservableCollection<ShowTicket> ShowTickets
        {
            get { return showTickets; }
            set
            {
                showTickets = value;
                NotifyOfPropertyChange(() => ShowTickets);
            }
        }
        private ShowTicket _selectedTicket;
        public ShowTicket SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                _selectedTicket = value;
                NotifyOfPropertyChange(() => SelectedTicket);
            }
        }

        // Search Properties
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
                LoadTickets(_searchText, DateFrom, DateTo); // Load lại tickets khi searchText thay đổi
            }
        }
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
        // Contructor
        private readonly TicketViewModel _ticketViewModel;
        public TicketManagementViewModel(TicketViewModel ticketViewModel)
        {
            _ticketViewModel = ticketViewModel;
            // Lấy ngày bắt đầu và kết thúc của tháng hiện tại
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateFrom.AddMonths(1).AddDays(-1);

            // Hiển thị nút New nếu là Admin
            IsNewButtonVisible = CachesServices.SystemRole?.Name == "Admin";
            

            // Update OverDue
            UpdateOverDue();

            LoadTickets(null,DateFrom,DateTo);
            
        }
        // Methods
        // Cập nhât OverDue cho tất cả các ticket, chỉ xét các ticket chưa hoàn thành, cancel và chưa quá hạn
        public async void UpdateOverDue()
        {
            List<Ticket> NotResolvedTicket = new List<Ticket>(CachesServices.Instance.Tickets.FindAll(x => x.Status != CachesServices.Instance.TicketStatuses[2] && !x.Overdue));
            if (NotResolvedTicket.Count == 0) return;
            foreach (var ticket in NotResolvedTicket)
            {
                if (ticket.DateDue < DateTime.Now)
                {
                    ticket.Overdue = true;
                    ticket.Status = null;
                    ticket.Issue = null;
                    ticket.Instrument = null;
                    ticket.Priority = null;
                    ticket.Type = null;
                    ticket.ServiceType = null;

                    // Lưu lại vào database
                    await new SaveDataServices().SaveTicketAsync(ticket);
                }
            }
            CachesServices.ResetInstance();
            await CachesServices.Instance.GetAllData();
            // Load tickets
            LoadTickets(SearchText, DateFrom, DateTo);

        }
        // Load tickets từ searchText, DateFrom, DateTo
        public void LoadTickets(string searchText, DateTime dateFrom, DateTime dateTo)
        {
            // Lấy thông tin user hiện tại và role
            var currentUserId = CachesServices.UserId;
            var currentRole = CachesServices.SystemRole?.Name;

            IEnumerable<Ticket> filteredTickets;

            if (string.Equals(currentRole, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Admin: lấy toàn bộ ticket theo điều kiện ngày và search
                filteredTickets = CachesServices.Instance.Tickets
                    .Where(t => t.DateCreated >= dateFrom && t.DateCreated <= dateTo);
            }
            else
            {
                // User thường: chỉ lấy ticket được gán cho user này (qua TicketTag)
                var assignedTicketIds = CachesServices.Instance.TicketTags
                    .Where(tag => tag.UserId == currentUserId)
                    .Select(tag => tag.TicketId)
                    .Distinct()
                    .ToList();

                filteredTickets = CachesServices.Instance.Tickets
                    .Where(t => assignedTicketIds.Contains(t.Id) && t.DateCreated >= dateFrom && t.DateCreated <= dateTo);
            }

            // Lọc theo searchText nếu có
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredTickets = filteredTickets
                    .Where(t => t.Title != null && t.Title.ToLower().Contains(searchText.ToLower()));
            }

            // Sắp xếp
            Tickets = new ObservableCollection<Ticket>(
                filteredTickets
                    .OrderBy(x => x.Status.Id)
                    .ThenByDescending(x => x.Priority.Id)
                    .ThenByDescending(x => x.DateCreated)
            );

            // Tạo danh sách ShowTicket từ Tickets
            var ticketTags = CachesServices.Instance.TicketTags;
            var users = CachesServices.Instance.Users;

            ShowTickets = new ObservableCollection<ShowTicket>(
                Tickets.Select(ticket =>
                {
                    var assignedUserIds = ticketTags
                        .Where(tag => tag.TicketId == ticket.Id)
                        .Select(tag => tag.UserId)
                        .ToList();

                    var assignedUsers = users
                        .Where(user => assignedUserIds.Contains(user.Id))
                        .ToList();

                    return new ShowTicket
                    {
                        Ticket = ticket,
                        AssignUsers = assignedUsers
                    };
                })
            );
        }
        // Button Commands
        public async Task btNew()
        {
            await _ticketViewModel.NewTicketView(new Ticket(), true);
        }
        public async Task OpenTicketDetailViewCommand()
        {
            await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == SelectedTicket.Ticket.Id));
        }
        public void btSearch()
        {
            LoadTickets(SearchText, DateFrom, DateTo);
        }
    }
}
