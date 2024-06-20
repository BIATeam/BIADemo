import { NgModule } from '@angular/core';
import { CrudItemBulkFormComponent } from './components/crud-item-bulk-form/crud-item-bulk-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [CrudItemBulkFormComponent],
  imports: [SharedModule],
  exports: [CrudItemBulkFormComponent],
})
export class CrudItemBulkModule {}
