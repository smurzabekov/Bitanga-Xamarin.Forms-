using Bitango_.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bitango_.ViewModel
{
    class PromotionViewModel : BaseViewModel
    {
        private Promotion selectedPromotion;
        /// <summary>
        /// Выбранная акция
        /// </summary>
        public Promotion SelectedPromotion {
            get {
                return selectedPromotion;
            }
            set {
                selectedPromotion = value;
                OnPropertyChanged();
            }
        }

    }
}
