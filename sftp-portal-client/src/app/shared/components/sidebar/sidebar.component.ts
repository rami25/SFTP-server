import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { FolderItem } from '../../../core/models/folder.model';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatListModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  @Input() folders: FolderItem[] = [];
  @Input() isOpen = true;
  @Input() selectedFolder: FolderItem | null = null;
  @Output() folderSelected = new EventEmitter<FolderItem>();

  getFolderIcon(type: string): string {
    switch (type) {
      case 'Demographic': return 'people';
      case 'Bank': return 'account_balance';
      case 'GL': return 'receipt_long';
      default: return 'folder';
    }
  }

  getFolderColor(type: string): string {
    switch (type) {
      case 'Demographic': return '#1976d2';
      case 'Bank': return '#388e3c';
      case 'GL': return '#f57c00';
      default: return '#757575';
    }
  }

  onFolderClick(folder: FolderItem): void {
    this.folderSelected.emit(folder);
  }
}