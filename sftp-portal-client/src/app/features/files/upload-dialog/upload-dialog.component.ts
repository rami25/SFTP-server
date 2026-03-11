import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { HttpEventType } from '@angular/common/http';
import { FileService } from '../../../core/services/file.service';
import { FolderItem } from '../../../core/models/folder.model';

@Component({
  selector: 'app-upload-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule
  ],
  templateUrl: './upload-dialog.component.html',
  styleUrls: ['./upload-dialog.component.scss']
})
export class UploadDialogComponent {
  selectedFile: File | null = null;
  isDragOver = false;
  isUploading = false;
  uploadProgress = 0;
  errorMessage = '';

  constructor(
    private fileService: FileService,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<UploadDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { folder: FolderItem }
  ) {}

  // ── Drag & Drop ──────────────────────────────────────────
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = true;
  }

  onDragLeave(): void {
    this.isDragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
    const file = event.dataTransfer?.files[0];
    if (file) this.selectFile(file);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.[0]) this.selectFile(input.files[0]);
  }

  selectFile(file: File): void {
    this.errorMessage = '';
    this.selectedFile = file;
  }

  // ── Upload ───────────────────────────────────────────────
  onUpload(): void {
    if (!this.selectedFile || !this.data.folder) return;

    this.isUploading = true;
    this.uploadProgress = 0;
    this.errorMessage = '';

    this.fileService.uploadFile(
      this.data.folder.remotePath,
      this.selectedFile
    ).subscribe({
      next: (event) => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
          this.uploadProgress = Math.round(100 * event.loaded / event.total);
        } else if (event.type === HttpEventType.Response) {
          const result = event.body as any;
          if (result?.success) {
            this.isUploading = false;
            this.dialogRef.close({ success: true });
          } else {
            this.isUploading = false;
            this.errorMessage = result?.errorReason ?? 'Upload failed.';
          }
        }
      },
      error: (err) => {
        this.isUploading = false;
        this.errorMessage = err.error?.message ?? 'Upload failed. Please try again.';
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  getFileSize(bytes: number): string {
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  }
}
