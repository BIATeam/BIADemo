import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { CrudItemBulkFormComponent } from './components/crud-item-bulk-form/crud-item-bulk-form.component';

@NgModule({
  declarations: [CrudItemBulkFormComponent],
  imports: [SharedModule],
  exports: [CrudItemBulkFormComponent],
})
export class CrudItemBulkModule {}
