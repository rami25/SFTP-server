import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { FileItem } from '../../../core/models/file.model';

@Component({
  selector: 'app-file-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatChipsModule
  ],
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss']
})
export class FileListComponent {
  @Input() files: FileItem[] = [];
  @Input() canDownload = false;
  @Output() downloadFile = new EventEmitter<FileItem>();

  displayedColumns: string[] = ['icon', 'name', 'size', 'lastModified', 'actions'];

  getFileIcon(fileName: string): string {
    if (fileName.endsWith('.pgp')) return 'lock';
    if (fileName.endsWith('.csv')) return 'table_chart';
    if (fileName.endsWith('.pdf')) return 'picture_as_pdf';
    if (fileName.endsWith('.zip')) return 'folder_zip';
    return 'insert_drive_file';
  }

  getFileIconColor(fileName: string): string {
    if (fileName.endsWith('.pgp')) return '#f57c00';
    if (fileName.endsWith('.csv')) return '#388e3c';
    if (fileName.endsWith('.pdf')) return '#d32f2f';
    return '#1976d2';
  }

  onDownload(file: FileItem): void {
    this.downloadFile.emit(file);
  }
}