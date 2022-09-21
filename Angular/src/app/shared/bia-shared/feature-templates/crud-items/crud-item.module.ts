import { NgModule } from '@angular/core';
import { CrudItemFormComponent } from './components/crud-item-form/crud-item-form.component';
import { CrudItemsIndexComponent } from './views/crud-items-index/crud-items-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { CrudItemNewComponent } from './views/crud-item-new/crud-item-new.component';
import { CrudItemEditComponent } from './views/crud-item-edit/crud-item-edit.component';
import { CrudItemItemComponent } from './views/crud-item-item/crud-item-item.component';
import { CrudItemTableComponent } from './components/crud-item-table/crud-item-table.component';
import { BaseDto } from '../../model/base-dto';


@NgModule({
  declarations: [
    CrudItemItemComponent,
    CrudItemsIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    CrudItemFormComponent,
    CrudItemNewComponent,
    CrudItemEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    CrudItemTableComponent,
  ],
  imports: [
    SharedModule,
    // TODO redefine in plane
    /*
    StoreModule.forFeature(storeKey, reducers),
    EffectsModule.forFeature([CrudItemsEffects]),*/
  ]
})

export class CrudItemModule<CrudItem extends BaseDto> {
}

