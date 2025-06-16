using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel;

namespace Client.ViewModels
{
    public class EditItemEntityMessageBoxViewModel : Screen
    {
        private bool _result;
        public bool Result
        {
            get { return _result; }
            set
            {
                _result = value;
                NotifyOfPropertyChange(() => Result);
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private object _selectedItemEntity;
        public object SelectedItemEntity
        {
            get { return _selectedItemEntity; }
            set
            {
                _selectedItemEntity = value;
                NotifyOfPropertyChange(() => SelectedItemEntity);
                NotifyOfPropertyChange(() => ItemProperties);
            }
        }

        public IEnumerable<PropertyDescriptor> ItemProperties
        {
            get
            {
                if (SelectedItemEntity == null)
                    return null;

                var properties = TypeDescriptor.GetProperties(SelectedItemEntity);
                var filteredProperties = new List<PropertyDescriptor>();

                foreach (PropertyDescriptor property in properties)
                {
                    if (property.Name.ToLower() != "id")
                    {
                        filteredProperties.Add(property);
                    }
                }

                return filteredProperties;
            }
        }

        public EditItemEntityMessageBoxViewModel(string tl, object selectedItemEntity)
        {
            Title = tl;
            SelectedItemEntity = selectedItemEntity;
        }

        //Button actions
        public void bnOk()
        {
            if (SelectedTestT != null)
            {
                Result = true;
                TryCloseAsync();
            }
            else
            {
                var customMessageBoxViewModel = new CustomMessageBoxViewModel
                {
                    Message = "No test selected",
                    TxtMessage = "Please select a test",
                    IsInformation = true
                };
                var windowManager = new WindowManager();
                windowManager.ShowDialogAsync(customMessageBoxViewModel);
            }
        }
        public void bnCancel()
        {
            Result = false;
            TryCloseAsync();
        }
    }
}
