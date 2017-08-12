using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MLPresentAndAcquireItemAction : MLAcquireItemAction
{
    private SpritePresenter _presenter;
    private SpritePresenter presenter {
        get {
            if(!_presenter) { _presenter = FindObjectOfType<SpritePresenter>(); }
            return _presenter;
        }
    }
    protected override void performStaticAction(MLNumericParam value) {
        presenter.present(item.srendrr, () => {
            base.performStaticAction(value);
        });
    }
}

