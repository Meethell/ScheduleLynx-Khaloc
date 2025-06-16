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
    public class TicketPendingViewModel : Screen
    {
        public class ShowTicket
        {
            public Ticket Ticket { get; set; }
            public List<User> AssignUsers { get; set; }
        }

        // Full properties
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
                LoadTickets(SearchText);
            }
        }

        // Constructor
        private readonly TicketViewModel _ticketViewModel;
        public TicketPendingViewModel(TicketViewModel ticketViewModel)
        {
            _ticketViewModel = ticketViewModel;
            LoadTickets(SearchText);
        }

        // Methods
        public void LoadTickets(string searchText)
        {
            var currentUserId = CachesServices.UserId;
            var currentRole = CachesServices.SystemRole?.Name;

            IEnumerable<Ticket> filteredTickets;

            // Lấy các ticket có Status khác "Resolved" (3) và "Cancelled" (4)
            Func<Ticket, bool> pendingStatus = t => t.Status.Id != 3 && t.Status.Id != 4;

            if (string.Equals(currentRole, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                filteredTickets = CachesServices.Instance.Tickets
                    .Where(pendingStatus);
            }
            else
            {
                var assignedTicketIds = CachesServices.Instance.TicketTags
                    .Where(tag => tag.UserId == currentUserId)
                    .Select(tag => tag.TicketId)
                    .Distinct()
                    .ToList();

                filteredTickets = CachesServices.Instance.Tickets
                    .Where(t => assignedTicketIds.Contains(t.Id) && pendingStatus(t));
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredTickets = filteredTickets
                    .Where(t => t.Title != null && t.Title.ToLower().Contains(searchText.ToLower()));
            }

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
        public async Task OpenTicketDetailViewCommand()
        {
            if (SelectedTicket?.Ticket != null)
                await _ticketViewModel.TicketDetail(CachesServices.Instance.Tickets.Find(x => x.Id == SelectedTicket.Ticket.Id));
        }
    }
}
