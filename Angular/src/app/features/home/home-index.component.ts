import { Component, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MyDocument } from 'src/app/core/databases/biademo/entities/document.entity';
import { MyDocumentRepository } from 'src/app/core/databases/biademo/repositories/my-document.repository';
import { UserRepository } from 'src/app/core/databases/biademo/repositories/user.repository';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { User } from '../bia-features/users/model/user';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  documents: MyDocument[] = [];
  selectedDocument: MyDocument | null = null;
  selectedDocumentType: string | null = null;
  documentContent: string | SafeUrl | null = null;

  constructor(
    private layoutService: BiaLayoutService,
    protected userRepository: UserRepository,
    protected documentRepository: MyDocumentRepository,
    private sanitizer: DomSanitizer
  ) {}
  ngOnInit(): void {
    this.layoutService.hideBreadcrumb();
  }
  ngOnDestroy(): void {
    this.layoutService.showBreadcrumb();
  }

  async addUser() {
    const users = await this.userRepository.read();
    let userName = 'User';
    if (users.length > 0) {
      userName += users[users.length - 1].id;
    } else {
      userName += 0;
    }
    const user = {
      name: userName,
      email: `${userName}@mail.com`,
      newProperty: 0,
    };
    await this.userRepository.create(user);
  }

  async deleteUser(user: User) {
    await this.userRepository.delete(user.id!);
  }

  async onFileSelect() {
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = '*/*'; // Allow all file types
    fileInput.onchange = (event: any) => this.handleFileUpload(event);
    fileInput.click();
  }

  async handleFileUpload(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const blob = new Blob([file], { type: file.type });
      const document = { name: file.name, content: blob };

      // Store document in IndexedDB
      await this.documentRepository.create(document);
    }
  }

  openDocument(document: MyDocument) {
    const url = URL.createObjectURL(document.content);
    window.open(url, '_blank');
  }

  async deleteDocument(document: MyDocument) {
    if (this.selectedDocument?.name === document.name) {
      this.selectedDocument = null;
      this.selectedDocumentType = null;
      this.documentContent = null;
    }
    this.documentRepository.delete(document.name);
  }

  viewDocument(document: MyDocument) {
    this.selectedDocument = document;

    const fileType = document.content.type;

    // Determine the content type
    if (fileType.startsWith('text')) {
      this.selectedDocumentType = 'text';
      const reader = new FileReader();
      reader.onload = () => {
        this.documentContent = reader.result as string;
      };
      reader.readAsText(document.content);
    } else if (fileType.startsWith('image')) {
      this.selectedDocumentType = 'image';
      this.documentContent = this.sanitizer.bypassSecurityTrustUrl(
        URL.createObjectURL(document.content)
      );
    } else if (fileType === 'application/pdf') {
      this.selectedDocumentType = 'pdf';
      this.documentContent = this.sanitizer.bypassSecurityTrustResourceUrl(
        URL.createObjectURL(document.content)
      );
    } else {
      this.selectedDocumentType = null;
      this.documentContent = null;
    }
  }
}
