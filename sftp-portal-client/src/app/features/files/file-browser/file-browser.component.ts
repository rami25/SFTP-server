import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FileService } from '../../../core/services/file.service';
import { FolderItem } from '../../../core/models/folder.model';
import { FileItem } from '../../../core/models/file.model';
import { FileListComponent } from '../file-list/file-list.component';
import { UploadDialogComponent } from '../upload-dialog/upload-dialog.component';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-file-browser',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTooltipModule,
    FileListComponent
  ],
  templateUrl: './file-browser.component.html',
  styleUrls: ['./file-browser.component.scss']
})
export class FileBrowserComponent implements OnChanges {
  @Input() folder: FolderItem | null = null;

  files: FileItem[] = [];
  isLoading = false;
  isDownloading = false;

  constructor(
    private fileService: FileService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['folder'] && this.folder) {
      this.loadFiles();
    }
  }

  // ── Load files ───────────────────────────────────────────
  loadFiles(): void {
    if (!this.folder) return;
    this.isLoading = true;

    this.fileService.listFiles(this.folder.remotePath).subscribe({
      next: (files) => {
        this.files = files;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.snackBar.open('Failed to load files.', 'Close', { duration: 3000 });
      }
    });
  }

  // ── Upload ───────────────────────────────────────────────
  onUpload(): void {
    const dialogRef = this.dialog.open(UploadDialogComponent, {
      width: '500px',
      data: { folder: this.folder }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result?.success) {
        this.loadFiles(); // Refresh file list after upload
        this.snackBar.open('File uploaded successfully!', 'Close', {
          duration: 3000,
          panelClass: 'success-snackbar'
        });
      }
    });
  }

  // ── Download ─────────────────────────────────────────────
  onDownload(file: FileItem): void {
    if (!this.folder) return;
    this.isDownloading = true;

    this.fileService.downloadFile(this.folder.remotePath, file.name).subscribe({
      next: (blob) => {
        // Create a download link and trigger it
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = file.name;
        link.click();
        window.URL.revokeObjectURL(url);
        this.isDownloading = false;
        this.snackBar.open(`Downloading ${file.name}...`, 'Close', { duration: 3000 });
      },
      error: () => {
        this.isDownloading = false;
        this.snackBar.open('Download failed.', 'Close', { duration: 3000 });
      }
    });
  }

  // ── Refresh ──────────────────────────────────────────────
  onRefresh(): void {
    this.loadFiles();
  }

  getFolderIcon(type: string): string {
    switch (type) {
      case 'Demographic': return 'people';
      case 'Bank': return 'account_balance';
      case 'GL': return 'receipt_long';
      default: return 'folder';
    }
  }
}