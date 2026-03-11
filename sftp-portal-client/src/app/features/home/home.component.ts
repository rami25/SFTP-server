import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';
import { FileBrowserComponent } from '../files/file-browser/file-browser.component';
import { FolderService } from '../../core/services/folder.service';
import { AuthService } from '../../core/services/auth.service';
import { FolderItem } from '../../core/models/folder.model';



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    NavbarComponent,
    SidebarComponent,
    FileBrowserComponent,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatIconModule 
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  folders: FolderItem[] = [];
  selectedFolder: FolderItem | null = null;
  isSidebarOpen = true;
  isLoading = false;

  constructor(
    private folderService: FolderService,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadFolders();
  }

  loadFolders(): void {
    const entity = this.authService.getUserEntity();
    if (!entity) return;

    this.isLoading = true;
    this.folderService.getFolders(entity).subscribe({
      next: (folders) => {
        this.folders = folders;
        this.isLoading = false;
        // Auto-select first folder
        if (folders.length > 0) {
          this.selectedFolder = folders[0];
        }
      },
      error: () => {
        this.isLoading = false;
        this.snackBar.open('Failed to load folders.', 'Close', { duration: 3000 });
      }
    });
  }

  onFolderSelected(folder: FolderItem): void {
    this.selectedFolder = folder;
  }

  onToggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }
}