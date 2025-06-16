using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using Client.Entities.ModelEntities;

namespace Client.ViewModels
{
    public class SpecificationViewModel : Conductor<IScreen>
    {
        // Trạng thái các radio button
        private bool _isModelViewChecked = false;
        public bool IsModelViewChecked
        {
            get { return _isModelViewChecked; }
            set
            {
                _isModelViewChecked = value;
                NotifyOfPropertyChange(() => IsModelViewChecked);
            }
        }
        private bool _isModelSpecificationViewChecked = false;
        public bool IsModelSpecificationViewChecked
        {
            get { return _isModelSpecificationViewChecked; }
            set
            {
                _isModelSpecificationViewChecked = value;
                NotifyOfPropertyChange(() => IsModelSpecificationViewChecked);
            }
        }
        private bool _isConsumableViewChecked = false;
        public bool IsConsumableViewChecked
        {
            get { return _isConsumableViewChecked; }
            set
            {
                _isConsumableViewChecked = value;
                NotifyOfPropertyChange(() => IsConsumableViewChecked);
            }
        }
        private bool _isSpecificationSetupViewChecked = false;
        public bool IsSpecificationSetupViewChecked
        {
            get { return _isSpecificationSetupViewChecked; }
            set
            {
                _isSpecificationSetupViewChecked = value;
                NotifyOfPropertyChange(() => IsSpecificationSetupViewChecked);
            }
        }

        public SpecificationViewModel()
        {
            
        }
        public async Task ModelView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsModelViewChecked = true;
            await ActivateItemAsync(new ModelViewModel(this,false));
        }
        public async Task ConsumableView()
        {
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsConsumableViewChecked = true;
            await ActivateItemAsync(new ModelViewModel(this, true));
        }
        public async Task ModelSpecificationView(Model model)
        {
            if(model == null) return;
            if (ActiveItem != null)
            {
                await DeactivateItemAsync(ActiveItem, true);
                (ActiveItem as IDisposable)?.Dispose();
            }
            IsModelSpecificationViewChecked = true;
            await ActivateItemAsync(new ModelSpecificationViewModel(this, model));
        }
        
    }
}
