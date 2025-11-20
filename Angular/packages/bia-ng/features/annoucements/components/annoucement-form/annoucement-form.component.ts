import { AfterViewInit, Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { EditorInitEvent, EditorModule } from 'primeng/editor';
import { FloatLabel } from 'primeng/floatlabel';
import Quill from 'quill';
import { Annoucement } from '../../model/annoucement';

@Component({
  selector: 'app-annoucement-form',
  templateUrl: 'annoucement-form.component.html',
  styleUrls: ['annoucement-form.component.scss'],
  imports: [
    BiaFormComponent,
    EditorModule,
    FloatLabel,
    TranslatePipe,
    PrimeTemplate,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class AnnoucementFormComponent
  extends CrudItemFormComponent<Annoucement>
  implements AfterViewInit
{
  quillEditor: Quill | undefined;

  ngAfterViewInit(): void {
    const rawContentForm = this.biaFormComponent.form?.get('rawContent');
    rawContentForm?.valueChanges.subscribe((val: string) => {
      if (val) {
        const formatted = rawContentForm.value.replace(/<\/?p>/g, '');
        rawContentForm.setValue(formatted, { emitEvent: false });
        this.quillEditor?.setSelection(formatted.length);
      }
    });
  }

  onEditorInit(event: EditorInitEvent) {
    this.quillEditor = event.editor as Quill;
    this.quillEditor.keyboard.bindings['Enter'] = [];

    this.quillEditor.keyboard.addBinding({ key: 'Enter' }, () => {
      return false;
    });

    this.quillEditor.keyboard.addBinding(
      { key: 'Enter', shiftKey: true },
      () => {
        return false;
      }
    );
  }
}
