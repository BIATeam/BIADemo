import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { CrudItemImportFormComponent } from './components/crud-item-import-form/crud-item-import-form.component';

@NgModule({
  declarations: [CrudItemImportFormComponent],
  imports: [SharedModule],
  exports: [CrudItemImportFormComponent],
})
export class CrudItemImportModule {}
