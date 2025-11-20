import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'packages/bia-ng/shared/public-api';
import { annoucementCRUDConfiguration } from '../../annoucement.constants';
import { AnnoucementFormComponent } from '../../components/annoucement-form/annoucement-form.component';
import { Annoucement } from '../../model/annoucement';
import { AnnoucementService } from '../../services/annoucement.service';

@Component({
  selector: 'app-annoucement-new',
  templateUrl: './annoucement-new.component.html',
  imports: [AnnoucementFormComponent, AsyncPipe],
})
export class AnnoucementNewComponent
  extends CrudItemNewComponent<Annoucement>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public annoucementService: AnnoucementService
  ) {
    super(injector, annoucementService);
    this.crudConfiguration = annoucementCRUDConfiguration;
  }
}
