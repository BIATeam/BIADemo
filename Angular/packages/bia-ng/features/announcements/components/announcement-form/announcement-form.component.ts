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
import { Announcement } from '../../model/announcement';

@Component({
  selector: 'app-announcement-form',
  templateUrl: 'announcement-form.component.html',
  styleUrls: ['announcement-form.component.scss'],
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
export class AnnouncementFormComponent
  extends CrudItemFormComponent<Announcement>
  implements AfterViewInit
{
  quillEditor: Quill | undefined;

  ngAfterViewInit(): void {
    const rawContentForm = this.biaFormComponent.form?.get('rawContent');
    rawContentForm?.valueChanges.subscribe((val: string) => {
      if (val) {
        const newValue = rawContentForm.value.replace(/<\/?p>/g, '');
        const editorCursorIndex = this.quillEditor?.getSelection()?.index;

        if (editorCursorIndex) {
          rawContentForm.setValue(newValue, { emitEvent: false });
          this.quillEditor!.setSelection(editorCursorIndex);
        }
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
