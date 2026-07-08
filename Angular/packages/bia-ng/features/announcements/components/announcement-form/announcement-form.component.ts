import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { Announcement } from 'packages/bia-ng/models/public-api';
import {
  BiaFormComponent,
  CrudItemFormComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { EditorInitEvent, EditorModule } from 'primeng/editor';
import { FloatLabel } from 'primeng/floatlabel';
import Quill from 'quill';

@Component({
  selector: 'bia-announcement-form',
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
export class AnnouncementFormComponent extends CrudItemFormComponent<Announcement> {
  quillEditor: Quill | undefined;

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

  override onSave(announcement: Announcement) {
    if (announcement.rawContent) {
      announcement.rawContent = announcement.rawContent.replace(/<\/?p>/g, '');
    }
    super.onSave(announcement);
  }
}
